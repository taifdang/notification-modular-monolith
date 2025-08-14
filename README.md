<div align="center">
  <img width="277" height="249" alt="image" src="https://github.com/user-attachments/assets/aeb1531e-7966-4e2d-be63-fc96f90afe5e" />
</div>
<br>

>🤖 ***Practical Modular Monolith, built with .Net 8,Vertical Slice Architecture, CQRS, Event-Driven Architecture and the latest technologies.***

## The goals of this project
- ❇️ Using `Vertical Slice Architecture` for architecture level.
- ❇️ Using `CQRS` implementation with `MediatR` library.
- ❇️ Using `InMemory Broker`on top of `Masstransit` for `Event Driven Architecture`.
- ❇️ Using `OpenIDict` for authentication and authorization
- ❇️ Using `Fluent Validation` for validate data
- ❇️ Using `Mapster` for mapping data


## Technologies - Libraries
- ✔️ [`.NET 8`](https://github.com/dotnet/aspnetcore)
- ✔️ [`EF Core`]()
- ✔️ [`MediatR`]()
- ✔️ [`FluentValidation`]()
- ✔️ [`Serilog`]()
- ✔️ [`Polly`]()
- ✔️ [`Mapster`]()
- ✔️ [`Grpc-dotnet`]()
- ✔️ [`Masstransit`]()
- ✔️ [`OpenIdDict`]()
- ✔️ [`Hangfire`]()
- ✔️ [`SignalR`]()

## Structure of Project
### 🧱 Modular Monolith

[A Modular Monolith]() is a software architecture that structures the application as a single deployment unit (like a traditional monolith) but organizes its internal components or modules in such a way that they are loosely coupled and highly cohesive.<br>

Each module within the architecture focuses on a specific `business domain` or `functionality`, similar to how microservices operate, but without the distributed system complexity.

<div align="center"> 
  <img width="734" height="291" alt="image" src="https://github.com/user-attachments/assets/f3ffad49-6fe6-4e4a-b148-4e1e7d39e8fc" />
</div>

### 🚀 Event-Driven Achitecture

[Event-driven architecture](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven) (EDA) is a software architecture paradigm concerning the production and detection of events.<br>

An event-driven architecture consists of `event producers` that generate a stream of `event`, `event consumers` that listen for these events, and `event channels` that transfer events from producers to consumers.

<div align="center">
  <img width="717" height="458" alt="image" src="https://github.com/user-attachments/assets/5e177781-0243-4e1a-8f20-055679bf6392" />
</div>
