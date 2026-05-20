# Architecture & Design Patterns

## Application Architecture

This application follows the **MVC (Model-View-Controller)** pattern combined with a **Service Layer** architecture:

```
User Interface (Views)
    ↓
Controllers (HTTP requests/responses)
    ↓
Services (Business Logic)
    ↓
Repositories (Data Access)
    ↓
Database (MSSQL)
```

## Design Patterns Used

### 1. **Repository Pattern**
- Abstracts data access logic
- Located in: `Services/IRepository.cs`
- Benefits: Easy to test, swap implementations

### 2. **Dependency Injection**
- Services are injected via constructor
- Configured in: `Program.cs`
- Benefits: Loose coupling, testability

### 3. **MVC Pattern**
- Models: Data structures
- Views: HTML/CSS presentation
- Controllers: Handle HTTP requests

### 4. **Service Layer**
- Business logic separated from controllers
- Easy to test independently
- Reusable across controllers

## Project Flow

### Authentication Flow
```
Registration/Login → AuthController → AuthService → UserRepository → Database
                         ↓
                    Set Session
                         ↓
                  Redirect to Dashboard
```

### Project Management Flow
```
User → ProjectController → ProjectService → ProjectRepository → Database
         ↓
     Load/Create/Edit Project
         ↓
     Update View
```

### Document Upload Flow
```
User → DocumentController → DocumentService → Save File → DocumentRepository → Database
                                ↓
                           Update Session
```

## Database Design

### Key Entities

1. **Users** - Authentication and user info
2. **Workers** - Employee/Worker information
3. **Projects** - Construction projects
4. **ProjectWorkers** - Junction table (M:N relationship)
5. **ProjectImages** - Project photos
6. **Documents** - File storage
7. **DashboardMetrics** - Analytics data

### Relationships

```
User (1) --- (1) Worker
     ↓
Document (assigned by)

Project (1) --- (*) ProjectWorker
              (*) --- (1) Worker

Project (1) --- (*) ProjectImage
Project (1) --- (*) Document
```

## Security Architecture

### Password Security
- BCrypt hashing with salt
- 11 rounds of hashing

### Session Management
- 30-minute idle timeout
- Stored server-side

### Input Validation
- Client-side: HTML5 validation
- Server-side: Model validation
- SQL injection prevention via Entity Framework

## Performance Considerations

### Database Optimization
- Indexes on frequently queried columns
- Junction tables for M:N relationships
- Denormalization for dashboard metrics

### Caching Strategies (Recommended)
- User authentication tokens
- Dashboard metrics (5-minute cache)
- Static files (browser cache)

### Scalability Options
1. **Horizontal Scaling**
   - Multiple application instances
   - Shared database
   - Session state in Redis

2. **Vertical Scaling**
   - Upgrade server resources
   - Database optimization
   - Query optimization

## File Organization

```
Controllers/    - HTTP request handlers
Models/        - Data structures, DTOs
Services/      - Business logic
Data/          - Database context, migrations, schema
Views/         - Razor templates
wwwroot/       - Static files (CSS, JS, images)
```

## Error Handling

### Controller Level
- Try-catch blocks for critical operations
- Return appropriate HTTP status codes
- Redirect to error pages on exception

### Service Level
- Validation before database operations
- Custom exception messages
- Logging for debugging

### View Level
- Display user-friendly error messages
- Form validation feedback
- Modal dialogs for confirmations

## Testing Approach

### Unit Testing
- Test services independently
- Mock repositories
- Verify business logic

### Integration Testing
- Test service + repository combination
- Use test database

### Controller Testing
- Mock services
- Verify view selection
- Check redirect logic

Example:
```csharp
[TestClass]
public class WorkerServiceTests
{
    private IWorkerService _service;
    
    [TestInitialize]
    public void Setup()
    {
        var mockRepo = new Mock<IWorkerRepository>();
        _service = new WorkerService(mockRepo.Object);
    }

    [TestMethod]
    public async Task GetAllWorkers_ReturnsWorkersList()
    {
        // Arrange
        var expectedWorkers = new List<Worker> { /* ... */ };
        
        // Act
        var result = await _service.GetAllWorkersAsync();
        
        // Assert
        Assert.AreEqual(expectedWorkers.Count, result.Count);
    }
}
```

## Development Workflow

### Feature Development
1. Create model/entity
2. Add database migration
3. Create controller action
4. Implement service method
5. Create/update repository method
6. Build view(s)
7. Add CSS/JS as needed
8. Test thoroughly
9. Commit to version control

### Database Changes
1. Create migration: `dotnet ef migrations add MigrationName`
2. Update database: `dotnet ef database update`
3. Test in development
4. Document breaking changes

## Deployment Strategy

### Pre-Deployment
- Run all tests
- Security review
- Performance testing
- Database backup

### Deployment
- Stop application
- Backup database
- Update code
- Run migrations
- Start application
- Verify functionality

### Post-Deployment
- Monitor logs
- Check performance
- Verify backups
- Document changes
