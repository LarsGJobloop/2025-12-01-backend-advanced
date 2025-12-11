# Backend Advanced

A service-oriented architecture project demonstrating TDD practices with two independent services: AssetManagementService and ReservationService.

![Overview](/docs/sketches/solution-overveiw.excalidraw.png)

## Getting Started

### Prerequisites

- .NET 10.0 SDK

> [!NOTE]
>
> A Nix flake Dev Shell is also provided. So if you have Nix set up on your system
> you can just `git clone` -> `nix develop` and be up and running with the correct versions.

### Running Tests

```sh
dotnet test
```

### Running Services

**Start compose file** (and rebuild the images):

```sh
docker compose up --build
```

**Clean everything up**:

```sh
docker compose down --volumes --remove-orphans
```

**Log into the PostgreSQL Admin Web UI**:

1. Go to http://localhost:5050
2. Log in with the credentials from [compose.yaml#pgadmin](/compose.yaml)

   - `username = admin@example.com`
   - `password = password`

3. Connect to the database using [compose.yaml#postgres](/compose.yaml)

   - `database = backend`
   - `host = postgres` <- This is whatever name you called the service
   - `user = postgres` <- Default PostgreSQL username
   - `password = admin` <- Configurable through Environment Variables

4. Peruse your database

### Adding a New Service

See [docs/setup-new-service.md](docs/setup-new-service.md) for instructions on setting up a new service with its test suite.

### Walking Through the Commit History

> [!IMPORTANT]
>
> **The commit history is intended to be replayed step-by-step.** It follows TDD conventions (red → green → refactor) and serves as a **replayable walkthrough** of the development process.

```sh
# View commit history
git log --oneline

# View a specific commit
git show <commit-hash>

# Checkout a specific commit to see the code at that point
git checkout <commit-hash>

# Return to latest
git checkout main
```

## Project Structure

- `src/AssetManagementService/` - Service for managing assets (registration, retrieval, listing)
- `src/ReservationService/` - Service for managing reservations (creation, validation, overlap detection)
- `src/Contracts/` - Shared contracts (DTOs) between services
- `tests/` - Integration tests for both services

## Overview

![Module Overview](/docs/sketches/backend-advanced-overview.excalidraw.png)

- Week 1 - TDD

  - Overview
    ![Overview](/docs/sketches/week1.excalidraw.png)
  - Internal/External Interfaces
    ![Internal/External Interfaces](/docs/sketches/internal-external-interfaces.excalidraw.png)
  - Solution Draft
    ![Solution draft](/docs/sketches/solution-draft.excalidraw.png)
  - [Week 1 Checklist](week-1-checklist.md) - Suggested scenarios to consider for your services

- Week 2 - SOA

  - Overview
    ![Overview](/docs/sketches/week2.excalidraw.png)
  - Service examples
    ![Services in real life](/docs/sketches/soa-in-real-life.excalidraw.png)

### [Technology Reference](/docs/technology-references.md)

## Sketches

![TDD Rythm](/docs/sketches/tdd-rythm.excalidraw.png)

## Nix and Flakes

Nix and Flakes are a solution for ensuring reproducible development environments. You can safely ignore the following files:

- [/.envrc](.envrc)
- [/flake.lock](flake.lock)
- [/flake.nix](flake.nix)
