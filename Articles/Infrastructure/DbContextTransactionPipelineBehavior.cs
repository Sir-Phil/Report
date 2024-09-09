using MediatR;

namespace Articles.Infrastructure
{

    /// <summary>
    /// Adds transaction to the processing pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// 
    public class DbContextTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> 
    {
        private readonly ReportDbContext _dbContext;
        public DbContextTransactionPipelineBehavior(ReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse result = default;
            try
            {
                _dbContext.BeginTransaction();
                result = await next();
                _dbContext.CommitTransaction();
            }
            catch (Exception)
            {
                _dbContext.RollbackTransaction();
                throw;
            }
            return result;
        }
    }
}
