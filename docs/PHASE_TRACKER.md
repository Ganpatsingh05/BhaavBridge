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
**Target:** 7–8 November 2026 | **Status:** Implementation complete — independent clean-database verification pending  
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
**Target:** 8–9 November 2026 | **Status:** ✅ Done — closed 14 November 2026  
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

### Final gate
- [ ] Ganpat verifies latest `main` builds cleanly.
- [ ] Test Farmer registration end-to-end: Identity user, Farmer role and FarmerProfile created.
- [ ] Test Buyer registration end-to-end: Identity user, Buyer role and BuyerProfile created.
- [ ] Test duplicate email, weak password, mismatched confirmation and invalid credentials.
- [ ] Confirm successful Farmer/Buyer login, role-based redirects and logout.
- [ ] Confirm Farmer cannot access Buyer/Admin pages and Buyer cannot access Farmer/Admin pages.
- [ ] Test real profile updates and cross-account profile tampering.
- [ ] Configure a dedicated Development admin account and verify Admin dashboard access.
- [ ] Re-run the database smoke check.
- [x] Ganpat reviewed the Identity integration/security fixes and approved Phase 2 on 14 November 2026.

### Exit criteria
Rahul's reported owner verification and Ganpat's integration/security review were accepted; Issue #3 was closed on 14 November 2026. The next full integrated build/regression run is tracked under Phase 4/final QA.

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
**Target:** 14 November 2026 | **Status:** Implementation complete — latest build and end-to-end tests pending  
**Lead:** Ganpat; Rahul owns the submitted implementation; Gautam supports marketplace integration.

### Implementation delivered — Rahul, commit `8e487f5`
- [x] Buyer offer submission on Published lots with server-controlled full-lot quantity.
- [x] Initial OfferRevision history record.
- [x] One active buyer/lot offer guarded by application check and filtered unique index.
- [x] Buyer My Offers page and negotiation history.
- [x] Farmer Incoming page and per-lot side-by-side comparison sorted by Net Daam per quintal.
- [x] Farmer accept/reject/counteroffer actions.
- [x] Buyer accept/reject counteroffer actions.
- [x] Net Daam and per-quintal calculations using decimal arithmetic.
- [x] Acceptance creates an Order snapshot, marks the offer Accepted and closes the lot in a serializable transaction.
- [x] Role/ownership checks, POST actions and antiforgery validation.
- [x] Counteroffer acceptance farmer-profile loading and form redisplay fix (commit `fe2f03c`).

### Remaining
- [ ] Build and run live Buyer/Farmer offer workflow.
- [ ] Verify OfferRevision history after multiple counteroffers.
- [ ] Verify direct farmer acceptance and buyer counteroffer acceptance each create one correct Order.
- [ ] Verify reject paths create no Order.
- [ ] Test Net Daam with independent numeric cases.
- [ ] Verify one accepted offer per lot, including repeated/concurrent attempts.
- [ ] Verify cross-user and wrong-role access.
- [ ] Re-run database smoke check and record screenshots.
- [ ] Ganpat reviews and team approves Issue #5.

### Exit criteria
Buyer/farmer negotiation, revision history, Net Daam and acceptance/order integrity pass live functional, authorization and transaction tests.

## Phase 5 — Orders and Market Intelligence
**Target:** 15 November 2026 | **Status:** Implementation complete — build and final flow check pending
**Lead:** Ganpat; Rahul supports authorization; Gautam supports integration.

### Implemented
- [x] Order snapshot is created transactionally when an offer is accepted (Phase 4).
- [x] Order stores price, quantity, gross, farmer costs and estimated net return snapshots.
- [x] Unique OfferId relationship enforces one order per accepted offer.
- [x] Buyer/Farmer/Admin order list actions and role-scoped data.
- [x] Authorized order details and permitted status transitions.
- [x] Admin market-reference price create/edit; authenticated price list/filter.
- [x] Market price forms validate min/modal/max and record source/date.
- [x] Order/market price views, dashboard cards and navigation.

### Remaining
- [ ] Latest build passes.
- [ ] Test one accepted order through list/details/status.
- [ ] Admin creates a dated reference price; Farmer/Buyer can view it.
- [ ] Existing database smoke check passes.

### Exit criteria
Build, order flow and reference-price flow pass on the integrated application; Ganpat approves Issue #6.

---
## Phase 6 — REST API and Postman Testing
**Target:** 15 November 2026 | **Status:** API implementation and Postman collection delivered — local run pending
**Lead:** Ganpat; Rahul supports Identity/authorization; Gautam checks listing API.

### Implemented
- [x] Attribute-routed API controllers mapped in Program.cs.
- [x] Crops GET/POST/PUT.
- [x] Produce lots GET/list/detail, POST, PUT Draft, publish and close.
- [x] Offers POST, mine/incoming GET and farmer counteroffer POST.
- [x] Orders GET list/detail with caller ownership scope.
- [x] Market-reference prices GET with filters.
- [x] Role/ownership checks, DTO validation and API 401/403 cookie responses.
- [x] API endpoint guide and Postman collection/environment created.
- [x] Expected status assertions added to Postman requests.

### Remaining
- [ ] Build latest main.
- [ ] Import collection/environment and run with Farmer/Buyer/Admin cookies.
- [ ] Verify 401, wrong-role and ownership behaviour.
- [ ] Save Postman evidence without real cookie/token values.
- [ ] Run database smoke check.

### Exit criteria
API project builds, Postman happy-path and authorization requests pass, and documentation/collection are committed.

---
## Phase 7 — UI Integration and Optimization
**Target:** 15 November 2026 | **Status:** ⬜ Backlog  
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
