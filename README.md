# Idempotency.Net

Example implementation of idempotent request handling in .NET (Redis + PostgreSQL).  
Code is for portfolio/demo purposes, not a production NuGet package.

## Features

- ASP.NET Core support (controllers + minimal APIs)
- Redis and PostgreSQL storage providers
- Integration tests with Testcontainers

## Run tests

Requires Docker.

```bash
dotnet test tests/Idempotency.Net.Redis.IntegrationTests
dotnet test tests/Idempotency.Net.PostgreSql.IntegrationTests
