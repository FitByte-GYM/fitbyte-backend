using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym_FitByte.Data;
using Gym_FitByte.Models;

namespace Gym_FitByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AsistenciaController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // ?? Verificar membres�a y registrar asistencia
        // ============================
        [HttpPost("verificar")]
        public async Task<IActionResult> VerificarAsistencia([FromBody] AsistenciaRequest request)
        {
            var miembro = await _context.Membresias
                .FirstOrDefaultAsync(m => m.CodigoCliente == request.CodigoCliente);

            if (miembro == null)
                return NotFound(new { mensaje = "No se encontr� ninguna membres�a con ese c�digo." });

            // Validar vencimiento
            if (!miembro.Activa || miembro.FechaVencimiento < DateTime.Now)
            {
                miembro.Activa = false;
                _context.Membresias.Update(miembro);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    estado = "inactiva",
                    mensaje = "Membres�a vencida o inactiva.",
                    miembro.Nombre,
                    miembro.FotoUrl,
                    miembro.FechaVencimiento
                });
            }

            // Registrar asistencia
            var asistencia = new Asistencia
            {
                CodigoCliente = miembro.CodigoCliente,
                FechaHora = DateTime.Now
            };

            _context.Asistencias.Add(asistencia);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                estado = "activa",
                mensaje = "Acceso permitido. Bienvenido/a " + miembro.Nombre,
                miembro.Nombre,
                miembro.FotoUrl,
                miembro.FechaVencimiento
            });
        }

        // ============================
        // ?? Ver historial de asistencias
        // ============================
        [HttpGet("historial/{codigo}")]
        public async Task<IActionResult> ObtenerHistorial(string codigo)
        {
            var historial = await _context.Asistencias
                .Where(a => a.CodigoCliente == codigo)
                .OrderByDescending(a => a.FechaHora)
                .ToListAsync();

            if (!historial.Any())
                return NotFound("No hay asistencias registradas para este c�digo.");

            return Ok(historial);
        }
    }

    // DTO para la solicitud
    public class AsistenciaRequest
    {
        public string CodigoCliente { get; set; } = string.Empty;
    }
}
