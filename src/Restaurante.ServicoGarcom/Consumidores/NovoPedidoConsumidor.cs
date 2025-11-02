using MassTransit;
using Restaurante.Core;
using Restaurante.ServicoGarcom.Data; 
using Restaurante.ServicoGarcom.Data.Modelos;

namespace Restaurante.ServicoGarcom.Consumidores
{
    public class NovoPedidoConsumidor : IConsumer<INovoPedidoContrato>
    {
        private readonly ILogger<NovoPedidoConsumidor> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly GarcomDbContext _dbContext;

        public NovoPedidoConsumidor(ILogger<NovoPedidoConsumidor> logger, IPublishEndpoint publishEndpoint, GarcomDbContext dbContext) // Injetar DbContext
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<INovoPedidoContrato> context)
        {
            var pedidoMensagem = context.Message;
            _logger.LogInformation($"Serviço de Garçom: Recebido novo pedido {pedidoMensagem.PedidoId} da mesa {pedidoMensagem.NumeroMesa}.");

            // *** LÓGICA DO BANCO DE DADOS ***
            
            // 1. Criar a entidade Pedido
            var novoPedido = new Pedido
            {
                Id = pedidoMensagem.PedidoId,
                NumeroMesa = pedidoMensagem.NumeroMesa,
                Itens = string.Join(", ", pedidoMensagem.Itens), // Converte lista para string
                Status = StatusPedido.Recebido,
                HorarioRecebimento = pedidoMensagem.Timestamp
            };

            // 2. Salvar no banco
            _dbContext.Pedidos.Add(novoPedido);
            
            // 3. Envia a mensagem para a Cozinha (fazemos isso ANTES de salvar no DB)
            //    Isso segue um padrão de "Outbox" (embora simplificado).
            //    Se o envio da mensagem falhar, não salvamos no DB e a mensagem original (INovoPedidoContrato)
            //    voltará para a fila para ser re-processada.
            // TODO:  LOGICA DE TESTAR O ENVIO PARA A FILA E DEPOIS SALVAR NO BANCO
            _logger.LogInformation($"Serviço de Garçom: Enviando pedido {novoPedido.Id} para a Cozinha.");
            await _publishEndpoint.Publish<IPrepararPedidoContrato>(new
            {
                pedidoMensagem.PedidoId,
                pedidoMensagem.Itens
            });
            
            // 4. Atualiza o status local e salva tudo no banco
            novoPedido.Status = StatusPedido.EnviadoParaCozinha;
            await _dbContext.SaveChangesAsync(); 
            
            _logger.LogInformation($"Serviço de Garçom: Pedido {novoPedido.Id} salvo no banco com status {novoPedido.Status}.");
        }
    }
}