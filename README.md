# Energy Management System

A comprehensive energy management solution built with .NET Core and Vue.js, implementing clean architecture principles and modern software development practices. The system consists of a backend API built with .NET Core and a frontend application developed with Vue.js.

![Dashboard View](https://github.com/alpertoruun/EnergyManagementSystem/blob/main/dashboard.png)
![Sample Screen](https://github.com/alpertoruun/EnergyManagementSystem/blob/main/sample.png)

## Project Overview

The Energy Management System is a robust application designed to monitor and manage energy usage across different devices and locations. It provides features for user authentication, device management, energy usage tracking, scheduling, and notifications.

For detailed information about the system design, architecture, and technical specifications, please refer to the [design document](https://github.com/alpertoruun/EnergyManagementSystem/blob/main/design.pdf) in the repository.

### Architecture

The project follows Clean Architecture principles with a layered structure:

- **EnergyManagementSystem.Core**: Contains business entities, interfaces, DTOs, and business logic
- **EnergyManagementSystem.Data**: Handles data persistence and database operations
- **EnergyManagementSystem.Service**: Implements business logic and services
- **EnergyManagementSystem.API**: Presents the REST API endpoints

### Key Features

- JWT-based authentication and authorization
- Email confirmation and password reset functionality
- Device energy usage monitoring and history tracking
- Scheduling system with background jobs (Hangfire)
- Real-time notifications
- User settings and preferences management

### Technologies & Patterns

#### Backend
- **.NET Core 6.0+**
- **Entity Framework Core** with PostgreSQL
- **JWT** for authentication
- **Hangfire** for background job processing
- **AutoMapper** for object mapping
- **FluentValidation** for request validation
- **Swagger** for API documentation

The project implements several design patterns and principles:
- SOLID Principles
- Repository Pattern
- Dependency Injection
- Unit of Work Pattern

## Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- PostgreSQL database
- SMTP server for email functionality

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/EnergyManagementSystem.git
```

2. Configure the database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=energydb;Username=youruser;Password=yourpassword"
  }
}
```

3. Configure email settings in `Core/Configuration/EmailSettings.cs`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "SmtpPort": 587,
    "SenderEmail": "your-email@example.com",
    "SenderName": "Energy Management System",
    "Username": "your-username",
    "Password": "your-password"
  }
}
```

4. Run database migrations:
```bash
dotnet ef database update
```

5. Start the backend application:
```bash
dotnet run
```

### Frontend Setup

1. Clone the UI repository:
```bash
git clone https://github.com/alpertoruun/EMS-UI.git
```

2. Navigate to the project directory:
```bash
cd EMS-UI
```

3. Install dependencies:
```bash
npm install
```

4. Start the development server:
```bash
npm run dev
```

The frontend application will be available at `http://localhost:5173` (or another port if 5173 is in use).

## Documentation

### Technical Documentation
- [System Design Document](https://github.com/alpertoruun/EnergyManagementSystem/blob/main/design.pdf) - Detailed system architecture, database schema, and component interactions
- API documentation available through Swagger UI at `/swagger`
- Code documentation within each project using XML comments

### User Documentation
- Installation and setup guides (in this README)
- API endpoints and usage examples (via Swagger)
- Frontend component documentation (in EMS-UI repository)

- Authentication (Register, Login, Password Reset)
- Device Management
- Energy Usage Monitoring
- Scheduling
- User Settings

### Authentication

The system uses JWT Bearer tokens for authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-token>
```

## Background Jobs

The system uses Hangfire for scheduling and background processing. The Hangfire dashboard is available at `/hangfire` for monitoring job status and execution.

## Security

- JWT token-based authentication
- Password hashing with modern cryptographic standards
- Email verification for new accounts
- Refresh token mechanism
- Rate limiting and request validation

## Project Structure

### Backend Structure
```
EnergyManagementSystem/
├── EnergyManagementSystem.Core/        # Business logic, interfaces, DTOs
├── EnergyManagementSystem.Data/        # Data access, migrations, repositories
├── EnergyManagementSystem.Service/     # Service implementations
└── EnergyManagementSystem.API/         # API controllers, middleware
```

### Frontend Structure
```
EMS-UI/
├── src/
│   ├── assets/          # Static assets
│   ├── components/      # Reusable Vue components
│   ├── views/           # Page components
│   ├── store/           # Pinia stores
│   ├── router/          # Vue router configuration
│   └── services/        # API service integrations
├── tailwind.config.js   # TailwindCSS configuration
└── vite.config.js       # Vite configuration
```

## Development Workflow

1. Start the backend API server first
2. Start the frontend development server
3. Make sure both applications can communicate (CORS is properly configured)
4. Use Swagger UI for API testing and documentation
5. Use Vue DevTools for frontend debugging

## Production Deployment

### Backend
1. Update connection strings and configurations for production
2. Build and publish the .NET application:
```bash
dotnet publish -c Release
```

### Frontend
1. Update environment variables for production
2. Build the Vue.js application:
```bash
npm run build
```
3. Deploy the contents of the `dist` directory to your web server

## Contributing

Please read our contributing guidelines before submitting pull requests.

1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
