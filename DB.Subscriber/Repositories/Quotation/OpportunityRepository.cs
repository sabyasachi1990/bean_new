using System;
using System.Linq;
using DB.Subscriber.Entities.Quotation;
using DB.Subscriber.RepositoryPattern.Quotation;

namespace DB.Subscriber.Repositories.Quotation
{
    public static class OpportunityRepository
    {
        public static Opportunity GetOpporunityById(this IQuotationDBRepositoryAsync<Opportunity> repository, Guid id)
        {
            return repository.Queryable().Where(x => x.Id == id).FirstOrDefault();
        }
        
    }
}
