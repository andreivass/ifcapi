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

app.Run();
