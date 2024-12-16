using System;
using Service.Pattern;
using DB.Subscriber.Entities.Quotation;

namespace DB.Subscriber.DomainServices.Quotation
{
    public interface IQuotationDetailService : IService<QuotationDetail>
    {
        QuotationDetail GetQuotDetByOpportunityId(Guid id);
    }
}
