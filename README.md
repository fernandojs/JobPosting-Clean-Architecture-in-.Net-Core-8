## Overview

This project was building a test application using ASP.NET MVC and ASP.NET Core, adhering to Clean Architecture principles. 

### User Case: Job Posting Information API

**Objective:**
Develop an API for managing job postings, enabling users to create, read, update, and delete job listings. The User table will be used solely for authentication, providing JWT tokens to secure the API endpoints.

### Detailed Description

#### Entities and Relationships:

1. **User**
   - **Fields:**
     - `Id` (UUID, Primary Key)
     - `Username` (String)
     - `PasswordHash` (String)
     - `Email` (String)
     - `DateJoined` (DateTime)

2. **JobPosting**
   - **Fields:**
     - `Id` (UUID, Primary Key)
     - `Title` (String)
     - `Description` (String)
     - `Company` (String)
     - `Location` (String)
     - `SalaryRange` (String, optional)
     - `PostedDate` (DateTime)
     - `ClosingDate` (DateTime, optional)

#### API Endpoints:

1. **User Management**
   - **Register User**: `POST /api/token/createuser`
     - Request: `{ "username": "string", "password": "string", "email": "string" }`
     - Response: `201 Created`
   - **Login User**: `POST /api/token/loginuser`
     - Request: `{ "username": "string", "password": "string" }`
     - Response: `200 OK` with JWT token  

2. **Job Posting Management**
   - **Create Job Posting**: `POST /api/jobpostings`
     - Request: `{ "title": "string", "description": "string", "company": "string", "location": "string", "salaryRange": "string", "closingDate": "DateTime" }`
     - Response: `201 Created`
   - **Get Job Posting**: `GET /api/jobpostings/{id}`
     - Response: `200 OK` with job posting details
   - **Update Job Posting**: `PUT /api/jobpostings/{id}`
     - Request: `{ "title": "string", "description": "string", "company": "string", "location": "string", "salaryRange": "string", "closingDate": "DateTime" }`
     - Response: `200 OK`
   - **Delete Job Posting**: `DELETE /api/jobpostings/{id}`
     - Response: `204 No Content`
   - **Get All Job Postings**: `GET /api/jobpostings`
     - Response: `200 OK` with list of job postings

#### Business Rules:

1. **User Registration and Login:**
   - Passwords must be hashed before storing.
   - Email should be unique for each user.
   - Login should return a JWT token for authenticated requests.

2. **Job Posting Management:**
   - Users can create job postings with details like title, description, company, location, salary range, and closing date.
   - Job postings should have a posted date set automatically when created.
   - Only authenticated users can create, update, or delete job postings.
   - Public endpoints should allow anyone to view job postings without authentication.

#### Validation and Error Handling:

1. **Validation:**
   - Ensure required fields are provided in requests.
   - Validate email format and password strength during registration.
   - Ensure dates are valid and logical (e.g., closingDate should not be before postedDate).

2. **Error Handling:**
   - Return appropriate HTTP status codes (e.g., 400 Bad Request for validation errors, 401 Unauthorized for authentication errors, 404 Not Found for non-existent resources).
   - Provide meaningful error messages to help users correct their inputs.

#### Security Considerations:

1. **Authentication and Authorization:**
   - Use JWT for securing API endpoints.
   - Ensure that only authenticated users can access job posting management endpoints.
   - Protect sensitive data such as user passwords and personal information.

2. **Data Integrity:**
   - Ensure that job posting records cannot be tampered with by unauthorized users.
   - Implement checks to prevent duplicate job postings based on unique combinations of title and company.


By adhering to these detailed requirements and guidelines, you can create a robust and secure API for managing job posting information. This API will help users efficiently manage job listings and ensure a well-organized job posting platform.  

## Main Technologies

* ASP.NET Core 8
* ASP.NET MVC
* [XUnit](https://xunit.org/)
* [Docker](https://www.docker.com/)

## Layers

This project follows the principles of Clean Architecture, where dependencies point inwards to maintain greater cohesion and reduce coupling within the software.
By ensuring that the outer layers depend on the inner layers, the design promotes independence of the domain logic from external concerns such as UI and data access technologies.
Here's a detailed breakdown of each layer in the architecture:

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all business application logic. It is dependent on the domain layer and infra layer. 

### Infrastructure

This layer contains classes for accessing external resources such as databases, file systems, web services, smtp, and so on. 

### IoC

Implementing Inversion of Control (IoC) through Dependency Injection (DI) is a fundamental technique to achieve decoupling in software architecture. 

### Presentation (WebUI)

The project includes an API built with ASP.NET Core 8, featuring a comprehensive Swagger UI. This enables easy interaction with all API methods directly through a web interface. The Swagger UI provides a user-friendly way to test and demonstrate all functionalities of the API, including creating, retrieving, updating, and deleting job postings.

### Tests

This layer is a contains unit tests for the Domain layer.

## How to Run the Project

### Prerequisites:

    Docker installed on your machine.

### Steps   

    1. Start the Docker environment:

    Navigate to the project directory where the docker-compose.yml file is located.
    Open a terminal or command prompt.
    Run the following command to build and start the services defined in the Docker Compose file:

    docker-compose up --build

    This command will start all the necessary containers including the application and any databases or other services it depends on.

    2. Access the Application:

    Once the containers are running, the application will be accessible via your web browser.
    Open your web browser and navigate to http://localhost:8080.
    You should see the application running and can interact with the API through the provided endpoints.

## How to Execute Unit and Integration Tests

### Prerequisites:

    Docker installed on your machine.

### Steps 

    1. Prepare the testing environment:

    Ensure the Docker environment for testing is defined in docker-compose-for-tests.yml, which should configure the necessary services including a temporary database specifically for testing.
    Run the following command to start the test database:

    docker-compose -f docker-compose-for-tests.yml up --build

    This ensures that the integration tests interact with a dedicated test database, avoiding any impact on production or development databases.

    2. Run the tests:

    Open your development environment or terminal.
    Navigate to the project directory.
    Use the following command to execute the tests:

    dotnet test

    This command will run both unit and integration tests defined within your project, using the test configuration and connecting to the test database set up by Docker.
    

## Future features and improvements

On the Backend:
* Increase the test covarage
* Caching
* Pagination
* CQRS
* Event handling
* Database optimizations (Indexes)

On the frontEnd
* Add ASP.Net MVC Project
* Angular