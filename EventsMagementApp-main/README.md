# 🗓️ Events Management App REST API

![.NET](https://img.shields.io/badge/.NET-7.0-%23512BD4?logo=.net)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-%23CC2927?logo=microsoft-sql-server)
![License](https://img.shields.io/badge/License-MIT-green)

A robust REST API for managing events, organizers, participants, and sponsors with modern architectural practices. Built for scalability and maintainability.

## 🚀 Features
- ✅ **CRUD Operations** for Events, Organizers, Participants, Sponsors
- 🔄 **Repository Pattern** & **Dependency Injection**
- 🗃️ **Entity Framework Core** with SQL Server
- 🛡️ **Layered Architecture** (API | Services | Data Access)
- 📄 **Swagger/OpenAPI** Documentation
- 📊 **AutoMapper** for DTO transformations

## ⚙️ Tech Stack
| **Layer**       | **Technologies**                                                                 |
|------------------|----------------------------------------------------------------------------------|
| **Core**         | .NET 7, C#, MediatR, FluentValidation                                            |
| **Data**         | Entity Framework Core 7, SQL Server, Repository Pattern                          |
| **API**          | REST Standards, ASP.NET Core, Swagger/OpenAPI, AutoMapper                        |
| **Tools**        | Git, GitHub, SQL Server Management Studio, Postman                               |

## 🛠️ Installation
```bash
# Clone repository
git clone https://github.com/Gaitan19/EventsMagementApp.git

# Restore dependencies
dotnet restore

# Configure database connection in appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_SQL_Server_Connection_String"
  }
}

# Run database migrations
dotnet ef database update

# Start application
dotnet run
