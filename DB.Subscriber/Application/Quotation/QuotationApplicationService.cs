using System;
//using AppsWorld.Framework;
using DB.Subscriber.RepositoryPattern.Quotation;
using DB.Subscriber.DomainServices.Quotation;
using DB.Subscriber.Entities.Quotation;
using DB.Subscriber.Models.Quotation;

namespace DB.Subscriber.Application.Quotation
{
    public class QuotationApplicationService
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IQuotationDetailService _quotDetService;

        IQuotationDBUnitOfWorkAysnc _unitOfWorkAsync;


        public QuotationApplicationService(IQuotationDBUnitOfWorkAysnc unitOfWorkAsync, IOpportunityService opportunityService, IQuotationDetailService quotDetService)
        {
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._opportunityService = opportunityService;
            this._quotDetService = quotDetService;
        }

        public void UpdateOpportunity(OpportunityModel oppModel)
        {
            Opportunity opportunity = _opportunityService.GetOpporunityById(oppModel.Id);
            opportunity.Stage = "quoted";
            _opportunityService.Update(opportunity);

            try
            {
                _unitOfWorkAsync.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQuotationDetail(OpportunityModel oppModel)
        {
            QuotationDetail quotDetail = _quotDetService.GetQuotDetByOpportunityId(oppModel.Id);
            if(oppModel.IsChangeHappen == true)
            {
                quotDetail.IsModified = true;
                _quotDetService.Update(quotDetail);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
