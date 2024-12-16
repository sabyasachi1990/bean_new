using Repository.Pattern.Ef6;

namespace DB.Subscriber.RepositoryPattern.Quotation
{
    public class QuotationDBUnitOfWork : UnitOfWork, IQuotationDBUnitOfWorkAysnc
    {
        public QuotationDBUnitOfWork(IQuotationDBDataContextAsync dataContext)
            : base(dataContext) { }
    }
}
