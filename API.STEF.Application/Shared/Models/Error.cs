using System.Runtime.InteropServices;

namespace API.STEF.Application.Shared.Models
{
    public class Error
    {
        public string Code { get; init; }
        public string Description { get; init; }

        public static Error Create(string description, string code = null)
        {
            return new()
            {
                Description = description,
                Code = code is null ? Guid.NewGuid().ToString() : code
            };
        }
    }
}
