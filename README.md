# TodoDDD

A Domain-Driven Design (DDD) implementation of a Todo application built with modern security practices.

## Features

- **Task Management**: Create, update, delete, and organize todo items
- **Priority Levels**: Set task priorities (High, Medium, Low)
- **Due Dates**: Schedule tasks with deadline tracking
- **Categories**: Organize tasks into custom categories
- **Search & Filter**: Find tasks by keywords, status, or category
- **User Management**: Multi-user support with personal task lists

## Security Measures

- **Authentication**: Secure user login with JWT tokens
- **Authorization**: Role-based access control (RBAC)
- **Input Validation**: Server-side validation for all user inputs
- **SQL Injection Protection**: Parameterized queries and ORM usage
- **CORS Configuration**: Controlled cross-origin resource sharing
- **Rate Limiting**: API request throttling to prevent abuse
- **Data Encryption**: Sensitive data encrypted at rest and in transit

## Getting Started

### Running with .NET Aspire

1. Install .NET 8.0 SDK and .NET Aspire workload:
    ```bash
    dotnet workload install aspire
    ```

2. Clone the repository:
    ```bash
    git clone <repository-url>
    cd TodoDDD
    ```

3. Run the Aspire app host:
    ```bash
    dotnet run --project TodoDDD.AppHost
    ```

4. Open the Aspire dashboard at `https://localhost:15888` to monitor the application

### Traditional Setup

1. Clone the repository
2. Install dependencies
3. Configure environment variables
4. Run database migrations
5. Start the application

## Tech Stack

- Backend: .NET Core / ASP.NET
- Database: SQL Server / PostgreSQL
- Authentication: JWT
- Architecture: Domain-Driven Design (DDD)
- Orchestration: .NET Aspire