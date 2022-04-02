using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

#region Projects

app.MapGet("/projects", async (AppDbContext dbContext) => 
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

app.MapGet("/ccieepps/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.CciEePps.FindAsync(id)
        is CciEePp cciEepp ? Results.Ok(cciEepp) : Results.NotFound());

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
});

#endregion CciEePps

#region IfcElements

app.MapGet("/ifcelements", async (AppDbContext dbContext) =>
    await dbContext.IfcElements.ToListAsync());

app.MapGet("/ifcelements/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.IfcElements.FindAsync(id)
        is IfcElement cciEepp ? Results.Ok(cciEepp) : Results.NotFound());

app.MapPost("/ifcelements", async (IfcElement ifcElement, AppDbContext dbContext) =>
{
    dbContext.IfcElements.Add(ifcElement);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/ifcelements/{ifcElement.IfcElementId}", ifcElement);
});

app.MapPost("/ifcelementslist", async (ICollection<IfcElement> ifcElement, AppDbContext dbContext) =>
{
    dbContext.IfcElements.AddRange(ifcElement);
    await dbContext.SaveChangesAsync();

    return Results.Ok(ifcElement);
});

app.MapPut("/ifcelements/{id}", async (int id, IfcElement ifcElement, AppDbContext dbContext) =>
{
    dbContext.IfcElements.Update(ifcElement);

    await dbContext.SaveChangesAsync();

    return Results.Ok(ifcElement);
});

app.MapDelete("/ifcelements/{id}", async (int id, AppDbContext dbContext) =>
{
    if (await dbContext.IfcElements.FindAsync(id) is IfcElement ifcElement)
    {
        dbContext.IfcElements.Remove(ifcElement);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }

    return Results.NotFound();
});

#endregion IfcElements

#region ModelElement

app.MapGet("/modelelements", async (AppDbContext dbContext) =>
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
});

#endregion ModelElement

#region WorkPackage

app.MapGet("/workpackages", async (AppDbContext dbContext) =>
    await dbContext.WorkPackages.ToListAsync());

app.MapGet("/workpackages/{id}", async (int id, AppDbContext dbContext) =>
    await dbContext.WorkPackages.FindAsync(id)
        is WorkPackage workPackage ? Results.Ok(workPackage) : Results.NotFound());

app.MapPost("/workpackages", async (WorkPackage workPackage, AppDbContext dbContext) =>
{
    dbContext.WorkPackages.Add(workPackage);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/workpackages/{workPackage.WorkPackageId}", workPackage);
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
