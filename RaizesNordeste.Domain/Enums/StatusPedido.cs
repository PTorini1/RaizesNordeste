namespace RaizesNordeste.Domain.Enums;

public enum StatusPedido
{
    Criado = 1,
    AguardandoPagamento = 2,
    PagamentoAprovado = 3,
    PagamentoRecusado = 4,
    EmPreparo = 5,
    Pronto = 6,
    Entregue = 7,
    Cancelado = 8
}
