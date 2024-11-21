namespace API.STEF.Domain.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task BeginTrasactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTrasactionAsync();
    }
}
