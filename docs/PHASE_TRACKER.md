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
**Target:** 8–9 November 2026 | **Status:** Implementation complete — final live checks and approval pending  
**Owner:** Rahul Bora; Ganpat reviews authentication and authorization.

### Implementation
- [x] ASP.NET Core Identity with EF Core stores and authentication middleware.
- [x] Farmer, Buyer and Administrator role seeding.
- [x] Farmer/Buyer registration, role assignment and profile persistence.
- [x] Login, logout and role-based redirects.
- [x] Protected dashboards, access-denied page and own-profile updates.
- [x] Development-only administrator bootstrap with User Secrets and safeguards.
- [x] Unique email, password, lockout and cookie configuration.
- [x] Fix registration redirect, lockout feedback and cookie configuration.

### Evidence reported by Rahul — 12 November 2026
- [x] Account lockout after repeated failed attempts.
- [x] Anonymous access to Farmer, Buyer and Admin dashboards redirects to Login.
- [x] Profile ownership protection reviewed.
- [x] Role seeding is idempotent after application restart.
- [x] 16 database tables, primary/foreign keys, unique indexes and 12 check constraints inspected.
- [x] EF Core context connected to SQL Server LocalDB.
- [x] Final solution build succeeded.
- [x] Cross-role access restrictions reported verified.

### Remaining integration tests
- [ ] Ganpat verifies latest `main` builds cleanly.
- [ ] Test Farmer registration end-to-end: Identity user, Farmer role and FarmerProfile created.
- [ ] Test Buyer registration end-to-end: Identity user, Buyer role and BuyerProfile created.
- [ ] Test duplicate email, weak password, mismatched confirmation and invalid credentials.
- [ ] Confirm successful Farmer/Buyer login, role-based redirects and logout.
- [ ] Confirm Farmer cannot access Buyer/Admin pages and Buyer cannot access Farmer/Admin pages.
- [ ] Test real profile updates and cross-account profile tampering.
- [ ] Configure a dedicated Development admin account and verify Admin dashboard access.
- [ ] Re-run the database smoke check.
- [ ] Ganpat records security review and team approval.

### Exit criteria
All registration/login/profile/lockout/authorization tests pass on the integrated application, the database smoke check passes, and Ganpat/team approve Issue #3.

---

## Phase 3 — Harvest Marketplace
**Target:** 13 November 2026 | **Status:** Implementation complete — live tests pending  
**Owner:** Gautam; Ganpat reviews persistence and ownership rules.

### Implementation delivered
- [x] Admin-only crop catalogue with create/edit/list and duplicate-name guard.
- [x] Farmer harvest-lot creation saved as Draft.
- [x] Draft lot editing, publishing and closing with ownership checks.
- [x] Farmer lot list/details with status.
- [x] Buyer-only marketplace and Published-only buyer detail pages.
- [x] Crop, district, state, quantity and price filters.
- [x] Server-side validation and database constraints.
- [x] Role-aware navigation and dashboard quick actions.
- [x] Farmer-name projection and details ownership fixes.

Commits: `95ece85` (marketplace implementation), `16e0eb4` (Phase 2/3 audit fixes; commit reports build with zero warnings/errors).

