using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Gym_FitByte.Data;
using Gym_FitByte.Models;

namespace Gym_FitByte.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembresiasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private const string ContainerName = "fotos";

        public MembresiasController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // =========================
        // 🔹 Registrar membresía
        // =========================
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarMembresia([FromForm] CrearMembresiaDto dto)
        {
            if (dto.Foto == null || dto.Foto.Length == 0)
                return BadRequest("Debe subir una foto del cliente.");

            // Subir foto a Azure Blob
            var urlFoto = await SubirFotoABlob(dto.Foto);

            // Generar código único numérico
            string codigo = await GenerarCodigoUnicoAsync();

            var m = new Membresia
            {
                CodigoCliente = codigo,
                Nombre = dto.Nombre,
                Edad = dto.Edad,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Correo = dto.Correo,
                Rutina = dto.Rutina,
                EnfermedadesOLesiones = dto.EnfermedadesOLesiones,
                FotoUrl = urlFoto,
                FechaRegistro = dto.FechaRegistro,
                FechaVencimiento = dto.FechaVencimiento,
                FormaPago = dto.FormaPago,
                Tipo = dto.Tipo,
                MontoPagado = dto.MontoPagado,
                Activa = true
            };

            _context.Membresias.Add(m);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Membresía registrada correctamente.",
                m.Id,
                m.CodigoCliente,
                m.Nombre,
                m.Correo,
                m.FotoUrl
            });
        }

        // =========================
        // 🔹 Actualizar membresía
        // =========================
        [HttpPut("actualizar/{id:int}")]
        public async Task<IActionResult> ActualizarMembresia(int id, [FromForm] ActualizarMembresiaDto dto)
        {
            var m = await _context.Membresias.FindAsync(id);
            if (m == null) return NotFound("Membresía no encontrada.");

            if (dto.Nombre != null) m.Nombre = dto.Nombre;
            if (dto.Edad.HasValue) m.Edad = dto.Edad.Value;
            if (dto.Telefono != null) m.Telefono = dto.Telefono;
            if (dto.Direccion != null) m.Direccion = dto.Direccion;
            if (dto.Correo != null) m.Correo = dto.Correo;
            if (dto.Rutina != null) m.Rutina = dto.Rutina;
            if (dto.EnfermedadesOLesiones != null) m.EnfermedadesOLesiones = dto.EnfermedadesOLesiones;
            if (dto.FechaVencimiento.HasValue) m.FechaVencimiento = dto.FechaVencimiento.Value;

            // Nueva foto (opcional)
            if (dto.FotoNueva != null && dto.FotoNueva.Length > 0)
            {
                var urlNueva = await SubirFotoABlob(dto.FotoNueva);
                m.FotoUrl = urlNueva;
            }

            _context.Membresias.Update(m);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Membresía actualizada correctamente.", m.Id });
        }

        // =========================
        // 🔹 Renovar membresía
        // =========================
        [HttpPut("renovar/{id:int}")]
        public async Task<IActionResult> RenovarMembresia(int id, [FromBody] RenovarMembresiaDto dto)
        {
            var m = await _context.Membresias.FindAsync(id);
            if (m == null) return NotFound("Membresía no encontrada.");

            if (dto.NuevaFechaVencimiento <= m.FechaRegistro)
                return BadRequest("La nueva fecha de vencimiento debe ser posterior a la fecha actual.");

            m.FechaVencimiento = dto.NuevaFechaVencimiento;
            m.FormaPago = dto.TipoPago;
            m.MontoPagado = dto.MontoPagado;
            m.Activa = true;

            _context.Membresias.Update(m);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Membresía renovada correctamente.",
                m.CodigoCliente,
                m.FechaVencimiento,
                m.FormaPago,
                m.MontoPagado
            });
        }

        // =========================
        // 🔹 Buscar membresía por código
        // =========================
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> ObtenerPorCodigo(string codigo)
        {
            var m = await _context.Membresias
                .FirstOrDefaultAsync(x => x.CodigoCliente == codigo);

            if (m == null)
                return NotFound("No se encontró una membresía con ese código.");

            return Ok(m);
        }

        // =========================
        // 🔹 Listar todas las membresías
        // =========================
        [HttpGet]
        public async Task<IActionResult> ObtenerMembresias()
        {
            var list = await _context.Membresias
                .OrderByDescending(x => x.Id)
                .ToListAsync();
            return Ok(list);
        }

        // =========================
        // 🔹 Subir foto a Azure Blob
        // =========================
        private async Task<string> SubirFotoABlob(IFormFile archivo)
        {
            var connectionString = _config.GetConnectionString("AzureBlobStorage");
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();
            await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);

            var nombre = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            var blobClient = containerClient.GetBlobClient(nombre);

            using var stream = archivo.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        // =========================
        // 🔹 Generar código numérico único
        // =========================
        private async Task<string> GenerarCodigoUnicoAsync()
        {
            string codigo;
            Random random = new();

            do
            {
                codigo = random.Next(100000, 999999).ToString(); // Solo números
            }
            while (await _context.Membresias.AnyAsync(m => m.CodigoCliente == codigo));

            return codigo;
        }
    }

    // =========================
    // 🔹 DTOs
    // =========================

    public class CrearMembresiaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Rutina { get; set; } = string.Empty;
        public string EnfermedadesOLesiones { get; set; } = "Ninguna";
        public IFormFile Foto { get; set; } = default!;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaVencimiento { get; set; }
        public string FormaPago { get; set; } = "Efectivo";
        public string Tipo { get; set; } = "Inscripción";
        public decimal MontoPagado { get; set; }
    }

    public class ActualizarMembresiaDto
    {
        public string? Nombre { get; set; }
        public int? Edad { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
        public string? Rutina { get; set; }
        public string? EnfermedadesOLesiones { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public IFormFile? FotoNueva { get; set; }
    }

    public class RenovarMembresiaDto
    {
        public DateTime NuevaFechaVencimiento { get; set; }
        public string TipoPago { get; set; } = "Efectivo";
        public decimal MontoPagado { get; set; }
    }
}
