using MassTransit;
using Restaurante.Core;

namespace Restaurante.ServicoGarcom.Consumidores
{
    public class PedidoProntoConsumidor : IConsumer<IPedidoProntoContrato>
    {
        private readonly ILogger<PedidoProntoConsumidor> _logger;

        public PedidoProntoConsumidor(ILogger<PedidoProntoConsumidor> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IPedidoProntoContrato> context)
        {
            var pedido = context.Message;
            _logger.LogWarning($"Serviço de Garçom: ATENÇÃO! Pedido {pedido.PedidoId} está pronto na cozinha desde {pedido.HorarioPronto}. Buscar imediatamente!");

            // Aqui você poderia, por exemplo, enviar uma notificação para o app do garçom

            return Task.CompletedTask;
        }
    }
}