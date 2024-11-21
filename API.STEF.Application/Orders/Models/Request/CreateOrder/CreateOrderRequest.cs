using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace API.STEF.Application.Orders.Models.Request.CreateOrder
{
    public record CreateOrderRequest
    {
        [JsonPropertyName("nomeCliente")]
        [Required(ErrorMessage = "Nome obrigatorio")]
        public string CustomerName { get; set; }

        [JsonPropertyName("emailCliente")]
        [EmailAddress(ErrorMessage ="Email invalido")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("itensPedido")]
        public List<CreateOrderItemRequest> OrderItems { get; set; }
    }
}
