### 0. Setup database
- Install DB management system (if you haven't) - recommend SQL Server Express
- Run ```..\DatabaseSetup.sql```

### 1. Insall dependencies

    ```bash
    dotnet add package Microsoft.Data.SqlClient
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

### 2. Install EF tool

    ```bash
    dotnet tool install --global dotnet-ef
    ```

### 3. Scaffold database

- For SQL Server Express:
    ```bash
    dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=MyStore;Trusted_Connection=true;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models
    ```

- For LocalDB:
    ```bash
    dotnet ef dbcontext scaffold "Server=(localdb)\MSSQLLocalDB;Database=MyStore;Trusted_Connection=true;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models
    ```

- For Docker SQL Server:
    ```bash
    dotnet ef dbcontext scaffold "Server=localhost,1433;Database=MyStore;User Id=sa;Password=<YourPassword>;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models
    ```

### 4. Update namespace if needed

### 5. Run Main
