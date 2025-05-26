using Buzzard.DemoApi.Notification;
using Buzzard.DemoApi.Request;
using Buzzard.Extensions;
using Buzzard.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBuzzard();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Buzzard Demo API");

app.MapPost("/echo", async ([FromServices] IBuzzardMediator mediator, [FromBody] TextRequest request) =>
{
    var response = await mediator.SendAsync(request);
    return Results.Ok(response);
});

app.MapPost("/notify", async ([FromServices] IBuzzardMediator mediator, [FromBody] string message) =>
{
    await mediator.PublishAsync(new MessageNotification { Message = message });
    return Results.Ok("Notification sent, check the console log.");
});

app.Run();