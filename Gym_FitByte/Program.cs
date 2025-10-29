using Gym_FitByte.Data;              // Contexto (AppDbContext)
using Microsoft.EntityFrameworkCore; // Entity Framework Core

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// 🔹 1️⃣ Configurar conexión con MySQL (desde appsettings.json)
// ------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 39)))
);

// ------------------------------------------------------
// 🔹 2️⃣ Configurar CORS (permite acceso desde PWA y app móvil)
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
// 🔹 3️⃣ Agregar controladores y Swagger
// ------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------------------------------------
// 🔹 4️⃣ Construir la app
// ------------------------------------------------------
var app = builder.Build();

// ------------------------------------------------------
// 🔹 5️⃣ Configurar el pipeline HTTP
// ------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ Activar CORS antes de los controladores
app.UseCors("NuevaPolitica");

app.UseAuthorization();

app.MapControllers();

app.Run();
