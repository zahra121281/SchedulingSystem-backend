# Scheduling System Backend

This is the backend component of the Scheduling System project, which provides the necessary APIs and functionalities to manage users and tasks.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)

## Introduction

The Scheduling System Backend is built using ASP.NET Core and Microsoft SQL Server to facilitate user authentication and task management functionalities. It allows users to sign up, create tasks, assign tasks to others, and perform CRUD operations on both user and task objects.

## Features

- User authentication (sign up, login)
- Task creation, updating, and deletion
- Task assignment to other users
- Full CRUD operations for users and tasks

## Technologies Used

- ASP.NET Core
- Microsoft SQL Server
- ASP.NET Core Identity
- Entity Framework Core
- Dependency Injection in ASP.NET Core

## Installation

To set up the Scheduling System Backend locally, follow these steps:

1. Clone this repository: `git clone <repository-url>`
2. Navigate to the project directory: `cd scheduling-system-backend`
3. Update the database connection string in `appsettings.json` to point to your Microsoft SQL Server instance.
4. Apply database migrations: `dotnet ef database update`
5. Run the application: `dotnet run`

## Usage

After installation, you can interact with the backend through HTTP requests to the provided API endpoints. See [API Endpoints](#api-endpoints) section for details on available endpoints and their usage.

## API Endpoints

run the swagger service to see all the available endpoints :)  

