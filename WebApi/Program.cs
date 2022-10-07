using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi;
using WebApi.Dtos;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Models.Identity;

var builder = WebApplication.CreateBuilder(args);

#region JWT authentication configuration
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();
#endregion JWT authentication configuration

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "IFC App API", Version = "v1" });
    o.AddSecurityDefinition("Bearer", JwtHelper.CreateSecurityScheme());
    o.AddSecurityRequirement(JwtHelper.CreateSecurityRequirements());
});

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.Urls.Add("http://localhost:5208");
    app.Urls.Add("http://10.0.0.4:5208");
}

app.UseCors("corsapp");
app.UseAuthentication();
app.UseAuthorization();

#region authentication
var scope = app.Services.CreateScope();
app.MapPost("/login", [AllowAnonymous] async (LoginDto loginDto) =>
{
    var result = await scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>().PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var appUser = await userManager.FindByEmailAsync(loginDto.Email);
    if (result.Succeeded)
    {
        var loginSuccessDto = new LoginSuccessDto
        {
            UserId = appUser.Id,
            Token = JwtHelper.BuildToken(loginDto, builder.Configuration)
        };

        return Results.Ok(loginSuccessDto);
    }

    return Results.Unauthorized();
});

app.MapPost("/register", [AllowAnonymous] async (RegisterDto registerDto) =>
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var appUser = await userManager.FindByEmailAsync(registerDto.Email);
    if (appUser != null)
    {
        return Results.NotFound("User already registered!");
    }

    appUser = new AppUser
    {
        Email = registerDto.Email,
        UserName = registerDto.Email,
        FirstName = registerDto.FirstName,
        LastName = registerDto.LastName,
    };
    var result = await userManager.CreateAsync(appUser, registerDto.Password);
    var errors = string.Empty;
    if (result.Succeeded)
    {
        var user = await userManager.FindByEmailAsync(appUser.Email);
        if (user != null)
        {
            var loginSuccessDto = new LoginSuccessDto
            {
                UserId = appUser.Id,
                Token = JwtHelper.BuildToken(registerDto, builder.Configuration)
            };

            return Results.Ok(loginSuccessDto);
        }

        return Results.BadRequest("User not found after creation.");
    }
    else
    {
        foreach (var error in result.Errors)
        {
            errors += error.Description;
        }
    }

    return Results.BadRequest($"Registration failed with errors: {errors}");
});
#endregion authentication

#region Projects
app.MapGet("/projects/{userId}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
async (string userId, AppDbContext dbContext) =>
    await dbContext.Projects.Where(p => p.AppUserId == userId).ToListAsync());

app.MapGet("/projects/{projectId}/{userId}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
async (int projectId, string userId, AppDbContext dbContext) =>
    await dbContext.Projects.FirstOrDefaultAsync(p => p.ProjectId == projectId && p.AppUserId == userId)
        is Project project ? Results.Ok(project) : Results.NotFound()); ;

app.MapPost("/projects/{userId}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
async (string userId, Project project, AppDbContext dbContext) =>
{
    project.AppUserId = userId;
    dbContext.Projects.Add(project);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/projects/{project.ProjectId}", project);
});

app.MapPut("/projects/{id}", 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (int id, Project project, AppDbContext dbContext) =>
{
    dbContext.Projects.Update(project);

    await dbContext.SaveChangesAsync();

    return Results.Ok(project);
});

app.MapDelete("/projects/{id}", 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.Projects.FindAsync(id) is Project project)
    {
        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

#endregion Projects

#region CciEePps

app.MapGet("/ccieepps",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (AppDbContext dbContext) =>
    await dbContext.CciEePps.ToListAsync());

#endregion CciEePps

#region WorkPackage

app.MapGet("/workpackages/byproject/",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (int id, AppDbContext dbContext) =>
{
    var workPackages = await dbContext.WorkPackages.Where(x => x.ProjectId == id)
        .Include(x => x.ModelElementInWorkPackages)
            .ThenInclude(x => x.ModelElement)
        .ToListAsync();

    var dtos = new List<WorkPackageDto>();
    foreach (var wp in workPackages)
    {
        var wpDto = new WorkPackageDto
        {
            WorkPackageId = wp.WorkPackageId,
            Name = wp.Name,
            Description = wp.Description,
            Code = wp.Code,
            ProjectId = wp.ProjectId,
            CciEePpId = wp.CciEePpId
        };

        if (wp.ModelElementInWorkPackages != null)
        {
            wpDto.ModelElements = DtoMappingHelper.MapModelElementsToDtos(wp.ModelElementInWorkPackages);
        };

        wpDto.ClassificatorNameEe = (await dbContext.CciEePps.FindAsync(wp.CciEePpId))!.TermEe;

        dtos.Add(wpDto);
    }

    return dtos;
});

app.MapPost("/workpackages", 
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (WorkPackageDto dto, AppDbContext dbContext) =>
{
    var workPackage = new WorkPackage
    {
        Name = dto.Name,
        Description = dto.Description,
        Code = dto.Code,
        ProjectId = dto.ProjectId,
        CciEePpId = dto.CciEePpId
    };
    dbContext.WorkPackages.Add(workPackage);
    await dbContext.SaveChangesAsync();
    dto.WorkPackageId = workPackage.WorkPackageId;

    if (dto.ModelElements != null)
    {
        foreach (var element in dto.ModelElements)
        {
            var existingElement = await dbContext.ModelElements.FirstOrDefaultAsync(x => x.Guid == element.Guid);
            var newElementId = 0;
            if (existingElement == null)
            {
                var modelElement = new ModelElement
                {
                    ExpressId = element.ExpressId,
                    Guid = element.Guid,
                    IfcStorey = element.IfcStorey,
                    IfcType = element.IfcType,
                    Name = element.Name,
                    ObjectType = element.ObjectType
                };

                dbContext.ModelElements.Add(modelElement);
                await dbContext.SaveChangesAsync();
                newElementId = modelElement.ModelElementId;
            }
            dbContext.ModelElementInWorkPackages.Add(new()
            {
                ModelElementId = existingElement?.ModelElementId ?? newElementId,
                WorkPackageId = workPackage.WorkPackageId
            });
            await dbContext.SaveChangesAsync();
        }
    }

    return Results.Created($"/workpackages/{dto.WorkPackageId}", dto);
});

app.MapPut("/workpackages/{id}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (int id, WorkPackage workPackage, AppDbContext dbContext) =>
{
    dbContext.WorkPackages.Update(workPackage);

    await dbContext.SaveChangesAsync();

    return Results.Ok(workPackage);
});

app.MapDelete("/workpackages/{id}",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.WorkPackages.FindAsync(id) is WorkPackage workPackage)
    {
        dbContext.WorkPackages.Remove(workPackage);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

#endregion WorkPackage

await DataInitializers.AddClassificators(scope.ServiceProvider.GetRequiredService<AppDbContext>());

app.Run();
