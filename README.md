# Device Management System

A web application for managing company mobile devices, built with ASP.NET Core and Angular.

## Tech Stack

- **Backend:** C#, ASP.NET Web API, .NET 9, Entity Framework Core
- **Frontend:** Angular 19, Angular Material
- **Database:** MS SQL Server
- **Auth:** ASP.NET Identity + JWT
- **AI:** Groq API (device description generator)
- **Infrastructure:** Docker, Docker Compose

---

## Prerequisites

Make sure you have the following installed:

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Node.js 20+](https://nodejs.org) *(only for local development)*
- [.NET 9 SDK](https://dotnet.microsoft.com/download) *(only for local development)*

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Rares5000/DeviceManagement.git
cd DeviceManagement
```

### 2. Create the `.env` file

Open `.env` and fill in the required values:

```env
CONNECTIONSTRINGS__DEFAULTCONNECTION=Server=sqlserver,1433;Database=DeviceManagementDB;User Id=sa;Password=Admin1234!;TrustServerCertificate=True;
JWTSETTINGS__SECRET=your-super-secret-key-minimum-32-characters
AI__APIKEY=your-ai-api-key
```

> **Note:** To use the AI description generator, get a free API key from [console.groq.com](https://console.groq.com). Without it, the generate description feature will not work but the rest of the app will function normally.

### 3. Run with Docker

```bash
docker-compose up --build -d
```

This will start three containers:
- **SQL Server** on port `1433`
- **API** on port `5001`
- **Frontend** on port `4200`

### 4. Open the app

Navigate to [http://localhost:4200](http://localhost:4200)

### 5. Create an account

- Click **Register**
- Fill in your name, email, password, role and location
- You will be redirected to the device list

---

## Features

### Devices
- View all devices in a table with assigned user
- View device details
- Add a new device
- Edit an existing device
- Delete a device
- Search devices by name, manufacturer, processor or RAM (ranked results)
- Generate AI description based on device specs

### Authentication
- Register with email and password
- Login / Logout
- JWT token based authentication

### Device Assignment
- Assign a device to yourself
- Unassign a device previously assigned to you

---

## Project Structure

```
DeviceManagement/
├── DeviceManagement.API/          # ASP.NET Web API
├── DeviceManagement.Core/         # Entities, Interfaces, DTOs
├── DeviceManagement.Infrastructure/  # Repositories, Services, DB
├── DeviceManagement.Tests/        # Integration & Unit Tests
├── device-management-ui/          # Angular application
├── docker-compose.yml
└── README.md
```

---

## Running Tests

```bash
cd DeviceManagement.Tests
dotnet test --verbosity normal
```

---

## Environment Variables

| Variable | Description |
|---|---|
| `CONNECTIONSTRINGS__DEFAULTCONNECTION` | SQL Server connection string |
| `JWTSETTINGS__SECRET` | JWT signing secret (min 32 characters) |
| `JWTSETTINGS__ISSUER` | JWT issuer (default: DeviceManagement) |
| `JWTSETTINGS__AUDIENCE` | JWT audience (default: DeviceManagement) |
| `JWTSETTINGS__EXPIRATIONINDAYS` | Token expiration in days (default: 7) |
| `AI__APIKEY` | Groq API key for AI description generation |
