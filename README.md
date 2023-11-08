# 🚀 **XoomCoreWeb Documentation**

## 1. Introduction

### 1.1 Project Overview
XoomCore is a web application developed using Entity Framework Core Code First Approach. It follows the principles of a layered architecture, specifically the Onion Architecture pattern.

### 1.2 Technologies Used
The XoomCore project utilizes the following technologies and frameworks:
- .NET 6 
- Entity Framework Core
- Microsoft SQL Server

## 2. Project Structure

### 2.1 Solution Structure
The XoomCore project is structured as follows within the solution:
1. XoomCore.Core
2. XoomCore.Infrastructure
3. XoomCore.Application
4. XoomCore.Services
5. XoomCore.Web

### 2.2 Layered Architecture
The project follows the Onion Architecture pattern, which promotes a clear separation of concerns and adheres to the dependency inversion principle. It consists of the following layers:
1. Core Layer: Contains core data entities for repositories.
2. Infrastructure Layer: Handles data access, including the database context and advanced features like caching and logging.
3. Application Layer: Focuses on application-specific business logic, services, and ViewModels for the user interface.
4. Services Layer: Contains middleware, session management, and mapping services.
5. Web Layer: Serves as the entry point, managing HTTP requests and responses through controllers.

### 2.3 Description of Each Layer


1. `XoomCore.Core/`: Contains core domain entities, enumerations, shared models, and startup configuration.
    - `Entities/`: Core domain entity classes.
    - `Enum/`: Enumerations for various entity properties.
    - `Shared/`: Shared models for common data structures.
    - `Startup.cs`: Configuration and setup for the core layer.

2. `XoomCore.Infrastructure/`: Handles infrastructure concerns, including caching, helpers, logging, database migrations, persistence, repositories, unit of work, and setup.
    - `Caching/`: Implementations related to caching.
    - `Helpers/`: Utility classes to assist with various tasks.
    - `Logging/`: Logging infrastructure for tracking application behavior.
    - `Migrations/`: Database migration scripts.
    - `Persistence/`: Database context and settings.
    - `Repositories/`: Data repositories for data access.
    - `UnitOfWorks/`: Implementation of the unit of work pattern.
    - `Startup.cs`: Configuration and setup for the infrastructure layer.

3. `XoomCore.Application/`: Manages view models for shaping data, request models for input validation, response models for API data, application-specific business logic, and configuration.
    - `ViewModels/`: View model classes used to shape data for presentation.
    - `RequestModels/`: Request models for input validation and data transfer.
    - `ResponseModels/`: Response models representing data returned from the API.
    - `BusinessLogic/`: Contains application-specific business logic.
    - `Startup.cs`: Configuration and setup for the application layer.

4. `XoomCore.Services/Concretes/`: Implements service interfaces, service concretes, AutoMapper profiles, middleware, session management, and setup.
    - `Contracts/`: Service interfaces defining the contract for service classes.
    - `Concretes/`: Service implementations that fulfill the service contracts.
    - `Mapper/`: Contains AutoMapper profiles for object mapping.
    - `Middleware/`: Middleware components for application processing.
    - `SessionControl/`: Session management services.
    - `Startup.cs`: Configuration and setup for the services layer.

5. `XoomCore.Web/`: Manages access control, authentication, configurations, controllers for handling HTTP requests, and serves as the application's entry point.
    - `AccessControl/`: Module handling access control logic, including controllers and views.
    - `Authentication/`: Module responsible for authentication, including controllers and views.
    - `Configurations/`: Configuration settings, including database configuration.
    - `Controllers/`: Controllers for handling HTTP requests and serving views.
    - `Program.cs`: Application entry point and setup.

## 3. Installation

