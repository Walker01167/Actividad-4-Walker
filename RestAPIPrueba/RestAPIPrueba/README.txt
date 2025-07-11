Para ejecutar este código se necesita lo siguiente:

- Tener instalado Visual Studio 2022 con el componente web y la versión .NET 8 instalada.
- Instalar Microsoft SQL Server 2022 en su computadora local.
- Ejecutar el script llamado "SCRIPT_DATABASE.sql", para crear la base de datos y la tabla.
- Abrir el proyecto Visual Studio 2022.
- Compilar el proyecto y comprobar que se está compilando sin ningún problema en el "Error List".

/* IMPORTANTE */

Se debe de modificar los archivos:
- appsettings.json -> Server: colocar su servidor SQL Server.
- Models/ActividadUnidad4Context.cs -> Modificar la linea 24; Server: colocar su servidor SQL Server.

- Compilar nuevamente el proyecto.
- Ejecutar el proyecto. 
- Probar todos los Endpoints.

/* Para ACTUALIZAR EL MODELO AL CODIGO SEGUIR LOS SIGUIENTES PASOS */
- En el Visual Studio 2022, abrir el Package Manager Console ubicado en el Tools -> NuGet Package Manager -> Package Manager Console.
- Debe sustituir el Server (colocar su servidor SQL Server) en el codigo de abajo.
- Scaffold-DbContext "Server=LOCALHOST;Database=ActividadUnidad4;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force 