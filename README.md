# RealEstate API

API para la gestión de propiedades, dueños y trazabilidad en un sistema de bienes raíces. Desarrollada en **ASP.NET Core 7** con arquitectura de capas y pruebas unitarias usando **NUnit** y **Moq**.

---

## 📦 Estructura del proyecto

- **RealEstate.Api** – Contiene los controladores y la configuración de la API.
- **RealEstate.Application** – Servicios, interfaces y modelos.
- **RealEstate.Infrastructure** – Entidades, Persistencia y acceso a datos.
- **RealEstate.Api.Tests** – Pruebas unitarias para los controladores.

---

## 🚀 Endpoints principales

### Owners
- `POST /api/owners` – Crear un dueño
- `GET /api/owners` – Obtener todos los dueños (con paginación)
- `GET /api/owners/{id}` – Obtener un dueño por ID

### Properties
- `POST /api/properties` – Crear propiedad
- `PUT /api/properties/{id}` – Actualizar propiedad
- `PATCH /api/properties/{id}/price` – Cambiar precio
- `GET /api/properties` – Obtener todas las propiedades (con filtros opcionales)
- `GET /api/properties/{id}` – Obtener propiedad por ID

### PropertyImages
- `POST /api/propertyimages/{propertyId}` – Subir imagen a propiedad
- `DELETE /api/propertyimages/{imageId}` – Eliminar imagen
- `GET /api/propertyimages/property/{propertyId}` – Obtener imágenes de propiedad
- `GET /api/propertyimages/{imageId}` – Obtener imagen por ID

### PropertyTraces
- `POST /api/propertytraces` – Crear registro de trazabilidad
- `GET /api/propertytraces/property/{propertyId}` – Obtener trazabilidad por propiedad
- `GET /api/propertytraces/{traceId}` – Obtener trazabilidad por ID

---

## 🗄️ Base de datos

Los archivos de la base de datos se encuentran en la carpeta `bd`:

| Archivo        | Descripción                                | Tamaño |
|----------------|--------------------------------------------|--------|
| `data.sql`     | Script SQL para poblar la base de datos | 2 KB   |
| `RealEstateDB.bak` | Backup completo de la base de datos SQL Server | 7.968 KB |

> Nota: Puedes restaurar el `.bak` en SQL Server o ejecutar `data.sql` para crear la base de datos desde cero.

---

## 🛠️ Instalación y ejecución

1. Clonar el repositorio:

```bash
git clone https://github.com/tuusuario/RealEstateAPI.git
cd RealEstateAPI