### 3.1 Prerequisites
Before you get started with the XoomCore project template, make sure you have the following prerequisites installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) (or a later version)
- Microsoft SQL Server (or another compatible database)
- [Visual Studio](https://visualstudio.microsoft.com/) (or Visual Studio Code) with C# support

### 3.2 Install the XoomCore Template

1. Visit the [Visual Studio Marketplace page for the XoomCoreWeb template](https://marketplace.visualstudio.com/items?itemName=chotonmishaque.XoomCoreWeb).
2. Click the "Download" button to obtain the template in the form of a VSIX file.
3. Once the download is complete, locate the downloaded VSIX file on your computer.
4. Double-click the downloaded VSIX file. This action will open Visual Studio and prompt you to install the template.
5. Follow the installation wizard's instructions to complete the installation.
6. It's advisable to restart Visual Studio after the installation to ensure that the template is loaded.

### Step 3.3 Create a New Project

1. Open your chosen development environment, such as Visual Studio or Visual Studio Code.
2. Create a new project and select "XoomCoreWeb" from the list of available project templates.
3. Follow the project creation wizard's instructions to set up your project.
4. After the project is created, you can start customizing it to meet your specific requirements. This may involve tasks like configuring the database connection string, adding controllers, views, and more.

## 4. Configuration

### 4.1 Database Connection

The XoomCore application relies on Entity Framework Core Code First Approach to interact with the database. To configure the database connection, follow these steps:

1. Open the `Configurations/database.json` file in your project.
2. Locate the `"ConnectionString"` setting and replace it with your own database connection string.

    Example:
    ```json
    "DatabaseSettings": {
        "DBProvider": "mssql",
        "ConnectionString": "Server=ServerName;Integrated Security=SSPI;Database=XoomCoreDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;User Id=your_user_id;password=your_password"
    }
    ```

### 4.2 Applying Migrations

To create and update your database using Entity Framework migrations, follow these steps:
1. Open the Package Manager Console in Visual Studio via `View > Other Windows > Package Manager Console`.
2. Ensure that the `XoomCore.Infrastructure` project is selected in the Package Manager Console. If not, select it.
3. Execute the following command to apply migrations and update the database:

    ```bash
    update-database
    ```

You're all set! You now have a new project based on the XoomCore template, and you're ready to start building your application. Happy coding!

### 4.3 USER CREDENTIALS

Admin:
```json
    {
        "Email":"admin@gmail.com",
        "Password":"Abcd1234."
    }
```

User:
```json
    {
        "Email":"user@gmail.com",
        "Password":"Abcd1234."
    }
```
## 5. Development Guidelines

### 5.1 Coding Standards
Follow the C# coding standards and guidelines outlined by Microsoft to maintain consistent and readable code.

### 5.2 Naming Conventions
Adhere to the following naming conventions:
- Class names should be in PascalCase.
- Interface names should begin with "I" and use PascalCase.
- Method names should be in PascalCase.
- Property names should be in PascalCase.

### 5.3 Logging
Utilize Serilog for logging throughout the application to track important events and errors.

## 6. Features

### 6.1 User Module
The XoomCore application provides a User Module for managing users, including action permissions.

### 6.2 Generic Repository Pattern
The application uses the generic repository pattern to abstract data access and provide a consistent interface for data operations.

### 6.3 Common Response Model
API responses follow a common response model (`CommonResponse<T>`) to provide a unified structure for success and error responses.

### 6.4 Request and Response Models
Request and response models are used to validate and transfer data between the client and server.

### 6.5 Audit Log for Data Changes
The application includes an `EntityLogEntity` to record data changes in the database. It is implemented in the `SaveChangesAsync` method of the `ApplicationDbContext`.

## 7. Conclusion

### 7.1 Summary

The XoomCore project is a .NET Core web application that follows best practices and the Onion Architecture pattern. It include the User Module, which enables comprehensive user management with action permissions, and the use of the generic repository pattern, ensuring efficient and consistent data access. The application adopts a common response model (CommonResponse<T>) for API interactions and employs request and response models for secure data transfer. Additionally, an audit log mechanism tracks data changes in the database, further enhancing data integrity and accountability.

### 7.2 UI Template

We'd also like to acknowledge the use of the [Sneat Bootstrap HTML Admin Template for the project's UI design](https://github.com/themeselection/sneat-bootstrap-html-admin-template-free)

### 7.3 License

This project is licensed under the [MIT License](https://github.com/chotonmishaque/XoomCoreWeb/blob/master/LICENSE.txt).

### Creators

<a href="https://github.com/chotonmishaque">
  <img src="https://avatars.githubusercontent.com/u/43789467" alt="Creator 2" width="50" style="border-radius: '50%';">
</a>

### Social Media

You can find us on social media:

- [Twitter](https://twitter.com/chotonmishaque)
- [LinkedIn](https://www.linkedin.com/in/chotonmishaque/)
- [Facebook](https://www.facebook.com/chotonmishaque/)

Feel free to connect with us and stay updated on our projects and activities.
