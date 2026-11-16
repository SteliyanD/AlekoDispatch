# AlekoDispatch

**AlekoDispatch** is an ASP.NET Core MVC web application for managing construction work sites, heavy machinery fleets, and daily operator shift logs. Built specifically for a construction company I have worked with—and tailored to its real-world workflows and operational needs—it is developed with **.NET 10**, **Entity Framework Core**, and **ASP.NET Core Identity**, following a layered MVC architecture with a dedicated service layer, strongly-typed ViewModels, and role-based access control.

## Key Features

* **Dispatch Dashboard (`Home`):** Displays live operational metrics (total/active sites, operational vs. maintenance units, today's shift logs and fuel consumption, registered users, and the 5 most recent work logs), adapting between a guest welcome view and an authenticated command center.

* **Work Site Management (`WorkSites`):** Full CRUD lifecycle supporting `Planned`, `Active`, `Paused`, and `Completed` statuses, keyword search (name/location), status filtering, sorting (date or name), and detail views with allocated machinery and recent shift history.

* **Heavy Machinery Fleet (`HeavyMachines`):** Full CRUD inventory across six equipment categories (`Excavator`, `Bulldozer`, `DumpTruck`, `HydraulicHammer`, `WheelLoader`, `Crusher`) with search, category/status filtering, and accumulated usage metrics.

* **Daily Work Logs & Automated Telemetry (`DailyWorkLogs`):** Tracks shift date, operator, hours worked, fuel consumed (L), debris hauled (tons), and notes with multi-criteria filtering and server-side pagination (10 records/page).

  * *Immutable Log Integrity:* Intentionally omits an edit/update action (supporting create, read, and delete only) so an administrator cannot alter the hours an employee has already logged.

  * *Automated Synchronization:* Creating a shift log adds worked hours to the machine's `CurrentEngineHours` and updates its `CurrentWorkSiteId` to match the log; deleting a log deducts those hours safely (clamped at zero).

  * *Explicit Assignment Management:* Deleting a historical work log subtracts the logged hours from the machine's total engine hours, but does not automatically revert `CurrentWorkSiteId` to a previous site. This prevents accidental fleet-location changes when removing an incorrectly entered entry; current machine assignments are updated only through new shift logs or explicit edits.

* **Role-Based Authorization:**

  * **`Administrator`:** Full CRUD on work sites and machinery, plus creating and deleting daily work logs.

  * **`Employee`:** Read access to the dashboard, sites, and fleet, plus submitting new daily work logs.

* **Resilience & Auto-Seeding:** Custom `404`, `403`, and `500` error views (with diagnostic trace IDs shown only on server errors) and an idempotent `DbInitializer` that applies pending migrations and seeds roles, test users, and initial dispatch records on startup.

## Tech Stack

| Layer | Technology | 
| ----- | ----- | 
| **Framework** | .NET 10 / ASP.NET Core MVC | 
| **Language** | C# | 
| **Database & ORM** | Microsoft SQL Server & Entity Framework Core 10 | 
| **Auth** | ASP.NET Core Identity (`Administrator`, `Employee` roles) | 
| **Frontend** | Razor Views, Bootstrap 5, Bootstrap Icons | 

## Project Structure

```
AlekoDispatch/
├── AlekoDispatch.slnx
└── AlekoDispatch/
    ├── Common/             # ValidationConstants
    ├── Controllers/        # Home, WorkSites, HeavyMachines, DailyWorkLogs
    ├── Data/
    │   ├── Enums/          # MachineType, WorkSiteStatus
    │   ├── Migrations/     # EF Core migrations
    │   ├── Models/         # WorkSite, HeavyMachine, DailyWorkLog, ErrorViewModel
    │   ├── ApplicationDbContext.cs
    │   └── DbInitializer.cs
    ├── Services/           # Service interfaces & implementations
    ├── ViewModels/         # Feature-grouped Form, Index, Details & Query ViewModels
    ├── Views/              # Strongly-typed Razor views & Shared error pages
    ├── appsettings.json
    └── Program.cs

```

## Getting Started

Follow these steps to set up and run **AlekoDispatch** on your local machine.

### Prerequisites

Before running the application, make sure you have installed:

* [.NET 10 SDK](https://dotnet.microsoft.com/download)

* Microsoft SQL Server or SQL Server LocalDB

* An IDE such as Visual Studio 2022 or Visual Studio Code (optional)

### 1. Clone the Repository

```
git clone https://github.com/SteliyanD/AlekoDispatch.git
cd AlekoDispatch

```

### 2. Configure the Database

Open `AlekoDispatch/appsettings.json` and locate the `ConnectionStrings` section. Update the `DefaultConnection` value to match your SQL Server instance.

For example, when using SQL Server LocalDB:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AlekoDispatch;Trusted_Connection=True;MultipleActiveResultSets=true"
}

```

Make sure your SQL Server instance is running and accessible.

### 3. Restore Dependencies

Navigate to the project directory containing `AlekoDispatch.csproj`:

```
cd AlekoDispatch

```

Restore the required NuGet packages:

```
dotnet restore

```

### 4. Run the Application

Start the application using:

```
dotnet run

```

Alternatively, open `AlekoDispatch.slnx` in Visual Studio and run the project using the **Start** button.

### 5. Database Initialization

On the first application startup, `DbInitializer` automatically:

* Applies any pending Entity Framework Core migrations.

* Creates the `Administrator` and `Employee` roles if they do not already exist.

* Seeds the initial administrator and employee accounts.

* Adds sample work sites, heavy machinery, and daily work logs when the corresponding tables are empty.

*No manual database migration is normally required.*

### 6. Access the Application & Demo Credentials

Once the application has started, open the local URL displayed in the terminal or Visual Studio output window. You can log in using one of the pre-seeded test accounts below:

| Role | Email | Password | Permissions | 
| ----- | ----- | ----- | ----- | 
| **Administrator** | `admin@aleko.bg` | `Admin123!` | Full CRUD on work sites & machinery; create/delete daily work logs | 
| **Employee** | `employee@aleko.bg` | `Employee123!` | View dashboard, work sites & fleet; submit new daily work logs | 

> **Note:** The default accounts and credentials are defined in `DbInitializer.cs`. Review and change them before deploying the application to a production environment.