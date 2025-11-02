namespace Restaurante.Core
{
    // Mensagem enviada pelo Cardapio para o Garçom
    public interface INovoPedidoContrato
    {
        Guid PedidoId { get; }
        DateTime Timestamp { get; }
        List<string> Itens { get; }
        int NumeroMesa { get; }
    }

    // Mensagem enviada pelo Garçom para a Cozinha
    public interface IPrepararPedidoContrato
    {
        Guid PedidoId { get; }
        List<string> Itens { get; }
    }

    // Mensagem enviada pela Cozinha de volta para o Garçom
    public interface IPedidoProntoContrato
    {
        Guid PedidoId { get; }
        DateTime HorarioPronto { get; }
    }
}