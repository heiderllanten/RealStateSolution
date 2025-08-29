using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Interfaces;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IPropertyImageService _propertyImageService;
        private readonly ILogger<PropertyImagesController> _logger;

        public PropertyImagesController(IPropertyImageService propertyImageService, ILogger<PropertyImagesController> logger)
        {
            _propertyImageService = propertyImageService;
            _logger = logger;
        }

        /// <summary>
        /// Agrega una imagen a una propiedad
        /// </summary>
        [HttpPost("{propertyId}")]
        public async Task<IActionResult> AddImage(Guid propertyId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("AddImage: Archivo inválido recibido para propiedad {PropertyId}.", propertyId);
                    return BadRequest("Debe subir un archivo válido.");
                }

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "properties");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativeUrl = $"/images/properties/{fileName}";

                var result = await _propertyImageService.AddAsync(propertyId, relativeUrl);
                if (result == null)
                {
                    _logger.LogWarning("AddImage: Propiedad no encontrada con ID {PropertyId}.", propertyId);
                    return NotFound($"Property {propertyId} not found.");
                }

                _logger.LogInformation("AddImage: Imagen agregada a propiedad {PropertyId}, URL: {Url}", propertyId, relativeUrl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar imagen a propiedad {PropertyId}.", propertyId);
                return StatusCode(500, "Ocurrió un error al agregar la imagen.");
            }
        }

        /// <summary>
        /// Actualiza la URL de una imagen
        /// </summary>
        [HttpPut("{imageId}")]
        public async Task<IActionResult> UpdateImage(Guid imageId, [FromForm] string newUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newUrl))
                {
                    _logger.LogWarning("UpdateImage: URL inválida recibida para imagen {ImageId}.", imageId);
                    return BadRequest("La nueva URL no puede estar vacía.");
                }

                var result = await _propertyImageService.UpdateUrlAsync(imageId, newUrl);
                if (result == null)
                {
                    _logger.LogWarning("UpdateImage: Imagen no encontrada con ID {ImageId}.", imageId);
                    return NotFound($"Image {imageId} not found.");
                }

                _logger.LogInformation("UpdateImage: URL actualizada para imagen {ImageId} a {NewUrl}.", imageId, newUrl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la imagen {ImageId}.", imageId);
                return StatusCode(500, "Ocurrió un error al actualizar la imagen.");
            }
        }

        /// <summary>
        /// Elimina una imagen
        /// </summary>
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            try
            {
                // 1️⃣ Obtener la imagen desde la DB
                var image = await _propertyImageService.GetByIdAsync(imageId);
                if (image == null)
                {
                    _logger.LogWarning("DeleteImage: Imagen no encontrada con ID {ImageId}.", imageId);
                    return NotFound($"Image {imageId} not found.");
                }

                // 2️⃣ Eliminar el archivo físico
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.Url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    _logger.LogInformation("DeleteImage: Archivo físico eliminado {FilePath}.", filePath);
                }

                // 3️⃣ Eliminar registro en DB
                var deleted = await _propertyImageService.RemoveAsync(imageId);

                if (!deleted)
                {
                    _logger.LogWarning("DeleteImage: Error al eliminar la imagen de la DB {ImageId}.", imageId);
                    return StatusCode(500, "No se pudo eliminar la imagen de la base de datos.");
                }

                _logger.LogInformation("DeleteImage: Imagen eliminada con ID {ImageId}.", imageId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la imagen {ImageId}.", imageId);
                return StatusCode(500, "Ocurrió un error al eliminar la imagen.");
            }
        }


        /// <summary>
        /// Obtiene todas las imágenes de una propiedad
        /// </summary>
        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetByProperty(Guid propertyId)
        {
            try
            {
                var result = await _propertyImageService.GetByPropertyAsync(propertyId);
                _logger.LogInformation("GetByProperty: Se obtuvieron {Count} imágenes para propiedad {PropertyId}.", result.Count(), propertyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener imágenes para propiedad {PropertyId}.", propertyId);
                return StatusCode(500, "Ocurrió un error al obtener las imágenes.");
            }
        }

        /// <summary>
        /// Obtiene una imagen por ID
        /// </summary>
        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetById(Guid imageId)
        {
            try
            {
                var result = await _propertyImageService.GetByIdAsync(imageId);
                if (result == null)
                {
                    _logger.LogWarning("GetById: Imagen no encontrada con ID {ImageId}.", imageId);
                    return NotFound();
                }

                _logger.LogInformation("GetById: Imagen obtenida con ID {ImageId}.", imageId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la imagen {ImageId}.", imageId);
                return StatusCode(500, "Ocurrió un error al obtener la imagen.");
            }
        }
    }
}
