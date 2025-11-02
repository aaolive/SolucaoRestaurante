using MassTransit;
using Restaurante.ServicoGarcom.Consumidores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Configuração do MassTransit
builder.Services.AddMassTransit(config => {

    // 1. Adicionar nossos consumidores
    config.AddConsumer<NovoPedidoConsumidor>();
    config.AddConsumer<PedidoProntoConsumidor>();

    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        // 2. Configurar a fila para NOVOS PEDIDOS
        cfg.ReceiveEndpoint("fila-novo-pedido", e =>
        {
            e.ConfigureConsumer<NovoPedidoConsumidor>(ctx);
        });

        // 3. Configurar a fila para PEDIDOS PRONTOS
        cfg.ReceiveEndpoint("fila-pedido-pronto", e =>
        {
            e.ConfigureConsumer<PedidoProntoConsumidor>(ctx);
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
