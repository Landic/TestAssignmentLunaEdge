# Task Management API

This is a task management API project built with .NET Core. It provides functionality for user registration, login, task management, and secure JWT-based authentication.

## Setup Instructions

Follow the steps below to run this project locally.

### Prerequisites

Before you begin, ensure you have the following tools installed:

- .NET 8.0
- MySQL or MariaDB server
- A tool like Postman or Insomnia to test API endpoints

### 1. Clone the Repository

Start by cloning the repository to your local machine:

git clone [https://github.com/Landic/TestAssignmentLunaEdge.git]
cd your-repository-name

### 2. Set Up the Database

Ensure that you have MySQL or MariaDB set up and running on your local machine. Create a new database and update the `appsettings.json` file with your connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=taskmanager;User=root;Password=yourpassword;"
  }
}

### 3. Install Dependencies

Run the following command to restore the project dependencies:

dotnet restore

### 4. Apply Migrations

To apply the migrations and create the database schema, run:

dotnet ef database update

This will create the necessary tables in your database based on the models in the application.

### 5. Run the Project

To run the project locally, use the following command:

dotnet run

The API will be accessible at `http://localhost:7142`.

## API Documentation

### Authentication

#### Register User

- **Endpoint**: `POST /api/user/register`
- **Request Body**:

  {
    "username": "user123",
    "email": "user@example.com",
    "password": "strongpassword"
  }

- **Response**:

  {
    "message": "User registered successfully."
  }

#### Login User

- **Endpoint**: `POST /api/user/login`
- **Request Body**:

  {
    "usernameOrEmail": "user@example.com",
    "password": "strongpassword"
  }

- **Response**:

  {
    "token": "jwt_token_here"
  }

  The response will include a JWT token that must be included in the `Authorization` header for subsequent requests.

### Task Management

#### Get All Tasks

- **Endpoint**: `GET /api/task`
- **Headers**:
  - `Authorization: Bearer {jwt_token}`

- **Response**:

  [
    {
      "id": "task-id-1",
      "title": "Task 1",
      "description": "Description of task 1",
      "dueDate": "2024-12-31T23:59:59Z",
      "status": "Pending",
      "priority": "High"
    },
    ...
  ]

#### Get Task by ID

- **Endpoint**: `GET /api/task/{id}`
- **Headers**:
  - `Authorization: Bearer {jwt_token}`

- **Response**:

  {
    "id": "task-id-1",
    "title": "Task 1",
    "description": "Description of task 1",
    "dueDate": "2024-12-31T23:59:59Z",
    "status": "Pending",
    "priority": "High"
  }

#### Create Task

- **Endpoint**: `POST /api/task`
- **Request Body**:

  {
    "title": "New Task",
    "description": "Description of new task",
    "dueDate": "2024-12-31T23:59:59Z",
    "status": "Pending",
    "priority": "Medium"
  }

- **Response**:
  {
    "message": "Task created successfully."
  }

#### Update Task

- **Endpoint**: `PUT /api/task/{id}`
- **Request Body**:
  {
    "title": "Updated Task",
    "description": "Updated description",
    "dueDate": "2024-12-31T23:59:59Z",
    "status": "In Progress",
    "priority": "High"
  }

- **Response**:
  {
    "message": "Task updated successfully."
  }

#### Delete Task

- **Endpoint**: `DELETE /api/task/{id}`
- **Response**:
  {
    "message": "Task deleted successfully."
  }
  

## Architecture and Design Choices

### 1. **Architecture Overview**

This application follows a layered architecture with the following components:

- **API Layer**: Contains controllers to handle HTTP requests and return responses.
- **Service Layer**: Contains business logic and communicates with repositories to interact with the database.
- **Repository Layer**: Encapsulates the data access logic, interacting with the database via Entity Framework.

### 2. **JWT Authentication**

- **JWT** (JSON Web Token) is used for secure authentication. After logging in, a user receives a JWT token, which must be included in the `Authorization` header for accessing protected routes.
- The token includes claims (user information) and is signed with a secret key, ensuring its integrity.

### 3. **DTOs (Data Transfer Objects)**

DTOs are used to define the shape of data exchanged between layers and with the client. This helps in avoiding direct exposure of entity models and enables flexibility in the API responses. Examples of DTOs used include:
- `RegisterUserDto`
- `LoginUserDto`
- `TaskDTO`

### 4. **Error Handling**

- Errors are caught using try-catch blocks, and appropriate HTTP status codes are returned to the client (e.g., `BadRequest`, `Unauthorized`, `NotFound`, `Forbid`).
- Custom error messages are provided in the response to help the client understand the issue.

### 5. **Dependency Injection**

- The application uses dependency injection to manage the dependencies between services and repositories. Services like `UserService`, `TaskService`, and `JWTService` are registered in the `Startup.cs` and injected into controllers as needed.
