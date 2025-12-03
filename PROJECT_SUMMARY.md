# Todo List API - Project Summary

## Overview

A fully-featured RESTful API for managing to-do lists with user authentication, built with ASP.NET Core 9.0. This project implements all required features including user registration, JWT-based authentication, CRUD operations, pagination, and comprehensive security measures.

## ✅ Implemented Features

### Core Requirements

1. **User Authentication**
   - ✅ User registration with email validation
   - ✅ Password hashing using BCrypt
   - ✅ JWT token generation and validation
   - ✅ Secure login with email and password
   - ✅ Token-based authentication for protected routes

2. **CRUD Operations**
   - ✅ Create todo items
   - ✅ Read todo items with pagination
   - ✅ Update todo items
   - ✅ Delete todo items
   - ✅ User ownership validation

3. **Database**
   - ✅ SQLite database with Entity Framework Core
   - ✅ Proper schema design with relationships
   - ✅ Automatic migrations on startup
   - ✅ Indexed email field for performance

4. **Security**
   - ✅ Password hashing (BCrypt)
   - ✅ JWT token authentication
   - ✅ Authorization checks (users can only access their own todos)
   - ✅ Proper HTTP status codes (401, 403, 404, etc.)
   - ✅ Input validation on all endpoints

5. **Data Validation**
   - ✅ Required fields validation
   - ✅ Email format validation
   - ✅ Password length validation (minimum 6 characters)
   - ✅ String length constraints
   - ✅ Unique email validation

6. **Pagination & Filtering**
   - ✅ Page-based pagination
   - ✅ Configurable page size (default: 10, max: 100)
   - ✅ Total count in response
   - ✅ Ordered by creation date (newest first)

7. **Error Handling**
   - ✅ Proper error responses with descriptive messages
   - ✅ HTTP status codes (200, 201, 204, 400, 401, 403, 404)
   - ✅ Consistent error response format
   - ✅ Validation error messages

## 🏗️ Architecture

### Project Structure
```
TodoListAPI/
├── Controllers/          # API endpoints
│   ├── AuthController.cs       # /auth/register, /auth/login
│   └── TodosController.cs      # /todos CRUD endpoints
├── Models/              # Domain entities
│   ├── User.cs                # User entity
│   └── TodoItem.cs            # Todo entity
├── DTOs/                # Data Transfer Objects
│   ├── RegisterRequest.cs
│   ├── LoginRequest.cs
│   ├── CreateTodoRequest.cs
│   ├── UpdateTodoRequest.cs
│   ├── AuthResponse.cs
│   ├── TodoItemResponse.cs
│   ├── TodoListResponse.cs
│   └── ErrorResponse.cs
├── Data/                # Database context
│   └── ApplicationDbContext.cs
├── Services/            # Business logic
│   └── AuthService.cs         # JWT & password hashing
├── Migrations/          # EF Core migrations
└── Program.cs          # Application configuration
```

### Technology Stack
- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core 9.0
- **Authentication**: JWT Bearer tokens
- **Password Hashing**: BCrypt.Net
- **Documentation**: Swagger/OpenAPI

## 📋 API Endpoints

### Authentication Endpoints

#### POST /auth/register
Register a new user and receive a JWT token.

