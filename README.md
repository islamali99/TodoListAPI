# Todo List API

A RESTful API for managing to-do lists with user authentication built with ASP.NET Core 9.0.

## Features

- ✅ User registration and authentication with JWT tokens
- ✅ Password hashing with BCrypt
- ✅ CRUD operations for todo items
- ✅ User-specific todo lists
- ✅ Pagination support
- ✅ Authorization and ownership validation
- ✅ SQLite database with Entity Framework Core
- ✅ Input validation
- ✅ Proper error handling

## Tech Stack

- ASP.NET Core 9.0
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- BCrypt.Net for password hashing

## Getting Started

### Prerequisites

- .NET 9.0 SDK

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Update the JWT secret key in `appsettings.json` (for production):
   ```json
   "JwtSettings": {
     "SecretKey": "your-secure-secret-key-here-minimum-32-characters"
   }
   ```

5. Run the application:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`)

### Database

The application uses SQLite and will automatically create and migrate the database on startup. The database file `todolist.db` will be created in the project root directory.

## API Endpoints

### Authentication

#### Register a New User
```http
POST /auth/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@doe.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Login
```http
POST /auth/login
Content-Type: application/json

{
  "email": "john@doe.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Todo Items

All todo endpoints require authentication. Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-token>
```

#### Get All Todos (Paginated)
```http
GET /todos?page=1&limit=10
Authorization: Bearer <your-token>
```

**Response:**
```json
{
  "data": [
    {
      "id": 1,
      "title": "Buy groceries",
      "description": "Buy milk, eggs, and bread",
      "isCompleted": false,
      "createdAt": "2025-12-03T10:00:00Z",
      "updatedAt": null
    }
  ],
  "page": 1,
  "limit": 10,
  "total": 1
}
```

#### Create a Todo
```http
POST /todos
Authorization: Bearer <your-token>
Content-Type: application/json

{
  "title": "Buy groceries",
  "description": "Buy milk, eggs, and bread"
}
```

**Response:**
```json
{
  "id": 1,
  "title": "Buy groceries",
  "description": "Buy milk, eggs, and bread",
  "isCompleted": false,
  "createdAt": "2025-12-03T10:00:00Z",
  "updatedAt": null
}
```

#### Update a Todo
```http
PUT /todos/1
Authorization: Bearer <your-token>
Content-Type: application/json

{
  "title": "Buy groceries",
  "description": "Buy milk, eggs, bread, and cheese",
  "isCompleted": true
}
```

**Response:**
```json
{
  "id": 1,
  "title": "Buy groceries",
  "description": "Buy milk, eggs, bread, and cheese",
  "isCompleted": true,
  "createdAt": "2025-12-03T10:00:00Z",
  "updatedAt": "2025-12-03T11:00:00Z"
}
```

#### Delete a Todo
```http
DELETE /todos/1
Authorization: Bearer <your-token>
```

**Response:** `204 No Content`

## Error Responses

### 401 Unauthorized
```json
{
  "message": "Unauthorized"
}
```

### 403 Forbidden
```json
{
  "message": "Forbidden"
}
```

### 404 Not Found
```json
{
  "message": "Todo item not found"
}
```

### 400 Bad Request
```json
{
  "message": "Email already registered"
}
```

## Security Features

- Passwords are hashed using BCrypt before storage
- JWT tokens for stateless authentication
- Authorization checks ensure users can only access their own todos
- Input validation on all endpoints
- HTTPS enforcement in production

## Testing the API

You can test the API using:
- **Swagger UI**: Available at `https://localhost:5001/swagger` when running in development mode
- **curl**: See examples below
- **Postman** or any other API testing tool

### Example curl commands

```bash
# Register a new user
curl -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@doe.com","password":"password123"}'

# Login
curl -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"john@doe.com","password":"password123"}'

# Create a todo (replace TOKEN with your JWT token)
curl -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TOKEN" \
  -d '{"title":"Buy groceries","description":"Buy milk, eggs, and bread"}'

# Get all todos
curl -X GET "https://localhost:5001/todos?page=1&limit=10" \
  -H "Authorization: Bearer TOKEN"

# Update a todo
curl -X PUT https://localhost:5001/todos/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer TOKEN" \
  -d '{"title":"Buy groceries","description":"Updated description","isCompleted":true}'

# Delete a todo
curl -X DELETE https://localhost:5001/todos/1 \
  -H "Authorization: Bearer TOKEN"
```

## Project Structure

```
TodoListAPI/
├── Controllers/
│   ├── AuthController.cs       # Registration and login endpoints
│   └── TodosController.cs      # CRUD operations for todos
├── Data/
│   └── ApplicationDbContext.cs # Entity Framework DbContext
├── DTOs/
│   ├── AuthResponse.cs
│   ├── CreateTodoRequest.cs
│   ├── ErrorResponse.cs
│   ├── LoginRequest.cs
│   ├── RegisterRequest.cs
│   ├── TodoItemResponse.cs
│   ├── TodoListResponse.cs
│   └── UpdateTodoRequest.cs
├── Models/
│   ├── TodoItem.cs            # Todo entity
│   └── User.cs                # User entity
├── Services/
│   └── AuthService.cs         # JWT token generation and password hashing
├── Program.cs                 # Application configuration
└── appsettings.json          # Configuration settings
```

## License

This project is open source and available for educational purposes.
