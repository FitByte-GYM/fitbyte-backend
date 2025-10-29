using Gym_FitByte.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// 🔹 1️⃣ Conexión con MySQL
// ------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 39)))
);

// ------------------------------------------------------
// 🔹 2️⃣ Configurar CORS
// ------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ------------------------------------------------------
// 🔹 3️⃣ Controladores y Swagger
// ------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------
// 🔹 4️⃣ Construir la app
// ------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------
// 🔹 5️⃣ Pipeline HTTP
// ------------------------------------------------------
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("NuevaPolitica");
app.UseAuthorization();
app.MapControllers();

app.Run();
