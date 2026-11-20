using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService;
using RepairIT.CatalogService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogDatabase"))
);

builder.Services.AddScoped<IComputerService, ComputerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();

    context.Database.Migrate();

    if (!context.Computers.Any())
    {
        context.Computers.AddRange(CatalogSeed.Computers());
        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
