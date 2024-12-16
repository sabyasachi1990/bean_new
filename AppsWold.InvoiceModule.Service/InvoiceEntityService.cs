using AppsWold.InvoiceModule.RepositoryPattern;
using AppsWorld.BeanCursor.Entities.Models;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Entities.Models;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.RepositoryPattern;
using FrameWork;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.Data.SqlClient;
using Polly;
using ServiceStack;

namespace AppsWorld.InvoiceModule.Service
{
    public class InvoiceEntityService : Service<Invoice>, IInvoiceEntityService
    {
        private readonly IInvoiceModuleRepositoryAsync<Invoice> _invoiceModuleService;
        private readonly IInvoiceModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly IInvoiceModuleRepositoryAsync<Company> _companyRepository;
        private readonly IInvoiceModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;

        private readonly IInvoiceModuleRepositoryAsync<Address> _addressRepository;
        private readonly IInvoiceModuleRepositoryAsync<Bank> _bankRepository;
        private readonly IInvoiceModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _CompanyUserDetailRepo;

        public InvoiceEntityService(IInvoiceModuleRepositoryAsync<Invoice> invoiceModuleService, IInvoiceModuleRepositoryAsync<BeanEntity> beanEntityRepository, IInvoiceModuleRepositoryAsync<Company> companyRepository, IInvoiceModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository, IInvoiceModuleRepositoryAsync<Address> addressRepository, IInvoiceModuleRepositoryAsync<Bank> bankRepository, IInvoiceModuleRepositoryAsync<CompanyUser> compUserRepo, IInvoiceModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> CompanyUserDetailRepo)
            : base(invoiceModuleService)
        {
            _invoiceModuleService = invoiceModuleService;
            _beanEntityRepository = beanEntityRepository;
            _companyRepository = companyRepository;
            _termsOfPaymentRepository = termsOfPaymentRepository;
            _CompanyUserDetailRepo = CompanyUserDetailRepo;
            _addressRepository = addressRepository;
            _bankRepository = bankRepository;
            _compUserRepo = compUserRepo;

        }

