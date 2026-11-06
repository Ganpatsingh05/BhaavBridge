# BhaavBridge — Team Responsibilities

The core implementation is shared primarily by Ganpat, Rahul and Gautam. Aakash is assigned smaller, clearly bounded UI, documentation and testing-support tasks. Every member must still be able to run, explain and make a small change to the application for the academic viva.

## Members and ownership

| Member | Primary role | Owned scope |
|---|---|---|
| Ganpat Singh | Project lead, database architect and integration lead | Architecture, SQL Server/EF Core, shared schema and migrations, reference-price administration, offer/price-discovery business rules, transaction integrity, integration and final review |
| Rahul Bora | Authentication and account workflow developer | ASP.NET Core Identity, registration/login/logout, roles, authorization, farmer/buyer profiles; later support for order views/status workflow and related API work as assigned |
| Gautam | Marketplace developer | Crop catalogue, harvest-lot CRUD, listing lifecycle, marketplace browsing, search and filters |
| Aakash Singh | UI and documentation support | Low-risk, separately scoped Razor pages (such as Help/About), isolated CSS/presentation tasks, sample content, screenshots, user-guide/report sections and assigned manual UI tests. No ownership of core database, authentication, offer-negotiation or transaction logic. |

## Suggested task boundaries

### Ganpat
- Own the shared data model and EF Core integration.
- Resolve cross-module technical decisions.
- Implement reference-price and core price-comparison/offer business rules.
- Coordinate transaction safety and order consistency.
- Integrate member branches and keep `main` stable.

### Rahul
- Own Identity and authentication.
- Implement role-aware farmer and buyer profile flows.
- Implement assigned account-related views and APIs.
- Support the order display/status module after the authentication foundation is stable.

### Gautam
- Own crop master data and harvest-lot CRUD.
- Implement buyer marketplace pages and filters.
- Enforce listing ownership and availability rules in coordination with Ganpat.

### Aakash
- Complete only small tasks with clear acceptance criteria and a limited file set.
- Examples: Help/About Razor page, a dedicated CSS file for static/support pages, simple UI copy, screenshot collection, user instructions and manual UI checks.
- Make at least one small, understandable code contribution and be prepared to explain it during viva.
- Do not assign critical security, schema, concurrency, pricing or offer-acceptance logic to this role.

## Shared responsibilities

All four members must understand the overall application at a high level, know how to clone/build/run it, understand their own contribution, keep assigned task cards updated, test their work and report blockers.

## Simplified GitHub workflow

- `main` is the stable integration branch.
- Each member pushes assigned work to a personal branch: `team/ganpat`, `team/rahul`, `team/gautam`, or `team/aakash`.
- Members do not need to create or review pull requests.
- After a task is pushed and local tests pass, its owner adds the branch/commit/test result to the task issue and moves the card to Done.
- Ganpat handles integration into `main`, conflict resolution and phase-level regression tests.
- A task being Done does not automatically mean the phase gate is approved.

## Phase checkpoints

- Start: confirm task owner, scope, files, dependencies and acceptance tests.
- During work: update the board and report blockers.
- Completion: push the change and record commit/test evidence.
- Integration: Ganpat checks the integrated result.
- Phase gate: the team approves only after the combined application passes required tests.

GitHub Project access and private repository access are separate permissions; verify both for each member.
