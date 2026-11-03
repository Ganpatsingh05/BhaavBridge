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

The actual .NET SDK, ASP.NET Core, EF Core and SQL Server versions used will be recorded after the development environment is finalized.

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
| Aakash Singh | Offers, negotiation, Net Daam workflow and orders |

All members share responsibility for integration, testing, documentation and viva preparation.

## Development method

The team follows nine phase gates. A phase is not complete merely because its code is written. Implementation, self-testing, peer testing, defect fixing, regression testing and approval are required before the next dependent phase is started.

See:
- [Project specification](docs/PROJECT_SPECIFICATION.md)
- [Phase tracker and exit criteria](docs/PHASE_TRACKER.md)
- [Database design](docs/DATABASE_DESIGN.md)
- [API plan](docs/API_DOCUMENTATION.md)
- [Team responsibilities](docs/TEAM_RESPONSIBILITIES.md)
- [Contribution workflow](CONTRIBUTING.md)

## Repository workflow

- `main` is the shared stable branch.
- Create a feature branch for each issue.
- Open a pull request instead of directly merging unreviewed work.
- Link the relevant issue in each pull request.
- Do not commit secrets, connection strings containing credentials, database passwords or personal data.
- Update documentation and tests together with implementation.

## Local setup

The exact setup commands will be finalized once the team locks the SDK and SQL Server configuration. At minimum, contributors will need the agreed .NET SDK, SQL Server/SQL Server Express or LocalDB, and an IDE such as Visual Studio or VS Code.

The final project will include:
- the solution and source code;
- EF Core migrations;
- a safe example configuration file;
- database setup instructions;
- dependency/version information;
- test instructions.

## Academic stack confirmation

The INT402 syllabus identifies C# and ASP.NET Core. The separate project handout contains Django/Python-specific implementation and rubric wording. The team is implementing the syllabus stack as instructed by the student, but must obtain instructor confirmation that ASP.NET Core is accepted for the selected Q1 project before substantial implementation.

## Scope exclusions

The initial submission excludes online payments, AI/ML crop predictions, IoT sensors, live GPS tracking, real-time chat and automatic live mandi-price integration. The priority is a complete and testable core application.

## Submission target

**16 November 2026.** Final release must be built and tested, documented, and packaged according to the instructor's accepted submission instructions.
