# Backend Advanced

## Setup xUnit Project

1. Create new project

    ```sh
    dotnet new sln --name BackendAdvanced
    ```

2. Create new xUnit Project

    ```sh
    dotnet new xunit --name Tests
    ```

3. Connect project to solution

    ```sh
    dotnet sln add Tests
    ```

    > [!TIP]
    >
    > If `dotnet sln add Tests` fails. You can use direct reference
    > `dotnet sln add Tests/Tests.csproj`
    > It might work better.

4. Verify that tests run

    ```sh
    dotnet tests
    ```

5. Add a .gitignore for C# and .NET files

    - bin directory
    - obj directory

## Overview


![Module Overview](/docs/backend-advanced-overview.excalidraw.png)

- Week 1 - TDD

    - Overview
        ![Overview](/docs/week1.excalidraw.png)
    - Internal/External Interfaces
    ![Internal/External Interfaces](/docs/internal-external-interfaces.excalidraw.png)
    - Solution Draft
    ![Solution draft](/docs/solution-draft.excalidraw.png)

### [Technology Reference](/docs/technology-references.md)

## Sketches

![TDD Rythm](/docs/tdd-rythm.excalidraw.png)

## Nix and Flakes

Nix and Flakes are a solution for ensuring reproducible development environments. You can safely ignore the following files:

- [/.envrc](.envrc)
- [/flake.lock](flake.lock)
- [/flake.nix](flake.nix)
