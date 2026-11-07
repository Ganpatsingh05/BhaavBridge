# BhaavBridge — Local Database Setup

This guide configures the local development database for the ASP.NET Core application.

## Prerequisites

- .NET SDK 10.0.400 or compatible .NET 10 SDK
- SQL Server 2025 Express (or the agreed SQL Server development instance)
- SSMS 22
- Repository cloned locally

## 1. Pull the current project

```powershell
git switch main
git pull origin main
dotnet tool restore
dotnet restore .\BhaavBridge.sln
```

## 2. Configure the SQL Server connection with User Secrets

Run this command from the repository root:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost\SQLEXPRESS;Database=BhaavBridge_Dev;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True" --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj
```

This uses Windows Authentication for the local SQL Server Express instance. The connection string is stored in the local user-secrets store and is not committed to Git.

Do not place local passwords or connection strings in `appsettings.json` or commit them to the repository.

## 3. Verify the EF Core context

```powershell
dotnet ef dbcontext info --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj --startup-project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj
```

Expected: EF Core identifies `BhaavBridge.Web.Data.ApplicationDbContext` and the SQL Server provider.

## 4. Create and apply the initial migration

Run these commands only after the solution builds and the context discovery command succeeds.

```powershell
dotnet ef migrations add InitialCreate --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj --startup-project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj --output-dir Data\Migrations

dotnet ef database update --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj --startup-project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj
```

The migration files are part of the source code and must be committed. The local database files and user secrets must not be committed.

## 5. Verify in SSMS

Connect to `localhost\SQLEXPRESS` using Windows Authentication and refresh `BhaavBridge_Dev`.

Verify that the database contains:
- ASP.NET Core Identity tables (for users, roles, claims, logins and tokens)
- FarmerProfiles
- BuyerProfiles
- Crops
- ProduceLots
- MarketReferencePrices
- Offers
- OfferRevisions
- Orders

Check the primary keys, foreign keys, unique indexes and check constraints before marking the database phase complete.
