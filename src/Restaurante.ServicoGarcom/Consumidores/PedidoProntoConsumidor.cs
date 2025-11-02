using MassTransit;
using Microsoft.EntityFrameworkCore;
using Restaurante.Core;
using Restaurante.ServicoGarcom.Data; 
using Restaurante.ServicoGarcom.Data.Modelos;

namespace Restaurante.ServicoGarcom.Consumidores
{
    public class PedidoProntoConsumidor : IConsumer<IPedidoProntoContrato>
    {
        private readonly ILogger<PedidoProntoConsumidor> _logger;
        private readonly GarcomDbContext _dbContext;

        public PedidoProntoConsumidor(ILogger<PedidoProntoConsumidor> logger, GarcomDbContext dbContext) // Injetar
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<IPedidoProntoContrato> context)
        {
            var pedidoMensagem = context.Message;
            _logger.LogWarning($"Serviço de Garçom: ATENÇÃO! Pedido {pedidoMensagem.PedidoId} está pronto na cozinha.");

            // *** LÓGICA DO BANCO DE DADOS ***
            
            // 1. Encontrar o pedido no banco de dados
            var pedido = await _dbContext.Pedidos.FirstOrDefaultAsync(p => p.Id == pedidoMensagem.PedidoId);

            if (pedido == null)
            {
                // Isso não deveria acontecer se tudo correr bem
                _logger.LogError($"Serviço de Garçom: Pedido pronto {pedidoMensagem.PedidoId} recebido, mas não encontrado no banco de dados!");
                return;
            }

            // 2. Atualizar o status e salvar
            pedido.Status = StatusPedido.Pronto;
            pedido.HorarioPronto = pedidoMensagem.HorarioPronto;

            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"Serviço de Garçom: Pedido {pedido.Id} atualizado para 'Pronto' no banco.");
            
            // Aqui você poderia, por exemplo, enviar outra mensagem (ex: INotificacaoGarcom)
            // ou disparar um SignalR para o front-end.
        }
    }
}