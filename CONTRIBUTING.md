# Contributing to BhaavBridge

This is a small student team. The workflow is intentionally simple: each developer completes an assigned task, pushes code to their own branch, and updates the GitHub task card. Ganpat coordinates integration into `main`.

## 1. Branch strategy

- `main` is the stable, integrated branch. Team members must not push directly to `main`.
- Each developer uses one personal working branch: `team/ganpat`, `team/rahul`, `team/gautam`, or `team/aakash`.
- Ganpat creates or prepares the personal branches after the starter application is merged into `main`.
- Do not create a new branch for every small task unless Ganpat asks.
- Do not force-push or rewrite shared branch history.

## 2. Simple task workflow for members

1. Open the assigned item in the BhaavBridge Project board.
2. Move its status to `In Progress`.
3. Switch to your assigned personal branch and pull its latest version.
4. Work only on your assigned files/module.
5. Run `dotnet build` and any relevant tests.
6. Commit and push your changes to your personal branch.
7. Add a short comment to the GitHub task issue with the branch name, commit hash and tests performed.
8. Move the task card to `Done` only after the change is pushed and local checks pass.

Members are not required to create pull requests or review other members' pull requests. Ganpat handles integration and merging into `main`.

## 3. Commands members use

First-time setup after Ganpat provides the branch:

```powershell
git switch team/<your-name>
git pull
```

After completing a task:

```powershell
git status
git add <your-changed-files>
git commit -m "feat: describe your change"
git push
```

Use the exact branch name provided by Ganpat. Do not use `git push origin main`.

## 4. Integration and phase approval

- Ganpat fetches and integrates completed member branches into `main`.
- Ganpat resolves cross-module conflicts and checks the integrated build.
- A task may be marked Done by its owner after pushing and passing local checks.
- A phase is not approved until Ganpat's integration checks, phase-level tests, defect fixes and team gate approval are complete.
- Important security, authorization, database and transaction logic receives additional review by Ganpat.
- Members should report blockers in the task issue instead of silently changing shared files.

## 5. File ownership and coordination

- Ganpat owns shared architecture, `Program.cs`, database context, shared models, migrations and integration.
- Rahul owns Identity/authentication/profile files.
- Gautam owns crop catalogue, produce-lot and marketplace files.
- Aakash owns only explicitly assigned low-risk Razor views, isolated CSS, help/about content, screenshots and documentation/testing support.
- Do not edit shared files owned by another member without coordinating with Ganpat.
- Shared entity and relationship changes must be agreed before implementation.

## 6. Coding conventions

- Use clear, descriptive C# names.
- Keep controllers focused on request orchestration.
- Put reusable business rules in services.
- Use ViewModels for view-specific input/output where appropriate.
- Validate input on the server; client-side validation is supplementary.
- Use asynchronous EF Core operations for database I/O where appropriate.
- Handle expected failures and return meaningful messages/status codes.
- Avoid hard-coded application records in controllers or views.

## 7. Phase gates

A phase can be approved only after:
1. implementation checklist is complete;
2. owners' local tests pass;
3. Ganpat integrates the branches and verifies the combined build;
4. phase-level normal, invalid and edge-case tests pass;
5. defects are fixed and regression tests pass;
6. documentation is updated;
7. Ganpat records the approval and the team accepts the gate.

A task marked Done means the owner's work is pushed and locally tested. A phase marked Done means the integrated system passed the phase gate. These are different statuses.

## 8. Secrets and data

Never commit passwords, tokens, real database credentials, personal information, private keys or API secrets. Use local user secrets or environment variables for development configuration.
