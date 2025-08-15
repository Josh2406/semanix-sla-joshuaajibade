#### 📄 Forms & Rendering Services - Assessment

### Overview
This solution implements a **Forms Service** and a **Rendering Service** that communicate via an event-driven architecture.  
The core feature demonstrated is publishing and updating a form in the **Forms Service**, with the change propagated to the **Rendering Service** via a message bus.

The system also demonstrates **observability** via structured logging (**Serilog**) and monitoring metrics (**Prometheus Counter**).

---

### ✨ Features

a. **Forms Service**

* Publishes forms from Draft to Published state.

* Persists data in SQL Server using EF Core.

* Emits domain events (FormPublishedEvent, FormUpdatedEvent) to EentBus.

* Structured logging via Serilog.

* Publishes metrics to Prometheus.


b. **Rendering Service**

* Listens to EventBus events from Forms Service.

* Updates its own rendering database accordingly.

* Logs operations and exposes metrics.


### 🧪 Tests

a. **Unit Tests**

* Validates legal and illegal state transitions in PublishFormHandler.

* Ensures correct event publishing for valid requests.

* Asserts error responses for missing forms or invalid states.

b. **Integration Test**

* Verifies that a publish/update action in the Forms Service is reflected in the Rendering Service.

* Uses an in-memory RabbitMQ (or test container) to simulate real communication.

* Uses test SQL Server container for database operations.


### 📊 Observability

a. **Serilog**

* Structured logging to console and file.

* Logs key fields: TenantId, FormId, State, Action.

b. **Prometheus**

* Metric: fe_forms_published_total – increments every time a form is published.

* Useful for monitoring publishing activity.


### 📂 Project Structure

```mermaid
graph TD
    A[SemanixEngine]
    
    A --> B[src]
    A --> T[tests]

    %% FormsService
    B --> B1[FormsService]
    B1 --> B1A[FormsService.API]
    B1A --> B1A1[Controllers]
    B1A --> B1A2[Middleware]

    B1 --> B1B[FormsService.Application]
    B1B --> B1B1[Commands]
    B1B --> B1B2[Constants]
    B1B --> B1B3[Extensions]
    B1B --> B1B4[Mapping]
    B1B --> B1B5[Models]
    B1B5 --> B1B5A[Request]
    B1B5 --> B1B5B[Response]
    B1B --> B1B6[Queries]
    B1B --> B1B7[Validators]

    B1 --> B1C[FormsService.Domain]
    B1C --> B1C1[Entities]
    B1C --> B1C2[Enums]

    B1 --> B1D[FormsService.Infrastructure]
    B1D --> B1D1[Handlers]
    B1D --> B1D2[Metrics]
    B1D --> B1D3[Migrations]
    B1D --> B1D4[Persistence]
    B1D --> B1D5[Repository]

    %% RenderingService
    B --> B2[RenderingService]
    B2 --> B2A[RenderingService.API]
    B2A --> B2A1[Controllers]

    B2 --> B2B[RenderingService.Application]
    B2B --> B2B1[Implementations]
    B2B --> B2B2[Interfaces]

    B2 --> B2C[RenderingService.Domain]
    B2C --> B2C1[Constants]
    B2C --> B2C2[Entities]
    B2C --> B2C3[Models]

    B2 --> B2D[RenderingService.Infrastructure]
    B2D --> B2D1[Migrations]
    B2D --> B2D2[Persistence]
    B2D --> B2D3[Repository]
    B2D3 --> B2D3A[Command]
    B2D3 --> B2D3B[Query]

    %% Shared
    B --> B3[Shared]
    B3 --> B3A[Shared.Common]
    B3A --> B3A1[Contracts]
    B3A --> B3A2[Events]
    B3A --> B3A3[Messaging]

    %% Tests
    T --> T1[SemanixEngine.Tests]
    T1 --> T1A[FormsService]
    T1A --> T1A1[IntegrationTests]
    T1A --> T1A2[UnitTests]

```


### 🏗 Architecture

```mermaid
flowchart LR
    User[User / API Client]
    FS[Forms Service]
    RS[Rendering Service]
    EB[In-Memory]
    DB1[(SQL Server - FormsServiceDb)]
    DB2[(SQL Server - RenderingServiceDb)]
    Logs[Serilog Logs]
    Metrics[Prometheus]

    User -->|PublishFormCommand| FS
    FS --> DB1
    FS -->|FormPublishedEvent / FormUpdatedEvent| MQ
    MQ --> RS
    RS --> DB2

    FS --> Logs
    RS --> Logs
    FS --> Metrics
    RS --> Metrics
```