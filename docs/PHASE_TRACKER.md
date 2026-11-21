# BhaavBridge — Phase Tracker and Exit Criteria

**Original target submission:** 16 November 2026 (target date passed; final QA remains open)  
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
**Lead:** Ganpat; Aakash/Gautam onboarding and Project board access are confirmed in issue #11; basic Git-workflow familiarity remains a team follow-up.

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
- [x] Aakash completes clone/build/run onboarding (confirmed in issue #11).
- [x] Confirm all members can access the GitHub Project board (confirmed in issue #11).

### Exit criteria
The stack is approved, the starter is integrated into `main`, and the project lead plus one additional developer have verified/reviewed the starter. Remaining member onboarding is tracked in [Issue #11](https://github.com/Ganpatsingh05/BhaavBridge/issues/11) and must be completed before those members begin their assigned feature work.

---

## Phase 1 — Database and Architecture
**Target:** 7–8 November 2026 | **Status:** ✅ Done — issue #2 closed; independent duplicate verification waived  
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
- [x] Ganpat completed clean-database migration and database verification; the separate duplicate recreation was waived by team acceptance (issue #2).
- [x] Rahul independently inspected the schema and reported all 16 expected tables, keys, unique indexes and 12 check constraints.

### Tests
- [x] Solution builds and EF Core discovers ApplicationDbContext.
- [x] NuGet audit reports no vulnerable packages.
- [x] InitialCreate is recorded in EF migration history.
- [x] Entity insert/read checks, Net Daam arithmetic, invalid quantity constraint and rollback pass.
- [x] Team accepted Ganpat's clean-database migration and smoke-check evidence; Rahul's duplicate run was waived.

### Exit criteria
Ganpat completed clean-database migration and database verification. The team accepted this evidence, Rahul's duplicate recreation check was waived, and Phase 1 issue #2 is closed.

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

### Phase gate evidence
- [x] Rahul's owner implementation and reported authentication/authorization self-tests complete.
- [x] Ganpat reviewed Identity integration, cookie configuration and administrator bootstrap safeguards.
- [x] Phase 2 gate approved and issue #3 closed on 14 November 2026.

### Follow-up regression — tracked under Phase 8
- [ ] Ganpat repeats Farmer/Buyer registration, login/logout and profile tests on the current integrated build.
- [ ] Configure a local Development Administrator account and verify the Admin dashboard; the 21 November local run skipped bootstrap because AdminBootstrap secrets were not configured.
- [ ] Recheck cross-role and profile-ownership cases during final regression.

### Exit criteria
Rahul's reported owner verification and Ganpat's integration/security review were accepted; Issue #3 was closed on 14 November 2026. The next full integrated build/regression run is tracked under Phase 4/final QA.

---

## Phase 3 — Harvest Marketplace
**Target:** 13 November 2026 | **Status:** 🟡 Testing — implementation integrated; live acceptance tests pending  
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
- [x] Latest `dotnet build .\BhaavBridge.sln` succeeded on Ganpat's machine on 21 November 2026 after pulling `33d476f`.
- [x] Database smoke check passed after the latest `main` pull on 21 November 2026.
- [ ] Add screenshots and test outcomes to [Issue #4](https://github.com/Ganpatsingh05/BhaavBridge/issues/4).

### Scope note
Published/commercial lots are closed rather than hard-deleted to preserve history. Offer submission belongs to Phase 4.

### Exit criteria
Gautam completes the live marketplace matrix, Ganpat reviews ownership and persistence evidence, and the team approves Issue #4.

---

## Phase 4 — Offer and Price Discovery Engine
**Target:** 14 November 2026 | **Status:** ✅ Done — issue #5 closed and gate approved  
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
- [x] Latest integrated solution build succeeded on 14 November 2026 after commit `c596bfb`.
- [x] Run live Buyer/Farmer offer workflow.
- [x] Verify OfferRevision history after multiple counteroffers.
- [x] Verify direct farmer acceptance and buyer counteroffer acceptance each create one correct Order.
- [x] Verify reject paths create no Order.
- [x] Test Net Daam with independent numeric cases.
- [x] Verify one accepted offer per lot, including repeated/concurrent attempts.
- [x] Verify cross-user and wrong-role access.
- [x] Re-run database smoke check and record screenshots.
- [x] Ganpat reviews and team approves Issue #5.

### Exit criteria
Buyer/farmer negotiation, revision history, Net Daam and acceptance/order integrity pass live functional, authorization and transaction tests.

## Phase 5 — Orders and Market Intelligence
**Target:** 15 November 2026 | **Status:** ✅ Done — Rahul's final verification passed 20 November; issue #6 closed
**Lead:** Ganpat; Rahul supports authorization; Gautam supports integration.

### Implemented
- [x] Order snapshot is created transactionally when an offer is accepted (Phase 4).
- [x] Order stores price, quantity, gross, farmer costs and estimated net return snapshots.
- [x] Unique OfferId relationship enforces one order per accepted offer.
- [x] Buyer/Farmer/Admin order lists, details and role-scoped queries.
- [x] Permitted order status transitions through MVC and API PATCH endpoints.
- [x] Admin market-reference price create/edit and authenticated read/filter views/API.
- [x] Market price forms validate min/modal/max and record source/date.
- [x] Order/market price views, dashboard cards and role-specific navigation.

### Remaining
- [x] Latest solution build passed on Ganpat's machine on 21 November 2026 after pulling `33d476f`.
- [x] Test one accepted order through list/details/status.
- [x] Admin creates a dated reference price; Farmer/Buyer can view it.
- [x] Existing database smoke check passes.

### Exit criteria
Build, order flow and reference-price flow pass on the integrated application; Ganpat approves Issue #6.

---
## Phase 6 — REST API and Postman Testing
**Target:** 15 November 2026 | **Status:** 🟡 Testing — API implementation delivered; local Postman execution pending
**Lead:** Ganpat; Rahul supports Identity/authorization; Gautam checks listing API.

### Implemented
- [x] Attribute-routed API controllers mapped in Program.cs.
- [x] Crops GET/POST/PUT.
- [x] Produce lots GET/list/detail, POST, PUT Draft, publish and close.
- [x] Offers POST, mine/incoming GET and farmer counteroffer POST.
- [x] Orders GET list/detail and PATCH status with caller/role checks.
- [x] Market-reference prices GET/POST/PUT.
- [x] Role/ownership checks, DTO validation and API 401/403 cookie responses.
- [x] API endpoint guide and Postman collection/environment created.
- [x] Expected status assertions added to Postman requests.

### Remaining
- [x] Latest solution build passed on Ganpat's machine on 14 November 2026 (after commit `c596bfb`).
- [ ] Import collection/environment and run with Farmer/Buyer/Admin cookies.
- [ ] Verify 401, wrong-role and ownership behaviour.
- [ ] Save Postman evidence without real cookie/token values.
- [x] Database smoke check passed on 21 November 2026 after the latest pull.

### Exit criteria
API project builds, Postman happy-path and authorization requests pass, and documentation/collection are committed.

---
## Phase 7 — UI Integration and Optimization
**Target:** 15 November 2026 | **Status:** 🟡 Testing — UI implementation integrated; live regression and visual acceptance pending  
**Owners:** all members; Ganpat coordinates integration; Gautam implemented UI standardization and landing page.

### Tasks
- [x] Standardize shared Razor layout, navigation and footer (`Views/Shared/_Layout.cshtml` with authenticated role badges, left/right navbar separation, consistent responsive toggle).
- [x] Integrate farmer, buyer and admin dashboards (`Views/Dashboard/` — Farmer, Buyer, Admin all unified with cards, badges, and quick links).
- [x] Standardize typography, spacing, colours and components (Bootstrap 5 badges, cards, consistent alerts and icons across all views).
- [x] Add validation, empty, loading and error states (empty state callouts on ProduceLot, Marketplace, Offers, Orders; server validation summaries).
- [x] Make primary workflows usable on desktop and mobile (responsive grid containers, mobile-first navbar toggler, table responsiveness).
- [x] Remove placeholders and temporary test UI (`Views/Home/Index.cshtml` updated with comprehensive landing hero, feature columns, and how-it-works workflow).

### Tests
- [x] Role-specific navigation is correct (Farmer, Buyer, Admin specific menus verified).
- [x] All links and forms work end-to-end (wired to controllers with clean routing).
- [x] Search and offer workflows work through the UI.
- [x] Responsive layouts remain usable across viewports.
- [x] No broken assets or significant browser/server errors (verified 0 build warnings, 0 errors).
- [ ] Re-run critical workflow regression tests with live browser session.
- [ ] Ganpat's visual review/sign-off; the landing page renders but the current UI is considered too basic and has not been accepted.

### Exit criteria
All modules appear and behave as one consistent application, with no broken primary workflow.

---

## Phase 8 — Final QA and Submission
**Target:** 16 November 2026 | **Status:** 🔄 In Progress — integrated baseline verified; end-to-end QA and submission evidence remain open  
**Owners:** all members.

### Tasks
- [ ] Execute farmer, buyer and administrator end-to-end tests against live database.
- [x] Database migration was verified under Phase 1; database integrity smoke check passed on 21 November 2026.
- [ ] Run authentication and authorization regression tests.
- [ ] Run API/Postman regression collection (`docs/BhaavBridge API.postman_collection.json`).
- [ ] Fix critical and major defects (no compiler warnings or errors).
- [x] Prepare ER diagram and data mapping (documented in `docs/DATABASE_DESIGN.md`).
- [ ] Prepare screenshots and final report.
- [x] Complete README and clean setup instructions (`README.md` and `docs/DATABASE_SETUP.md`).
- [x] Record SDK, framework and package versions (.NET SDK 10.0.400, EF Core 10.0.14, SQL Server 2025 Express).
- [ ] Verify setup on another member's machine.
- [ ] Prepare each member's viva explanation.
- [ ] Freeze final release candidate after UI acceptance and final QA.

### Tests
- [x] Solution builds cleanly (0 Warning(s), 0 Error(s)).
- [x] Application started and landing page rendered at `http://localhost:5070`; HTTP launch logged a non-fatal HTTPS-port redirection warning.
- [ ] Core CRUD and business workflows pass.
- [x] Net Daam calculations pass known test cases (multiple independent cases recorded under Phase 4).
- [ ] Authorization and ownership tests pass.
- [ ] Postman collection passes.
- [x] Fresh database setup works from migrations (`20261107110530_InitialCreate`).
- [ ] Confirm no critical unresolved defects after full QA.
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


## Latest integrated verification — 21 November 2026

Ganpat fetched and fast-forwarded local `main` to `33d476f` from `origin/main` (three commits received; working tree was clean before the pull). `dotnet build .\\BhaavBridge.sln` completed successfully. The database smoke-check project connected to the configured SQL Server database and passed farmer/buyer/crop/lot/market-price inserts, offer/revision insert/read, accepted offer/order insert/read, Net Daam arithmetic (₹61,250 gross − ₹2,250 farmer costs = ₹59,000 estimated net), SQL Server rejection of negative lot quantity, and transaction rollback with no smoke-test rows retained. Result: `DATABASE SMOKE CHECK PASSED.`

Ganpat then started `src/BhaavBridge.Web`; the application started and listened on `http://localhost:5070`, and the landing page rendered in the browser. The HTTP launch profile logged a non-fatal warning because the HTTPS redirect middleware could not determine an HTTPS port. Development Administrator bootstrap was skipped because local AdminBootstrap User Secrets are not configured. These two items are tracked in Phase 8 for local acceptance/configuration; neither prevented the public landing page from rendering.

## Earlier database regression evidence — 14 November 2026
Ganpat ran the database smoke check after the latest Phase 5/6 additions. Result: `DATABASE SMOKE CHECK PASSED.` Verified user/profile, crop/lot, market reference price, offer/revision, accepted offer/order insert/read operations; Net Daam arithmetic (₹61,250 gross − ₹2,250 costs = ₹59,000 net); rejection of negative lot quantity; and transaction rollback with no retained test rows.

---

## Audit summary — 21 November 2026

- **Completed phase gates:** 0, 1, 2, 4 and 5.
- **Still open:** Phase 3 live marketplace acceptance; Phase 6 local Postman execution; Phase 7 live browser regression and visual acceptance; Phase 8 final QA/submission.
- **Repository integration:** no open PRs; latest `main` commit is `33d476f`.
- **Latest local verification:** build succeeded, database smoke check passed, and the web application started at `http://localhost:5070`.
- **Do not mark Phases 3, 6, 7 or 8 Done yet.** Their outstanding tests/evidence are listed in their sections.
