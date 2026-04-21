# ?? Task Board System - API Documentation for UI Development

## ?? Project Overview

This document provides comprehensive API documentation for building a **modern, responsive Task Board (Kanban-style) web application** that integrates with the NeuraNx Task Board System .NET 8 Web API.

The system manages **Boards**, **Tasks**, **Comments**, and **Employee** assignments with full CRUD operations and advanced features like drag-and-drop status updates with optimistic concurrency control.

---

## ??? Backend API Integration

### **Base URL**: `https://localhost:7165/api`

### **Content Type**: `application/json`
### **Authentication**: Currently no authentication required (ready for JWT integration)

---

## ?? API Endpoints Overview

### **Boards Management** (`/api/boards`)
| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/boards` | Get all boards | None | `BoardDto[]` |
| `GET` | `/api/boards/{id}` | Get board by ID | None | `BoardDto` |
| `POST` | `/api/boards` | Create new board | `CreateBoardDto` | `BoardDto` |
| `PUT` | `/api/boards/{id}` | Update board | `UpdateBoardDto` | `BoardDto` |
| `DELETE` | `/api/boards/{id}` | Delete board | None | `204 No Content` |

### **Tasks Management** (`/api/tasks`)
| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/tasks` | Get all tasks | None | `TaskResponseDto[]` |
| `GET` | `/api/tasks?boardId={guid}` | Get tasks by board | None | `TaskResponseDto[]` |
| `GET` | `/api/tasks/assigned-to/{empId}` | Get tasks by employee | None | `TaskResponseDto[]` |
| `GET` | `/api/tasks/{id}` | Get task by ID | None | `TaskResponseDto` |
| `POST` | `/api/tasks` | Create new task | `CreateTaskDto` | `TaskResponseDto` |
| `PUT` | `/api/tasks/{id}` | Update task | `UpdateTaskDto` | `TaskResponseDto` |
| `PATCH` | `/api/tasks/{id}/status` | Update task status only | `UpdateTaskStatusDto` | `TaskResponseDto` |
| `DELETE` | `/api/tasks/{id}` | Delete task | None | `204 No Content` |

