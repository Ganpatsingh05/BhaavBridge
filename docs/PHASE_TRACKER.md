# BhaavBridge — Phase Tracker and Exit Criteria

**Target submission:** 16 November 2026  
**Method:** Phase-gated; dependent work does not advance until the current gate is approved.

## Status definitions

- **Backlog:** not started.
- **Ready:** prerequisites are met and work may begin.
- **In Progress:** actively being implemented.
- **Testing:** implementation is complete and verification is underway.
- **Blocked:** progress depends on an unresolved issue.
- **Done:** acceptance criteria pass and the gate is approved.

## Universal phase gate

A phase can be marked Done only after:
1. implementation checklist is complete;
2. owner self-tests normal, invalid and edge-case inputs;
3. Ganpat performs integration checks; targeted peer testing is used for security, transaction and other high-risk business rules;
4. defects are fixed and regression tests pass;
5. Ganpat checks integrated code and documentation is updated;
6. Ganpat records the integration review and the team approves the phase.

A failed test keeps the phase open. Do not mark a phase Done merely because the code has been written.

## Phase 0 — Project Foundation
**Target:** 6–7 November 2026
**Lead:** Ganpat; all members verify local setup.

### Tasks
- [x] Finalize scope, abstract, roles and stack.
- [x] Instructor confirmation received: ASP.NET Core/C# from the INT402 syllabus is accepted for the selected Q1 project (reported by Ganpat, 6 November 2026).
- [ ] Create private GitHub repository and grant team access.
- [ ] Create the team Project board and configure fields/views.
- [ ] Agree branch, commit, pull-request and review rules.
- [ ] Install and verify agreed .NET SDK and SQL Server tooling.
- [ ] Create initial ASP.NET Core MVC solution.

### Tests
- [ ] All four members can clone the repository.
- [ ] All four members can build and run the starter solution.
- [ ] SQL Server is reachable from the development environment.
- [ ] Each member can push a change to their assigned personal branch and update the corresponding task card; Ganpat handles integration.
- [ ] Repository permissions are verified.

### Exit criteria
All members can reproduce the development setup; project scope and stack approval are documented; starter solution builds.

## Phase 1 — Database and Architecture
**Target:** 7–8 November 2026
**Lead:** Ganpat; all members review their entity requirements.

### Tasks
- [ ] Finalize ER diagram and entity definitions.
- [ ] Define PKs, FKs, cardinalities, nullability and constraints.
- [ ] Configure SQL Server and EF Core.
- [ ] Implement ApplicationDbContext and model configurations.
- [ ] Add and apply initial migrations.
- [ ] Establish folders, service-registration and controller conventions.
- [ ] Agree shared ViewModels and naming conventions.

### Tests
- [ ] Migrations create the expected schema.
- [ ] Relationships and constraints behave as designed.
- [ ] Sample records can be inserted, queried and updated via EF Core.
- [ ] Invalid foreign keys and invalid numeric values are rejected.
- [ ] Team reviews the ER diagram and schema; Aakash may support screenshots and documentation.

### Exit criteria
Database is reproducibly created from migrations, sample ORM operations pass and the team approves the shared schema.

## Phase 2 — Authentication and User Management
**Target:** 8–9 November 2026
**Owner:** Rahul Bora; Ganpat reviews Identity integration.

### Tasks
- [ ] Configure ASP.NET Core Identity.
- [ ] Implement farmer and buyer registration.
- [ ] Implement login and logout.
- [ ] Configure Farmer, Buyer and Administrator roles.
- [ ] Implement role-based authorization.
- [ ] Implement farmer and buyer profiles.
- [ ] Create role-specific dashboard routes.

### Tests
- [ ] Valid registration and login work.
- [ ] Duplicate accounts and invalid credentials are handled.
- [ ] Farmer cannot access buyer-only/admin-only routes.
- [ ] Buyer cannot access farmer-only/admin-only routes.
- [ ] Anonymous users cannot access protected pages.
- [ ] Logout invalidates the authenticated session/cookie as configured.
- [ ] Profile updates persist correctly.

### Exit criteria
Authentication, role assignment, profile CRUD and route protections pass peer testing.

