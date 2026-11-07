# BhaavBridge — Team Responsibilities

Our core implementation work is shared primarily by Ganpat, Rahul and Gautam. Aakash has smaller, clearly bounded UI, documentation and testing-support tasks. Every member must still be able to run, explain and make a small change to the application for the academic viva.

## Members and ownership

| Member | Primary role | Owned scope |
|---|---|---|
| Ganpat Singh | Project lead, database architect and integration lead | Architecture, SQL Server/EF Core, shared schema and migrations, reference-price administration, offer/price-discovery business rules, transaction integrity, integration and final review |
| Rahul Bora | Authentication and account workflow developer | ASP.NET Core Identity, registration/login/logout, roles, authorization, farmer/buyer profiles; later support for order views/status workflow and related API work as assigned |
| Gautam | Marketplace developer | Crop catalogue, harvest-lot CRUD, listing lifecycle, marketplace browsing, search and filters |
| Aakash Singh | UI and documentation support | Low-risk Razor pages (such as Help/About), isolated CSS/presentation tasks, sample content, screenshots, user-guide/report sections and assigned manual UI tests. Core database, authentication, offer negotiation and transaction logic are not assigned to this role. |

## Task boundaries

### Ganpat
- Own the shared data model and EF Core integration.
- Resolve cross-module technical decisions.
- Implement reference-price and core price-comparison/offer business rules.
- Coordinate transaction safety and order consistency.
- Keep the shared `main` branch stable and coordinate integration.

### Rahul
- Own Identity and authentication.
- Implement role-aware farmer and buyer profile flows.
- Implement assigned account-related views and APIs.
- Support the order display/status module after the authentication foundation is stable.

### Gautam
- Own crop master data and harvest-lot CRUD.
- Implement buyer marketplace pages and filters.
- Enforce listing ownership and availability rules in coordination with the team.

### Aakash
- Complete small tasks with clear acceptance criteria and a limited file set.
- Examples: Help/About Razor page, dedicated CSS for support pages, simple UI copy, screenshot collection, user instructions and manual UI checks.
- Make an understandable code contribution and be prepared to explain it during viva.
- Coordinate before editing shared files.

## Shared repository workflow

- `main` is the only shared development branch.
- Members pull the latest `main`, work on assigned files, run local checks, commit and push directly to `main`.
- Routine feature branches and pull requests are not required.
- Coordinate before changing the same files; never force-push.
- Every completed task records its commit hash and test result in the task issue.
- A task being Done does not automatically mean the phase gate is approved.

## Shared responsibilities

All four members must understand the overall application workflow, know how to clone/build/run it, understand their own contribution, keep assigned task cards updated, test their work, report blockers and contribute to documentation and viva preparation.