### **Comments Management** (`/api/comments`)
| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/comments/task/{taskId}` | Get comments for a specific task | None | `CommentDto[]` |
| `GET` | `/api/comments/{id}` | Get comment by ID | None | `CommentDto` |
| `POST` | `/api/comments` | Create new comment | `CreateCommentDto` | `CommentDto` |
| `PUT` | `/api/comments/{id}` | Update comment | `UpdateCommentDto` | `CommentDto` |
| `DELETE` | `/api/comments/{id}` | Delete comment | None | `204 No Content` |

### **Employees Management** (`/api/employees`)
| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `GET` | `/api/employees` | Get all employees | None | `EmployeeViewModel[]` |
| `GET` | `/api/employees/{id}` | Get employee by ID | None | `EmployeeViewModel` |
| `POST` | `/api/employees` | Create new employee | `EmployeeViewModel` | `EmployeeViewModel` |
| `PUT` | `/api/employees/{id}` | Update employee | `EmployeeViewModel` | `EmployeeViewModel` |
| `DELETE` | `/api/employees/{id}` | Delete employee | None | `204 No Content` |

---

## ?? Data Transfer Objects (DTOs)

### **Board DTOs**

#### `BoardDto` (Response)
```typescript
interface BoardDto {
  id: string;                    // GUID
  name: string;                  // Board name (max 200 chars)
  createdOn: string;             // ISO 8601 DateTime
  updatedOn: string;             // ISO 8601 DateTime  
  taskCount: number;             // Number of tasks in this board
}
```

#### `CreateBoardDto` (Request)
```typescript
interface CreateBoardDto {
  name: string;                  // Required, 1-200 characters
}
```

#### `UpdateBoardDto` (Request)
```typescript
interface UpdateBoardDto {
  name: string;                  // Required, 1-200 characters
}
```

---

### **Task DTOs**

#### `TaskResponseDto` (Response)
```typescript
interface TaskResponseDto {
  id: string;                    // GUID
  title: string;                 // Task title (max 500 chars)
  description?: string;          // Optional description (max 2000 chars)
  status: TaskStatus;            // 0=Todo, 1=InProgress, 2=Done
  priority: TaskPriority;        // 0=Low, 1=Medium, 2=High
  dueDate?: string;              // Optional ISO 8601 DateTime
  boardId: string;               // GUID of parent board
  boardName: string;             // Name of parent board
  assignedToId?: string;         // Optional GUID of assigned employee
  assignedToName?: string;       // Name of assigned employee
  createdOn: string;             // ISO 8601 DateTime
  updatedOn: string;             // ISO 8601 DateTime
  rowVersion: string;            // Base64 string for concurrency control
  commentCount: number;          // Number of comments on this task
}
```

#### `CreateTaskDto` (Request)
```typescript
interface CreateTaskDto {
  title: string;                 // Required, 1-500 characters
  description?: string;          // Optional, max 2000 characters
  priority: TaskPriority;        // 0=Low, 1=Medium, 2=High (default: Medium)
  dueDate?: string;              // Optional ISO 8601 DateTime
  boardId: string;               // Required GUID of target board
  assignedToId?: string;         // Optional GUID of employee to assign
}
```

#### `UpdateTaskDto` (Request)
```typescript
interface UpdateTaskDto {
  title: string;                 // Required, 1-500 characters
  description?: string;          // Optional, max 2000 characters
  status: TaskStatus;            // 0=Todo, 1=InProgress, 2=Done
  priority: TaskPriority;        // 0=Low, 1=Medium, 2=High
  dueDate?: string;              // Optional ISO 8601 DateTime
  assignedToId?: string;         // Optional GUID of employee
  rowVersion: string;            // Required Base64 string for concurrency
}
```

#### `UpdateTaskStatusDto` (Request) - For Drag & Drop
```typescript
interface UpdateTaskStatusDto {
  status: TaskStatus;            // 0=Todo, 1=InProgress, 2=Done
  rowVersion: string;            // Required Base64 string for concurrency
}
```

---

### **Employee DTOs**

#### `EmployeeViewModel` (Request & Response)
```typescript
interface EmployeeViewModel {
  id?: string;                   // GUID (optional for create, required for update)
  name: string;                  // Required employee name
  designation: string;           // Required job title/role
  email: string;                 // Required email address (must be valid format and unique)
  phone?: string;                // Optional phone number (must be valid format if provided)
  createdOn?: string;            // ISO 8601 DateTime (read-only)
  updatedOn?: string;            // ISO 8601 DateTime (read-only)
}
```

---

### **Comments DTOs**

#### `CommentDto` (Response)
```typescript
interface CommentDto {
  id: string;                    // GUID
  taskId: string;                // GUID of the related task
  content: string;               // Comment content (max 1000 chars)
  createdOn: string;             // ISO 8601 DateTime
  updatedOn: string;             // ISO 8601 DateTime
  authorId: string;              // GUID of the employee who made the comment
  authorName: string;            // Name of the employee who made the comment
}
```

#### `CreateCommentDto` (Request)
```typescript
interface CreateCommentDto {
  taskId: string;                // Required GUID of the related task
  content: string;               // Required, 1-1000 characters
}
```

#### `UpdateCommentDto` (Request)
```typescript
interface UpdateCommentDto {
  content: string;               // Required, 1-1000 characters
}
```

---

### **Enums**

#### `TaskStatus`
```typescript
enum TaskStatus {
  Todo = 0,
  InProgress = 1,
  Done = 2
}
```

#### `TaskPriority`
```typescript
enum TaskPriority {
  Low = 0,
  Medium = 1,
  High = 2
}
```

---

## ?? Key Integration Features

### **1. Optimistic Concurrency Control**
All task update operations require a `rowVersion` field to prevent conflicting updates:

```typescript
// When updating a task, always include the current rowVersion
const updateTask = async (taskId: string, updates: UpdateTaskDto) => {
  try {
    const response = await fetch(`/api/tasks/${taskId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        ...updates,
        rowVersion: currentTask.rowVersion  // Critical for concurrency
      })
    });
    
    if (response.status === 409) {
      // Conflict - task was updated by another user
      throw new Error('Task was modified by another user. Please refresh.');
    }
    
    return await response.json();
  } catch (error) {
    // Handle concurrency conflicts
    console.error('Concurrency conflict:', error);
  }
};
```

### **2. Drag & Drop Status Updates**
Use the PATCH endpoint for efficient status-only updates:

```typescript
const updateTaskStatus = async (taskId: string, newStatus: TaskStatus, rowVersion: string) => {
  const response = await fetch(`/api/tasks/${taskId}/status`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      status: newStatus,
      rowVersion: rowVersion
    })
  });
  
  return await response.json();
};
```

### **3. Board Filtering**
Get tasks for a specific board efficiently:

```typescript
const getBoardTasks = async (boardId: string) => {
  const response = await fetch(`/api/tasks?boardId=${boardId}`);
  return await response.json();
};
```

---

## ?? Error Handling

### **HTTP Status Codes**
| Code | Meaning | When it occurs | UI Action |
|------|---------|----------------|-----------|
| `200` | OK | Successful GET/PUT requests | Display data |
| `201` | Created | Successful POST requests | Show success message |
| `204` | No Content | Successful DELETE requests | Remove from UI |
| `400` | Bad Request | Validation errors, malformed requests | Show validation errors |
| `404` | Not Found | Resource doesn't exist | Show "not found" message |
| `409` | Conflict | Concurrency conflict (rowVersion mismatch) | Prompt user to refresh |
| `500` | Server Error | Internal server errors | Show generic error message |

### **Error Response Format**
```typescript
interface ApiError {
  message: string;               // Human-readable error message
  timestamp: string;             // ISO 8601 DateTime when error occurred
  error?: string;                // Technical error details (dev only)
}
```

### **Sample Error Handling**
```typescript
const handleApiError = (response: Response) => {
  switch (response.status) {
    case 400:
      return 'Please check your input and try again.';
    case 404:
      return 'The requested item was not found.';
    case 409:
      return 'This item was updated by another user. Please refresh and try again.';
    case 500:
      return 'A server error occurred. Please try again later.';
    default:
      return 'An unexpected error occurred.';
  }
};
```

---

## ?? Sample API Calls

### **Employee Management**

#### **Create Employee**
```typescript
const createEmployee = async (employeeData: EmployeeViewModel) => {
  const response = await fetch('/api/employees', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(employeeData)
  });
  
  if (!response.ok) throw new Error('Failed to create employee');
  return await response.json();
};

// Usage
const newEmployee = await createEmployee({
  name: 'John Doe',
  designation: 'Software Developer',
  email: 'john.doe@company.com',
  phone: '+1234567890'
});
```

#### **Update Employee**
```typescript
const updateEmployee = async (employeeId: string, employeeData: EmployeeViewModel) => {
  const response = await fetch(`/api/employees/${employeeId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(employeeData)
  });
  
  if (response.status === 404) {
    throw new Error('Employee not found');
  }
  if (!response.ok) throw new Error('Failed to update employee');
  return await response.json();
};
```

#### **Get Employee by ID**
```typescript
const getEmployee = async (employeeId: string) => {
  const response = await fetch(`/api/employees/${employeeId}`);
  
  if (response.status === 404) {
    return null; // Employee not found
  }
  if (!response.ok) throw new Error('Failed to get employee');
  return await response.json();
};
```

### **Create a Board**
```typescript
const createBoard = async (name: string) => {
  const response = await fetch('/api/boards', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name })
  });
  
  if (!response.ok) throw new Error('Failed to create board');
  return await response.json();
};

