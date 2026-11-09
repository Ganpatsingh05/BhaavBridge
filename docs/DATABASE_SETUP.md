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

## 6. Optional Development administrator bootstrap

The application always ensures the Farmer, Buyer and Administrator roles exist. It creates a Development administrator account only when both of the following User Secrets are configured. If neither secret is present, normal local startup continues without creating an administrator. Supplying only one secret is treated as a configuration error.

Run from the repository root and replace the example email and password with values intended only for your local development machine:

```powershell
dotnet user-secrets set "AdminBootstrap:Email" "admin@bhaavbridge.local" --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj

dotnet user-secrets set "AdminBootstrap:Password" "<your-unique-strong-password>" --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj
```

The password must satisfy the configured Identity policy: at least 8 characters and include uppercase, lowercase, a digit and a non-alphanumeric character.

Start the application in the Development environment. The bootstrapper creates the administrator if the email is new, or ensures the Administrator role is assigned if the account already exists without another role. It refuses to promote an account that already has a Farmer or Buyer role; use a dedicated administrator email. The bootstrapper is never run outside Development.

To disable future local bootstrap attempts, remove both values from User Secrets:

```powershell
dotnet user-secrets remove "AdminBootstrap:Email" --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj

dotnet user-secrets remove "AdminBootstrap:Password" --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj
```

Never commit administrator credentials or User Secrets to the repository.
