using System;
using System.Linq;
using Service.Pattern;
using DB.Subscriber.Entities.Quotation;
using DB.Subscriber.RepositoryPattern.Quotation;


namespace DB.Subscriber.DomainServices.Quotation
{
    public class QuotationDetailService : Service<QuotationDetail> , IQuotationDetailService
    {
        private readonly IQuotationDBRepositoryAsync<QuotationDetail> _quoteDetRepository;
        public QuotationDetailService(IQuotationDBRepositoryAsync<QuotationDetail> quoteDetRepository)
            : base(quoteDetRepository)
        {
            _quoteDetRepository = quoteDetRepository;
        }

        //public IEnumerable<Opportunity> GetAllOpportunity(long companyId)
        //{
        //    return _opportunityRepository.Queryable().Where(x => x.CompanyId.Equals(companyId)).AsEnumerable();
        //}
        public QuotationDetail GetQuotDetByOpportunityId(Guid id)
        {
            return _quoteDetRepository.Queryable().Where(c => c.OpportunityId == id).FirstOrDefault();
        }
    }
}