// Usage
const newBoard = await createBoard('Project Alpha Sprint 1');
```

### **Create a Task**
```typescript
const createTask = async (taskData: CreateTaskDto) => {
  const response = await fetch('/api/tasks', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(taskData)
  });
  
  if (!response.ok) throw new Error('Failed to create task');
  return await response.json();
};

// Usage
const newTask = await createTask({
  title: 'Implement user authentication',
  description: 'Add JWT-based authentication system with role-based authorization',
  priority: 2, // High priority
  dueDate: '2024-02-15T10:00:00Z',
  boardId: '123e4567-e89b-12d3-a456-426614174000',
  assignedToId: '987fcdeb-51a2-43d1-b765-123456789abc'
});
```

### **Update Task Status (Drag & Drop)**
```typescript
const moveTaskToInProgress = async (taskId: string, rowVersion: string) => {
  const response = await fetch(`/api/tasks/${taskId}/status`, {
    method: 'PATCH',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      status: 1, // InProgress
      rowVersion: rowVersion
    })
  });
  
  if (response.status === 409) {
    throw new Error('Task was modified by another user');
  }
  
  return await response.json();
};
```

### **Get Board with Tasks**
```typescript
const loadBoardData = async (boardId: string) => {
  const [board, tasks] = await Promise.all([
    fetch(`/api/boards/${boardId}`).then(r => r.json()),
    fetch(`/api/tasks?boardId=${boardId}`).then(r => r.json())
  ]);
  
  return { board, tasks };
};
```

### **Create a Comment**
```typescript
const createComment = async (taskId: string, content: string) => {
  const response = await fetch('/api/comments', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ taskId, content })
  });
  
  if (!response.ok) throw new Error('Failed to create comment');
  return await response.json();
};

