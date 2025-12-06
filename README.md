# Chat With Local LLM – ASP.NET Web API (.NET Framework 4.7.2)

A layered **ASP.NET Web API** backend for a simple chat system that talks to a **local LLM via Ollama**.

- Users can register & log in
- Auth is handled with **tokens** (database-backed)
- Authenticated users can send messages to an AI model (e.g. **Mistral** running in Ollama)
- Chat history is saved per user in SQL Server
- Clean 3-layer architecture: **DAL → BLL → Presentation**

---

## Tech Stack

- **Backend Framework**: ASP.NET Web API (C#, .NET Framework 4.7.2)
- **Architecture**: 3-layer (DAL / BLL / Presentation)
- **ORM**: Entity Framework 6 (Code First + Migrations)
- **Database**: SQL Server / SQL Server Express
- **Auth**: Custom token-based auth (`Token` table + `Logged` filter)
- **LLM**: [Ollama](https://ollama.com/) running locally
  - Default model used: `mistral`
- **JSON**: Newtonsoft.Json (`13.0.x`)
- **Mapping**: [AutoMapper](https://automapper.org/) (by Jimmy Bogard) for mapping EF entities to DTOs

---

## Project Structure

Rough structure (names may vary slightly):

````text
Solution
├── DAL                     # Data Access Layer
│   ├── EF
│   │   ├── UMSContext.cs   # DbContext
│   │   └── Tables
│   │       ├── User.cs
│   │       ├── ChatMessage.cs
│   │       └── Token.cs
│   ├── Interfaces
│   │   ├── IRepo.cs
│   │   ├── IAuth.cs
│   │   └── IChatMessageRepo.cs
│   ├── Repos
│   │   ├── UserRepo.cs
│   │   ├── ChatMessageRepo.cs
│   │   └── TokenRepo.cs
│   └── DataAccessFactory.cs
│
├── BLL                     # Business Logic Layer
│   ├── DTOs
│   │   ├── UserDTO.cs
│   │   ├── ChatMessageDTO.cs
│   │   ├── TokenDTO.cs
│   │   └── ChatRequestDTO.cs
│   ├── External
│   │   └── OllamaClient.cs # HTTP client for Ollama
│   └── Services
│       ├── UserService.cs
│       ├── AuthService.cs
│       └── ChatService.cs
│
└── ChatApiApp              # Presentation / Web API
    ├── Controllers
    │   ├── AuthController.cs
    │   ├── UserController.cs
    │   └── ChatController.cs
    ├── Auth
    │   └── Logged.cs        # Authorization filter
    ├── Models
    │   └── LoginModel.cs
    └── Web.config



---


## Design & SOLID Principles

This project is not just “working code” – I also tried to follow **4 SOLID principles**
(S, O, I, D – I did not explicitly focus on Liskov Substitution here).

### 1. Single Responsibility Principle (SRP)

> A class should have only one reason to change.

How it’s applied:

- **Entities / Tables** (`User`, `ChatMessage`, `Token`)
  - Only represent data + relationships.
- **Repositories** (`UserRepo`, `ChatMessageRepo`, `TokenRepo`)
  - Only handle **database operations** (CRUD) for a specific entity.
- **Services** (`UserService`, `AuthService`, `ChatService`)
  - Only contain **business logic** (auth, token creation, chat flow, etc.).
- **Controllers** (`AuthController`, `UserController`, `ChatController`)
  - Only handle **HTTP concerns** (routes, status codes, request/response).

So if business rules change, I only touch **Services**.
If DB changes, I touch **Repos/Entities**.
If API format changes, I touch **Controllers**.

---

### 2. Open/Closed Principle (OCP)

> Classes should be **open for extension**, but **closed for modification**.

How this project reflects OCP:

- `DataAccessFactory` returns interfaces like `IRepo<User,int,bool>`, `IChatMessageRepo`, etc.
  If I want a new data source (e.g., another repo implementation), I can **add a new class** and **adjust the factory**, without changing existing services.
- `ChatService` is written in a way that I can **swap the LLM implementation**
  (`OllamaClient` → future `RemoteLlmClient`) without changing controller logic:
  - Controllers only call: `ChatService.SendMessage(...)`
  - Inside `ChatService`, I can change which client is used.

---

### 3. Interface Segregation Principle (ISP)

> Many small, specific interfaces are better than one huge interface.

Used interfaces:

- `IRepo<T, PK, RET>` – generic CRUD operations
- `IAuth<RET>` – only for authentication (`Authenticate(userName, password)`)
- `IChatMessageRepo` – extends `IRepo<ChatMessage, int, bool>` + adds
  `List<ChatMessage> GetByUser(int userId, int take)`

This way:

- Auth-related logic doesn’t force every repo to implement `Authenticate`.
- Chat-specific logic doesn’t pollute all other repositories.
- Each class only depends on the interfaces it actually uses.

---

### 4. Dependency Inversion Principle (DIP)

> High-level modules should not depend on low-level modules;
> both should depend on **abstractions**.

How it’s applied:

- **Services** (`UserService`, `ChatService`, `AuthService`) do **not** directly create `new UserRepo()` or `new ChatMessageRepo()`.
  Instead, they use:

  ```csharp
  DataAccessFactory.UserData();           // returns IRepo<User, int, bool>
  DataAccessFactory.ChatMessageData();   // returns IChatMessageRepo
  DataAccessFactory.TokenData();         // returns IRepo<Token, string, Token>

DataAccessFactory hides the concrete repo classes (UserRepo, ChatMessageRepo, etc.) behind interfaces (IRepo, IAuth, IChatMessageRepo).
````
