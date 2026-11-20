using Microsoft.EntityFrameworkCore;
using RepairIT.UserService;
using RepairIT.UserService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("UserDatabase"))
);

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserContext>();

    context.Database.Migrate();

    if (!context.Users.Any())
    {
        context.Users.AddRange(UserSeed.Users());
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
