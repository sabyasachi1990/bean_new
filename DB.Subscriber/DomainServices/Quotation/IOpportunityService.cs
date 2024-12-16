using System;
using Service.Pattern;
using DB.Subscriber.Entities.Quotation;

namespace DB.Subscriber.DomainServices.Quotation
{
    public interface IOpportunityService : IService<Opportunity>
    {
        //IEnumerable<Opportunity> GetAllOpportunity(long companyId);
        Opportunity GetOpporunityById(Guid id);
    }
}
