using System.ComponentModel.DataAnnotations;

namespace Restaurante.ServicoGarcom.Data.Modelos
{
    // Enum para facilitar o gerenciamento do status
    public enum StatusPedido
    {
        Recebido,
        EnviadoParaCozinha,
        Pronto,
        Entregue
    }

    public class Pedido
    {
        // Usaremos o mesmo Guid da mensagem como nossa Primary Key
        [Key]
        public Guid Id { get; set; }
        public int NumeroMesa { get; set; }

        // Vamos "serializar" a lista de itens como uma string simples
        public string Itens { get; set; } = string.Empty;
        public StatusPedido Status { get; set; }
        public DateTime HorarioRecebimento { get; set; }
        public DateTime? HorarioPronto { get; set; }
    }
}