using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly ILogger<OwnersController> _logger;

        public OwnersController(IOwnerService ownerService, ILogger<OwnersController> logger)
        {
            _ownerService = ownerService;
            _logger = logger;
        }

        /// <summary>
        /// Crea un nuevo dueño
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OwnerModel model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Create: Se recibió un modelo nulo.");
                    return BadRequest("El modelo no puede ser nulo.");
                }

                var result = await _ownerService.CreateAsync(model);

                _logger.LogInformation("Create: Dueño creado con ID {OwnerId}", result.Id);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el dueño.");
                return StatusCode(500, "Ocurrió un error al crear el dueño.");
            }
        }

        /// <summary>
        /// Obtiene todos los dueños
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    _logger.LogWarning("GetAll: Parámetros de paginación inválidos. page={Page}, pageSize={PageSize}", page, pageSize);
                    return BadRequest("Parámetros de paginación inválidos.");
                }

                var result = await _ownerService.GetAllAsync(page, pageSize);

                _logger.LogInformation("GetAll: Se obtuvieron {Count} dueños.", result.TotalCount);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los dueños.");
                return StatusCode(500, "Ocurrió un error al obtener los dueños.");
            }
        }

        /// <summary>
        /// Obtiene un dueño por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _ownerService.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("GetById: Dueño no encontrado con ID {OwnerId}", id);
                    return NotFound();
                }

                _logger.LogInformation("GetById: Dueño obtenido con ID {OwnerId}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el dueño con ID {OwnerId}", id);
                return StatusCode(500, "Ocurrió un error al obtener el dueño.");
            }
        }
    }
}
