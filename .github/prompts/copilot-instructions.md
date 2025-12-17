# Copilot Instructions

## Project Overview

This is a .NET 9.0 ASP.NET Core Web API project following Test-Driven Development (TDD) practices. The solution consists of:

- **ApiProject**: Main Web API application
- **ApiProjectTests**: Test project using xUnit and WebApplicationFactory
- Docker containerization with multi-stage builds
- Code coverage reporting

## Coding Standards and Conventions

### C# Conventions

- Use **implicit usings** and **nullable reference types** enabled
- Follow **PascalCase** for public members, classes, and methods
- Use **camelCase** for private fields and local variables
- Prefer **record types** for DTOs and value objects (like `WeatherForecast`)
- Use **minimal APIs** pattern for simple endpoints
- Enable and utilize **top-level programs** (Program.cs)

### File Organization

```
ApiProject/
├── Controllers/          # MVC controllers (if used)
├── Services/            # Business logic services
├── Models/              # Data models and DTOs
├── Properties/          # Launch settings
├── Program.cs           # Application entry point
└── *.csproj            # Project file

ApiProjectTests/
├── Integration/         # Integration tests using WebApplicationFactory
├── Unit/               # Unit tests for services/logic
└── *.csproj            # Test project file
```

### Naming Conventions

- **Controllers**: Suffix with `Controller` (e.g., `NumberController`)
- **Services**: Suffix with `Service` (e.g., `RandomService`)
- **Tests**: Suffix with `Tests` (e.g., `NumberControllerTests`)
- **Integration Tests**: Suffix with `IntegrationTests`
- **Test Methods**: Use descriptive names describing the scenario

## Project-Specific Context

### Current Architecture

- **Minimal API** approach using `app.MapGet()` patterns
- **Dependency injection** through `builder.Services`
- **OpenAPI/Swagger** integration for development
- **Docker containerization** with port mapping (9080:8080)

### Existing Patterns

```csharp
// Minimal API endpoint pattern
app.MapGet("/endpoint", () =>
{
    // Logic here
    return result;
})
.WithName("EndpointName");

// Record for DTOs
record DataModel(Type Property1, Type Property2);
```

## Preferred Patterns and Practices

### API Development

1. **Use minimal APIs** for simple CRUD operations
2. **Implement proper HTTP status codes**
3. **Use record types** for request/response models
4. **Add endpoint names** with `.WithName()` for OpenAPI
5. **Validate input** and return appropriate error responses

### Service Layer

```csharp
// Service interface pattern
public interface IRandomService
{
    int Get();
}

// Service implementation
public class RandomService : IRandomService
{
    public int Get()
    {
        return Random.Shared.Next(1, 11); // 1-10 only
    }
}
```

### Dependency Injection

```csharp
// Register services in Program.cs
builder.Services.AddScoped<IRandomService, RandomService>();
```

## Testing Approaches

### Test-Driven Development (TDD)

1. **Write failing test first**
2. **Write minimal code to pass**
3. **Refactor while keeping tests green**

### Integration Testing with WebApplicationFactory

```csharp
public class ControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetEndpoint_ShouldReturnExpectedResult()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/endpoint");

        // Assert
        response.EnsureSuccessStatusCode();
        // Additional assertions
    }
}
```

### Service Mocking for Integration Tests

```csharp
// Override services for testing
var factory = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            // Remove real service
            services.RemoveAll<IRandomService>();

            // Add mock/stub
            var mockService = new Mock<IRandomService>();
            mockService.Setup(x => x.Get()).Returns(5);
            services.AddSingleton(mockService.Object);
        });
    });
```

### Test Structure

- **Arrange**: Set up test data and mocks
- **Act**: Execute the method under test
- **Assert**: Verify the expected outcome

### Test Coverage

- Maintain **high test coverage** (aim for >90%)
- Use **coverlet.collector** for coverage reporting
- Run tests in **Docker build** process for CI/CD

## Code Review Guidelines

### Required Checks

1. **Tests must pass** before merging
2. **Code coverage** should not decrease
3. **Follow naming conventions**
4. **Proper error handling** implemented
5. **No hardcoded values** (use configuration/constants)

### API Endpoints

- **Validate all inputs**
- **Return appropriate HTTP status codes**
- **Include proper error responses**
- **Document with OpenAPI attributes** if needed

### Testing Requirements

- **Every new feature** must have tests
- **Integration tests** for API endpoints
- **Unit tests** for business logic
- **Mock external dependencies**

## Docker and Deployment

### Docker Build Process

1. **Run tests** during build (`RUN dotnet test`)
2. **Multi-stage builds** for optimization
3. **Port mapping**: Host 9080 → Container 8080
4. **Health checks** for production readiness

### Environment Configuration

- Use **appsettings.json** for configuration
- **Environment-specific** settings in appsettings.{Environment}.json
- **Secrets** should use proper configuration providers

## Git Workflow

### Conventional Commits

Follow the pattern from [.github/prompts/git-add-commit.prompt.md](.github/prompts/git-add-commit.prompt.md):

- `feat:` for new features
- `fix:` for bug fixes
- `test:` for adding/updating tests
- `refactor:` for code refactoring
- `docs:` for documentation changes

### Example Commit Messages

```
feat: add NumberController with random number generation
test: add integration tests for NumberController
fix: correct random number range validation
refactor: extract random service interface
```

## Tools and Extensions

- **Microsoft.AspNetCore.Mvc.Testing** for integration testing
- **xUnit** for test framework
- **coverlet.collector** for code coverage
- **OpenAPI** for API documentation
- **Postman collections** for API testing ([postman/day1.postman_collection.json](postman/day1.postman_collection.json))