## Phase 3 — Harvest Marketplace
**Target:** 9–10 November 2026
**Owner:** Gautam; Ganpat reviews persistence and ownership rules.

### Tasks
- [ ] Implement crop catalogue.
- [ ] Implement farmer harvest-lot creation.
- [ ] Implement edit, close and permitted delete operations.
- [ ] Add listing status and detail pages.
- [ ] Build buyer marketplace.
- [ ] Add search by crop.
- [ ] Add filters for location, quantity and price.
- [ ] Display only published/available lots to buyers.

### Tests
- [ ] Valid harvest lots save to SQL Server.
- [ ] Required fields are validated.
- [ ] Zero/negative quantity and negative prices are rejected.
- [ ] Farmers can update only their own lots.
- [ ] Closed lots cannot receive new offers.
- [ ] Search and filters return correct results.
- [ ] Listing updates appear after reload.

### Exit criteria
A farmer can publish and manage a lot; a buyer can discover and inspect it; authorization and validation tests pass.

## Phase 4 — Offer and Price Discovery Engine
**Target:** 10–11 November 2026
**Owner:** Aakash Singh; Ganpat reviews transactional data rules.

### Tasks
- [ ] Implement buyer offer submission.
- [ ] Implement farmer incoming-offer list.
- [ ] Implement accept/reject actions.
- [ ] Implement counteroffers and OfferRevision history.
- [ ] Build side-by-side offer comparison.
- [ ] Implement Net Daam calculation.
- [ ] Display estimated net return and per-quintal value.
- [ ] Prevent competing accepted offers on one lot.

### Tests
- [ ] Offers are linked to valid lots and buyers.
- [ ] Closed lots reject new offers.
- [ ] Buyers can view/manage only their own offers.
- [ ] Farmers can manage offers only on their own lots.
- [ ] Counteroffer revisions are preserved.
- [ ] Acceptance is atomic and only one offer can be accepted per lot.
- [ ] Invalid prices, quantities and costs are rejected.
- [ ] Calculator matches independently calculated test cases.
- [ ] Concurrent accept attempts cannot create multiple accepted offers.

### Exit criteria
Offer, counteroffer, comparison and Net Daam workflows pass functional, authorization and edge-case tests.

## Phase 5 — Orders and Market Intelligence
**Target:** 11–12 November 2026
**Owners:** Aakash (orders), Ganpat (reference prices and integration).

### Tasks
- [ ] Create an order from an accepted offer.
- [ ] Store agreed quantity, price and buyer/farmer references.
- [ ] Ensure one order per accepted offer.
- [ ] Implement permitted order status transitions.
- [ ] Create dated market-reference price records.
- [ ] Implement administrator price management.
- [ ] Display reference price, effective date and source.

### Tests
- [ ] Accepted offer creates exactly one order.
- [ ] Repeated requests do not create duplicate orders.
- [ ] Buyer and farmer see their own relevant orders.
- [ ] Other users cannot view or update the order.
- [ ] Invalid status transitions are rejected.
- [ ] Reference-price crop, market, date and source are displayed correctly.
- [ ] Historical dated records remain intact.

### Exit criteria
Accepted offers generate trackable orders and administrator-managed reference prices display with clear dates/sources.

## Phase 6 — REST API and Postman Testing
**Target:** 12–13 November 2026
**Lead:** Ganpat; feature owners verify API behaviour for their modules.

### Tasks
- [ ] Create API controllers and routes.
- [ ] Implement listing/crop GET and CRUD endpoints.
- [ ] Implement offer and counteroffer endpoints.
- [ ] Implement order and reference-price endpoints.
- [ ] Add request validation and consistent error responses.
- [ ] Apply authentication, role and ownership checks.
- [ ] Create Postman collection and environment without secrets.

### Tests
- [ ] Valid requests return expected response bodies/status codes.
- [ ] Invalid input returns validation errors.
- [ ] Missing records return not-found responses.
- [ ] Anonymous protected requests are rejected.
- [ ] Authenticated users without permission are forbidden.
- [ ] Ownership checks prevent cross-user access.
- [ ] All collection tests pass against a clean test dataset.