// Usage
const newComment = await createComment('123e4567-e89b-12d3-a456-426614174001', 'This is a comment');
```

### **Update a Comment**
```typescript
const updateComment = async (commentId: string, content: string) => {
  const response = await fetch(`/api/comments/${commentId}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ content })
  });
  
  if (!response.ok) throw new Error('Failed to update comment');
  return await response.json();
};
```

### **Get Comments for a Task**
```typescript
const getTaskComments = async (taskId: string) => {
  const response = await fetch(`/api/comments/task/${taskId}`);
  
  if (!response.ok) throw new Error('Failed to get comments');
  return await response.json();
};
```

---

## ?? UI Implementation Guidelines

### **Kanban Board Structure**
```typescript
interface KanbanColumn {
  status: TaskStatus;
  title: string;
  tasks: TaskResponseDto[];
  color: string;
}

const columns: KanbanColumn[] = [
  { 
    status: TaskStatus.Todo, 
    title: 'To Do', 
    tasks: [], 
    color: 'gray' 
  },
  { 
    status: TaskStatus.InProgress, 
    title: 'In Progress', 
    tasks: [], 
    color: 'blue' 
  },
  { 
    status: TaskStatus.Done, 
    title: 'Done', 
    tasks: [], 
    color: 'green' 
  }
];
```

### **Task Card Data Requirements**
Each task card should display:
- ? **Title** (truncated if too long)
- ? **Priority badge** (High=Red, Medium=Orange, Low=Gray)
- ? **Assignee avatar/name** (if assigned)
- ? **Due date** (with overdue highlighting)
- ? **Comment count** (if > 0)
- ? **Description preview** (optional)

### **Comment Section UI**
- Load comments in a collapsible section under each task
- Display comment author name, date, and content
- Support adding, editing, and deleting comments inline
- Show confirm dialogs for delete actions
- Optimize loading with lazy loading or pagination for comments

### **Priority Color Coding**
```css
.priority-high { 
  @apply border-l-4 border-red-500 bg-red-50; 
}
.priority-medium { 
  @apply border-l-4 border-orange-500 bg-orange-50; 
}
.priority-low { 
  @apply border-l-4 border-gray-400 bg-gray-50; 
}
```

### **Status Color Coding**
```css
.status-todo { 
  @apply bg-gray-100 border-gray-300; 
}
.status-inprogress { 
  @apply bg-blue-100 border-blue-300; 
}
.status-done { 
  @apply bg-green-100 border-green-300; 
}
```

---

## ?? Responsive Design Considerations

### **Desktop (1024px+)**
- Full 3-column Kanban layout
- Sidebar navigation with board list
- Task cards in comfortable grid layout
- Drag & drop between columns

### **Tablet (768px - 1023px)**
- Horizontal scrolling Kanban
- Collapsible sidebar
- Medium-sized task cards
- Touch-friendly drag & drop

### **Mobile (< 768px)**
- Single-column stacked view with tabs
- Bottom navigation
- Full-screen task details modal
- Swipe gestures for status change

---

## ?? Real-time Considerations

While the API doesn't currently support real-time updates, prepare for future implementation:

```typescript
// Future WebSocket integration structure
interface TaskUpdate {
  taskId: string;
  boardId: string;
  action: 'created' | 'updated' | 'deleted' | 'moved';
  task: TaskResponseDto;
  userId: string;
}
```

---

## ?? Performance Optimization Tips

### **1. Lazy Loading**
```typescript
// Only load tasks when board is opened
const [tasks, setTasks] = useState<TaskResponseDto[]>([]);
const [isLoading, setIsLoading] = useState(false);

useEffect(() => {
  if (selectedBoardId) {
    setIsLoading(true);
    loadBoardTasks(selectedBoardId)
      .then(setTasks)
      .finally(() => setIsLoading(false));
  }
}, [selectedBoardId]);
```

### **2. Optimistic Updates**
```typescript
const moveTask = async (taskId: string, newStatus: TaskStatus) => {
  // Update UI immediately
  updateTaskInState(taskId, { status: newStatus });
  
  try {
    // Sync with server
    await updateTaskStatus(taskId, newStatus, rowVersion);
  } catch (error) {
    // Revert on failure
    revertTaskStatus(taskId);
    showError('Failed to move task');
  }
};
```

### **3. Caching Strategy**
```typescript
// Cache frequently accessed data
const [employeeCache, setEmployeeCache] = useState<EmployeeViewModel[]>([]);
const [boardCache, setBoardCache] = useState<BoardDto[]>([]);

// Load once, use everywhere
useEffect(() => {
  Promise.all([
    fetch('/api/employees').then(r => r.json()),
    fetch('/api/boards').then(r => r.json())
  ]).then(([employees, boards]) => {
    setEmployeeCache(employees);
    setBoardCache(boards);
  });
}, []);
```

---

## ?? MVP Feature Checklist

### **Core Features**
- [ ] **Board Management**: Create, read, update, delete boards
- [ ] **Task Management**: Full CRUD operations for tasks
- [ ] **Employee Management**: Full CRUD operations for employees
- [ ] **Kanban Interface**: 3-column drag-and-drop board
- [ ] **Employee Assignment**: Assign/unassign tasks to employees
- [ ] **Status Updates**: Move tasks between columns
- [ ] **Priority Management**: Set and display task priorities
- [ ] **Due Date Handling**: Set due dates and highlight overdue tasks
- [ ] **Comments System**: Add/view task comments

### **User Experience**
- [ ] **Responsive Design**: Works on desktop, tablet, and mobile
- [ ] **Loading States**: Show spinners during API calls
- [ ] **Error Handling**: User-friendly error messages
- [ ] **Form Validation**: Client-side validation matching API requirements
- [ ] **Confirmation Dialogs**: Confirm destructive actions
- [ ] **Search & Filter**: Find tasks by various criteria

### **Advanced Features (Nice-to-Have)**
- [ ] **Bulk Operations**: Multi-select and bulk actions
- [ ] **Dark Mode**: Toggle between light and dark themes
- [ ] **Keyboard Navigation**: Full keyboard accessibility
- [ ] **Export Functionality**: Export board data
- [ ] **Notifications**: Due date reminders

---

## ?? Integration Testing

Use these test scenarios to verify your UI integration:

### **Happy Path Testing**
1. ? Create a board ? Should appear in board list
2. ? Create an employee ? Should appear in employee list
3. ? Add tasks to board ? Should appear in Todo column
4. ? Assign task to employee ? Should show assignee name
5. ? Drag task to In Progress ? Should update status via PATCH API
6. ? Edit task details ? Should update via PUT API
7. ? Update employee info ? Should reflect changes
8. ? Delete task ? Should remove from UI and call DELETE API
9. ? Add comment to task ? Should appear in comment section
10. ? Edit comment ? Should update comment content
11. ? Delete comment ? Should remove comment from UI

### **Error Scenario Testing**
1. ?? Try updating deleted task ? Should handle 404 gracefully
2. ?? Try updating non-existent employee ? Should handle 404 gracefully
3. ?? Create employee with duplicate email ? Should show validation error
4. ?? Concurrent edits ? Should handle 409 conflict properly
5. ?? Invalid data submission ? Should show validation errors
6. ?? Network timeout ? Should show retry options
7. ?? Server error ? Should show generic error message
8. ?? Delete task/comment and quickly try to update ? Should handle 404/409 gracefully

---

## ?? Support & Documentation

### **API Base URL**: `https://localhost:7165/api`
### **Swagger Documentation**: `https://localhost:7165/swagger`
### **Content Type**: `application/json`

### **Key API Behaviors**
- All GUIDs should be sent as strings in JSON
- DateTime fields use ISO 8601 format (`2024-02-15T10:00:00Z`)
- Concurrency control via `rowVersion` is required for task updates
- Board names must be unique across the system
- Employee emails must be unique across the system
- Employee names and designations are required fields
- Phone numbers are optional but must be valid format if provided

---

This documentation provides everything needed to build a production-ready Task Board UI that integrates seamlessly with the NeuraNx API. Focus on clean, intuitive design and smooth user interactions for the best user experience.