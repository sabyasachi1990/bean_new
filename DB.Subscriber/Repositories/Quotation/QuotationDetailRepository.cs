using System;
using System.Linq;
using DB.Subscriber.Entities.Quotation;
using DB.Subscriber.RepositoryPattern.Quotation;


namespace DB.Subscriber.Repositories.Quotation
{
    public static class QuotationDetailRepository
    {
        public static QuotationDetail GetQuotDetByOpportunityId(this IQuotationDBRepositoryAsync<QuotationDetail> repository, Guid id)
        {
            return repository.Queryable().Where(x => x.OpportunityId == id).FirstOrDefault();
        }
    }
}