**Request:**
```json
{
  "name": "John Doe",
  "email": "john@doe.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### POST /auth/login
Authenticate user and receive a JWT token.

**Request:**
```json
{
  "email": "john@doe.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Todo Endpoints (Require Authentication)

All endpoints require: `Authorization: Bearer <token>`

#### GET /todos?page=1&limit=10
Get paginated list of user's todos.

**Response (200 OK):**
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

#### POST /todos
Create a new todo item.

**Request:**
```json
{
  "title": "Buy groceries",
  "description": "Buy milk, eggs, and bread"
}
```

**Response (201 Created):**
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

#### PUT /todos/{id}
Update an existing todo (only owner can update).

**Request:**
```json
{
  "title": "Buy groceries",
  "description": "Updated description",
  "isCompleted": true
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "title": "Buy groceries",
  "description": "Updated description",
  "isCompleted": true,
  "createdAt": "2025-12-03T10:00:00Z",
  "updatedAt": "2025-12-03T11:00:00Z"
}
```

#### DELETE /todos/{id}
Delete a todo (only owner can delete).

**Response:** 204 No Content

## 🔒 Security Implementation

### Password Security
- Passwords are hashed using BCrypt with salt rounds
- Plain text passwords are never stored
- Password minimum length: 6 characters

### JWT Authentication
- Tokens expire after 60 minutes (configurable)
- Tokens contain user ID, email, and name
- HMAC SHA256 signing algorithm
- Issuer and audience validation

### Authorization
- Each todo belongs to a specific user
- Users can only view, update, or delete their own todos
- Ownership validation on all CRUD operations
- Proper 403 Forbidden responses for unauthorized access

### Input Validation
- Required fields validation
- String length constraints
- Email format validation
- Type safety with C# strong typing

## 🚀 Running the Application

### Prerequisites
- .NET 9.0 SDK

### Setup & Run
```bash
# Navigate to project directory
cd TodoListAPI

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `https://localhost:5001/swagger`

### Database
- SQLite database (`todolist.db`) is created automatically
- Migrations are applied on startup
- No manual database setup required

## 🧪 Testing

### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Test all endpoints interactively
3. Use the "Authorize" button to add your JWT token

### Using Test Script
```bash
# Make sure the API is running
./test-api.sh
```

### Manual Testing
See `TESTING.md` for detailed curl examples and test scenarios.

## 📊 Database Schema

### Users Table
- Id (Primary Key, Auto-increment)
- Name (Required, Max 100 chars)
- Email (Required, Unique, Max 255 chars, Indexed)
- PasswordHash (Required)
- CreatedAt (Timestamp)

### TodoItems Table
- Id (Primary Key, Auto-increment)
- Title (Required, Max 200 chars)
- Description (Optional, Max 1000 chars)
- IsCompleted (Boolean, Default: false)
- CreatedAt (Timestamp)
- UpdatedAt (Nullable Timestamp)
- UserId (Foreign Key to Users, Cascade Delete)

## 🎯 Key Features

### Pagination
- Default page size: 10
- Maximum page size: 100
- Returns total count for UI implementation
- Ordered by creation date (newest first)

### Error Handling
- Consistent error response format
- Descriptive error messages
- Proper HTTP status codes
- Validation error details

### Configuration
All settings configurable in `appsettings.json`:
- JWT secret key
- Token expiration time
- Database connection string
- Issuer and audience

## 📝 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=todolist.db"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key-min-32-chars-long",
    "Issuer": "TodoListAPI",
    "Audience": "TodoListAPI",
    "ExpirationMinutes": "60"
  }
}
```

**Important:** Change the `SecretKey` in production!

## 🎓 Learning Outcomes

This project demonstrates:
- RESTful API design principles
- JWT authentication implementation
- Password hashing and security best practices
- Entity Framework Core with SQLite
- Dependency injection in ASP.NET Core
- Repository pattern (through DbContext)
- DTO pattern for request/response
- Input validation and error handling
- Authorization and ownership validation
- Pagination implementation
- Database schema design
- One-to-many relationships
- API documentation with Swagger

## 📦 Dependencies

- Microsoft.EntityFrameworkCore.Sqlite (9.0.0)
- Microsoft.EntityFrameworkCore.Design (9.0.0)
- Microsoft.AspNetCore.Authentication.JwtBearer (9.0.0)
- BCrypt.Net-Next (4.0.3)
- System.IdentityModel.Tokens.Jwt (8.1.2)
- Swashbuckle.AspNetCore (6.9.0)

## 🔄 Future Enhancements (Bonus Features)

Potential additions:
- Refresh token mechanism
- Rate limiting and throttling
- Unit tests
- Filtering and sorting options
- Todo categories/tags
- Due dates and reminders
- Soft delete functionality
- Audit logging
- CORS configuration
- Health check endpoints

## 📄 Documentation Files

- `README.md` - Main documentation and setup guide
- `TESTING.md` - Detailed testing guide with curl examples
- `PROJECT_SUMMARY.md` - This file - comprehensive project overview
- `test-api.sh` - Automated test script

## ✅ Requirements Checklist

- [x] User registration endpoint
- [x] Login endpoint with token generation
- [x] CRUD operations for todos
- [x] User authentication on protected routes
- [x] Authorization (users can only access their own todos)
- [x] Error handling with proper status codes
- [x] Security measures (password hashing, JWT)
- [x] Database integration (SQLite)
- [x] Data validation on all inputs
- [x] Pagination for todo list
- [x] Proper API documentation
- [x] RESTful design principles

## 🎉 Conclusion

This Todo List API project successfully implements all required features including:
- Secure user authentication with JWT
- Complete CRUD operations
- Database integration with proper relationships
- Comprehensive security measures
- Input validation and error handling
- Pagination support

The project is production-ready with proper security, error handling, and follows ASP.NET Core best practices.
