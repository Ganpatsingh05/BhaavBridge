# Contributing to BhaavBridge

Our team uses one shared working branch: `main`. We keep the workflow simple and coordinate before editing shared files.

## 1. Daily workflow

1. Open the assigned task in the BhaavBridge Project board.
2. Move the task to `In Progress`.
3. Open PowerShell in the repository folder and run `git pull origin main`.
4. Work only on the files included in the assigned task.
5. Run `dotnet build` and the relevant tests.
6. Check `git status`, stage only the files changed for the task, and commit with a descriptive message.
7. Push to `main` using `git push origin main`.
8. Add the commit hash and test result to the task issue, then move the task to `Done`.

Example commands:

```powershell
git pull origin main
git status
dotnet build
git add <changed-files>
git commit -m "feat: describe the completed change"
git push origin main
```

## 2. Coordination rules

- `main` is the only shared development branch.
- We do not create personal or feature branches for routine work.
- Pull requests are not required for routine tasks.
- Before editing, check that another team member is not changing the same files.
- Pull the latest `main` before starting and before committing.
- If `git push` is rejected or a merge conflict appears, stop and coordinate with the project lead. Do not force-push.
- Do not overwrite another member's unfinished work.
- Keep commits small and describe the actual change.

## 3. File ownership

- Shared architecture, database context, entities, migrations and application configuration are coordinated by the project lead.
- Rahul owns authentication, authorization and profile implementation.
- Gautam owns crop catalogue, harvest lots and marketplace implementation.
- Aakash handles assigned low-risk UI, help/about pages, screenshots, user-guide content and manual UI tests.
- A member must coordinate before changing files owned by another module.

## 4. Database change policy

- Discuss shared entity and relationship changes before implementation.
- Keep EF Core migrations in source control.
- Never edit or delete an already-applied migration to conceal a schema change.
- Test migrations against the development database.
- Never commit passwords, access tokens or production credentials.

## 5. Coding conventions

- Use clear, descriptive C# names.
- Keep controllers focused on request orchestration.
- Put reusable business rules in services.
- Use ViewModels for view-specific input/output where appropriate.
- Validate input on the server; client-side validation is supplementary.
- Use asynchronous EF Core operations for database I/O.
- Handle expected failures and return meaningful messages/status codes.

## 6. Definition of done

A task is Done when its code is committed to `main`, its required local checks pass and the task card records the commit and test result.

A phase is Done only when the integrated application passes its phase-level tests, defects are fixed, documentation is updated and the team approves the gate. A task being Done does not automatically complete the phase.
