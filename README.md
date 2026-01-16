# GeoAsset Management System - .NET Web API

A robust RESTful API backend built with .NET 6+ and Entity Framework Core, providing secure authentication and comprehensive CRUD operations for geospatial asset management. This API serves as the backend for the Angular Map Application.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Installation & Setup](#installation--setup)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Project Structure](#project-structure)
- [Security Features](#security-features)
- [Testing with Swagger](#testing-with-swagger)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)

## Overview

This .NET Web API provides a secure, scalable backend service for managing geospatial assets. It implements JWT-based authentication, role-based access control, and follows the Repository pattern for clean, maintainable code. Each user can only access and manage their own location data, ensuring data privacy and security.

## Features

### Core Functionality
- **RESTful API Design**: Standard HTTP methods for CRUD operations
- **JWT Authentication**: Secure token-based authentication system
- **User Management**: Registration and login with ASP.NET Core Identity
- **Location CRUD**: Complete Create, Read, Update, Delete operations for locations
- **Search Functionality**: Search locations by name with filtering
- **User Isolation**: Each user can only access their own data
- **Data Validation**: Comprehensive input validation with Data Annotations
- **Error Handling**: Structured error responses

### Architecture
- **Repository Pattern**: Clean separation of concerns
- **Dependency Injection**: Built-in DI container for loose coupling
- **Entity Framework Core**: Code-first database approach
- **DTOs (Data Transfer Objects)**: Separation between domain models and API contracts
- **CORS Support**: Configured for Angular frontend integration

## Technology Stack

- **.NET**: 6.0 or higher
- **ASP.NET Core Web API**: RESTful API framework
- **Entity Framework Core**: ORM for database operations
- **ASP.NET Core Identity**: User authentication and authorization
- **SQL Server**: Relational database
- **JWT (JSON Web Tokens)**: Secure authentication
- **Swagger/OpenAPI**: API documentation and testing

## Prerequisites

Before you begin, ensure you have the following installed:

- **.NET SDK**: 6.0 or higher
  - Download from: https://dotnet.microsoft.com/download
  - Verify installation: `dotnet --version`

- **SQL Server**: Express, Developer, or Enterprise edition
  - Download SQL Server Express: https://www.microsoft.com/sql-server/sql-server-downloads
  - Or use LocalDB (included with Visual Studio)

- **Visual Studio 2022** (recommended) or **Visual Studio Code**
  - Visual Studio: https://visualstudio.microsoft.com/
  - VS Code: https://code.visualstudio.com/

- **SQL Server Management Studio (SSMS)** (optional, for database management)
  - Download: https://aka.ms/ssmsfullsetup

## Installation & Setup

### 1. Clone the Repository

```bash
git clone <[backend-repository-url](https://github.com/shahenda23/GeoAssetManagementSystemBackend.git)>
cd GeoAssetManagementSystem
```

### 2. Restore NuGet Packages

```bash
dotnet restore
```

### 3. Update Database Connection String

Open `appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "cs": "Data Source=YOUR_SERVER_NAME;Initial Catalog=GeoAssetManagementSystem;Integrated Security=True;Encrypt=False"
  }
}
```

**Common connection string formats:**

**For SQL Server Express:**
```
Data Source=.\\SQLEXPRESS;Initial Catalog=GeoAssetManagementSystem;Integrated Security=True;Encrypt=False
```

**For LocalDB:**
```
Data Source=(localdb)\\mssqllocaldb;Initial Catalog=GeoAssetManagementSystem;Integrated Security=True;Encrypt=False
```

**For SQL Server with username/password:**
```
Data Source=YOUR_SERVER_NAME;Initial Catalog=GeoAssetManagementSystem;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;Encrypt=False
```

### 4. Configure JWT Settings

The `appsettings.json` already includes a secure JWT key. For production, generate a new secure key:

```json
{
  "JwtSettings": {
    "Key": "YOUR_SECURE_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"
  }
}
```

**Important**: Never commit production keys to source control. Use User Secrets or environment variables.

## Database Setup

### Method 1: Using Package Manager Console (Visual Studio)

1. Open Package Manager Console (Tools → NuGet Package Manager → Package Manager Console)

2. Create initial migration:
```powershell
Add-Migration InitialCreate
```

3. Apply migration to database:
```powershell
Update-Database
```

### Method 2: Using .NET CLI

1. Install EF Core tools (if not already installed):
```bash
dotnet tool install --global dotnet-ef
```

2. Create migration:
```bash
dotnet ef migrations add InitialCreate
```

3. Update database:
```bash
dotnet ef database update
```

### Verify Database Creation

1. Open SQL Server Management Studio (SSMS)
2. Connect to your server
3. Verify that `GeoAssetManagementSystem` database exists
4. Check that the following tables are created:
   - `AspNetUsers`
   - `AspNetRoles`
   - `AspNetUserRoles`
   - `Locations`
   - Other Identity tables

## Configuration

### CORS Configuration

The API is pre-configured to accept requests from the Angular frontend running on `http://localhost:4200`. To modify this:

```csharp
// In Program.cs
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200", "YOUR_PRODUCTION_URL")
              .AllowAnyMethod()
              .AllowAnyHeader());
});
```

### Environment-Specific Settings

For production deployment:

1. Create `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "cs": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "JwtSettings": {
    "Key": "YOUR_PRODUCTION_JWT_KEY"
  }
}
```

2. Use environment variables or Azure Key Vault for sensitive data

## Running the Application

### Using Visual Studio
1. Open `GeoAssetManagementSystem.sln`
2. Press `F5` or click the "Run" button
3. The API will start at `https://localhost:44322` (or similar)

### Using .NET CLI
```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:44322`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:44322/swagger`

## API Endpoints

### Authentication Endpoints

#### Register New User
```http
POST /api/Account/register
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

**Response:**
```json
{
  "message": "Registration successful"
}
```

#### Login
```http
POST /api/Account/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePass123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "john_doe"
}
```

### Location Endpoints

**Note**: All location endpoints require authentication. Include the JWT token in the Authorization header:
```
Authorization: Bearer YOUR_JWT_TOKEN
```

#### Get All Locations
```http
GET /api/Locations
```

**Response:**
```json
[
  {
    "id": 1,
    "name": "Cairo Tower",
    "latitude": 30.0461,
    "longitude": 31.2242,
    "description": "Famous landmark in Cairo",
    "dateTime": "2026-01-15T10:30:00Z"
  }
]
```

#### Search Locations by Name
```http
GET /api/Locations?name=Cairo
```

#### Create New Location
```http
POST /api/Locations
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN

{
  "name": "Pyramids of Giza",
  "latitude": 29.9792,
  "longitude": 31.1342,
  "description": "Ancient Egyptian pyramids"
}
```

**Response:**
```json
{
  "id": 2,
  "name": "Pyramids of Giza",
  "latitude": 29.9792,
  "longitude": 31.1342,
  "description": "Ancient Egyptian pyramids",
  "dateTime": "2026-01-16T12:00:00Z"
}
```

#### Update Location
```http
PUT /api/Locations/2
Content-Type: application/json
Authorization: Bearer YOUR_JWT_TOKEN

{
  "id": 2,
  "name": "Great Pyramid of Giza",
  "latitude": 29.9792,
  "longitude": 31.1342,
  "description": "Largest pyramid in Giza"
}
```

#### Delete Location
```http
DELETE /api/Locations/2
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:** `204 No Content`

## Authentication

### JWT Token Structure

The API uses JWT tokens with the following claims:
- `NameIdentifier`: User ID
- `Name`: Username
- `Jti`: Unique token identifier

### Token Expiration
- Default: 24 hours
- Configurable in `AccountController.GenerateJwtToken()`

### Using Authentication in Requests

Include the token in the Authorization header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Project Structure

```
GeoAssetManagementSystem/
├── Controllers/
│   ├── AccountController.cs      # Authentication endpoints
│   └── LocationsController.cs     # CRUD operations for locations
├── DTOs/
│   ├── AuthDTOs.cs               # Authentication data transfer objects
│   └── LocationDTOs.cs           # Location data transfer objects
├── Interfaces/
│   └── ILocationRepository.cs    # Repository interface
├── Models/
│   ├── Location.cs               # Location entity
│   ├── User.cs                   # User entity (extends IdentityUser)
│   └── LocationContext.cs        # EF Core DbContext
├── Repositories/
│   └── LocationRepository.cs     # Repository implementation
├── appsettings.json              # Application configuration
└── Program.cs                    # Application entry point & DI setup
```

## Security Features

### Data Protection
- **User Isolation**: Each user can only access their own locations
- **Password Hashing**: ASP.NET Core Identity handles secure password storage
- **JWT Validation**: Tokens are validated on every request
- **HTTPS Enforcement**: Redirects HTTP to HTTPS in production

### Validation
- **Model Validation**: Data annotations ensure data integrity
  - Name: Required, max 100 characters
  - Latitude: Required, range -90 to 90
  - Longitude: Required, range -180 to 180
  - Email: Valid email format
  - Password: Minimum 6 characters

### Authorization
- `[Authorize]` attribute protects all location endpoints
- User ID extracted from JWT claims ensures data ownership

## Testing with Swagger

### Accessing Swagger UI
1. Run the application
2. Navigate to: `https://localhost:44322/swagger`

### Testing Authenticated Endpoints
1. **Register a user** via `/api/Account/register`
2. **Login** via `/api/Account/login` and copy the token
3. Click the **Authorize** button (🔓 icon) in Swagger UI
4. Enter: `Bearer YOUR_TOKEN_HERE`
5. Click **Authorize**
6. All location endpoints will now include the token automatically

## Data Models

### Location Entity
```csharp
public class Location
{
    public int ID { get; set; }
    public string Name { get; set; }
    public decimal Latitude { get; set; }    // Precision: 18,10
    public decimal Longitude { get; set; }   // Precision: 18,10
    public string? Description { get; set; }
    public DateTime DateTime { get; set; }
    public string UserId { get; set; }       // Foreign key
    public User? User { get; set; }
}
```

### User Entity
```csharp
public class User : IdentityUser
{
    public List<Location>? Locations { get; set; }
}
```

## Troubleshooting

### Common Issues

#### Database Connection Failed
**Problem**: Cannot connect to SQL Server

**Solutions**:
- Verify SQL Server is running (check Services)
- Check connection string in `appsettings.json`
- Ensure SQL Server accepts TCP/IP connections
- For LocalDB: `sqllocaldb start mssqllocaldb`

#### Migration Errors
**Problem**: `Unable to create an object of type 'LocationContext'`

**Solution**:
```bash
dotnet ef database update --verbose
```

Check for:
- Connection string accuracy
- SQL Server running
- Permissions on database

#### JWT Key Missing
**Problem**: `JWT Key is missing from configuration`

**Solution**: 
- Verify `JwtSettings:Key` exists in `appsettings.json`
- Ensure the key is at least 32 characters long

#### CORS Errors
**Problem**: Angular app shows CORS error

**Solution**:
- Verify `UseCors("AllowAngular")` is called in `Program.cs`
- Check the Angular app URL matches the CORS policy
- Ensure CORS middleware is before `UseAuthorization()`

#### 401 Unauthorized
**Problem**: Authenticated requests return 401

**Solutions**:
- Verify token is included in Authorization header
- Check token format: `Bearer TOKEN` (note the space)
- Ensure token hasn't expired (check issue time)
- Verify JWT key matches between token generation and validation

## Database Schema

### Tables Created
- **AspNetUsers**: User accounts
- **AspNetRoles**: User roles (if using role-based authorization)
- **AspNetUserRoles**: User-role relationships
- **AspNetUserClaims**: User claims
- **AspNetUserLogins**: External login providers
- **AspNetUserTokens**: Authentication tokens
- **Locations**: Geospatial asset locations

### Key Relationships
```
User (1) ──────< (Many) Locations
```

Each user owns multiple locations, but each location belongs to exactly one user.

## Performance Considerations

- **Async/Await**: All database operations use async methods
- **EF Core Tracking**: Change tracking for efficient updates
- **Indexed Columns**: Primary keys and foreign keys are indexed by default
- **Connection Pooling**: Automatic connection pool management

## Future Enhancements

- Role-based authorization (Admin, User, Viewer)
- Location categories/tags
- Bulk import/export functionality
- Location sharing between users
- Geospatial queries (find locations within radius)
- Audit logging
- Rate limiting
- Caching layer (Redis)
- File upload for location images
- API versioning

## Environment Variables (Production)

For production deployment, use environment variables instead of `appsettings.json`:

```bash
export ConnectionStrings__cs="YOUR_PRODUCTION_CONNECTION_STRING"
export JwtSettings__Key="YOUR_PRODUCTION_JWT_KEY"
```

Or in Azure App Service, set these in Application Settings.

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Follow C# coding conventions
4. Add unit tests for new features
5. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
6. Push to the branch (`git push origin feature/AmazingFeature`)
7. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## Related Projects

**Frontend Application**: [Angular Map Asset Manager](link-to-frontend-repo)

---

**Author**: [Shahenda]  
**Contact**: [shahendakhaled267@gmail.com]  
**Repository**: [[GitHub URL]](https://github.com/shahenda23/GeoAssetManagementSystemBackend.git)  
**API Documentation**: Available at `/swagger` when running locally

## Support

For issues and questions:
- Open an issue on GitHub
- Check existing documentation
- Review Swagger API documentation

---

**Built with ❤️ using .NET and Entity Framework Core**
