using Idempotency.Net.AspNetCore;
using Idempotency.Net.AspNetCore.Extensions;
using Idempotency.Net.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdempotency()
                .UseInMemory();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapPost("/minimal-api", () =>
{
    return Results.Ok(Guid.NewGuid().ToString());
}).WithIdempotency();

app.Run();

public partial class Program { }