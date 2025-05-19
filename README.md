# OOP Coursework Project

## Description

This project is a part of an Object-Oriented Programming course and implements a web-platform for buy/sell advertisements

## Table of Contents

- [Features](#features)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)

## Features

- User registration and authentication
- Advertisement posting and browsing
- Real-time chat functionality

## Project Structure

```
oop_project/
├── oop_project/            # Main project logic (C# backend)
├── ui/                     # User Interface (e.g., Blazor, ASP.NET Core MVC)
│   ├── Components/         # UI components
│   ├── wwwroot/            # Static assets (CSS, JS, images)
│   ├── Program.cs          # Main entry point for the UI
│   └── *.json              # Configuration and data files (ads.json, chats.json, users.json)
├── tests_xunit/            # XUnit test projects
│   ├── *.cs                # Test files for different components
├── .git/                   # Git version control files
├── .github/                # GitHub specific files (e.g., workflows)
├── .vs/                    # Visual Studio specific files
├── oop_project.sln         # Visual Studio Solution file
├── README.md               # This file
└── .gitignore              # Specifies intentionally untracked files that Git should ignore
```

## Technologies Used

-   **Backend:** C# (.NET)
-   **Frontend (UI):** Blazor
-   **Testing:** XUnit
-   **Data Storage:** JSON files (for `ads.json`, `chats.json`, `users.json`)
