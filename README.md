# 🏋️‍♂️ ProjectGym - Gym Management REST API

## 🔗 Links

- **Repository:** https://github.com/Aygen5/ProjectGym
- **Live API:** https://projectgym-ki4o.onrender.com

[![.NET Version](https://img.shields.io/badge/.NET-10.0-512BD4?logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Enabled-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

ProjectGym is a production-ready, highly structured **Gym Management REST API** built using **ASP.NET Core (.NET 10)**. It follows the principles of **Clean Architecture** to ensure testability, maintainability, and clear separation of concerns. Equipped with robust **JWT Authentication**, role-based authorization, automated database migrations, and containerization, it provides a solid foundation for modern gym administration systems.

---

## 📌 Table of Contents

- [Project Overview](#-project-overview)
- [Tech Stack](#-tech-stack)
- [Clean Architecture & Layer Breakdown](#-clean-architecture--layer-breakdown)
  - [Architecture Flow Diagram](#architecture-flow-diagram)
  - [Layer-by-Layer Separation](#layer-by-layer-separation)
- [Software Design Patterns & Principles](#-software-design-patterns--principles)
- [Authentication & Authorization Flow](#-authentication--authorization-flow)
- [Features & Modules](#-features--modules)
- [Database Setup & Seed Data](#-database-setup--seed-data)
- [API Overview & Endpoints](#-api-overview--endpoints)
- [Project Directory Structure](#-project-directory-structure)
- [Installation & Local Setup](#-installation--local-setup)
- [Docker Configuration](#-docker-configuration)
- [Environment Variables](#-environment-variables)
- [Live Demo & Postman Collection](#-live-demo--postman-collection)
- [Future Improvements](#-future-improvements)
- [Software Engineering Outcomes](#-software-engineering-outcomes)
- [License](#-license)

---

## 🔍 Project Overview

ProjectGym is designed to manage day-to-day fitness club operations, focusing on membership plans, trainers, workout sessions, registrations, and secure roles. By implementing a standardized API response wrapper, the system guarantees consistent payloads for client applications, preventing internal stack traces or database errors from leaking to users.

This project serves as a showcase of **Enterprise Software Development in .NET**, proving how clean code principles, repository patterns, and structured application layers lead to scalable and professional backend applications.

---

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core (.NET 10.0)
- **Language**: C# 13
- **Database Persistence**: Entity Framework Core 10 (EF Core) / PostgreSQL
- **Security & Auth**: ASP.NET Core Identity & JWT Bearer Authentication
- **Mapping**: AutoMapper
- **Containerization**: Docker & Docker Compose
- **Hosting / CI-CD**: Render / Railway ready
- **Validation**: System.ComponentModel.DataAnnotations

---

## 📐 Clean Architecture & Layer Breakdown

Clean Architecture places the **Domain** model at the center of the system. Dependencies point inward toward the Domain, meaning the business logic does not know anything about the database, web controllers, or external libraries.

### Architecture Flow Diagram

```
 ┌────────────────────────────────────────────────────────┐
 │                    ProjectGym.API                      │ (Web API, Controllers, Filters, Extensions)
 └───────────┬───────────────────────────────┬────────────┘
             │                               │
             │ (Uses Services/DTOs)          │ (Registers Dependencies via DI)
             ▼                               ▼
 ┌───────────────────────────┐   ┌────────────────────────┐
 │ ProjectGym.Application    │   │ProjectGym.Infrastructure│ (EF Core, Repositories, Database Context, Identity)
 └───────────┬───────────────┘   └───────────┬────────────┘
             │                               │
             │ (Implements Core Interfaces)  │ (Implements Core Repositories)
             ▼                               ▼
 ┌────────────────────────────────────────────────────────┐
 │                    ProjectGym.Domain                   │ (Entities, Value Objects, Core Interfaces)
 └────────────────────────────────────────────────────────┘
```

### Layer-by-Layer Separation

1. **`ProjectGym.Domain`**
   - The core layer containing database-agnostic business logic.
   - **Entities**: [Member](file:///src/ProjectGym.Domain/Entities/Member.cs), [Membership](file:///src/ProjectGym.Domain/Entities/Membership.cs), [MembershipPlan](file:///src/ProjectGym.Domain/Entities/MembershipPlan.cs), [Trainer](file:///src/ProjectGym.Domain/Entities/Trainer.cs), [WorkoutSession](file:///src/ProjectGym.Domain/Entities/WorkoutSession.cs), [Attendance](file:///src/ProjectGym.Domain/Entities/Attendance.cs).
   - **Enums**: Core business enums like `MembershipStatus` and `AttendanceStatus`.
   - **Interfaces**: Definitions of abstractions like `IRepository<T>` and unit of work specifications, decoupling business entities from specific database engines.

2. **`ProjectGym.Application`**
   - Defines the system's use cases, coordinates core workflows, and exposes interfaces for business services.
   - **DTOs**: Data Transfer Objects structured for specific requests/responses, ensuring strict separation from EF Core entities.
   - **Mappings**: AutoMapper profile config ([MappingProfile.cs](file:///src/ProjectGym.Application/Mappings/MappingProfile.cs)) implementing safe entity-to-dto projections.
   - **Services**: Business services like `MembershipPlanService` and `TrainerService` that coordinate repository calls and business validations.

3. **`ProjectGym.Infrastructure`**
   - The concrete implementation of external concerns (databases, authentication mechanisms, file storage).
   - **Data**: Entity Framework Core DB Context ([ProjectGymDbContext.cs](file:///src/ProjectGym.Infrastructure/Data/ProjectGymDbContext.cs)), [UnitOfWork.cs](file:///src/ProjectGym.Infrastructure/Data/UnitOfWork.cs), and [Configurations](file:///src/ProjectGym.Infrastructure/Data/Configurations) using Fluent API.
   - **Repositories**: Database access implementations using EF Core (`MemberRepository`, `TrainerRepository`, `WorkoutSessionRepository`, etc.).
   - **Identity**: Identity models ([AppIdentityUser.cs](file:///src/ProjectGym.Infrastructure/Identity/AppIdentityUser.cs)) and [AuthService.cs](file:///src/ProjectGym.Infrastructure/Services/AuthService.cs) to generate JWT security tokens.

4. **`ProjectGym.API`**
   - The presentation layer and application entry point.
   - **Controllers**: Exposes HTTP endpoints, inherits from [ApiControllerBase.cs](file:///src/ProjectGym.API/Controllers/ApiControllerBase.cs), handles routing, and processes requests.
   - **Extensions**: [ServiceExtensions.cs](file:///src/ProjectGym.API/Extensions/ServiceExtensions.cs) to clean up `Program.cs` by grouping Dependency Injection configuration.
   - **Filters**: [ApiResponseFilter.cs](file:///src/ProjectGym.API/Filters/ApiResponseFilter.cs) to intercept HTTP results and automatically wrap them into a unified response schema.

---

## 💎 Software Design Patterns & Principles

* **Clean Architecture**: Decouples domain business logic from databases and UI, enabling easier migrations and unit testing.
* **Repository Pattern**: Abstracts data persistence behind repository interfaces (`IRepository<T>`), isolating the EF Core implementation from application services.
* **Unit of Work Pattern**: Bundles multiple repository write operations into a single database transaction (`IUnitOfWork.SaveChangesAsync()`), guaranteeing transactional integrity and rollback support.
* **Result Pattern (`Result<T>`)**: Avoids using exceptions for normal flow control (e.g., entity not found). Services return a structured `Result<T>` containing status types (`NotFound`, `Validation`, `Conflict`), which controllers cleanly translate into corresponding HTTP status codes.
* **DTO Pattern**: Keeps database entities inside the persistence layer. External clients interact purely with request/response DTOs containing validation rules.
* **Fluent API & Separation of Configurations**: Rather than cluttering the DbContext or polluting entities with database annotations, entity configurations are split into distinct classes under the `Configurations` folder, keeping database schemas clean and customizable.

---

## 🔐 Authentication & Authorization Flow

ProjectGym leverages **ASP.NET Core Identity** and **JWT Bearer Authentication** to secure administrative endpoints:

```
[ Client ]                        [ API / AuthController ]              [ DB ]
    │                                        │                            │
    │ 1. POST /api/auth/login                │                            │
    ├───────────────────────────────────────>│                            │
    │                                        │ 2. Verify Credentials      │
    │                                        ├───────────────────────────>│
    │                                        │<───────────────────────────┤
    │                                        │                            │
    │ 3. Sign JWT Token with Claims          │                            │
    │    (roles: Admin/Member, memberId)     │                            │
    │                                        │                            │
    │ 4. Return JWT Token + User profile     │                            │
    │<───────────────────────────────────────┤                            │
    │                                        │                            │
    │ 5. GET /api/trainers (Header: Bearer)  │                            │
    ├───────────────────────────────────────>│                            │
    │                                        │ 6. Validate JWT & Role     │
    │                                        │    (Only Admin allowed)    │
    │                                        │                            │
    │ 7. Return 200 OK (Trainers List)       │                            │
    │<───────────────────────────────────────┤                            │
```

* **Roles**: Supported roles include `Admin` and `Member`. 
* **Dynamic Scopes**: Member registers automatically generate a linked `Member` entity inside the database. The `memberId` is embedded directly into the JWT token claims, allowing standard members to view their specific sessions and logs without query parameters.

---

## 📦 Features & Modules

### 1. Authentication & Security
* **Register**: User register automatically assigns the `Member` role and creates a corresponding database profile.
* **Login**: Verifies credentials, locks out users after 5 failed attempts, and returns a short-lived JWT token.
* **Profile (`/api/auth/me`)**: Retrieves current user profile details securely using name identifier claims.

### 2. Membership Plans
* **Public Queries**: Anonymous users can fetch active plans.
* **Admin Management**: Admins can manage plans via create, update, delete, and paginated overview endpoints.
* **Active Checking**: Logic to dynamically check active dates on member plans.

### 3. Trainer Management
* **Admin CRUD**: Full CRUD support for fitness instructors, complete with specialties.
* **Paginated Queries**: Efficient paginated querying for trainer listings.

### 4. Underlying Modules (Core & Infrastructure Ready)
* **Workout Sessions**: Supports class creation, scheduling, trainer mapping, capacity controls, and cancellation status.
* **Attendance**: Validates registrations and logs member check-ins.

---

## 🗄️ Database Setup & Seed Data

The project runs on **PostgreSQL**.
* **Configurations**: Fluent configurations enforce constraints like:
  - `MembershipPlan`: Decimal Precision for `PricePerMonth` (18,2).
  - `Member`: Unique Indexes on `Email` and `UserId`.
  - `Trainer`: Maximum length constraints on names and specialties.
* **DbInitializer**: In development environments, the system checks migrations, seeds default roles (`Admin`, `Member`), creates a default administrator (`admin@projectgym.local` / `Admin123!`), and populates initial membership plans, trainers, and sample workout classes.

---

## 🚀 API Overview & Endpoints

All responses follow a standard envelope schema:

**Success Response Envelope (`200 OK` / `201 Created`)**
```json
{
  "isSuccess": true,
  "value": {
     // Actual payload
  },
  "message": null
}
```

**Error Response Envelope (`400 Bad Request` / `401 Unauthorized` / `404 Not Found`)**
```json
{
  "isSuccess": false,
  "value": null,
  "message": "Error description details here."
}
```

### Key API Endpoints

| Controller | HTTP Method | Endpoint | Authorization | Description |
| :--- | :--- | :--- | :--- | :--- |
| **Auth** | `POST` | `/api/auth/register` | Anonymous | Register a new member. |
| **Auth** | `POST` | `/api/auth/login` | Anonymous | Authenticate and obtain JWT token. |
| **Auth** | `GET` | `/api/auth/me` | Authenticated | Retrieve current user profile. |
| **Membership Plans** | `GET` | `/api/membershipplans` | Anonymous | Get list of active membership plans. |
| **Membership Plans** | `GET` | `/api/membershipplans/{id}` | Anonymous | Get details of a single membership plan. |
| **Membership Plans** | `GET` | `/api/membershipplans/manage` | Admin Role | Paginated list of all plans. |
| **Membership Plans** | `POST` | `/api/membershipplans` | Admin Role | Create a new membership plan. |
| **Membership Plans** | `PUT` | `/api/membershipplans/{id}` | Admin Role | Update an existing membership plan. |
| **Membership Plans** | `DELETE`| `/api/membershipplans/{id}` | Admin Role | Delete a membership plan. |
| **Trainers** | `GET` | `/api/trainers` | Admin Role | Paginated list of trainers. |
| **Trainers** | `GET` | `/api/trainers/{id}` | Admin Role | Get trainer by ID. |
| **Trainers** | `POST` | `/api/trainers` | Admin Role | Add a new trainer. |
| **Trainers** | `PUT` | `/api/trainers/{id}` | Admin Role | Update a trainer's details. |
| **Trainers** | `DELETE`| `/api/trainers/{id}` | Admin Role | Remove a trainer. |

---

## 📂 Project Directory Structure

```
ProjectGym/
├── .gitignore
├── docker-compose.yml
├── Dockerfile
├── README.md
├── ProjectGym.slnx
├── postman/
│   ├── ProjectGym.postman_collection.json
│   └── ProjectGym.postman_environment.json
└── src/
    ├── ProjectGym.Domain/
    │   ├── Entities/
    │   │   ├── Attendance.cs
    │   │   ├── Member.cs
    │   │   ├── Membership.cs
    │   │   ├── MembershipPlan.cs
    │   │   ├── Trainer.cs
    │   │   └── WorkoutSession.cs
    │   ├── Enums/
    │   └── Interfaces/
    ├── ProjectGym.Application/
    │   ├── DTOs/
    │   ├── Interfaces/
    │   ├── Mappings/
    │   └── Services/
    ├── ProjectGym.Infrastructure/
    │   ├── Data/
    │   │   ├── Configurations/
    │   │   ├── DbInitializer.cs
    │   │   ├── ProjectGymDbContext.cs
    │   │   └── UnitOfWork.cs
    │   ├── Identity/
    │   ├── Repositories/
    │   └── Services/
    └── ProjectGym.API/
        ├── Controllers/
        ├── Extensions/
        ├── Filters/
        ├── Program.cs
        └── appsettings.json
```

---

## 💻 Installation & Local Setup

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Quick Start (Local Running)

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/ProjectGym.git
   cd ProjectGym
   ```

2. **Spin up PostgreSQL via Docker Compose**
   ```bash
   docker-compose up -d
   ```
   *This starts the PostgreSQL server on localhost port `5433`.*

3. **Verify App Settings**
   Ensure `appsettings.Development.json` has the correct database credentials:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5433;Database=ProjectGymDB;Username=postgres;Password=postgres;"
   }
   ```

4. **Run Database Migrations & Run API**
   ```bash
   dotnet run --project src/ProjectGym.API
   ```
   *Once run, the application will initialize, run migrations, seed data, and start on `https://localhost:7106` / `http://localhost:5265`.*

---

## 🐳 Docker Configuration

A production-ready [Dockerfile](file:///Dockerfile) is included at the root level of the project. It uses a multi-stage compilation flow to keep output images small and optimized:

```dockerfile
# 1. Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Build environment
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
...
```

You can build and run the full Docker image locally:
```bash
docker build -t projectgym-api .
docker run -p 8080:8080 --name projectgym -e ConnectionStrings__DefaultConnection="Host=your_host;Database=gymdb;Username=postgres;Password=your_pass;" projectgym-api
```

---

## 🔐 Environment Variables

When deploying the API to production environments (such as **Render** or **Railway**), override sensitive variables without editing configuration files. 

Define these settings in your dashboard:

```bash
# Environment Mode
ASPNETCORE_ENVIRONMENT=Production

# Database Connection (Postgres)
ConnectionStrings__DefaultConnection=Host=production-db-instance;Port=5432;Database=projectgym;Username=postgres;Password=your_secret_production_password;

# JWT Security Configurations
JwtSettings__SecretKey=your_strong_production_jwt_secret_key_at_least_64_characters_long
JwtSettings__Issuer=ProjectGymAPI
JwtSettings__Auidence=ProjectGymClient
JwtSettings__ExpirationMinutes=60
```

---

## 🚀 Live Demo & Postman Collection

## 🌐 Live API

The API is deployed on Render and is publicly accessible.

**Base URL**

```text
https://projectgym-ki4o.onrender.com
```

You can verify that the API is running by opening the base URL. The root endpoint returns a simple health/status response, while the remaining functionality is available through the REST API endpoints.

**Health Check**

Opening the base URL returns a simple JSON response confirming that the API is running successfully.

### Importing Postman Collection
Pre-configured Postman workspace files are located inside the `postman/` directory:
- 📂 **Postman Collection**: [ProjectGym.postman_collection.json](postman/ProjectGym.postman_collection.json)
- ⚙️ **Postman Environment**: [ProjectGym.postman_environment.json](postman/ProjectGym.postman_environment.json)

**Features of the Postman Setup:**
* Automatically extracts and saves the JWT token to the environment variable `token` upon executing the `Login (Admin)` or `Login (Member)` requests.
* Inherits authorization header fields globally, enabling authentication-free endpoints while securing administrative routes.

---

## 🔮 Future Improvements

- [ ] **Exposing Remaining Endpoints**: Adding controller endpoints for Members, Workout Sessions, and Attendances (already implemented in Domain/Infrastructure).
- [ ] **Validation Filters**: Upgrading custom action filters to handle validation states via FluentValidation automatically.
- [ ] **API Documentation**: Moving from .NET 10 default OpenAPI to complete Scalar / Swagger UI endpoints for visual documentation.
- [ ] **Integration Tests**: Adding an integration test suite using WebApplicationFactory and Respawn.

---

## 🎓 Software Engineering Outcomes

Technical recruiters and developers analyzing this project will observe key backend engineering competencies:

1. **Architecture Separation**: Domain entities are decoupled from EF Core constraints. Business rules remain stateless, testable, and completely independent of third-party packages.
2. **Robust Error Handling via Result Object**: The project completely bypasses HTTP status code assignment inside services, returning domain-level responses. This design mitigates the performance overhead of throw-catch exceptions for standard control flows.
3. **Database Performance Best Practices**: Enforces strict index lengths, keys, decimal precisions, and constraints via Fluent API configurations, preventing SQL overflow errors.
4. **Clean Identity Extension**: Extends default Identity users with additional properties (like FirstName, LastName) without corrupting default tables. Extends references seamlessly to distinct domain tables.
5. **Secure Configurations**: Separates local development secrets from production environments, ensuring zero credentials are ever exposed on public repositories.

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.