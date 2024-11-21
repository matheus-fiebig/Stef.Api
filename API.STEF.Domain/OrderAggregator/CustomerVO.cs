namespace API.STEF.Domain.OrderAggregator
{
    public class CustomerVO
    {
        public required string Name { get; init; }

        public required string Email { get; init; }

        public CustomerVO()
        {

        }

        public static CustomerVO CreateNew(string name, string email)
        {
            return new()
            { Email = email, Name = name };
        }
    }
}
