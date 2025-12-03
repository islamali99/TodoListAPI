#!/bin/bash

# Test Script for Todo List API
# Run this script while the API is running (dotnet run in another terminal)

BASE_URL="http://localhost:5000"

echo "=== Todo List API Test Script ==="
echo ""

# Test 1: Register a new user
echo "1. Testing User Registration..."
REGISTER_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com","password":"password123"}')
echo "Response: $REGISTER_RESPONSE"
TOKEN=$(echo $REGISTER_RESPONSE | grep -o '"token":"[^"]*' | cut -d'"' -f4)
echo "Token: $TOKEN"
echo ""

# Test 2: Login
echo "2. Testing User Login..."
LOGIN_RESPONSE=$(curl -s -X POST "$BASE_URL/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password123"}')
echo "Response: $LOGIN_RESPONSE"
echo ""

# Test 3: Create a todo without token (should fail)
echo "3. Testing Create Todo Without Token (Should Fail)..."
curl -s -X POST "$BASE_URL/todos" \
  -H "Content-Type: application/json" \
  -d '{"title":"Buy groceries","description":"Buy milk, eggs, and bread"}'
echo ""
echo ""

# Test 4: Create a todo with token
echo "4. Testing Create Todo With Token..."
CREATE_RESPONSE=$(curl -s -X POST "$BASE_URL/todos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Buy groceries","description":"Buy milk, eggs, and bread"}')
echo "Response: $CREATE_RESPONSE"
echo ""

# Test 5: Create more todos
echo "5. Creating More Todos..."
curl -s -X POST "$BASE_URL/todos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Pay bills","description":"Pay electricity and water bills"}' > /dev/null
curl -s -X POST "$BASE_URL/todos" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Complete project","description":"Finish the Todo API project"}' > /dev/null
echo "Created 2 more todos"
echo ""

# Test 6: Get all todos
echo "6. Testing Get All Todos (Paginated)..."
curl -s -X GET "$BASE_URL/todos?page=1&limit=10" \
  -H "Authorization: Bearer $TOKEN" | jq '.'
echo ""

# Test 7: Update a todo
echo "7. Testing Update Todo..."
UPDATE_RESPONSE=$(curl -s -X PUT "$BASE_URL/todos/1" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"title":"Buy groceries","description":"Buy milk, eggs, bread, and cheese","isCompleted":true}')
echo "Response: $UPDATE_RESPONSE"
echo ""

# Test 8: Delete a todo
echo "8. Testing Delete Todo..."
curl -s -X DELETE "$BASE_URL/todos/2" \
  -H "Authorization: Bearer $TOKEN" \
  -w "\nHTTP Status: %{http_code}\n"
echo ""

# Test 9: Get updated todos list
echo "9. Testing Get Updated Todos List..."
curl -s -X GET "$BASE_URL/todos?page=1&limit=10" \
  -H "Authorization: Bearer $TOKEN" | jq '.'
echo ""

echo "=== All Tests Completed ==="
