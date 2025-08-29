using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Models;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly ILogger<PropertiesController> _logger;

        public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
        {
            _propertyService = propertyService;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva propiedad
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyModel model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Create: Modelo nulo recibido.");
                    return BadRequest("El modelo no puede ser nulo.");
                }

                var result = await _propertyService.CreateAsync(model);

                _logger.LogInformation("Create: Propiedad creada con ID {PropertyId}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la propiedad.");
                return StatusCode(500, "Ocurrió un error al crear la propiedad.");
            }
        }

        /// <summary>
        /// Actualiza una propiedad
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PropertyModel model)
        {
            try
            {
                if (model == null)
                {
                    _logger.LogWarning("Update: Modelo nulo recibido para la propiedad {PropertyId}.", id);
                    return BadRequest("El modelo no puede ser nulo.");
                }

                var result = await _propertyService.UpdateAsync(id, model);
                if (result == null)
                {
                    _logger.LogWarning("Update: Propiedad no encontrada con ID {PropertyId}.", id);
                    return NotFound();
                }

                _logger.LogInformation("Update: Propiedad actualizada con ID {PropertyId}.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la propiedad con ID {PropertyId}.", id);
                return StatusCode(500, "Ocurrió un error al actualizar la propiedad.");
            }
        }

        /// <summary>
        /// Cambia el precio de una propiedad
        /// </summary>
        [HttpPatch("{id}/price")]
        public async Task<IActionResult> ChangePrice(Guid id, [FromBody] double newPrice)
        {
            try
            {
                if (newPrice < 0)
                {
                    _logger.LogWarning("ChangePrice: Precio inválido {NewPrice} para propiedad {PropertyId}.", newPrice, id);
                    return BadRequest("El precio no puede ser negativo.");
                }

                var result = await _propertyService.ChangePriceAsync(id, newPrice);
                if (result == null)
                {
                    _logger.LogWarning("ChangePrice: Propiedad no encontrada con ID {PropertyId}.", id);
                    return NotFound();
                }

                _logger.LogInformation("ChangePrice: Precio actualizado para la propiedad {PropertyId} a {NewPrice}.", id, newPrice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el precio de la propiedad con ID {PropertyId}.", id);
                return StatusCode(500, "Ocurrió un error al cambiar el precio de la propiedad.");
            }
        }

        /// <summary>
        /// Obtiene todas las propiedades (con filtros opcionales)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? address,
                                               [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0)
                {
                    _logger.LogWarning("GetAll: Rangos de precio inválidos. minPrice={MinPrice}, maxPrice={MaxPrice}", minPrice, maxPrice);
                    return BadRequest("Los precios no pueden ser negativos.");
                }

                var result = await _propertyService.GetAllAsync(name, address, minPrice, maxPrice);
                _logger.LogInformation("GetAll: Se obtuvieron {Count} propiedades.", result.TotalCount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las propiedades.");
                return StatusCode(500, "Ocurrió un error al obtener las propiedades.");
            }
        }

        /// <summary>
        /// Obtiene una propiedad por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _propertyService.GetByIdAsync(id);
                if (result == null)
                {
                    _logger.LogWarning("GetById: Propiedad no encontrada con ID {PropertyId}.", id);
                    return NotFound();
                }

                _logger.LogInformation("GetById: Propiedad obtenida con ID {PropertyId}.", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la propiedad con ID {PropertyId}.", id);
                return StatusCode(500, "Ocurrió un error al obtener la propiedad.");
            }
        }
    }
}
