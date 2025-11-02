using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Core;

namespace Restaurante.ServicoCardapio.Controllers
{
    // DTO (Data Transfer Object) para receber os dados do pedido via API
    public class PedidoViewModel
    {
        public List<string> Itens { get; set; } = new();
        public int NumeroMesa { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<PedidosController> _logger;

        // Injetamos o IPublishEndpoint do MassTransit
        public PedidosController(IPublishEndpoint publishEndpoint, ILogger<PedidosController> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoViewModel pedido)
        {
            var pedidoId = Guid.NewGuid();
            _logger.LogInformation($"Serviço de Cardápio: Recebido novo pedido {pedidoId} da mesa {pedido.NumeroMesa}.");

            // Publicamos a mensagem no RabbitMQ usando o contrato
            await _publishEndpoint.Publish<INovoPedidoContrato>(new
            {
                PedidoId = pedidoId,
                Timestamp = DateTime.UtcNow,
                Itens = pedido.Itens,
                pedido.NumeroMesa
            });

            _logger.LogInformation($"Serviço de Cardápio: Mensagem 'INovoPedidoContrato' publicada para o pedido {pedidoId}.");

            return Accepted(new { PedidoId = pedidoId });
        }
    }
}