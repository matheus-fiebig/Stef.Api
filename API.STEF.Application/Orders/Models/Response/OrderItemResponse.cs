using API.STEF.Domain.OrderAggregator;
using System.Text.Json.Serialization;

namespace API.STEF.Application.Orders.Models.Response
{
    public record OrderItemResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("idProduto")]
        public int ProductId { get; set; }

        [JsonPropertyName("nomeProduto")]
        public string ProductName { get; set; }

        [JsonPropertyName("valorUnitario")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantity { get; set; }

        public static OrderItemResponse MapFrom(OrderItem orderItem)
        {
            return new()
            {
                Id = orderItem.Id,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.Product?.Name,
                UnitPrice = orderItem.Product?.Value ?? 0,
                Quantity = orderItem.Quantity,
            };
        }
    }
}