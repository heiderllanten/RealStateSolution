using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTracesController : ControllerBase
    {
        private readonly IPropertyTraceService _propertyTraceService;
        private readonly ILogger<PropertyTracesController> _logger;

        public PropertyTracesController(IPropertyTraceService propertyTraceService, ILogger<PropertyTracesController> logger)
        {
            _propertyTraceService = propertyTraceService;
            _logger = logger;
        }

        /// <summary>
        /// Crea un registro de trazabilidad (venta o cambio relevante) de una propiedad
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddTrace([FromBody] PropertyTraceModel model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("AddTrace: Modelo nulo recibido.");
                    return BadRequest("El modelo no puede ser nulo.");
                }

                var result = await _propertyTraceService.AddAsync(model);
                if (result == null)
                {
                    _logger.LogWarning("AddTrace: Propiedad no encontrada con ID {PropertyId}.", model.PropertyId);
                    return NotFound($"Property {model.PropertyId} not found.");
                }

                _logger.LogInformation("AddTrace: Registro de trazabilidad creado para propiedad {PropertyId}, TraceId: {TraceId}.",
                    model.PropertyId, result.Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar registro de trazabilidad para propiedad {PropertyId}.", model?.PropertyId);
                return StatusCode(500, "Ocurrió un error al agregar el registro de trazabilidad.");
            }
        }

        /// <summary>
        /// Obtiene todos los registros de trazabilidad de una propiedad
        /// </summary>
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetByProperty(Guid propertyId)
        {
            try
            {
                var result = await _propertyTraceService.GetByPropertyAsync(propertyId);
                _logger.LogInformation("GetByProperty: Se obtuvieron {Count} registros de trazabilidad para propiedad {PropertyId}.",
                    result.Count(), propertyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros de trazabilidad para propiedad {PropertyId}.", propertyId);
                return StatusCode(500, "Ocurrió un error al obtener los registros de trazabilidad.");
            }
        }

        /// <summary>
        /// Obtiene un registro de trazabilidad por ID
        /// </summary>
        [HttpGet("{traceId}")]
        public async Task<IActionResult> GetById(Guid traceId)
        {
            try
            {
                var result = await _propertyTraceService.GetByIdAsync(traceId);
                if (result == null)
                {
                    _logger.LogWarning("GetById: Registro de trazabilidad no encontrado con ID {TraceId}.", traceId);
                    return NotFound();
                }

                _logger.LogInformation("GetById: Registro de trazabilidad obtenido con ID {TraceId}.", traceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registro de trazabilidad con ID {TraceId}.", traceId);
                return StatusCode(500, "Ocurrió un error al obtener el registro de trazabilidad.");
            }
        }
    }
}
