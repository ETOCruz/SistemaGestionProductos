# SistemaGestionProductos
Desarrollar un sistema básico que permita gestionar órdenes de productos desde su creación hasta su proceso de surtido dentro de un almacén.

# ApiGestionProductos NET C#

## Migracion Entity Framework

Verificar que se tenga instalado las dependencias del proyecto WebApi:
- Microsoft.EntityFrameworkCore.Design 

Verificar que en se tengan instalados las dependencias en el proyecto Data:
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer


Comandos utilizados:

Instalación de dotnet-ef de manrea global, necesario para ejecutar comandos de Entity Framework:
```cmd
dotnet tool install --global dotnet-ef
```

Creación de migración inicial:

```cmd
dotnet ef migrations add InitialCreate --project Data --startup-project WebApi
```

Aplicar migraciones pendientes a la base de datos:
```cmd
dotnet ef database update --project Data --startup-project WebApi
```