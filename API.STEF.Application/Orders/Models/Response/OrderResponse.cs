using API.STEF.Domain.OrderAggregator;
using System.Text.Json.Serialization;

namespace API.STEF.Application.Orders.Models.Response
{
    public record OrderResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nomeCliente")]
        public string CustomerName { get; set; }

        [JsonPropertyName("emailCliente")]
        public string CustomerEmail { get; set; }

        [JsonPropertyName("pago")]
        public bool Paid { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal TotalValue { get; set; }

        [JsonPropertyName("itensPedido")]
        public List<OrderItemResponse> OrderItems { get; set; }

        public static OrderResponse MapFrom(Order order)
        {
            return new()
            {
                Id = order.Id,
                CustomerName = order.Customer.Name,
                CustomerEmail = order.Customer.Email,
                Paid = order.Paid,
                TotalValue = order.OrderItems.Sum(x => (x.Product?.Value ?? 0) * x.Quantity),
                OrderItems = order.OrderItems.Select(OrderItemResponse.MapFrom).ToList()
            };
        }
    }
}
