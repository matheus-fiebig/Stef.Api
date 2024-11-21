namespace API.STEF.Application.Shared.Models
{
    public class EitherOf<L, R>
    {
        public L Left { get; set; }
        public R Right { get; set; }
        
        public static implicit operator EitherOf<L, R>(R right)
        {
            return new()
            {
                Right = right,
            };
        }

        public static implicit operator EitherOf<L, R>(L left)
        {
            return new()
            {
                Left = left,
            };
        }
    }
}
