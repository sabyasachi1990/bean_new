using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;

namespace DB.Subscriber.RepositoryPattern.Quotation
{
    public class QuotationDBRepository<TEntity> : Repository<TEntity>, IQuotationDBRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
        public QuotationDBRepository(IQuotationDBDataContextAsync dataContext, IQuotationDBUnitOfWorkAysnc unitOfWork)
            : base(dataContext, unitOfWork) { }
    }
}
