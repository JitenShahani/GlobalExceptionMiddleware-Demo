# 🧾 Source Code to generate Mermaid Charts.

This document contains the raw Mermaid source used to generate the sequence diagram for each endpoint.

## Consolidated Mermaid Chart with all endpoints

```Mermaid
---
config:
	theme: redux-dark-color
---
sequenceDiagram
	autonumber
	title Global Exception Handling Demo

	participant Client
	participant MyEndpoints as 💻 MyEndpoints.cs
	participant Validator as 🧪 FluentValidation
	participant ValidationHandler as 🔗 ValidationExceptionHandler.cs
	participant ExceptionHandler as 🔗 ExceptionHandler.cs
	participant StatusPages as 🧱 UseStatusCodePages()
	participant ProblemFactory as 🏭 ProblemDetailsFactory
	participant ProblemWriter as 🧬 IProblemDetailsService
	participant JsonResponse as 📤 JSON Response

	Client->>MyEndpoints: GET /exception
	MyEndpoints->>ExceptionHandler: throw Exception
	ExceptionHandler->>ProblemFactory: Create ProblemDetails
	ProblemFactory-->>ExceptionHandler: ProblemDetails
	ExceptionHandler->>JsonResponse: Write response
	JsonResponse-->>Client: 500 Internal Server Error

	Client->>MyEndpoints: GET /divideByZero
	MyEndpoints->>ExceptionHandler: DivideByZeroException
	ExceptionHandler->>ProblemFactory: Create ProblemDetails
	ProblemFactory-->>ExceptionHandler: ProblemDetails
	ExceptionHandler->>JsonResponse: Write response
	JsonResponse-->>Client: 500 Internal Server Error

	Client->>MyEndpoints: GET /badRequest
	MyEndpoints->>StatusPages: Missing route parameter
	StatusPages->>ProblemFactory: Create ProblemDetails
	ProblemFactory-->>StatusPages: ProblemDetails
	StatusPages->>ProblemWriter: Construct response
	ProblemWriter->>JsonResponse: Write response
	JsonResponse-->>Client: 404 Not Found

	Client->>MyEndpoints: POST /validationException
	MyEndpoints->>Validator: Validate Person
	Validator->>ValidationHandler: throw ValidationException
	ValidationHandler->>ProblemFactory: Create ValidationProblemDetails
	ProblemFactory-->>ValidationHandler: ValidationProblemDetails
	ValidationHandler->>ProblemWriter: Write response
	ProblemWriter-->>Client: 500 Internal Server Error
```