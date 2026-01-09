# ClaimsPortal - Claims Processing API

A lightweight ASP.NET Core Web API for managing insurance claims with CRUD operations, SQLite database, and XML import functionality.

## 🚀 Features

- ✅ Full CRUD operations for claims
- ✅ SQLite database with Entity Framework Core
- ✅ XML file import for bulk claim creation
- ✅ Auto-create database on startup with seed data
- ✅ Swagger/OpenAPI documentation
- ✅ Async/await throughout
- ✅ Logging and error handling
- ✅ Input validation

## 📋 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (or .NET 8)
- Any IDE (Visual Studio, VS Code, or Rider)

## 🏃 Quick Start

### 1. Restore Dependencies.

```bash
dotnet restore
```

### 2. Run the Application

```bash
dotnet run
```

The API will start and automatically:
- Create the SQLite database (`claims.db`)
- Apply the schema
- Seed 3 sample claims
- Launch Swagger UI

### 3. Access the API

- **Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger) or [https://localhost:5001/swagger](https://localhost:5001/swagger)
- **API Base URL**: `http://localhost:5000/api` or `https://localhost:5001/api`

> **Note**: The root URL (`/`) automatically redirects to Swagger.

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/claims` | Get all claims |
| `GET` | `/api/claims/{id}` | Get a specific claim by ID |
| `POST` | `/api/claims` | Create a new claim |
| `PUT` | `/api/claims/{id}` | Update an existing claim |
| `DELETE` | `/api/claims/{id}` | Delete a claim |
| `POST` | `/api/claims/import/xml` | Import claims from XML file |

## 📊 Data Model

```csharp
{
  "claimId": 0,
  "policyNumber": "string",
  "claimantName": "string",
  "dateOfLoss": "2024-11-11T00:00:00Z",
  "claimAmount": 0.00,
  "status": "Pending",
  "createdAt": "2024-11-11T00:00:00Z",
  "remarks": "string (optional)"
}
```

## 🧪 Testing the API

### Using Swagger UI (Recommended)

1. Navigate to `/swagger`
2. Expand any endpoint
3. Click "Try it out"
4. Fill in the parameters
5. Click "Execute"

### Using cURL

#### Get All Claims
```bash
curl -X GET "http://localhost:5000/api/claims" -H "accept: application/json"
```

#### Create a Claim
```bash
curl -X POST "http://localhost:5000/api/claims" \
  -H "Content-Type: application/json" \
  -d '{
    "policyNumber": "POL-2024-999",
    "claimantName": "Test User",
    "dateOfLoss": "2024-11-11",
    "claimAmount": 1500.00,
    "status": "Pending",
    "remarks": "Test claim"
  }'
```

#### Import XML
```bash
curl -X POST "http://localhost:5000/api/claims/import/xml" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@Samples/claims.xml"
```

## 📁 XML Import Format

Place XML files in the `/Samples` folder or upload via API. Format:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Claims>
    <Claim>
        <PolicyNumber>POL-2024-004</PolicyNumber>
        <ClaimantName>Emily Davis</ClaimantName>
        <ClaimAmount>3500.00</ClaimAmount>
        <DateOfLoss>2024-11-08</DateOfLoss>
        <Status>Pending</Status>
        <Remarks>Fire damage to property</Remarks>
    </Claim>
    <!-- More claims... -->
</Claims>
```

## 🗂️ Project Structure

```
ClaimsPortal/
├── Controllers/
│   └── ClaimsController.cs       # API endpoints
├── Data/
│   ├── AppDbContext.cs           # EF Core DbContext
│   └── DbSeeder.cs               # Database seeding
├── Models/
│   └── Claim.cs                  # Claim entity
├── Services/
│   ├── IClaimService.cs          # Service interface
│   └── ClaimService.cs           # Business logic
├── Samples/
│   └── claims.xml                # Sample XML file
├── Program.cs                    # Application entry point
├── appsettings.json             # Configuration
├── ClaimsPortal.csproj          # Project file
└── README.md                    # This file
```

## 🔧 Configuration

### Database Connection String

Located in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=claims.db"
  }
}
```

The SQLite database file (`claims.db`) will be created in the project root directory.

## 📦 Dependencies

- **Microsoft.EntityFrameworkCore.Sqlite** (8.0.0)
- **Microsoft.EntityFrameworkCore.Design** (8.0.0)
- **Swashbuckle.AspNetCore** (6.5.0)

## 🐛 Troubleshooting

### Database Issues
If you encounter database issues, delete `claims.db` and restart:
```bash
rm claims.db
dotnet run
```

### Port Conflicts
Change the port in `Properties/launchSettings.json` or use:
```bash
dotnet run --urls "http://localhost:5005"
```

### Build Errors
Clean and rebuild:
```bash
dotnet clean
dotnet build
```

## 📝 Seed Data

The application comes with 3 pre-seeded claims:

1. **POL-2024-001** - John Smith - $2,500 (Vehicle collision)
2. **POL-2024-002** - Sarah Johnson - $5,000 (Property damage)
3. **POL-2024-003** - Michael Brown - $1,200.50 (Medical expenses)

## 🎯 Development Notes

- **No Authentication**: This is a demo project without authentication
- **No Docker**: Runs directly with `dotnet run`
- **Logging**: Console logging enabled for debugging
- **Validation**: Input validation on all endpoints
- **Error Handling**: Try-catch blocks with proper HTTP status codes

## 📚 Additional Commands

```bash
# Build the project
dotnet build

# Run with specific environment
dotnet run --environment Development

# Watch mode (auto-restart on file changes)
dotnet watch run

# Publish for deployment
dotnet publish -c Release -o ./publish
```
## Latest Update
Added claim analytics and logging for dashboard charts - 09-Jan-2026

## ✅ Submission Checklist

- [x] Complete CRUD operations
- [x] SQLite database with EF Core
- [x] XML import functionality
- [x] Seed data included
- [x] Swagger documentation
- [x] Clean code structure
- [x] Async/await implementation
- [x] Error handling and validation
- [x] README with clear instructions

## 📞 Support

For issues or questions:
1. Check the logs in the console output
2. Review the Swagger documentation at `/swagger`
3. Verify all dependencies are installed with `dotnet restore`

---

**Built with ❤️ using ASP.NET Core 10.0**

