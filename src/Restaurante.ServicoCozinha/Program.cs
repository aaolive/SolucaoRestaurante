using MassTransit;
using Restaurante.ServicoCozinha.Consumidores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Configuração do MassTransit
builder.Services.AddMassTransit(config => {

    // 1. Adicionar o nosso consumidor
    config.AddConsumer<PrepararPedidoConsumidor>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        // 2. Configurar o "Receive Endpoint" (a Fila)
        // O MassTransit criará a fila 'fila-preparar-pedido' e fará o 'bind'
        // automático da mensagem IPrepararPedidoContrato para esta fila.
        cfg.ReceiveEndpoint("fila-preparar-pedido", e =>
        {
            e.ConfigureConsumer<PrepararPedidoConsumidor>(ctx);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
