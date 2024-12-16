using System;
using System.Linq;
using Service.Pattern;
using DB.Subscriber.Entities.Quotation;
using DB.Subscriber.RepositoryPattern.Quotation;


namespace DB.Subscriber.DomainServices.Quotation
{
    public class OpportunityService : Service<Opportunity> , IOpportunityService
    {
        private readonly IQuotationDBRepositoryAsync<Opportunity> _opportunityRepository;
        public OpportunityService(IQuotationDBRepositoryAsync<Opportunity> opportunityRepository)
            : base(opportunityRepository)
        {
            _opportunityRepository = opportunityRepository;
        }

        //public IEnumerable<Opportunity> GetAllOpportunity(long companyId)
        //{
        //    return _opportunityRepository.Queryable().Where(x => x.CompanyId.Equals(companyId)).AsEnumerable();
        //}
        public Opportunity GetOpporunityById(Guid id)
        {
            return _opportunityRepository.Queryable().Where(c => c.Id == id).FirstOrDefault();
        }
   
    }
}
