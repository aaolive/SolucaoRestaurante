using MassTransit;
using Restaurante.Core;

namespace Restaurante.ServicoGarcom.Consumidores
{
    public class NovoPedidoConsumidor : IConsumer<INovoPedidoContrato>
    {
        private readonly ILogger<NovoPedidoConsumidor> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public NovoPedidoConsumidor(ILogger<NovoPedidoConsumidor> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<INovoPedidoContrato> context)
        {
            var pedido = context.Message;
            _logger.LogInformation($"Serviço de Garçom: Recebido novo pedido {pedido.PedidoId} da mesa {pedido.NumeroMesa}.");

            // Lógica do Garçom: validar, atribuir, etc.
            // ...
            _logger.LogInformation($"Serviço de Garçom: Enviando pedido {pedido.PedidoId} para a Cozinha.");

            // Envia a mensagem para a Cozinha
            await _publishEndpoint.Publish<IPrepararPedidoContrato>(new
            {
                pedido.PedidoId,
                pedido.Itens
            });
        }
    }
}