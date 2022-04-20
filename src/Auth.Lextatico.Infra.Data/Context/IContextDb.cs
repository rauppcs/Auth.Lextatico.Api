using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.Lextatico.Infra.Data.Context
{
    public interface IContextDb : IUnityOfWork
    {
        bool ActiveTransaction { get; }

        IDbContextTransaction CurrentTransaction { get; }

        Task<IDbContextTransaction> StartTransactionAsync();

        IExecutionStrategy CreateExecutionStrategy();

        Task SubmitTransactionAsync(IDbContextTransaction transaction);

        Task UndoTransaction(IDbContextTransaction transaction);
    }
}
