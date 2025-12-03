# API Testing Guide

This guide provides step-by-step instructions to test all the API endpoints.

## Setup

1. Start the API:
   ```bash
   dotnet run
   ```

2. The API will be available at:
   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`
   - Swagger UI: `https://localhost:5001/swagger`

## Test Scenarios

### 1. Register a New User

**Request:**
```bash
curl -k -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "email": "john@doe.com",
    "password": "password123"
  }'
```

**Expected Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Save the token for subsequent requests!**

### 2. Try to Register with Same Email (Should Fail)

**Request:**
```bash
curl -k -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jane Doe",
    "email": "john@doe.com",
    "password": "password456"
  }'
```

**Expected Response (400 Bad Request):**
```json
{
  "message": "Email already registered"
}
```

### 3. Login with Correct Credentials

**Request:**
```bash
curl -k -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@doe.com",
    "password": "password123"
  }'
```

**Expected Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 4. Login with Wrong Password (Should Fail)

**Request:**
```bash
curl -k -X POST https://localhost:5001/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@doe.com",
    "password": "wrongpassword"
  }'
```

**Expected Response (401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

### 5. Create a Todo (Without Token - Should Fail)

**Request:**
```bash
curl -k -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Buy groceries",
    "description": "Buy milk, eggs, and bread"
  }'
```

**Expected Response (401 Unauthorized):**
```json
{
  "message": "Unauthorized"
}
```

### 6. Create a Todo (With Token - Should Succeed)

**Replace `YOUR_TOKEN` with the token from login/register:**

**Request:**
```bash
curl -k -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "title": "Buy groceries",
    "description": "Buy milk, eggs, and bread"
  }'
```

**Expected Response (201 Created):**
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

### 7. Create More Todos

```bash
curl -k -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "title": "Pay bills",
    "description": "Pay electricity and water bills"
  }'

curl -k -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "title": "Complete project",
    "description": "Finish the Todo API project"
  }'
```

### 8. Get All Todos (Paginated)

**Request:**
```bash
curl -k -X GET "https://localhost:5001/todos?page=1&limit=10" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Expected Response (200 OK):**
```json
{
  "data": [
    {
      "id": 3,
      "title": "Complete project",
      "description": "Finish the Todo API project",
      "isCompleted": false,
      "createdAt": "2025-12-03T10:02:00Z",
      "updatedAt": null
    },
    {
      "id": 2,
      "title": "Pay bills",
      "description": "Pay electricity and water bills",
      "isCompleted": false,
      "createdAt": "2025-12-03T10:01:00Z",
      "updatedAt": null
    },
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
  "total": 3
}
```

### 9. Update a Todo

**Request:**
```bash
curl -k -X PUT https://localhost:5001/todos/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "title": "Buy groceries",
    "description": "Buy milk, eggs, bread, and cheese",
    "isCompleted": true
  }'
```

**Expected Response (200 OK):**
```json
{
  "id": 1,
  "title": "Buy groceries",
  "description": "Buy milk, eggs, bread, and cheese",
  "isCompleted": true,
  "createdAt": "2025-12-03T10:00:00Z",
  "updatedAt": "2025-12-03T10:05:00Z"
}
```

### 10. Try to Update Another User's Todo (Should Fail)

**First, register a second user and get their token:**
```bash
curl -k -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jane Smith",
    "email": "jane@smith.com",
    "password": "password456"
  }'
```

**Then try to update the first user's todo with the second user's token:**
```bash
curl -k -X PUT https://localhost:5001/todos/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SECOND_USER_TOKEN" \
  -d '{
    "title": "Hacked todo"
  }'
```

**Expected Response (403 Forbidden):**
```json
{
  "message": "Forbidden"
}
```

### 11. Delete a Todo

**Request:**
```bash
curl -k -X DELETE https://localhost:5001/todos/1 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Expected Response (204 No Content):**
(No response body)

### 12. Try to Delete Non-Existent Todo

**Request:**
```bash
curl -k -X DELETE https://localhost:5001/todos/999 \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Expected Response (404 Not Found):**
```json
{
  "message": "Todo item not found"
}
```

### 13. Test Pagination

**Create 15 todos, then test pagination:**

```bash
# Get first page (10 items)
curl -k -X GET "https://localhost:5001/todos?page=1&limit=10" \
  -H "Authorization: Bearer YOUR_TOKEN"

# Get second page (5 items)
curl -k -X GET "https://localhost:5001/todos?page=2&limit=10" \
  -H "Authorization: Bearer YOUR_TOKEN"

# Get with custom page size
curl -k -X GET "https://localhost:5001/todos?page=1&limit=5" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## Testing with Swagger UI

1. Open your browser and navigate to: `https://localhost:5001/swagger`
2. Accept the self-signed certificate warning
3. Use the Swagger UI to test all endpoints interactively
4. Click "Authorize" and enter: `Bearer YOUR_TOKEN` (replace YOUR_TOKEN with your actual token)
5. Test all endpoints through the UI

## Validation Tests

### Test Email Validation

```bash
curl -k -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "invalid-email",
    "password": "password123"
  }'
```

### Test Password Length

```bash
curl -k -X POST https://localhost:5001/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@test.com",
    "password": "123"
  }'
```

### Test Required Fields

```bash
curl -k -X POST https://localhost:5001/todos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "description": "Missing title field"
  }'
```

## Notes

- The `-k` flag in curl commands ignores SSL certificate warnings (useful for development with self-signed certificates)
- Replace `YOUR_TOKEN` with the actual JWT token received from login/register
- Tokens expire after 60 minutes by default (configurable in appsettings.json)
- The API uses SQLite, so the database file `todolist.db` will be created in the project root
- All timestamps are in UTC
