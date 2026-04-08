# Sistema de Gestión de Productos

Este proyecto es una solución integral para gestionar órdenes de productos, desde su creación hasta el proceso de surtido en almacén. La arquitectura se divide en un backend robusto con .NET y un frontend reactivo con React + Vite.

## Guía de Inicio Rápido: Backend (ApiGestionProductos)

Para poner en marcha el backend por primera vez, sigue estos pasos:

### 1. Requisitos Previos
Asegúrate de tener instalado:
- [.NET 9 SDK](https://dotnet.microsoft.com/download) (o la versión actual del proyecto)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) o LocalDB (instalado con Visual Studio)
- Herramientas de Entity Framework Core

### 2. Instalación de Herramientas EF
Si aún no tienes las herramientas globales de Entity Framework, instálalas con el siguiente comando:
```bash
dotnet tool install --global dotnet-ef
```

### 3. Configuración de la Base de Datos
1. Dirígete al archivo `ApiGestionProductos/ApiGestionProductos/WebApi/appsettings.json`.
2. Verifica o actualiza la cadena de conexión `DefaultConnection` para que apunte a tu instancia local de SQL Server.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SistemaGestionProductosDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 4. Ejecución de Migraciones
Para crear la base de datos y aplicar el esquema inicial, abre una terminal en la carpeta principal del backend (`ApiGestionProductos/ApiGestionProductos`) y ejecuta:

> [!IMPORTANT]
> Es crucial ejecutar los comandos desde el directorio raíz de la solución donde se encuentran las carpetas `Data` y `WebApi`.

#### Aplicar migraciones existentes:
```bash
dotnet ef database update --project Data --startup-project WebApi
```

#### ¿Cómo crear una nueva migración? (Opcional)
Si realizas cambios en los modelos del dominio:
```bash
dotnet ef migrations add NombreDeLaMigracion --project Data --startup-project WebApi
```

---

## Guía de Inicio: Frontend (gestion-ordenes-reactApp)

### 1. Requisitos Previos
Asegúrate de tener instalado:
- [Node.js](https://nodejs.org/) (versión 16 o superior)
- [pnpm](https://pnpm.io/) (gestor de paquetes)

### 2. Instalación
Navega a la carpeta del frontend y ejecuta:
```bash
pnpm install
```

### 3. Configuración de Variables de Entorno
Antes de ejecutar la aplicación, es necesario configurar las variables de entorno para que el frontend pueda comunicarse con la API:

1. Crea un archivo llamado `.env` en la raíz de la carpeta `gestion-ordenes-reactApp` (puedes duplicar el archivo `.env.example` y renombrarlo).
2. Asegúrate de que `VITE_API_GESTION_ORDENES` apunte a la URL de tu API de .NET (por defecto suele ser `https://localhost:7008/api/v1`).

### 4. Ejecución
Para iniciar el servidor de desarrollo:
```bash
pnpm run dev
```

### Inicio de sesión

Rol Ventas

Usuario: sales1
Contraseña: 1234

Rol Jefe de Almacén

Usuario: admin1
Contraseña: 1234


## Tecnologías Utilizadas
- **Backend**: .NET 9, EF Core, Clean Architecture.
- **Frontend**: React 19, Vite, TypeScript.
- **Base de Datos**: SQL Server.
