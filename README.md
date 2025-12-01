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

4. Verify that tests run

    ```sh
    dotnet tests
    ```

5. Add a .gitignore for C# and .NET files

    - bin directory
    - obj directory

## Overview

![Module Overview](/docs/backend-advanced-overview.excalidraw.png)

## Nix and Flakes

Nix and Flakes are a solution for ensuring reproducible development environments. You can safely ignore the following files:

- [/.envrc](.envrc)
- [/flake.lock](flake.lock)
- [/flake.nix](flake.nix)
