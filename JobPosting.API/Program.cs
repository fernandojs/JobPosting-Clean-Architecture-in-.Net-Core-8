using JobPosting.Domain.Interfaces;
using JobPosting.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddInfrastructureJWT(builder.Configuration);
builder.Services.AddInfrastructureSwagger();

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var seedJobPosting = scope.ServiceProvider.GetService<IJobPostSeedData>();    
    seedJobPosting.Initialize(services);

    var seedUser = scope.ServiceProvider.GetService<IUserSeedData>();
    seedUser.Initialize(services);
}

// Redirect to swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/") && context.Request.Method == "GET")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
