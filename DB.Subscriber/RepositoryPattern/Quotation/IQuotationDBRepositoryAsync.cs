using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace DB.Subscriber.RepositoryPattern.Quotation
{
    public interface IQuotationDBRepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IObjectState { }
}