        public async Task<IQueryable<InvoiceModelK>> GetAllInvoicesKOld(string userName, long companyId, string internalState)
        {
            try
            {
                IQueryable<BeanEntity> beanEntityRepository = await Task.Run(()=>_beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId));
                IQueryable<Invoice> invoiceRepository = await Task.Run(()=> _invoiceModuleService.Queryable().Where(c => c.CompanyId == companyId));
                IQueryable<InvoiceModelK> invoiceModelK = from invoice in invoiceRepository
                                                          from beanEntity in beanEntityRepository
                                                          where (invoice.EntityId == beanEntity.Id)
                                                          join company in await Task.Run(()=> _companyRepository.Queryable()) on invoice.ServiceCompanyId equals company.Id
                                                          join compUser in await Task.Run(()=> _compUserRepo.Queryable()) on company.ParentId equals compUser.CompanyId
                                                          join cud in await Task.Run(()=> _CompanyUserDetailRepo.Queryable()) on compUser.Id equals cud.CompanyUserId
                                                          where company.Id == cud.ServiceEntityId
                                                          where
                  invoice.DocType == DocTypeConstants.Invoice && /*invoice.InternalState == internalState*/ (internalState != "Parked" ? (invoice.InternalState == internalState || invoice.InternalState == "Void") : invoice.InternalState == internalState) && invoice.Status == RecordStatusEnum.Active && compUser.Username == userName

                                                          select new InvoiceModelK()
                                                          {
                                                              Id = invoice.Id,
                                                              CompanyId = invoice.CompanyId,
                                                              DocNo = invoice.DocNo,
                                                              DueDate = invoice.DueDate,
                                                              DocDate = invoice.DocDate,
                                                              DocumentState = invoice.DocumentState,
                                                              EntityName = beanEntity.Name,
                                                              EntityId = beanEntity.Id,
                                                              GrandTotal = (double)(invoice.GrandTotal),
                                                              CreatedDate = invoice.CreatedDate,
                                                              DocCurrency = invoice.DocCurrency,
                                                              Nature = invoice.Nature,
                                                              BalanceAmount = (double)(invoice.BalanceAmount),
                                                              ExchangeRate = (invoice.ExchangeRate).ToString(),
                                                              PONo = invoice.PONo,
                                                              ServiceCompanyName = company.ShortName,
                                                              ServiceCompanyId = company.Id,
                                                              BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                              UserCreated = invoice.UserCreated,
                                                              ModifiedBy = invoice.ModifiedBy,
                                                              ModifiedDate = invoice.ModifiedDate,
                                                              BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                              InternalState = invoice.InternalState,
                                                              DocType = invoice.DocType,
                                                              DocSubType = invoice.DocSubType,
                                                              IsSystem = (invoice.IsWorkFlowInvoice == true || invoice.IsOBInvoice == true) ? true : false,
                                                              ScreenName = "Invoice",
                                                              IsLocked = invoice.IsLocked
                                                          };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<IQueryable<InvoiceModelK>, int>> GetAllInvoicesK(string userName, long companyId, string internalState)
        {
            try
            {
                var retryPolicy = Policy
                    .Handle<SqlException>(ex => ex.Number == -2 || ex.Number == 1205)
                    .RetryAsync(3);
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                        TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var query = from invoice in _invoiceModuleService.Queryable().AsNoTracking()
                                    join beanEntity in _beanEntityRepository.Queryable().AsNoTracking() on invoice.EntityId equals beanEntity.Id
                                    join company in _companyRepository.Queryable().AsNoTracking() on invoice.ServiceCompanyId equals company.Id
                                    join compUser in _compUserRepo.Queryable().AsNoTracking() on company.ParentId equals compUser.CompanyId
                                    join cud in _CompanyUserDetailRepo.Queryable().AsNoTracking() on compUser.Id equals cud.CompanyUserId
                                    where invoice.CompanyId == companyId
                                          && company.Id == cud.ServiceEntityId
                                          && invoice.DocType == DocTypeConstants.Invoice
                                          && (internalState != "Parked" ? (invoice.InternalState == internalState || invoice.InternalState == "Void") : invoice.InternalState == internalState)
                                          && invoice.Status == RecordStatusEnum.Active
                                          && compUser.Username == userName
                                    select new InvoiceModelK()
                                    {
                                        Id = invoice.Id,
                                        CompanyId = invoice.CompanyId,
                                        DocNo = invoice.DocNo,
                                        DueDate = invoice.DueDate,
                                        DocDate = invoice.DocDate,
                                        DocumentState = invoice.DocumentState,
                                        EntityName = beanEntity.Name,
                                        EntityId = beanEntity.Id,
                                        GrandTotal = (double)invoice.GrandTotal,
                                        CreatedDate = invoice.CreatedDate,
                                        DocCurrency = invoice.DocCurrency,
                                        Nature = invoice.Nature,
                                        BalanceAmount = (double)invoice.BalanceAmount,
                                        ExchangeRate = invoice.ExchangeRate.ToString(),
                                        PONo = invoice.PONo,
                                        ServiceCompanyName = company.ShortName,
                                        ServiceCompanyId = company.Id,
                                        BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                        UserCreated = invoice.UserCreated,
                                        ModifiedBy = invoice.ModifiedBy,
                                        ModifiedDate = invoice.ModifiedDate,
                                        BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                        InternalState = invoice.InternalState,
                                        DocType = invoice.DocType,
                                        DocSubType = invoice.DocSubType,
                                        IsSystem = invoice.IsWorkFlowInvoice == true || invoice.IsOBInvoice == true,
                                        ScreenName = "Invoice",
                                        IsLocked = invoice.IsLocked
                                    };
                        var count = await query.CountAsync();

                        var orderedQuery = query.OrderByDescending(a => a.CreatedDate);

                        scope.Complete();

                        return new Tuple<IQueryable<InvoiceModelK>, int>(orderedQuery, count);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the invoices.", ex);
            }
        }

        public async Task<IQueryable<RecurringInvoiceK>> GetAllRecurringInvoicesK(string userName, long companyId)
        {
            try
            {
                IQueryable<BeanEntity> beanEntityRepository = await Task.Run(()=> _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId).AsQueryable());
                IQueryable<Invoice> invoiceRepository = await Task.Run(()=> _invoiceModuleService.Queryable().Where(c => c.CompanyId == companyId).AsQueryable());
                IQueryable<RecurringInvoiceK> invoiceModelK = from invoice in invoiceRepository
                                                              from beanEntity in beanEntityRepository
                                                              where (invoice.EntityId == beanEntity.Id)
                                                              join company in await Task.Run(()=> _companyRepository.Queryable()) on invoice.ServiceCompanyId equals company.Id
                                                              join trms in await Task.Run(()=> _termsOfPaymentRepository.Queryable()) on invoice.CreditTermsId equals trms.Id
                                                              join compUser in await Task.Run(()=> _compUserRepo.Queryable()) on company.ParentId equals compUser.CompanyId
                                                              join cud in await Task.Run(()=> _CompanyUserDetailRepo.Queryable()) on compUser.Id equals cud.CompanyUserId where company.Id == cud.ServiceEntityId
                                                              where (
                                                              invoice.DocType == DocTypeConstants.Invoice && invoice.InternalState == "Recurring" && invoice.Status == RecordStatusEnum.Active) && compUser.Username == userName
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
                                                                  FrequencyType = invoice.RepEveryPeriod,
                                                                  EntityName = beanEntity.Name,
                                                                  GrandTotal = (double)(invoice.GrandTotal),
                                                                  CreatedDate = invoice.CreatedDate,
                                                                  DocCurrency = invoice.DocCurrency,
                                                                  ExCurrency = invoice.ExCurrency,
                                                                  InvoiceNumber = invoice.InvoiceNumber,
                                                                  Nature = invoice.Nature,
                                                                  BalanceAmount = (double)(invoice.BalanceAmount),
                                                                  DocDescription = invoice.Remarks,
                                                                  ExchangeRate = (double)(invoice.ExchangeRate),
                                                                  PONo = invoice.PONo,
                                                                  CreditTermName = trms.Name,
                                                                  ServiceCompanyName = company.ShortName,
                                                                  NoSupportingDocument = invoice.NoSupportingDocs,
                                                                  BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                                  UserCreated = invoice.UserCreated,
                                                                  ModifiedBy = invoice.ModifiedBy,
                                                                  ModifiedDate = invoice.ModifiedDate,
                                                                  BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                                  InternalState = invoice.InternalState,
                                                                  DocType = invoice.DocType,
                                                                  DocSubType = invoice.DocSubType,
                                                                  Posted = invoice.Counter,
                                                                  IsLocked = invoice.IsLocked
                                                              };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<InvoiceVoidK> GetAllVoidInvoicesK(long companyId)
        {
            try
            {

                //IQueryable<BeanEntity> beanEntityRepository = _invoiceModuleService.GetRepository<BeanEntity>().Queryable();
                IQueryable<BeanEntity> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<Invoice> invoiceRepository = _invoiceModuleService.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceVoidK> invoiceModelK = from invoice in invoiceRepository
                                                         from beanEntity in beanEntityRepository
                                                         where (invoice.EntityId == beanEntity.Id)
                                                         join company in _companyRepository.Queryable()
                                                         on invoice.ServiceCompanyId equals company.Id
                                                         where (
                                                         invoice.DocType == DocTypeConstants.Invoice && invoice.InternalState == "Void" && invoice.Status == RecordStatusEnum.Active)
                                                         select new InvoiceVoidK()
                                                         {
                                                             Id = invoice.Id,
                                                             CompanyId = invoice.CompanyId,
                                                             DocNo = invoice.DocNo,
                                                             DocDate = invoice.DocDate,
                                                             DocumentState = invoice.DocumentState,
                                                             EntityName = beanEntity.Name,
                                                             CreatedDate = invoice.CreatedDate,
                                                             DocCurrency = invoice.DocCurrency,
                                                             InvoiceNumber = invoice.InvoiceNumber,
                                                             DocDescription = invoice.Remarks,
                                                             ServiceCompanyName = company.ShortName,
                                                             UserCreated = invoice.UserCreated,
                                                             ModifiedBy = invoice.ModifiedBy,
                                                             ModifiedDate = invoice.ModifiedDate,
                                                             InternalState = invoice.InternalState,
                                                             DocType = invoice.DocType,
                                                             DocSubType = invoice.DocSubType
                                                         };
                return invoiceModelK.OrderByDescending(a => a.ModifiedBy).AsQueryable();

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
                IQueryable<BeanEntity> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<Invoice> invoiceRepository = _invoiceModuleService.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceModelK> invoiceModelK = from invoice in invoiceRepository
                                                          from beanEntity in beanEntityRepository
                                                          where (invoice.EntityId == beanEntity.Id)
                                                          join company in _companyRepository.Queryable()
                                                          on invoice.ServiceCompanyId equals company.Id
                                                          //join trms in _termsOfPaymentRepository.Queryable()
                                                          //.Where(c => c.CompanyId == companyId)
                                                          //on invoice.CreditTermsId equals trms.Id
                                                          where (
                                                          invoice.DocType == DocTypeConstants.Invoice && (invoice.InternalState == internalState || invoice.DocumentState == "Deleted") && invoice.RecurInvId == id /*&& invoice.Status == RecordStatusEnum.Active*/)
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
                                                              //InvoiceNumber = invoice.InvoiceNumber,
                                                              CreatedDate = invoice.CreatedDate,
                                                              DocCurrency = invoice.DocCurrency,
                                                              //BaseCurrency = invoice.ExCurrency,
                                                              Nature = invoice.Nature,
                                                              BalanceAmount = (double)(invoice.BalanceAmount),
                                                              //DocDescription = invoice.Remarks,
                                                              ExchangeRate = (invoice.ExchangeRate).ToString(),
                                                              PONo = invoice.PONo,
                                                              //CreditTermName = trms.Name,
                                                              ServiceCompanyName = company.ShortName,
                                                              //NoSupportingDocument = invoice.NoSupportingDocs,
                                                              BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                              UserCreated = invoice.UserCreated,
                                                              ModifiedBy = invoice.ModifiedBy,
                                                              ModifiedDate = invoice.ModifiedDate,
                                                              //Repeating = invoice.IsRepeatingInvoice,
                                                              BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                              //IsWorkFlowInvoice = invoice.IsWorkFlowInvoice,
                                                              //CursorType = invoice.CursorType,
                                                              //InternalState = invoice.InternalState,
                                                              DocType = invoice.DocType,
                                                              DocSubType = invoice.DocSubType,
                                                              InternalState = invoice.InternalState,
                                                              Action = invoice.DocumentState == "Deleted" ? "Deleted" : invoice.InternalState,
                                                              IsLocked = invoice.IsLocked
                                                              //PostingDate = invoice.DocDate
                                                          };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IQueryable<CreditNoteModelK>> GetAllCreditNoteK(string userName, long companyId)
        {
            try
            {
                IQueryable<BeanEntity> beanEntityRepository = await Task.Run(()=> _beanEntityRepository.Queryable());
                IQueryable<Invoice> invoiceRepository = await Task.Run(()=> _invoiceModuleService.Queryable());
                IQueryable<CreditNoteModelK> invoiceModelK = from invoice in invoiceRepository
                                                             join beanEntity in beanEntityRepository on invoice.EntityId equals beanEntity.Id                               
                                                             join company in await Task.Run(()=> _companyRepository.Queryable()) on invoice.ServiceCompanyId equals company.Id
                                                             join compUser in await Task.Run(() => _compUserRepo.Queryable()) on company.ParentId equals compUser.CompanyId 
                                                             join cud in await Task.Run(() => _CompanyUserDetailRepo.Queryable()) on compUser.Id equals cud.CompanyUserId
                                                             where company.Id == cud.ServiceEntityId                                                         
                                                             where invoice.DocType == DocTypeConstants.CreditNote
                                                             where invoice.CompanyId == companyId && invoice.Status == RecordStatusEnum.Active && compUser.Username == userName
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
                                                                 EntityId = beanEntity.Id,
                                                                 ServiceCompanyName = company.ShortName,
                                                                 ServiceCompanyId = company.Id,
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
                                                                 IsExternal = invoice.ExtensionType == "Receipt" || invoice.ExtensionType == "OBCN" ? true : false,
                                                                 IsSystem = invoice.ExtensionType == "Receipt" || invoice.ExtensionType == "OBCN" ? true : false,
                                                                 BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                                 BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                                 ScreenName = "Credit Note",
                                                                 IsLocked = invoice.IsLocked
                                                             };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Invoice GetCompanyAndId(long companyId, Guid id)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(a => a.InvoiceNotes).Select().FirstOrDefault();
        }
        public Invoice GetByCompanyIdForInvoice(long companyId, string doctype)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance && e.DocType == doctype).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public Invoice GetByCompanyIdForInvoiceWithDocSubType(long companyId, string doctype, string docsubtype)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance && e.DocType == doctype && e.DocSubType == docsubtype).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public Invoice GetByCompanyId(long companyId)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public IEnumerable<dynamic> GetLastInvoiceDocDate(long companyId)
        {
            return _invoiceModuleService.Queryable().Where(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active).OrderByDescending(a => a.CreatedDate).Select(a => new { DocDate = a.DocDate, CreatedDate = a.CreatedDate }).ToList();
        }
        public Invoice GetinvoiceById(Guid Id)
        {
            return _invoiceModuleService.Query(c => c.Id == Id && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<Invoice> GetAllDDByInvoiceId(List<Guid> Ids)
        {
            return _invoiceModuleService.Queryable().Where(c => Ids.Contains(c.Id) && c.Status == RecordStatusEnum.Active).ToList();
        }
        public Invoice GetAllInvoiceLu(long companyId, Guid Id)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == Id && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Include(c => c.InvoiceDetails).Select().FirstOrDefault();
        }
        
        public Invoice GetAllInvoiceByIdDocType(Guid Id)
        {
            return _invoiceModuleService.Query(e => e.Id == Id && e.DocType == DocTypeConstants.Invoice && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(c => c.InvoiceNotes).Select().FirstOrDefault();
        }
        public Invoice GetAllInvoiceByDoctypeAndCompanyId(string docType, long companyId)
        {
            return _invoiceModuleService.Query(a => a.DocType == docType && a.CompanyId == companyId && a.DocumentState != InvoiceStates.Void && a.Status == RecordStatusEnum.Active && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }

        public Invoice GetAllInvovoice(string strNewDocNo, string docType, long CompanyId)
        {
            return _invoiceModuleService.Query(a => a.DocNo == strNewDocNo && a.DocType == docType && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public Invoice GetAllInvoice(Guid Id, string docType, string docNo, long companyId, string DocumentVoidState)
        {
            return _invoiceModuleService.Query(c => c.Id != Id && c.DocType == docType && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != DocumentVoidState && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public List<Invoice> GetCompanyIdAndDocType(long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public List<Invoice> GetCompanyIdByDoubtFulDbt(long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.DoubtFulDebitNote && a.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public List<Invoice> GetCompanyIdByCreditNote(long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocType == DocTypeConstants.CreditNote).Select().ToList();
        }
        public List<Invoice> GetCompanyIdAndId(long CompanyId, string DocType)
        {
            return _invoiceModuleService.Queryable().Where(c => c.CompanyId == CompanyId && c.DocType == DocType && c.Status < RecordStatusEnum.Disable).OrderBy(a => a.CreatedDate).ToList();
        }

        public Invoice GetInvoiceByIdAndComapnyId(long companyId, Guid Id)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Id == Id && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public Invoice GetCompanyById(long companyId)
        {
            return _invoiceModuleService.Query(e => e.DocType == DocTypeConstants.CreditNote && e.CompanyId == companyId && e.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        public Invoice GetAllInvoiceById(Guid Id)
        {
            return _invoiceModuleService.Query(e => e.Id == Id && e.DocType == DocTypeConstants.Invoice && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(c => c.InvoiceNotes).Include(c => c.BeanEntity).Select().FirstOrDefault();
        }
        public Invoice GetAllDebtNoteByCompanyId(long companyId)
        {
            return _invoiceModuleService.Query(e => e.DocType == DocTypeConstants.DoubtFulDebitNote && e.CompanyId == companyId && e.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public Invoice GetCreditNoteByCompanyIdAndId(long companyid, Guid id)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyid && e.Id == id && e.DocType == DocTypeConstants.CreditNote && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(a => a.InvoiceNotes).Select().FirstOrDefault();
        }
        public Invoice GetCreditNoteByCompanyId(long companyId)
        {
            return _invoiceModuleService.Query(e => e.DocType == DocTypeConstants.CreditNote && e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public Invoice GetCreditNoteById(Guid id)
        {
            return _invoiceModuleService.Query(e => e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(c => c.InvoiceNotes).Include(c => c.BeanEntity).Select().FirstOrDefault();
        }
        public Invoice GetDoubtfuldebtByCompanyIdAndId(Guid Id, long companyId)
        {
            return _invoiceModuleService.Query(a => a.Id == Id && a.DocType == DocTypeConstants.DoubtFulDebitNote && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public Invoice GetCreditNoteByDocumentId(Guid Id)
        {
            return _invoiceModuleService.Query(c => c.Id == Id && c.DocType == DocTypeConstants.Invoice && c.InternalState == "Posted" && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public List<Invoice> GetAllCreditNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date)
        {
            return _invoiceModuleService.Query(c => c.DocType == DocTypeConstants.Invoice && c.CompanyId == companyId && c.EntityId == EntityId && c.DocDate <= date && c.DocCurrency == DocCurrency && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) && c.InternalState == "Posted").Select().ToList();
        }

        public Invoice GetDoubtfulDebtByIdAndCompanyId(long companyid, Guid Id)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyid && e.DocType == DocTypeConstants.DoubtFulDebitNote && e.Id == Id && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public Invoice GetDoubtFuldbtByCompanyId(long companyId)
        {
            return _invoiceModuleService.Query(e => e.CompanyId == companyId && e.DocType == DocTypeConstants.DoubtFulDebitNote && e.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public Invoice GetDoubtfulDebtNoteById(Guid id)
        {
            return _invoiceModuleService.Query(a => a.DocType == DocTypeConstants.DoubtFulDebitNote && a.Id == id && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public async Task<IQueryable<DoubtfullDebitModelK>> GetAllDebitfulldebitK(string userName, long companyId)
        {
            try
            {
                IQueryable<BeanEntity> beanEntityRepository = await Task.Run(()=> _beanEntityRepository.Queryable());
                IQueryable<Invoice> invoiceRepository = await Task.Run(()=> _invoiceModuleService.Queryable());
                IQueryable<DoubtfullDebitModelK> invoiceModelK = from invoice in invoiceRepository
                                                                 from beanEntity in beanEntityRepository
                                                                 where (invoice.EntityId == beanEntity.Id)
                                                                 join company in await Task.Run(()=> _companyRepository.Queryable()) on invoice.ServiceCompanyId equals company.Id
                                                                 join compUser in await Task.Run(()=> _compUserRepo.Queryable()) on company.ParentId equals compUser.CompanyId
                                                                 join CUD in await Task.Run(()=> _CompanyUserDetailRepo.Queryable()) on compUser.Id equals CUD.CompanyUserId where company.Id == CUD.ServiceEntityId
                                                                 where invoice.CompanyId == companyId
                                                                 where invoice.DocType == DocTypeConstants.DoubtFulDebitNote && invoice.Status == RecordStatusEnum.Active && compUser.Username == userName
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
                                                                     BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                                     IsLocked = invoice.IsLocked,
                                                                     DocType=invoice.DocType
                                                                 };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Invoice> GetAllInvoice(long companyId, Guid EntityId, string doccurrency, long servicecompanyid, DateTime date,string Nature)
        {
            return _invoiceModuleService.Query(c => c.DocType == DocTypeConstants.Invoice && c.CompanyId == companyId && c.DocDate <= date && c.Status == RecordStatusEnum.Active && c.EntityId == EntityId && c.DocCurrency == doccurrency && c.ServiceCompanyId == servicecompanyid && (c.DocumentState == InvoiceStates.NotPaid || c.DocumentState == InvoiceStates.PartialPaid) && c.InternalState == "Posted" &&c.Nature==Nature).Select().ToList();
        }
        public Invoice DoubtFulDebtbyId(Guid id)
        {
            return _invoiceModuleService.Query(e => e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(c => c.InvoiceNotes).Include(c => c.BeanEntity).Select().FirstOrDefault();
        }
        public List<Invoice> GetAllByEntityId(Guid id, Guid invoiceId)
        {
            return _invoiceModuleService.Query(c => c.EntityId == id && c.DocumentState != InvoiceStates.Void && c.Id != invoiceId && c.DocType == "Invoice" && c.Status == RecordStatusEnum.Active).Include(c => c.InvoiceDetails).Select().ToList();
        }
        public List<Invoice> GetAllCnByEntityId(Guid id)
        {
            return _invoiceModuleService.Query(c => c.EntityId == id && c.DocumentState != InvoiceStates.Void && c.DocType == "Credit Note" && c.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public List<Invoice> GetAllInvoiceByCidAndInvno(long companyId, string docType, string invNumber)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocSubType == docType && a.InvoiceNumber == invNumber && a.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public Invoice GetInvoiceByIdAndDocumentId(Guid documentId, long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.IsWorkFlowInvoice == true).Include(a => a.InvoiceDetails).Select().FirstOrDefault();
        }

        public List<Invoice> GetInvoiceNumber(long companyId, string invNumber)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.InvoiceNumber == invNumber && a.Status == RecordStatusEnum.Active).Select().ToList();
        }
        public List<Invoice> GetDocNumber(long companyId, string docNo)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId & a.DocNo.StartsWith(docNo) && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public List<string> GetListofPotedDocNo(long companyId, string invState)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.InternalState == invState && a.Status == RecordStatusEnum.Active).Select(a => a.DocNo).ToList();
        }
        public Invoice GetInvoiceByRecurId(long companyId, Guid recurInvId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == recurInvId && a.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public List<string> GetListOfPostedDocNo(long companyId, string internalState)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.InternalState == internalState && a.Status == RecordStatusEnum.Active).Select(a => a.DocNo).ToList();
        }
        public Invoice GetRecInvoiceByIStateAndCId(string docType, string internalSate, long companyId)
        {
            return _invoiceModuleService.Query(a => a.DocType == docType && a.InternalState == internalSate && a.CompanyId == companyId && a.Status == RecordStatusEnum.Active && a.DocumentState != InvoiceStates.Void && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public List<Invoice> GetAllInvoiceByServiceCompany(long serviceCompanyId, long? companyId)
        {
            return _invoiceModuleService.Query(a => a.ServiceCompanyId == serviceCompanyId && a.CompanyId == companyId && a.IsGstSettings == true && a.InternalState == "Recurring" && a.Status == RecordStatusEnum.Active).Include(d => d.InvoiceDetails).Select().ToList();
        }

        public bool GetRecurringDocNo(long companyId, Guid id, string internalState, string docNo)
        {
            return _invoiceModuleService.Query(a => a.Id != id && a.CompanyId == companyId && a.InternalState == internalState && a.DocNo == docNo && a.Status == RecordStatusEnum.Active).Select().Any();
        }
        public bool GetRecurringDocNo_modified(long companyId, Guid id, string docNo)
        {
            return _invoiceModuleService.Query(a => a.Id != id && a.CompanyId == companyId &&  a.DocNo == docNo && a.DocumentState != InvoiceStates.Void).Select().Any();
        }
        public Invoice GetRecurringMaster(long companyId, Guid recId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == a.CompanyId && a.RecurInvId == recId && a.InternalState == "Recurring" && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }

        public List<Invoice> GetAllInvoiceByCIDandType(long companyId, string docType)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocType == docType && a.Status == RecordStatusEnum.Active).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public Invoice GetAllIvoiceByCidAndDocSubtype(string docType, long companyId, string docSubType)
        {
            return _invoiceModuleService.Query(a => a.DocType == docType && a.CompanyId == companyId && a.DocSubType == docSubType && a.InternalState == "Posted" && a.DocumentState != InvoiceStates.Void && a.Status == RecordStatusEnum.Active && (a.IsWorkFlowInvoice == false || a.IsWorkFlowInvoice == null)).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }

        public decimal? GetBalanceAmount(long companyId, Guid id)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == id && a.Status == RecordStatusEnum.Active).Select(a => a.BalanceAmount).FirstOrDefault();
        }

        public IQueryable<InvoiceStateModel> GetDeletedAuditTrail(Guid invoiceId)
        {
            return from invoice in _invoiceModuleService.Queryable()
                   where invoice.RecurInvId == invoiceId && invoice.InternalState == "Deleted" && invoice.Status == RecordStatusEnum.Disable
                   select new InvoiceStateModel()
                   {
                       Status = invoice.Status,
                       DocNumber = invoice.DocNo,
                       Amount = invoice.GrandTotal
                   };
        }
        public Invoice GetDoubtfulDebtByCompanyId(long companyId)
        {
            return _invoiceModuleService.Query(e => e.DocType == DocTypeConstants.DoubtFulDebitNote && e.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }

        public Invoice GetInvoice(Guid invoiceId, string templateType)
        {
            return _invoiceModuleService.Query(v => v.Id == invoiceId && v.DocType == templateType).Include(b => b.InvoiceDetails).Include(v => v.BeanEntity).Select().FirstOrDefault();
        }



        public List<Address> GetAddress(Guid entityId)
        {
            List<Address> lstaddress = _addressRepository.Query(v => v.AddTypeId == entityId).Include(v => v.AddressBook).Select().OrderBy(v => v.AddSectionType).ToList();
            return lstaddress;
        }

        public Bank GetBankDetails(long companyId)
        {
            Bank bank = _bankRepository.Query(b => b.CompanyId == companyId).Select().FirstOrDefault();
            return bank;
        }
        public Invoice GetInvoicebyExtenctionType(Guid id)
        {
            return _invoiceModuleService.Query(e => e.ExtensionType == "Receipt" && e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Select().FirstOrDefault();
        }

        public Invoice GetInvoiceByCIdandId(Guid invoiceId, long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.DocumentId == invoiceId && a.IsWorkFlowInvoice == true).Select().FirstOrDefault();
        }
        public List<Invoice> GetListOfWFInvoice(long companyId)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.IsWorkFlowInvoice == true).Select().ToList();
        }
        public List<string> GetInvoiceStatusByIds(List<Guid> Ids)
        {
            return _invoiceModuleService.Queryable().Where(c => Ids.Contains(c.Id)).Select(c => c.DocumentState).ToList();
        }









        //for parked invoice need to delete later As a quick fix
        public IQueryable<InvoiceModelK> GetAllParkedInvoice(string username, long companyId, string internalState)
        {
            try
            {

                //IQueryable<BeanEntity> beanEntityRepository = _invoiceModuleService.GetRepository<BeanEntity>().Queryable();
                IQueryable<BeanEntity> beanEntityRepository = _beanEntityRepository.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<Invoice> invoiceRepository = _invoiceModuleService.Queryable().Where(c => c.CompanyId == companyId);
                IQueryable<InvoiceModelK> invoiceModelK = from invoice in invoiceRepository
                                                          from beanEntity in beanEntityRepository
                                                          where (invoice.EntityId == beanEntity.Id)
                                                          join company in _companyRepository.Queryable() on invoice.ServiceCompanyId equals company.Id
                                                          join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId
                                                          join cud in _CompanyUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where company.Id == cud.ServiceEntityId
                                                          //join trms in _termsOfPaymentRepository.Queryable().Where(c => c.CompanyId == companyId)
                                                          //on invoice.CreditTermsId equals trms.Id
                                                          where
                  invoice.DocType == DocTypeConstants.Invoice && /*invoice.InternalState == internalState*/ (internalState != "Parked" ? (invoice.InternalState == internalState || invoice.InternalState == "Void") : (invoice.InternalState == internalState && invoice.DocumentState != "Deleted")) && invoice.Status == RecordStatusEnum.Active && compUser.Username == username

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
                                                              //InvoiceNumber = invoice.InvoiceNumber,
                                                              CreatedDate = invoice.CreatedDate,
                                                              DocCurrency = invoice.DocCurrency,
                                                              //BaseCurrency = invoice.ExCurrency,
                                                              Nature = invoice.Nature,
                                                              BalanceAmount = (double)(invoice.BalanceAmount),
                                                              // DocDescription = invoice.Remarks,
                                                              ExchangeRate = (invoice.ExchangeRate).ToString(),
                                                              PONo = invoice.PONo,
                                                              //CreditTermName = trms.Name,
                                                              ServiceCompanyName = company.ShortName,
                                                              //NoSupportingDocument = invoice.NoSupportingDocs,
                                                              BaseAmount = Math.Round((double)(invoice.GrandTotal * invoice.ExchangeRate), 2),
                                                              UserCreated = invoice.UserCreated,
                                                              ModifiedBy = invoice.ModifiedBy,
                                                              ModifiedDate = invoice.ModifiedDate,
                                                              // Repeating = invoice.IsRepeatingInvoice,
                                                              BaseBal = Math.Round((double)(invoice.BalanceAmount * invoice.ExchangeRate), 2),
                                                              //IsWorkFlowInvoice = invoice.IsWorkFlowInvoice,
                                                              //CursorType = invoice.CursorType,
                                                              InternalState = invoice.InternalState,
                                                              DocType = invoice.DocType,
                                                              DocSubType = invoice.DocSubType,
                                                              IsSystem = (invoice.IsWorkFlowInvoice == true || invoice.IsOBInvoice == true) ? true : false,
                                                              ScreenName = "Invoice",
                                                              IsLocked = invoice.IsLocked
                                                              //PostingDate = invoice.DocDate,
                                                              //IsOBInvoice = invoice.IsOBInvoice
                                                          };
                return invoiceModelK.OrderByDescending(a => a.CreatedDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DateTime? GetLastPostedDate(long companyId)
        {
            return _invoiceModuleService.Queryable().Where(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance).OrderByDescending(a => a.CreatedDate).Select(a => a.DocDate).FirstOrDefault();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == InvoiceStates.Void).FirstOrDefault() == true;
        }




        public Invoice GetIntercoCreditNote(long companyId, Guid creditNoteId, string docSubType)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == creditNoteId && a.DocSubType == docSubType).Select().FirstOrDefault();
        }

        //for the Lst Of invoices
        public Dictionary<Guid,long> GetListOsServiceEntityByInvoiceId(long companyId, List<Guid> invoiceIds, string docType)
        {
            return _invoiceModuleService.Query(a => a.CompanyId == companyId && invoiceIds.Contains(a.Id) && a.DocType==docType).Select(c => new { Ids = c.Id, Code = c.ServiceCompanyId.Value }).ToDictionary(Id => Id.Ids, Name => Name.Code);

        }



        public Bank GetBankDetailsByCompanyId(long serviceComanyId)
        {
            Bank bank = new Bank();
            bank = _bankRepository.Query(x => x.SubcidaryCompanyId == serviceComanyId && x.Purpose.Contains("Invoice") && x.Status == Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            if (bank == null)
            {
                bank = _bankRepository.Query(x => x.SubcidaryCompanyId == serviceComanyId && x.Status == Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            }
            return bank;
        }

        public async Task<Invoice> GetByCompanyIdForInvoiceAsync(long companyId, string docType)
        {
            return await  Task.Run(()=>_invoiceModuleService.Queryable().Where(e => e.CompanyId == companyId && e.Status == RecordStatusEnum.Active && e.DocSubType != DocTypeConstants.OpeningBalance && e.DocType == docType).OrderByDescending(a => a.CreatedDate).FirstOrDefault());
        }

        public async Task<Invoice> GetAllInvoiceLuAsync(long companyId, Guid Id)
        {
            return await Task.Run(()=> _invoiceModuleService.Query(a => a.CompanyId == companyId && a.Id == Id && a.DocType == DocTypeConstants.Invoice && a.Status == RecordStatusEnum.Active).Include(c => c.InvoiceDetails).Select().FirstOrDefault());
        }
        public async Task<Invoice> GetCompanyAndIdAsync(long companyId, Guid id)
        {
            return await Task.Run(()=> _invoiceModuleService.Query(e => e.CompanyId == companyId && e.Id == id && e.Status == RecordStatusEnum.Active).Include(a => a.InvoiceDetails).Include(a => a.InvoiceNotes).Select().FirstOrDefault());
        }
    }
}
