using System.Text.Json.Serialization;

namespace API.STEF.Application.Orders.Models.Request.UpdateOrder
{
    public record UpdateOrderItemRequest
    {
        [JsonPropertyName("idProduto")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }
    }
}
