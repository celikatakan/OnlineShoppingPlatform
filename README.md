# Multiple Layered Asp.Net API Project – Online Shopping Platform

## Project Description
This project is a multi-layered online shopping platform developed using ASP.NET Core Web API. It uses Entity Framework Core with a Code First approach to interact with the database.
![Swagger](https://github.com/user-attachments/assets/096f31b6-ee95-453c-95e1-e1438bfd0bf2)

![dB](https://github.com/user-attachments/assets/92178a35-c6d9-4a29-bc29-a364e225294b)

## Project Requirements

### 1. Layered Architecture
The project is structured with at least 3 layers:

- **Presentation Layer (API Layer)**: Contains API controllers and handles user requests.
- **Business Layer**: Contains business logic and performs data operations through the Data Access Layer.
- **Data Access Layer**: Manages database operations using Entity Framework Core, implementing Repository and UnitOfWork patterns.

### 2. Data Model
The following data models are used in the project:

- **User**: Holds customer information.
  - `Id` (int, primary key)
  - `FirstName` (string)
  - `LastName` (string)
  - `Email` (string, unique)
  - `PhoneNumber` (string)
  - `Password` (string) - Encrypted using Data Protection.
  - `Role` (Enum) - For authorization purposes (e.g., Admin or Customer).

- **Product**: Represents products available for sale.
  - `Id` (int, primary key)
  - `ProductName` (string)
  - `Price` (decimal)
  - `StockQuantity` (int)

- **Order**: Stores customer orders.
  - `Id` (int, primary key)
  - `OrderDate` (DateTime)
  - `TotalAmount` (decimal)
  - `CustomerId` (int) - Linked to the relevant customer.

- **OrderProduct**: Creates a many-to-many relationship between orders and products.
  - `OrderId` (int)
  - `ProductId` (int)
  - `Quantity` (int)

### 3. Authentication and Authorization
- **Authentication**: Customer authentication is performed using ASP.NET Core Identity or a custom identity service.
- **Authorization**: Authorization is implemented with JWT, defining roles like "Customer" and "Admin".

### 4. Middleware
- Custom middleware is used to log each request.
- A `MaintenanceMiddleware` is included to handle maintenance scenarios.

### 5. Action Filter
- Custom action filters are implemented to control access to certain API endpoints.

### 6. Model Validation
- Validation rules are applied to customer and product models, such as mandatory fields, email format, and stock amount.

### 7. Dependency Injection
- Dependency Injection is used to manage service dependencies.

### 8. Data Protection
- User passwords are securely stored using Data Protection.

### Additional Features
- **Paging**: Paging is applied to listing operations.

## Technologies Used
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity & JWT
- Swagger (API Documentation)

