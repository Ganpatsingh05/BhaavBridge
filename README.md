# BhaavBridge

**INT402 – Modern Web Programming Tools and Techniques | Session 2026–27**

BhaavBridge is a role-based agricultural marketplace that helps farmers publish harvest lots, receive buyer offers, compare estimated net realization after farmer-borne costs, negotiate and manage accepted orders.

> **Tagline:** Know the offer. Compare the value. Make the right deal.

## Project at a glance

| Item | Details |
|---|---|
| Selected project question | Q1 – Fair Trade Agri-Portal: Empowering Farmers with Better Market Rates |
| Application type | Group web application |
| Team size | 4 |
| Submission target | 16 November 2026 |
| Project tracker | [BhaavBridge – Development & Phase Tracker](https://github.com/users/Ganpatsingh05/projects/8/views/3) |

## Technology stack

- C# and ASP.NET Core
- ASP.NET Core MVC and Razor Views
- HTML, CSS and JavaScript
- Microsoft SQL Server
- Entity Framework Core
- ASP.NET Core Identity, authentication and authorization
- ASP.NET Core Web API
- Built-in dependency injection and middleware
- Postman for API testing

The verified development baseline is .NET SDK 10.0.400, ASP.NET Core runtime 10.0.11, SQL Server 2025 Express and SSMS 22. Entity Framework Core SQL Server/Design and the `dotnet-ef` CLI are pinned to 10.0.14.

## Core functionality

1. Farmer, buyer and administrator roles.
2. Farmer and buyer registration, authentication and profile management.
3. Harvest-lot creation, editing, publishing and closure.
4. Buyer marketplace with search and filters.
5. Buyer offers, farmer counteroffers and offer history.
6. Net Daam Calculator to compare estimated net returns after farmer-borne costs.
7. Dated reference market prices maintained by an administrator.
8. Order creation from an accepted offer and order-status tracking.
9. RESTful APIs with authorization and Postman test coverage.

## Signature feature: Net Daam

`Gross offer value = offered price per quintal × quantity`

`Estimated net return = gross offer value − farmer-borne expenses`

The comparison is an estimate based on recorded offer terms and user-entered costs. It is not a guarantee of actual proceeds or a live logistics quote.

## Team

| Member | Primary ownership |
|---|---|
| Ganpat Singh | Project lead, database architecture, EF Core, integration and reference-price module |
| Rahul Bora | Authentication, authorization and user profiles |
| Gautam | Crop catalogue, harvest lots and marketplace |
| Aakash Singh | Low-risk UI, documentation, screenshots and manual testing support |

All members must be able to run and explain the application. We coordinate changes, test the work and keep the shared `main` branch stable.

## Development method

The team follows nine phase gates. Members complete assigned work, run local checks and push to the shared `main` branch. We coordinate changes to shared files and run phase-level tests before approving each gate.

See:
- [Project specification](docs/PROJECT_SPECIFICATION.md)
- [Phase tracker and exit criteria](docs/PHASE_TRACKER.md)
- [Database design](docs/DATABASE_DESIGN.md)
- [Local database setup](docs/DATABASE_SETUP.md)
- [Database smoke check](tests/BhaavBridge.DatabaseSmokeCheck/README.md)
- [API plan](docs/API_DOCUMENTATION.md)
- [Team responsibilities](docs/TEAM_RESPONSIBILITIES.md)
- [Contribution workflow](CONTRIBUTING.md)

## Repository workflow

- `main` is the shared stable branch.
- `main` is the only shared development branch.
- Pull the latest `main`, edit the files assigned to the task, run checks, commit and push directly to `main`.
- Routine feature branches and pull requests are not required.
- Update the task issue with the commit hash and tests performed.
- Do not commit secrets, connection strings containing credentials, database passwords or personal data.
- Update documentation and tests together with implementation.

## Local setup

The development baseline is .NET SDK 10.0.400 and SQL Server Express at `localhost\\SQLEXPRESS`. Configure the local connection with .NET User Secrets and follow [Database Setup](docs/DATABASE_SETUP.md) for restore, build, EF Core context verification and migration commands.

Never commit local connection strings, credentials or user-secret files.

## Academic stack confirmation

The INT402 syllabus identifies C# and ASP.NET Core. The separate project handout contains Django/Python-specific implementation and rubric wording. Our instructor has approved using the INT402 syllabus stack (C# / ASP.NET Core) for the selected Q1 project.

## Scope exclusions

The initial submission excludes online payments, AI/ML crop predictions, IoT sensors, live GPS tracking, real-time chat and automatic live mandi-price integration. The priority is a complete and testable core application.

## Submission target

**16 November 2026.** Final release must be built and tested, documented, and packaged according to the instructor's accepted submission instructions.

## REST API and Postman

- [Implemented API endpoints](docs/API_DOCUMENTATION.md)
- [Postman collection](docs/postman/BhaavBridge.postman_collection.json)
- [Local Postman environment template](docs/postman/BhaavBridge.local.postman_environment.json)

Never commit real authentication cookies or credentials.
