# BhaavBridge Database Smoke Check

This is a repeatable SQL Server + EF Core integration check for the initial database model.

## What it verifies

- Insert/read operations for Identity users, farmer/buyer profiles, crops, produce lots and market reference prices.
- Insert/read operations for offers and offer revisions.
- Accepted offer and order snapshot persistence.
- Net Daam arithmetic for a known sample.
- SQL Server rejection of a negative produce-lot quantity through the database check constraint.

All sample data is created inside one database transaction and rolled back at the end. No smoke-test rows should remain in the development database.

## Run

From the repository root in PowerShell:

```powershell
git switch main
git pull origin main
dotnet tool restore
dotnet run --project .\tests\BhaavBridge.DatabaseSmokeCheck\BhaavBridge.DatabaseSmokeCheck.csproj
```

The console project uses the same `BhaavBridge-Web` User Secrets ID as the web application, so it reads the existing `ConnectionStrings:DefaultConnection` value. Do not add credentials to source control.

Expected final line:

`DATABASE SMOKE CHECK PASSED.`

If the test fails, copy the complete error output and do not mark Phase 1 complete.
