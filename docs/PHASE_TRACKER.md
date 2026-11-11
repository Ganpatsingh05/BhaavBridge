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
2. owners commit their assigned changes to the shared `main` branch and run local checks;
3. the team runs integrated checks; targeted peer testing is used for security, authorization, transaction and other high-risk business rules;
4. defects are fixed and regression tests pass;
5. the project lead checks the integrated code and documentation is updated;
6. the team records approval of the phase.

A failed test keeps the phase open. Do not mark a phase Done merely because the code has been written.

---

## Phase 0 — Project Foundation
**Target:** 6–7 November 2026 | **Status:** ✅ Done  
**Lead:** Ganpat; onboarding continues for all members.

### Tasks
- [x] Finalize scope, abstract, roles and stack.
- [x] Instructor confirmation received: ASP.NET Core/C# from the INT402 syllabus is accepted for Q1.
- [x] Create the private GitHub repository and send collaborator invitations.
- [x] Create the GitHub Project board and add the phase/task issues.
- [x] Agree a single shared `main` branch workflow; routine feature branches and pull requests are not required.
- [x] Verify .NET 10 SDK, ASP.NET Core MVC template, Git, SQL Server 2025 and SSMS 22 on Ganpat's machine.
- [x] Create the ASP.NET Core MVC starter solution.

### Tests
- [x] Ganpat independently restored, built and ran the starter.
- [x] HTTP-to-HTTPS redirect verified.
- [x] SQL Server `localhost\\SQLEXPRESS` connected in SSMS and `BhaavBridge_Dev` verified `ONLINE`.
- [x] Rahul reviewed and approved the starter, confirming the .NET 10 target, MVC structure and successful build.
- [x] Starter PR #10 merged into `main` on 7 November 2026.
- [x] Gautam completed clone/build/run onboarding — commit `58a39d7` (11 November 2026).
- [ ] Aakash completes clone/build/run onboarding.
- [ ] Confirm all members can access the GitHub Project board.

