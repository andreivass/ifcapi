using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
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

app.MapGet("/projects", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (AppDbContext dbContext) =>
    await dbContext.Projects.ToListAsync());

app.MapGet("/projects/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.Projects.FindAsync(id)
        is Project project ? Results.Ok(project) : Results.NotFound());

app.MapPost("/projects", async (Project project, AppDbContext dbContext) =>
{
    dbContext.Projects.Add(project);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/projects/{project.ProjectId}", project);
});

app.MapPut("/projects/{id}", async (int id, Project project, AppDbContext dbContext) =>
{
    dbContext.Projects.Update(project);

    await dbContext.SaveChangesAsync();

    return Results.Ok(project);
});

app.MapDelete("/projects/{id}", async (int id, AppDbContext dbContext) =>
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

app.MapGet("/ccieepps", async (AppDbContext dbContext) =>
    await dbContext.CciEePps.ToListAsync());

/*app.MapGet("/ccieepps/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.CciEePps.FindAsync(id)
        is CciEePp cciEepp ? Results.Ok(cciEepp) : Results.NotFound());

app.MapPost("/ccieepps/list", async (List<CciEePp> cciEepps, AppDbContext dbContext) =>
{
    await dbContext.CciEePps.AddRangeAsync(cciEepps);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/ccieepps/", cciEepps);
});

app.MapPost("/ccieepps", async (CciEePp cciEepp, AppDbContext dbContext) =>
{
    dbContext.CciEePps.Add(cciEepp);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/ccieepps/{cciEepp.CciEePpId}", cciEepp);
});

app.MapPost("/ccieeppslist", async (ICollection<CciEePp> cciEepps, AppDbContext dbContext) =>
{
    dbContext.CciEePps.AddRange(cciEepps);
    await dbContext.SaveChangesAsync();

    return Results.Ok(cciEepps);
});

app.MapPut("/ccieepps/{id}", async (int id, CciEePp cciEepp, AppDbContext dbContext) =>
{
    dbContext.CciEePps.Update(cciEepp);

    await dbContext.SaveChangesAsync();

    return Results.Ok(cciEepp);
});

app.MapDelete("/ccieepps/{id}", async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.CciEePps.FindAsync(id) is CciEePp cciEepp)
    {
        dbContext.CciEePps.Remove(cciEepp);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});*/

#endregion CciEePps

#region ModelElement

/*app.MapGet("/modelelements", async (AppDbContext dbContext) =>
    await dbContext.ModelElements.ToListAsync());

app.MapGet("/modelelements/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.ModelElements.FindAsync(id)
        is ModelElement modelElement ? Results.Ok(modelElement) : Results.NotFound());

app.MapPost("/modelelements", async (ModelElement modelElement, AppDbContext dbContext) =>
{
    dbContext.ModelElements.Add(modelElement);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/modelelements/{modelElement.ModelElementId}", modelElement);
});

app.MapPut("/modelelements/{id}", async (int id, ModelElement modelElement, AppDbContext dbContext) =>
{
    dbContext.ModelElements.Update(modelElement);

    await dbContext.SaveChangesAsync();

    return Results.Ok(modelElement);
});

app.MapDelete("/modelelements/{id}", async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.ModelElements.FindAsync(id) is ModelElement modelElement)
    {
        dbContext.ModelElements.Remove(modelElement);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});*/

#endregion ModelElement

#region WorkPackage

/*app.MapGet("/workpackages", async (AppDbContext dbContext) =>
    await dbContext.WorkPackages
    .ToListAsync());*/

app.MapGet("/workpackages/byproject/", async (int id, AppDbContext dbContext) =>
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


/*app.MapGet("/workpackages/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.WorkPackages.FindAsync(id)
        is WorkPackage workPackage ? Results.Ok(workPackage) : Results.NotFound());*/

app.MapPost("/workpackages", async (WorkPackageDto dto, AppDbContext dbContext) =>
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

app.MapPut("/workpackages/{id}", async (int id, WorkPackage workPackage, AppDbContext dbContext) =>
{
    dbContext.WorkPackages.Update(workPackage);

    await dbContext.SaveChangesAsync();

    return Results.Ok(workPackage);
});

app.MapDelete("/workpackages/{id}", async (int id, AppDbContext dbContext) =>
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

app.Run();