### Exit criteria
Documented endpoints pass Postman tests, including invalid requests and authorization cases.

## Phase 7 — UI Integration and Optimization
**Target:** 14 November 2026
**Owners:** all members; Ganpat coordinates integration.

### Tasks
- [ ] Standardize shared Razor layout, navigation and footer.
- [ ] Integrate farmer, buyer and admin dashboards.
- [ ] Standardize typography, spacing, colours and components.
- [ ] Add validation, empty, loading and error states.
- [ ] Make primary workflows usable on desktop and mobile.
- [ ] Remove placeholders and temporary test UI.

### Tests
- [ ] Role-specific navigation is correct.
- [ ] All links and forms work end-to-end.
- [ ] Search and offer workflows work through the UI.
- [ ] Responsive layouts remain usable.
- [ ] No broken assets or significant browser/server errors.
- [ ] Re-run critical workflow regression tests.

### Exit criteria
All modules appear and behave as one consistent application, with no broken primary workflow.

## Phase 8 — Final QA and Submission
**Target:** 15 November 2026  
**Owners:** all members.

### Tasks
- [ ] Execute farmer, buyer and administrator end-to-end tests.
- [ ] Run database integrity and migration tests.
- [ ] Run authentication and authorization regression tests.
- [ ] Run API/Postman regression collection.
- [ ] Fix critical and major defects.
- [ ] Prepare ER diagram and data mapping.
- [ ] Prepare screenshots and final report.
- [ ] Complete README and clean setup instructions.
- [ ] Record SDK, framework and package versions.
- [ ] Verify setup on another member's machine.
- [ ] Prepare each member's viva explanation.
- [ ] Freeze final release candidate.

### Tests
- [ ] Solution builds cleanly.
- [ ] Application starts without runtime errors.
- [ ] Core CRUD and business workflows pass.
- [ ] Net Daam calculations pass known test cases.
- [ ] Authorization and ownership tests pass.
- [ ] Postman collection passes.
- [ ] Fresh database setup works from migrations.
- [ ] No critical unresolved defects.
- [ ] All team members can run and explain the application.

### Exit criteria
Release candidate, documentation, database setup/backup instructions and report are complete and verified.

## Submission — 16 November 2026

No new features on submission day. Submit the tested source, required database material and report in the format confirmed by the instructor.

## Current governance notes

- The phase target dates are the team's original schedule and should be re-baselined if work has slipped.
- A task may be completed independently, but the next dependent phase must not be approved until its gate passes.
- Team members must update issue status and evidence as work progresses.
- Record blockers openly; do not hide a failed test by moving an item to Done.


## Repository phase issues

Use these issues as the detailed implementation and approval records:

| Phase | GitHub issue |
|---|---|
| Phase 0 — Project Foundation | [#1](https://github.com/Ganpatsingh05/BhaavBridge/issues/1) |
| Phase 1 — Database and Application Architecture | [#2](https://github.com/Ganpatsingh05/BhaavBridge/issues/2) |
| Phase 2 — Authentication and User Management | [#3](https://github.com/Ganpatsingh05/BhaavBridge/issues/3) |
| Phase 3 — Harvest Marketplace | [#4](https://github.com/Ganpatsingh05/BhaavBridge/issues/4) |
| Phase 4 — Offer and Price Discovery Engine | [#5](https://github.com/Ganpatsingh05/BhaavBridge/issues/5) |
| Phase 5 — Orders and Market Intelligence | [#6](https://github.com/Ganpatsingh05/BhaavBridge/issues/6) |
| Phase 6 — REST API and Postman Testing | [#7](https://github.com/Ganpatsingh05/BhaavBridge/issues/7) |
| Phase 7 — UI Integration and Optimization | [#8](https://github.com/Ganpatsingh05/BhaavBridge/issues/8) |
| Phase 8 — Final QA and Submission | [#9](https://github.com/Ganpatsingh05/BhaavBridge/issues/9) |

The issue tracker is the detailed source for each phase's implementation and tests. The GitHub Project board is the team's visual status view; add these nine issues to that board and use the Phase, Status, Gate Approval and date fields there.