### Exit criteria
The stack is approved, the starter is integrated into `main`, and the project lead plus one additional developer have verified/reviewed the starter. Remaining member onboarding is tracked in [Issue #11](https://github.com/Ganpatsingh05/BhaavBridge/issues/11) and must be completed before those members begin their assigned feature work.

---

## Phase 1 — Database and Architecture
**Target:** 7–8 November 2026 | **Status:** ⚠️ Implementation Done — Gate Pending Rahul Verification  
**Lead:** Ganpat; Rahul performs the independent migration-recreation check.

### Tasks
- [x] Finalize the ER diagram, entity definitions and first-release rules.
- [x] Add EF Core SQL Server, Design, Identity and tool references at version 10.0.14.
- [x] Implement ApplicationUser, all eight entities and ApplicationDbContext.
- [x] Configure relationships, precision, indexes and check constraints.
- [x] Configure SQL Server and User Secrets.
- [x] Generate and apply migration `20261107110530_InitialCreate` to `BhaavBridge_Dev`.
- [x] Commit all migration files to `main` (commit `5b716cb`).
- [x] Verify tables in SSMS.
- [x] Pass the repeatable database smoke check twice without compiler warnings.
- [ ] Rahul recreates the schema from the committed migration on `BhaavBridge_Rahul_Verification`.
- [ ] Rahul runs the database smoke check against that separate database.

### Tests
- [x] Solution builds and EF Core discovers ApplicationDbContext.
- [x] NuGet audit reports no vulnerable packages.
- [x] InitialCreate is recorded in EF migration history.
- [x] Entity insert/read checks, Net Daam arithmetic, invalid quantity constraint and rollback pass.
- [ ] Rahul confirms clean-database migration and smoke-check results.

### Exit criteria
The committed migration can recreate the full schema on another machine, the smoke check passes there, and the project lead/team approve the database gate. Close Phase 1 issue #2 only after these checks.

---

## Phase 2 — Authentication and User Management
**Target:** 8–9 November 2026 | **Status:** ⚠️ Implementation Done — Live Test Matrix Pending  
**Owner:** Rahul Bora; Ganpat reviews authentication and authorization.

### Tasks
- [x] Configure ASP.NET Core Identity with EF Core stores and authentication middleware.
- [x] Seed Farmer, Buyer and Administrator roles.
- [x] Implement farmer/buyer registration and profile persistence (transaction-wrapped).
- [x] Implement login, logout and role-based redirects (Farmer → Farmer dashboard, Buyer → Buyer dashboard, Admin → Admin dashboard).
- [x] Implement role-restricted dashboards and own-profile updates.
- [x] Add Development-only admin bootstrap using User Secrets and prevent promotion of Farmer/Buyer accounts.
- [x] Configure unique email, password requirements, lockout and cookie paths.
- [x] Fix: configure Identity application cookie on services (commit `01e6acd`).
- [x] Fix: lockout error message shown separately from wrong-password error (commit `16e0eb4`).
- [x] Fix: post-registration redirects to Login page so success message is immediately visible (commit `16e0eb4`).
- [ ] Run full registration/login/lockout/ownership/profile test matrix against live database.
- [ ] Re-run database smoke check and approve the Phase 2 gate.

### Reported evidence
Rahul — implementation commits `b192277`, `224de22`. Audit fixes by Gautam in `16e0eb4`. Build verified: 0 warnings, 0 errors.

### Exit criteria
Farmer/buyer registration, login/logout, role assignment, profile CRUD, lockout and route protections pass the final integration test matrix. Ganpat and the team approve the gate.

---

## Phase 3 — Harvest Marketplace
**Target:** 9–10 November 2026 | **Status:** ⚠️ Implementation Done — Live Test Pending  
**Owner:** Gautam; Ganpat reviews persistence and ownership rules.

### Tasks
- [x] Implement crop catalogue — Admin-only CropController with Create/Edit/List and duplicate-name guard (commit `95ece85`).
- [x] Implement farmer harvest-lot creation — ProduceLotController Create, saved as Draft with crop validity check (commit `95ece85`).
- [x] Implement edit and close operations — only Draft lots editable; any non-Closed lot can be closed; ownership enforced (commit `95ece85`).
- [x] Add listing status and detail pages — ProduceLot/Index, ProduceLot/Details with Draft/Published/Closed badges (commit `95ece85`).
- [x] Build buyer marketplace — MarketplaceController Index showing only Published lots (commit `95ece85`).
- [x] Add search by crop — Crop dropdown filter on Marketplace/Index (commit `95ece85`).
- [x] Add filters for location, quantity and price — District, State, Min/Max Quantity, Min/Max Price filters (commit `95ece85`).
- [x] Display only published/available lots to buyers — `Status == Published` enforced in query (commit `95ece85`).
- [x] Fix: FarmerName populated correctly via eager-loaded User navigation property (commit `16e0eb4`).
- [x] Fix: Details enforces ownership — farmer sees only their own lots regardless of status (commit `16e0eb4`).
- [x] Update Farmer/Buyer/Admin dashboards with quick-action cards linking to Phase 3 features (commit `95ece85`).
- [x] Update `_Layout.cshtml` with role-aware navbar (My Lots / Marketplace / Crops) (commit `95ece85`).

### Tests
- [ ] Valid harvest lots save to SQL Server — **run against live DB**.
- [ ] Required fields are validated — client + server validation active; needs browser test.
- [ ] Zero/negative quantity and negative prices are rejected — server-side `[Range]` validation + DB check constraints.
- [ ] Farmers can update only their own lots — ownership check coded and audit-verified.
- [ ] Closed lots cannot receive new offers — `Status == Closed` guard coded (Phase 4 will enforce this on offer submission).
- [ ] Search and filters return correct results — **run against live DB with seed data**.
- [ ] Listing updates appear after reload — **run against live DB**.

### Exit criteria
A farmer can publish and manage a lot; a buyer can discover and inspect it; authorization and validation tests pass.

> **Action required — Gautam:**
> 1. Open a terminal: `cd d:\BhaavBridge && dotnet run --project src\BhaavBridge.Web --launch-profile https`
> 2. Run the manual test checklist below.
> 3. Record results and commit hash `16e0eb4` in [Issue #4](https://github.com/Ganpatsingh05/BhaavBridge/issues/4).
> 4. Ask Ganpat to approve the gate.
>
> **Manual test checklist:**
> - [ ] Register as Farmer → success message on Login page → login → Farmer dashboard shows.
> - [ ] Register as Buyer → success message on Login page → login → Buyer dashboard shows.
> - [ ] Admin creates a Crop (requires AdminBootstrap secrets configured).
> - [ ] Farmer creates a Draft lot → lot appears in My Lots with Draft badge.
> - [ ] Farmer edits the Draft lot → changes saved.
> - [ ] Farmer publishes the lot → status changes to Published.
> - [ ] Buyer logs in → Marketplace shows the Published lot.
> - [ ] Buyer filters by crop/district/state → correct results returned.
> - [ ] Farmer closes the lot → lot disappears from buyer Marketplace.
> - [ ] Farmer tries to edit a Published lot → error message shown.
> - [ ] Buyer tries to access `/ProduceLot` directly → redirected to Login.
> - [ ] Farmer tries to access `/Marketplace` → redirected to Login.

---

## Phase 4 — Offer and Price Discovery Engine
**Target:** 10–11 November 2026 | **Status:** ⬜ Backlog  
**Lead:** Ganpat Singh for offer rules, acceptance integrity and Net Daam; Rahul supports authorization; Aakash may assist with offer comparison presentation only.

### Tasks
- [ ] Implement buyer offer submission and farmer incoming-offer workflow.
- [ ] Implement accept/reject and counteroffer business rules with OfferRevision history.
- [ ] Build offer comparison and Net Daam calculation.
- [ ] Display estimated net return and per-quintal value (Aakash may implement the presentation after the business rules are finalized).
- [ ] Enforce one accepted offer per lot transactionally.

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

---

## Phase 5 — Orders and Market Intelligence
**Target:** 11–12 November 2026 | **Status:** ⬜ Backlog  
**Lead:** Ganpat Singh for order creation, transaction integrity and market-reference prices; Aakash may assist with low-risk order-status presentation and documentation.

### Tasks
- [ ] Create an order transactionally from an accepted offer.
- [ ] Store agreed quantity, price and cost snapshots.
- [ ] Enforce one order per accepted offer.
- [ ] Implement and validate permitted order status transitions.
- [ ] Create dated market-reference price records and administrator management.
- [ ] Display reference price, effective date and source (Aakash may assist with presentation and documentation).

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

---

## Phase 6 — REST API and Postman Testing
**Target:** 12–13 November 2026 | **Status:** ⬜ Backlog  
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

---

## Phase 7 — UI Integration and Optimization
**Target:** 14 November 2026 | **Status:** ⬜ Backlog  
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

---

## Phase 8 — Final QA and Submission
**Target:** 15 November 2026 | **Status:** ⬜ Backlog  
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

---

## Submission — 16 November 2026

No new features on submission day. Submit the tested source, required database material and report in the format confirmed by the instructor.

---

## Current governance notes

- The phase target dates are the team's original schedule and should be re-baselined if work has slipped.
- A task may be completed independently, but the next dependent phase must not be approved until its gate passes.
- Team members must update issue status and evidence as work progresses.
- Record blockers openly; do not hide a failed test by moving an item to Done.

---

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