### Live acceptance tests — Gautam to execute
- [ ] Admin creates/edits/deactivates a crop; duplicate crop name is rejected.
- [ ] Farmer creates a Draft lot; it appears in My Lots.
- [ ] Farmer edits own Draft; changes persist after reload.
- [ ] Farmer publishes the Draft; status changes to Published.
- [ ] Farmer closes a Published lot; it disappears from buyer marketplace.
- [ ] Farmer cannot edit Published/Closed lots.
- [ ] A second farmer cannot view/edit another farmer's lot by changing the URL ID.
- [ ] Required fields and zero/negative quantity or negative price are rejected.
- [ ] Buyer sees Published lots only and can open lot details.
- [ ] Crop/district/state/min-max quantity/min-max asking-price filters return expected rows.
- [ ] Draft/Closed lots are not shown, including direct buyer detail URL.
- [ ] Anonymous and wrong-role access is denied/redirected.
- [ ] Run latest solution build and database smoke check.
- [ ] Add screenshots and test outcomes to [Issue #4](https://github.com/Ganpatsingh05/BhaavBridge/issues/4).

### Scope note
Published/commercial lots are closed rather than hard-deleted to preserve history. Offer submission belongs to Phase 4.

### Exit criteria
Gautam completes the live marketplace matrix, Ganpat reviews ownership and persistence evidence, and the team approves Issue #4.

---

## Phase 4 — Offer and Price Discovery Engine
**Target:** 13–14 November 2026 | **Status:** Not implemented  
**Lead:** Ganpat — offer, counteroffer, acceptance integrity and Net Daam. Rahul supports authorization; Gautam supports marketplace integration; Aakash handles optional low-risk presentation/documentation only.

### Agreed first-release rules
- One complete lot per accepted offer; no partial offers or partial acceptance.
- One active offer per buyer per lot.
- Counteroffers are appended to OfferRevision; Offer stores current terms.
- Only one offer can be accepted per lot.
- Accepting an offer closes the lot; order creation follows in Phase 5.
- Offer and revision history are not hard-deleted.

### Implementation
- [ ] Buyer submits an offer for a Published lot; quantity equals the full lot quantity.
- [ ] Farmer sees incoming offers only for their own lots; buyer sees only their own offers.
- [ ] Farmer accepts, rejects or counteroffers offers on their own lot.
- [ ] Counteroffer creates OfferRevision and updates current Offer terms.
- [ ] Build side-by-side offer comparison.
- [ ] Implement Net Daam using decimal arithmetic: gross = price × quantity; net = gross − farmer-borne costs.
- [ ] Display estimated net return and per-quintal value.
- [ ] Accept an offer transactionally, close the lot and prevent competing accepted offers.
- [ ] Validate all prices, quantities and farmer-borne costs server-side.

### Tests
- [ ] Closed/Draft lots reject offers; ownership checks prevent cross-user access.
- [ ] Counteroffer history is preserved.
- [ ] Net Daam cases match independent calculations.
- [ ] Duplicate/concurrent acceptance cannot accept multiple offers for one lot.
- [ ] Invalid prices, quantities and costs are rejected.
- [ ] Build and database smoke check pass.

### Exit criteria
Offer/counteroffer, comparison, Net Daam and one-accepted-offer-per-lot integrity pass functional, authorization and edge-case tests; team approves Issue #5.

---

## Phase 5 — Orders and Market Intelligence
**Target:** 14 November 2026 | **Status:** Not implemented  
**Lead:** Ganpat — order creation, transaction integrity and dated market-reference prices. Rahul supports authorization; Gautam supports marketplace integration; Aakash handles optional low-risk presentation/documentation only.

### Implementation
- [ ] Accepting an offer creates exactly one Order transactionally.
- [ ] Store agreed quantity, price, gross value and farmer-cost snapshots on Order.
- [ ] Enforce one Order per accepted Offer.
- [ ] Implement permitted order status transitions.
- [ ] Buyer sees orders associated with their offers; farmer sees orders associated with their lots.
- [ ] Admin manages dated market-reference prices.
- [ ] Display crop, market, reference price, effective date and source.
- [ ] Preserve historical reference-price records and order snapshots.

### Tests
- [ ] One accepted offer creates one order; duplicate acceptance does not create another.
- [ ] Buyer/farmer cannot view or update another user's order.
- [ ] Invalid status transitions are rejected.
- [ ] Snapshots remain unchanged if source offer terms later change.
- [ ] Market-reference crop, market, date and source display correctly.
- [ ] Build and database smoke check pass.

### Exit criteria
Accepted offers generate trackable orders, authorization passes, and dated reference prices appear with their source; team approves Issue #6.

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
