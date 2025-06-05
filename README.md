# Motorsport API

Motorsport API is a .NET solution that provides a web-based data service for motorsports information (e.g. drivers, cars, tracks, and races).

## Installation

1. **Prerequisites**
   - [.NET SDK 9.0](https://dotnet.microsoft.com/download)
   - Git

2. **Clone the repository**
   ```bash
   git clone https://github.com/taczhed/motorsport-api.git
   cd motorsport-api
   ```

3. **Restore and build**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Configuration**
   - The application uses an in-memory EF Core database by default (no extra setup).
   - JWT configuration is set in `appsettings.json` and works out-of-the-box for development.

## Solution Structure

The solution consists of the following main projects located in the `src/` directory:

- **Domain** – Core business entities like `Car`, `Driver`, `Race`, `Track`, etc.
- **Application** – Application logic, services, and DTO mappings.
- **Infrastructure** – Data access layer using Entity Framework Core with in-memory provider.
- **Api** – ASP.NET Core Web API project exposing REST and GraphQL endpoints.
- **Web** – ASP.NET Razor Pages front-end for interacting with the API.

## Running the Projects

### Run the API
```bash
dotnet run --project src/Api/Api.csproj
```

- REST API endpoints available at `http://localhost:8080/api`
- Swagger available at `http://localhost:8080/swagger`
- GraphQL endpoint at `http://localhost:8080/graphql`

### Run the Web UI
```bash
dotnet run --project src/Web/Web.csproj
```
- Launches the Razor Pages front-end on a different port

### Roles
- Login details for `Admin`:
  - Login: `admin`
  - Password: `admin`
- Login details for `RaceManager`:
  - Login: `manager`
  - Password: `manager`

### Run the Tests

```bash
dotnet test src/Tests/Tests.csproj
```

```bash
/src/Tests/curl_test.sh
```

## Technologies Used

- **.NET 9 / ASP.NET Core**
- **Entity Framework Core (In-Memory)**
- **HotChocolate GraphQL**
- **AutoMapper**
- **JWT Authentication**
- **Swagger (Swashbuckle)**
- **Razor Pages**

---

This project is intended for learning and experimentation with modern .NET web technologies.