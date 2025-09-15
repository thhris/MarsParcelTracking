# Mars Parcel Tracking API

A .NET 9 Web API for registering parcels destined for Mars, tracking status during transition, and exposing parcel data for consumption.

---

## 1. Setup Instructions

Prerequisites: .NET 9 SDK installed.

Restore & build:
dotnet restore dotnet build

Run the API (from solution root):
dotnet run --project MarsParcelTracking

Swagger UI:
https://localhost:(port)/swagger
(Port shown in console; https enabled by default.)

Run tests:
dotnet test

## 2. API Endpoints Walkthrough

**Parcel Registration (POST /parcels)**
   - Validate barcode.
   - Calculate launch date based on service (Standard or Express).
   - Calculate ETA + estimated arrival date.
   - Enforce uniqueness for Barcodes at repository level.

**Status Update (PATCH /parcels/{barcode})**
   - Validate barcode.
   - Get Parcel and Validate transition against hardcoded matrix.
   - Append new History entry.
   - Return 400 if an error occurs.

**Parcel Retrieval (GET /parcels/{barcode})**
   - Validate barcode.
   - Return parcel or 404.

## 3. Summary of Current Design Choices & Trade-offs

- **In-Memory Data Store**: Used a singleton service with a thread-safe dictionary for simplicity. 
Suitable for an MVP but not for production. 
Future plans include replacing this with a persistent database.

- **Architecture**: Implemented a service layer for business logic and a repository layer for data access, promoting separation of concerns as per Clean Architecture.

- **Validation**: Used custom validation for Barcodes. More complex rules can be added as needed. The barcode does not filter white spaces at the moment.

- **Error Handling**: Basic error handling is implemented. More robust logging and error management can be added in later itterations.

- **Extensibility**: Designed to be easily extensible for future features like user authentication, notifications, and advanced search.

- **Testing**: Included unit tests for service and repository layers. More detailed tests can be added in future.

- **Hardcoded Values**: There are multiple places where hardcoded values have been used. In future iterations we should make them dynamic or store them in the AppResources.

- **Slight Alterations**: For testing purposes, I have changed the spec provided slightly where the timestamp value on the History object shows date and time. When testing it helps distinguish the order of creation for the records.

- **DTOs**: For the sake of time, I opted to using the Parcel model when returning an object in the requests. Ideally, I would have implemented DTOs instead as we do not want to expose the domain directly.


## 4. Future Improvements

- Replace the in-memory store with a persistent database(MSSQL, PostgreSQL, MongoDB).

- Implement authentication and authorization.

- Add more comprehensive validation and error handling.

- Improve logging and monitoring.

- Expand test coverage.

- Add support for user roles and permissions.

- Implement notification system for status updates.

- Optimize performance when working with larger datasets.


## 5. Enterprise-Scale Improvements

- Introduce DTO layer (request/response) to decouple domain models and not expose.

- Global exception handling rather than controller try/catch.

- Persist to database and use EF Core or other ORM.

- Start versioning the API (v1, v2) and start feature flagging.

- Introduce Delay events (eg. weather, rocket issues) adjusting ETA.

- Authentication/authorization (API keys or OAuth2 scopes).

- Introduce Docker to containarise the application and allow for better horizontal scaling (eg. Kubernetes)

- Create CI/CD pipelines for automated testing and deployment.

## 6. AI Usage

- Used GitHub Copilot for generating boilerplate code such as models and enums.

- Used GitHub Copilot for generating diagramd of the valid transitions between parcel statuses before implementing the Validation logic.

- Used GitHub Copilot for generating boilerplate code for unit tests.
