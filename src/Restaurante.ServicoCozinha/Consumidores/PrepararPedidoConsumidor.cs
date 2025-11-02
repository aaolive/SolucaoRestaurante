using MassTransit;
using Restaurante.Core;

namespace Restaurante.ServicoCozinha.Consumidores
{
    public class PrepararPedidoConsumidor : IConsumer<IPrepararPedidoContrato>
    {
        private readonly ILogger<PrepararPedidoConsumidor> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public PrepararPedidoConsumidor(ILogger<PrepararPedidoConsumidor> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IPrepararPedidoContrato> context)
        {
            var pedido = context.Message;
            _logger.LogInformation($"Serviço de Cozinha: Recebido pedido {pedido.PedidoId} para preparo.");
            _logger.LogInformation($"Itens: {string.Join(", ", pedido.Itens)}");

            // Simula o tempo de preparo da cozinha
            await Task.Delay(TimeSpan.FromSeconds(10)); 

            _logger.LogInformation($"Serviço de Cozinha: Pedido {pedido.PedidoId} está pronto!");

            // Publica a mensagem de volta para o Serviço de Garçom
            await _publishEndpoint.Publish<IPedidoProntoContrato>(new
            {
                PedidoId = pedido.PedidoId,
                HorarioPronto = DateTime.UtcNow
            });
        }
    }
}