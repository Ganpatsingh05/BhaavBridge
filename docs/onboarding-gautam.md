# Onboarding Evidence — Gautam Kumar (Roygautam8852)

Relates to: Issue #11 — _Team onboarding — Gautam and Aakash_

## Completed Steps

| Step | Status | Timestamp (IST) |
|------|--------|-----------------|
| Repository access confirmed | ✅ Done | 2026-11-07 20:14 |
| `git clone https://github.com/Ganpatsingh05/BhaavBridge.git` | ✅ Done | 2026-11-07 20:14 |
| `dotnet restore` | ✅ Done | 2026-11-07 20:24 |
| `dotnet build` | ✅ Done — 0 Errors, 0 Warnings | 2026-11-07 20:25 |
| Pull latest `main`, commit and push a small change | ✅ Done | 2026-11-07 20:25 |

## Build Output

```
BhaavBridge.Web -> D:\BhaavBridge\src\BhaavBridge.Web\bin\Debug\net10.0\BhaavBridge.Web.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:10.31
```

## Environment

- **Machine OS:** Windows 11
- **.NET SDK:** 10.0.400
- **Clone path:** `D:\BhaavBridge`
- **MVC project:** `src/BhaavBridge.Web/BhaavBridge.Web.csproj`

## Notes

- `dotnet restore` downloaded `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore`,
  `System.Security.Cryptography.Pkcs`, and `Microsoft.EntityFrameworkCore.Design` (all v10.0.14).
- Build compiled cleanly with no warnings.
- Launch command verified: `dotnet run --launch-profile https --project .\src\BhaavBridge.Web\BhaavBridge.Web.csproj`
