# BhaavBridge — Team Responsibilities

## Members and ownership

| Member | Primary role | Owned module |
|---|---|---|
| Ganpat Singh | Project lead, database architect and integration lead | Architecture, SQL Server/EF Core, shared schema, reference-price administration, integration and final review |
| Rahul Bora | Authentication and user-management developer | ASP.NET Core Identity, registration/login/logout, roles, profiles and access tests |
| Gautam | Marketplace developer | Crop catalogue, harvest-lot CRUD, marketplace browsing, search and filters |
| Aakash Singh | Offer and transaction developer | Offers, counteroffers, OfferRevision history, Net Daam comparison and order workflow |

## Shared responsibilities

All four members must:
- understand the application workflow and ER diagram;
- review cross-module changes that affect shared entities;
- perform peer testing for another member's work;
- keep issues, branches and pull requests updated;
- contribute to documentation and the final report;
- be able to run and explain the complete application during viva.

## Assignment rules

- Assign a task to its module owner once their exact GitHub username is confirmed.
- Ganpat coordinates cross-module dependencies and integration.
- Do not infer GitHub usernames from real names.
- Team access to the GitHub Project board and access to the private code repository are separate permissions and must both be verified.

## Collaboration checkpoints

- At the start of a phase: confirm scope, owner, dependencies and exit tests.
- During implementation: report blockers promptly.
- Before merge: peer review and required tests.
- At the phase gate: review test evidence, unresolved defects and documentation before approval.
