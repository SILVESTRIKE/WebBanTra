# WebBanTra - Tea E-Commerce Platform

A WebBanTra tea shop e-commerce web application built on the ASP.NET MVC 5 framework using C# and Microsoft SQL Server. The application provides an interactive customer storefront for viewing, filtering, and purchasing tea products, alongside a secure administrative portal for inventory and order management.

---

## Technical Features

- Framework: ASP.NET MVC 5 (C# Object-Oriented Programming, Razor View Engine, .NET Framework).
- Database: Microsoft SQL Server. Mapped using the SQL script file (DB_BanTra.sql).
- Server Configuration: ASP.NET routing and database connection pooling managed through Web.config.
- Packages: NuGet package references defined in packages.config.

---

## Project Structure

```
WebBanTra/
├── DB_BanTra.sql          # SQL Server database schema and seed data
└── WebBanTra/
    ├── App_Start/         # Routing, bundle, and area registrations
    ├── Areas/             # Administrative modules (Admin area controllers/views)
    ├── Content/           # CSS stylesheets and theme assets
    ├── Controllers/       # Main storefront MVC controllers
    ├── Models/            # Database contexts and data entities
    ├── Views/             # Razor views for storefront layout and pages
    ├── Scripts/           # JavaScript assets and helper libraries
    ├── Web.config         # Server and SQL connection configurations
    ├── WebBanTra.csproj   # Visual Studio project file
    └── WebBanTra.sln      # Visual Studio solution file
```

---

## Storefront & Administrative Features

### Storefront (Customer Flow)
- Product Catalog: Categorized listing of various tea products.
- Advanced Search & Filtering: Real-time search and category-based navigation.
- Shopping Cart: Interactive cart supporting adding items, updating quantities, calculating subtotals, and removing items.
- Checkout: Order submission forms linking customer details with cart contents.

### Administrative Portal (Admin Area)
- Product Management: CRUD operations for products, prices, images, and descriptions.
- Order Audit: Real-time view of customer orders and processing statuses.
- Category Management: Dynamic addition and organization of tea categories.

---

## Setup & Running

### Prerequisites
- Visual Studio (with .NET Framework development workload)
- Microsoft SQL Server and SQL Server Management Studio (SSMS)
- Internet connection for NuGet package restoration

### 1. Database Setup
1. Open SQL Server Management Studio (SSMS) and connect to your database instance.
2. Create a new database named `DB_BanTra` (or a name of your choice).
3. Open and run the `DB_BanTra.sql` script to restore the database schemas, tables, and seed records.

### 2. Connection String Configuration
1. Open the project solution file (`WebBanTra.sln`) in Visual Studio.
2. Open `Web.config` in the root of the project.
3. Locate the `<connectionStrings>` tag and update the connection string to match your SQL Server instance credentials:
   ```xml
   <connectionStrings>
     <add name="DB_BanTraEntities" 
          connectionString="metadata=res://*/Models.DB_Model.csdl...data source=YOUR_SERVER_NAME;initial catalog=DB_BanTra;integrated security=True;" 
          providerName="System.Data.EntityClient" />
   </connectionStrings>
   ```

### 3. Execution
1. Restore NuGet packages for the solution: Right-click the solution in Visual Studio and select **Restore NuGet Packages**.
2. Press **Start** (or `F5`) in Visual Studio to build and run the application using IIS Express.
3. The storefront will open in your default browser. Admin pages can be accessed via the `/Admin` route suffix.