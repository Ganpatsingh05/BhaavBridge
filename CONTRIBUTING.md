# Contributing to BhaavBridge

This document defines the team's minimum collaboration rules.

## 1. Branch strategy

- `main` is the stable integration branch.
- Do not push unfinished feature work directly to `main`.
- Create one branch per issue or tightly related task.
- Naming convention:
  - `feature/<issue-number>-<short-name>`
  - `fix/<issue-number>-<short-name>`
  - `docs/<issue-number>-<short-name>`

Example: `feature/12-farmer-profile`.

## 2. Issue-first development

1. Select an issue from the BhaavBridge project tracker.
2. Confirm its phase, owner, acceptance criteria and due date.
3. Move the task to In Progress.
4. Create a branch linked to the issue.
5. Implement the change and add tests.
6. Open a pull request and reference the issue using `Closes #<issue-number>` when appropriate.

## 3. Pull-request requirements

Every pull request must:
- explain what changed and why;
- link the relevant issue;
- list tests performed and their results;
- identify database migrations or configuration changes;
- include screenshots for UI changes where useful;
- mention known limitations;
- be reviewed by at least one other team member before merge.

Do not merge code with failing tests, unresolved critical bugs or unreviewed schema changes.

## 4. Database change policy

- Shared entity and relationship changes must be discussed before implementation.
- Add EF Core migrations for schema changes.
- Never silently edit or delete an already-applied migration to conceal a schema change.
- Test migrations against a development database.
- Do not commit real credentials or production connection strings.

## 5. Coding conventions

- Use clear, descriptive C# names.
- Keep controllers focused on HTTP/MVC orchestration.
- Put reusable business rules in services.
- Use ViewModels for view-specific input/output where appropriate.
- Validate input on the server; client-side validation is supplementary.
- Use asynchronous EF Core operations for database I/O where appropriate.
- Handle expected failures and return meaningful messages/status codes.
- Avoid hard-coded application records in controllers or views.

## 6. Phase gates

A phase can be approved only after:
1. implementation checklist is complete;
2. normal, invalid and edge cases have been tested;
3. peer testing is complete;
4. defects are fixed and regression tests pass;
5. documentation is updated;
6. the project lead records the approval.

A task marked Done must satisfy its acceptance criteria. Do not use Done as a synonym for “code written”.

## 7. Secrets and data

Never commit:
- passwords or access tokens;
- real database credentials;
- personal information of farmers/buyers;
- private keys or API secrets.

Use local user secrets or environment variables for development configuration.
