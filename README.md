## Git4PL2

**Git4PL2** is a plug-in for **PL/SQL Developer** that helps teams manage PL/SQL development through Git without leaving their everyday IDE.

The project was used in an enterprise production environment at a large insurance company. Its goal was to make Oracle DB object development safer and more transparent: save source code to a local Git repository, compare versions, inspect change history, and reduce the risk of conflicts in team workflows.

![Git4PL2 main screen](https://raw.githubusercontent.com/dbobylev/Git4PL2/master/screen.png)

![Git4PL2 diff screen](https://raw.githubusercontent.com/dbobylev/Git4PL2/master/screen2.png)

## Features

- **Save database objects to Git**  
  The plug-in reads the source code of the currently opened PL/SQL object from PL/SQL Developer and saves it to a local Git repository, preserving the schema and object type structure.

- **Load repository version back into the IDE**  
  Quickly replaces the current object text in PL/SQL Developer with the version stored in Git.

- **Git Diff inside PL/SQL Developer**  
  Shows differences between the current object text in the IDE and the version stored in the local repository. Includes line numbers, color highlighting, and navigation back to the corresponding line in the editor.

- **Git Blame for PL/SQL code**  
  Shows authorship information for selected lines and provides access to commit details.

- **Team Coding**  
  Provides a team-oriented object locking workflow: CheckOut / CheckIn, warnings when opening or compiling objects already used by another developer, and configurable restrictions for shared environments.

- **Safety warnings**  
  Checks the current Git branch before saving and checks the connected database server before loading code into the IDE. This helps avoid accidental changes in the wrong branch or environment.

- **Plug-in settings UI**  
  Includes configuration for repository paths, regular expressions, diff behavior, encoding, Team Coding, and supporting SQL queries.

- **Additional enterprise tools**  
  Includes helper windows for Dicti/Ftoggle and other Oracle DB workflows used in enterprise development.

## Tech Stack

- **C# / .NET Framework 4.7.2**
- **WPF** for the user interface
- **PL/SQL Developer Plug-In API** through unmanaged exports and callbacks
- **Oracle / PL/SQL** as the main development domain
- **Git CLI** for diff, blame, show, log, branch checks, and related operations
- **Ninject** for dependency injection
- **Serilog** for logging
- **Newtonsoft.Json** for JSON settings and the Team Coding file provider
- **NUnit / MSTest / Moq** for tests and test infrastructure

## Architecture

The solution is split into three main projects:

- `Git4PL2` - the main plug-in library.
- `Git4PL2.IDEStub` - a WPF stub application for local debugging without PL/SQL Developer.
- `Git4PL2.Tests` - Git API tests and infrastructure for creating temporary Git repositories.

The main project is organized into several modules:

- `IDE` - integration with PL/SQL Developer callbacks.
- `Git` - wrapper around Git CLI commands and command output parsing.
- `Plugin/Commands` - plug-in commands.
- `Plugin/Diff` - database object handling, PL/SQL formatting, and diff rendering.
- `Plugin/TeamCoding` - team object locking workflow.
- `Plugin/WPF` - windows, view models, styles, and converters.
- `Plugin/Settings` - settings model and settings UI.

## Main Workflows

1. A developer opens a PL/SQL object in PL/SQL Developer.
2. The plug-in detects the object type, owner, and name through the IDE API.
3. The corresponding file is resolved in the local Git repository.
4. The developer can:
   - compare the current text with Git;
   - save changes to the repository;
   - load the repository version back into the IDE;
   - inspect Git blame for selected lines;
   - perform Team Coding CheckOut / CheckIn.

## Project Status

This project was built as an internal engineering tool for real Oracle PL/SQL team development workflows. The code reflects the practical constraints of its time: integration with a legacy IDE, callback-based APIs, .NET Framework, and the need to solve day-to-day developer pain points quickly.

Despite those constraints, Git4PL2 was more than a pet project: it was used in an enterprise production environment and helped developers work more safely with PL/SQL code, Git history, and team-owned database changes.
