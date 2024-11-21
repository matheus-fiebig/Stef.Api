using API.STEF.Domain.OrderAggregator;
using API.STEF.Domain.Shared.Entities;

namespace API.STEF.Domain.ProductAggregator
{
    public class Product : Entity
    {
        public string Name { get; private set; }

        public decimal Value { get; private set; }

        protected Product()
        {

        }

        public static Product CreateNew(string name, decimal value)
        {
            return new()
            {
                Name = name,
                Value = value
            };
        }
    }
}
