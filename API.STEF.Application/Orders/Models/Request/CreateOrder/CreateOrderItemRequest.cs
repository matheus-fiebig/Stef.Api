using System.Text.Json.Serialization;

namespace API.STEF.Application.Orders.Models.Request.CreateOrder
{
    public record CreateOrderItemRequest
    {
        [JsonPropertyName("idProduto")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }
    }
}
