using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace API.STEF.Application.Orders.Models.Request.UpdateOrder
{
    public record UpdateOrderRequest
    {
        [JsonPropertyName("nomeCliente")]
        [Required(ErrorMessage="Nome obrigatorio")]
        public string CustomerName { get; set; }

        [JsonPropertyName("emailCliente")]
        [EmailAddress(ErrorMessage = "Email invalido")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("pago")]
        public bool Paid { get; set; }

        [JsonPropertyName("itensPedido")]
        public List<UpdateOrderItemRequest> OrderItems { get; set; }
    }
}
