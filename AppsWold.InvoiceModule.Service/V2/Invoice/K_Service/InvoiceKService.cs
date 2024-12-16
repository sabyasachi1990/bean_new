using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Linq;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class InvoiceKService : Service<InvoiceK>, IInvoiceKService
    {
        private readonly IInvoiceKModuleRepositoryAsync<InvoiceK> _invoiceKRepository;
        private readonly IInvoiceKModuleRepositoryAsync<BeanEntityCompact> _beanEntityRepository;
        private readonly IInvoiceKModuleRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IInvoiceKModuleRepositoryAsync<CompanyUserCompact> _companyUserRepository;


        public InvoiceKService(IInvoiceKModuleRepositoryAsync<InvoiceK> invoiceKRepository, IInvoiceKModuleRepositoryAsync<BeanEntityCompact> beanEntityRepository, IInvoiceKModuleRepositoryAsync<CompanyCompact> companyRepository, IInvoiceKModuleRepositoryAsync<CompanyUserCompact> companyUserRepository)
            : base(invoiceKRepository)
        {
            this._invoiceKRepository = invoiceKRepository;
            this._beanEntityRepository = beanEntityRepository;
            this._companyRepository = companyRepository;
            this._companyUserRepository = companyUserRepository;
        }


        public IQueryable<InvoiceModelK> GetAllInvoicesK(string username, long companyId, string internalState)
        {
            try
            {
                IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                return (from invoice in _invoiceKRepository.Queryable()
                        from beanEntity in beanEntityRepository
                        where (invoice.EntityId == beanEntity.Id && invoice.CompanyId == beanEntity.CompanyId)
                        join company in _companyRepository.Queryable() on invoice.ServiceCompanyId equals company.Id
                        join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                        where
invoice.DocType == DocTypeConstants.Invoice && (internalState != InvoiceStates.Parked ? (invoice.InternalState == internalState || invoice.InternalState == InvoiceStates.Void) : invoice.InternalState == internalState) && invoice.Status == RecordStatusEnum.Active
                          && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username

                        select new InvoiceModelK()
                        {
                            Id = invoice.Id,
                            CompanyId = invoice.CompanyId,
                            DocNo = invoice.DocNo,
                            DueDate = invoice.DueDate,
                            DocDate = invoice.DocDate,
                            DocumentState = invoice.DocumentState,
                            EntityName = beanEntity.Name,
                            GrandTotal = (double)(invoice.GrandTotal),
                            CreatedDate = invoice.CreatedDate,
                            DocCurrency = invoice.DocCurrency,
                            Nature = invoice.Nature,
                            BalanceAmount = (double)(invoice.BalanceAmount),
                            ExchangeRate = (invoice.ExchangeRate).ToString(),
                            PONo = invoice.PONo,
                            ServiceCompanyName = company.ShortName,
                            BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                            UserCreated = invoice.UserCreated,
                            ModifiedBy = invoice.ModifiedBy,
                            ModifiedDate = invoice.ModifiedDate,
                            BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                            InternalState = invoice.InternalState,
                            DocType = invoice.DocType,
                            DocSubType = invoice.DocSubType,
                            IsSystem = (invoice.IsWorkFlowInvoice == true || invoice.IsOBInvoice == true) ? true : false,
                            ScreenName = "Invoice"
                        }).OrderByDescending(a => a.CreatedDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<RecurringInvoiceK> GetAllRecurringInvoicesK(string username, long companyId)
        {
            try
            {
                IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceK> invoiceRepository = _invoiceKRepository.Queryable().Where(c => c.CompanyId == companyId);
                return (from invoice in invoiceRepository
                        from beanEntity in beanEntityRepository
                        where (invoice.EntityId == beanEntity.Id)
                        join company in _companyRepository.Queryable() on invoice.ServiceCompanyId equals company.Id
                        join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                        where (
                        invoice.DocType == DocTypeConstants.Invoice && invoice.InternalState == InvoiceStates.Recurring && invoice.Status == RecordStatusEnum.Active)
                        && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                        select new RecurringInvoiceK()
                        {
                            Id = invoice.Id,
                            CompanyId = invoice.CompanyId,
                            DocNo = invoice.DocNo,
                            DueDate = invoice.DueDate,
                            DocDate = invoice.DocDate,
                            NextDue = invoice.NextDue,
                            LastPosted = invoice.LastPostedDate,
                            DocumentState = invoice.DocumentState,
                            FreqEndDate = invoice.RepEndDate,
                            FreqValue = invoice.RepEveryPeriodNo,
                            EntityName = beanEntity.Name,
                            GrandTotal = (double)(invoice.GrandTotal),
                            CreatedDate = invoice.CreatedDate,
                            DocCurrency = invoice.DocCurrency,
                            InvoiceNumber = invoice.InvoiceNumber,
                            Nature = invoice.Nature,
                            BalanceAmount = (double)(invoice.BalanceAmount),
                            ExchangeRate = (double)(invoice.ExchangeRate),
                            PONo = invoice.PONo,
                            ServiceCompanyName = company.ShortName,
                            BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                            UserCreated = invoice.UserCreated,
                            ModifiedBy = invoice.ModifiedBy,
                            ModifiedDate = invoice.ModifiedDate,
                            BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                            InternalState = invoice.InternalState,
                            DocType = invoice.DocType,
                            DocSubType = invoice.DocSubType,
                            Posted = invoice.Counter
                        }).OrderByDescending(a => a.CreatedDate).AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<InvoiceModelK> GetAllRecuurringPostedInvoicesK(long companyId, string internalState, Guid id)
        {
            try
            {

                //IQueryable<BeanEntity> beanEntityRepository = _invoiceModuleService.GetRepository<BeanEntity>().Queryable();
                IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceK> invoiceRepository = _invoiceKRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceModelK> invoiceModelK = from invoice in invoiceRepository
                                                          from beanEntity in beanEntityRepository
                                                          where (invoice.EntityId == beanEntity.Id)
                                                          join company in _companyRepository.Queryable()
                                                          on invoice.ServiceCompanyId equals company.Id
                                                          where (
                                                          invoice.DocType == DocTypeConstants.Invoice && (invoice.InternalState == internalState || invoice.DocumentState == InvoiceStates.Deleted) && invoice.RecurInvId == id)
                                                          select new InvoiceModelK()
                                                          {
                                                              Id = invoice.Id,
                                                              CompanyId = invoice.CompanyId,
                                                              DocNo = invoice.DocNo,
                                                              DueDate = invoice.DueDate,
                                                              DocDate = invoice.DocDate,
                                                              DocumentState = invoice.DocumentState,
                                                              EntityName = beanEntity.Name,
                                                              GrandTotal = (double)(invoice.GrandTotal),
                                                              CreatedDate = invoice.CreatedDate,
                                                              DocCurrency = invoice.DocCurrency,
                                                              Nature = invoice.Nature,
                                                              BalanceAmount = (double)(invoice.BalanceAmount),
                                                              ExchangeRate = (invoice.ExchangeRate).ToString(),
                                                              PONo = invoice.PONo,
                                                              ServiceCompanyName = company.ShortName,
                                                              BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                              UserCreated = invoice.UserCreated,
                                                              ModifiedBy = invoice.ModifiedBy,
                                                              ModifiedDate = invoice.ModifiedDate,
                                                              BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                              DocType = invoice.DocType,
                                                              DocSubType = invoice.DocSubType,
                                                              InternalState = invoice.InternalState,
                                                              Action = invoice.DocumentState == "Deleted" ? "Deleted" : invoice.InternalState
                                                          };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<CreditNoteModelK> GetAllCreditNoteK(string username, long companyId)
        {
            try
            {

                IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceK> invoiceRepository = _invoiceKRepository.Queryable();
                return (from invoice in invoiceRepository
                        join beanEntity in beanEntityRepository on invoice.EntityId equals beanEntity.Id
                        join company in _companyRepository.Queryable() on invoice.ServiceCompanyId equals company.Id
                        join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                        where invoice.DocType == DocTypeConstants.CreditNote
                        where invoice.CompanyId == companyId && invoice.Status == RecordStatusEnum.Active
                        && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                        select new CreditNoteModelK()
                        {
                            Id = invoice.Id,
                            CompanyId = invoice.CompanyId,
                            DocNo = invoice.DocNo,
                            DueDate = invoice.DueDate,
                            DocDate = invoice.DocDate,
                            DocSubType = invoice.DocSubType,
                            DocumentState = invoice.DocumentState,
                            EntityName = beanEntity.Name,
                            ServiceCompanyName = company.ShortName,
                            GrandTotal = (double)(invoice.GrandTotal),
                            InvoiceNumber = invoice.InvoiceNumber,
                            CreatedDate = invoice.CreatedDate,
                            DocCurrency = invoice.DocCurrency,
                            BalanceAmount = (double)(invoice.BalanceAmount),
                            ExchangeRate = (invoice.ExchangeRate).ToString(),
                            Nature = invoice.Nature,
                            ModifiedBy = invoice.ModifiedBy,
                            ModifiedDate = invoice.ModifiedDate,
                            UserCreated = invoice.UserCreated,
                            ExtensionType = invoice.ExtensionType,
                            IsExternal = invoice.ExtensionType == DocTypeConstants.Receipt ? true : false,
                            IsSystem = invoice.ExtensionType == DocTypeConstants.Receipt ? true : false,
                            BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                            BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                            ScreenName = "Credit Note"
                        }).OrderByDescending(a => a.CreatedDate).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<DoubtfullDebitModelK> GetAllDebitfulldebitK(string username, long companyId)
        {
            try
            {
                IQueryable<BeanEntityCompact> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceK> invoiceRepository = _invoiceKRepository.Queryable();
                return (from invoice in invoiceRepository
                        from beanEntity in beanEntityRepository
                        where (invoice.EntityId == beanEntity.Id)
                        join company in _companyRepository.Queryable() on invoice.ServiceCompanyId equals company.Id
                        join compUser in _companyUserRepository.Queryable() on company.ParentId equals compUser.CompanyId
                        where invoice.CompanyId == companyId
                        where invoice.DocType == DocTypeConstants.DoubtFulDebitNote && invoice.Status == RecordStatusEnum.Active
                        && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                        select new DoubtfullDebitModelK()
                        {
                            Id = invoice.Id,
                            CompanyId = invoice.CompanyId,
                            DocNo = invoice.DocNo,
                            DueDate = invoice.DueDate,
                            DocDate = invoice.DocDate,
                            DocumentState = invoice.DocumentState,
                            DocCurrency = invoice.DocCurrency,
                            Nature = invoice.Nature,
                            BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                            ExchangeRate = (invoice.ExchangeRate).ToString(),
                            EntityName = beanEntity.Name,
                            GrandTotal = (double)(invoice.GrandTotal),
                            CreatedDate = invoice.CreatedDate,
                            UserCreated = invoice.UserCreated,
                            ModifiedBy = invoice.ModifiedBy,
                            ModifiedDate = invoice.ModifiedDate,
                            ServiceCompanyName = company.ShortName,
                            BalanceAmmount = (double)(invoice.BalanceAmount),
                            BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2)
                        }).OrderByDescending(a => a.CreatedDate).AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
