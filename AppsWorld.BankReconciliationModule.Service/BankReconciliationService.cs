using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.BankReconciliationModule.Models;
using AppsWorld.CommonModule.Service;
using AppsWorld.BankReconciliationModule.Service;
using AppsWorld.BankReconciliationModule.Infra;
using AppsWorld.CommonModule.Entities;

namespace AppsWorld.BankReconciliationModule.Service
{
    public class BankReconciliationService : Service<BankReconciliation>, IBankReconciliationService
    {
        private readonly IBankReconciliationModuleRepositoryAsync<BankReconciliation> _bankReconciliationRepository;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly AppsWorld.BankReconciliationModule.Service.IJournalService _journalService;
        private readonly IBeanEntityService _beanEntityService;

        private readonly IBankReconciliationModuleRepositoryAsync<CompanyUser> _CompUserRepo;
        private readonly IBankReconciliationModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _CompUserDetailRepo;
        //private readonly IBankReconciliationModuleRepositoryAsync<ChartOfAccount> _chartOfAccountService;
        //private readonly IBankReconciliationModuleRepositoryAsync<Company> _CompanyRepo;


        public BankReconciliationService(IBankReconciliationModuleRepositoryAsync<BankReconciliation> bankReconciliationRepository, AppsWorld.BankReconciliationModule.Service.IJournalService journalService, IBeanEntityService beanEntityService, IFinancialSettingService financialSettingService, /*IBankReconciliationModuleRepositoryAsync<AppsWorld.BankReconciliationModule.Entities.Company> CompanyRepo, */IBankReconciliationModuleRepositoryAsync<CompanyUser> compUserRepo, IBankReconciliationModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo/*, IBankReconciliationModuleRepositoryAsync<ChartOfAccount> chartOfAccountService*/)
            : base(bankReconciliationRepository)
        {
            this._bankReconciliationRepository = bankReconciliationRepository;
            this._journalService = journalService;
            this._beanEntityService = beanEntityService;
            this._financialSettingService = financialSettingService;
            //this. _CompanyRepo = CompanyRepo;
            _CompUserRepo = compUserRepo;
            //this._chartOfAccountService = chartOfAccountService;
            _CompUserDetailRepo = compUserDetailRepo;
        }

        public List<BankReconciliation> GetAllBankReconciliations(long companyId)
        {
            return _bankReconciliationRepository.Query(s => s.CompanyId == companyId).Select().ToList();
        }
        public BankReconciliation GetBankReconciliation(Guid id, long companyId)
        {
            return _bankReconciliationRepository.Query(s => s.Id == id && s.CompanyId == companyId && s.Status == Framework.RecordStatusEnum.Active).Include(c => c.BankReconciliationDetails).Select().FirstOrDefault();
        }
        public BankReconciliation GetBankReconciliationDate(long companyId)
        {
            return _bankReconciliationRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }

        public IQueryable<BankReconciliationModelk> GetAllBankReconciliationsK(string username, long companyId)
        {
            IQueryable<AppsWorld.BankReconciliationModule.Entities.Company> serviceCompanyRepository = _bankReconciliationRepository.GetRepository<AppsWorld.BankReconciliationModule.Entities.Company>().Queryable().Where(a => a.ParentId == companyId);
            IQueryable<BankReconciliation> bankReconciliationRepository = _bankReconciliationRepository.Queryable().Where(a => a.CompanyId == companyId);
            IQueryable<BankReconciliationModelk> bankReconciliationModelkDetails = from b in bankReconciliationRepository
                                                                                   join e in serviceCompanyRepository on b.ServiceCompanyId equals e.Id
                                                                                   //join coa in _chartOfAccountService.Queryable() on b.COAId equals coa.Id
                                                                                   join compUser in _CompUserRepo.Queryable() on e.ParentId equals compUser.CompanyId
                                                                                   join cud in _CompUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where e.Id == cud.ServiceEntityId
                                                                                   where b.CompanyId == companyId && compUser.Username == username
                                                                                   select new BankReconciliationModelk()
                                                                                   {
                                                                                       Id = b.Id,
                                                                                       CompanyId = b.CompanyId,
                                                                                       BankReconciliationDate = b.BankReconciliationDate,
                                                                                       SubsidiaryCompany = e.ShortName,
                                                                                       ServiceCompanyId = e.Id,
                                                                                       COAId = b.COAId,
                                                                                       BankAccount = b.BankAccount,
                                                                                       Currency = b.Currency,
                                                                                       StatementAmount = (double)(b.StatementAmount),
                                                                                       State = b.State,
                                                                                       CreatedDate = b.CreatedDate,
                                                                                       UserCreated = b.UserCreated,
                                                                                       ModifiedBy = b.ModifiedBy,
                                                                                       ModifiedDate = b.ModifiedDate,
                                                                                       StatementDate = b.StatementDate,

                                                                                       //Identity = b.State == "To re-run bank rec" ? "!" : string.Empty,
                                                                                       IsReRunBR = b.IsReRunBR,
                                                                                       RowVersion = b.Version,
                                                                                       IsLocked = b.IsLocked,
                                                                                       DocType = "Bank Reconciliation "
                                                                                   };
            return bankReconciliationModelkDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }


        public IQueryable<BankReconciliationDetailModel> GetClearingTransaction(string username, long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate)
        {
            bool isClearing = true;

            List<BankReconciliationDetailModel> lstbrd = new List<BankReconciliationDetailModel>();
            //List<BankReconciliationDetailModel> lstbrd1 = new List<BankReconciliationDetailModel>();
            //if (fromDate == null)
            //{
            //    var financials = _financialSettingService.GetFinancialSetting(companyId);
            //    if (financials == null)
            //        throw new Exception(BankReconciliationValidation.No_Active_Finance_Setting_found);
            //    string s = financials != null ? (financials.FinancialYearEnd) : null;
            //    string[] a = s.Split('-').ToArray();
            //    int year = toDate.Year;
            //    string str = a[0] + a[1] + year.ToString();
            //    DateTime fromDat = DateTime.Parse(str);
            //    fromDate = fromDat.AddDays(-1);
            //    if (fromDate >= DateTime.UtcNow)
            //        fromDate = fromDat.AddYears(-1);
            //}
            bool? isWithdrawal;

            //check last reconciled date

            DateTime? lastReconciledDate = GetLastReocnciledDate(companyId, chartid, subcompanyId, Guid.Empty, DateTime.Now, false);


            var lstbankR = _journalService.GetAllJournalDetails(companyId, subcompanyId, chartid, fromDate, toDate, isClearing, false, lastReconciledDate);
            lstbankR = (from jd in lstbankR
                        join j in _journalService.Queryable() on jd.JournalId equals j.Id
                        where (j.DocumentState != BRConstants.Void && j.DocumentState != BRConstants.Reset && j.DocumentState != BRConstants.Parked && j.DocumentState != BRConstants.Recurring)
                        select jd).ToList();
            //var lstbankr1 = lstbankR.Where(c => c.EntityId == Guid.Empty || c.EntityId == null).ToList();
            if (lstbankR.Any())
            {
                //IQueryable<AppsWorld.CommonModule.Entities.BeanEntity> beanEntityRepository = _bankReconciliationRepository.GetRepository<AppsWorld.CommonModule.Entities.BeanEntity>().Queryable();
                //List<AppsWorld.CommonModule.Entities.BeanEntity> lstEntity = _beanEntityService.GetListOfEntity(companyId, lstbankR.Select(c=>c.EntityId).ToList());
                var lstEntity = _beanEntityService.GetListOfEntity(companyId, lstbankR.Select(c => c.EntityId).ToList());
                lstbrd = (from br in lstbankR
                              //join e in lstEntity on br.EntityId equals e.Id into entity
                              //from ent in entity.DefaultIfEmpty()
                          select new BankReconciliationDetailModel
                          {
                              Id = br.Id,
                              JournalId = br.Id,
                              CompanyId = companyId,
                              DocumentDate = br.DocDate,
                              DocumentType = br.DocType,
                              BankReconciliationId = br.ReconciliationId,
                              ReconciliationDate = br.ReconciliationDate,
                              LastReconciliationDate = lastReconciledDate,
                              DocRefNo = br.DocNo,
                              DocSubType = "Bank Recon",
                              COAId = chartid,
                              ServiceEntityId = subcompanyId,
                              //RefNo = br.SystemRefNo,
                              EntityId = lstEntity.Where(c => c.Key == br.EntityId).Select(c => c.Key).FirstOrDefault(),
                              EntityName = lstEntity.Where(c => c.Key == br.EntityId).Select(c => c.Value).FirstOrDefault(),
                              //Withdrawal = br.Type == "Withdrawal",
                              Withdrawal = isWithdrawal = br.DocCredit >= 0,
                              //old Data
                              //Ammount = br.Type == BankReconciliationValidation.Withdrawal ? -((br.DocType == BankReconciliationValidation.payments ||
                              //          br.DocType == BankReconciliationValidation.CashPayments ||
                              //         br.DocType == BankReconciliationValidation.Journal || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashSale || br.DocType == BankReconciliationValidation.receipt ||
                              //         (br.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br.BaseCredit != 0 && br.BaseCredit != null) ? br.BaseCredit : br.BaseDebit) : br.BaseDebit) : (br.DocType == BankReconciliationValidation.payments ||
                              //          br.DocType == BankReconciliationValidation.CashPayments ||
                              //         br.DocType == BankReconciliationValidation.Journal || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashSale || br.DocType == BankReconciliationValidation.receipt ||
                              //         (br.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br.BaseCredit != 0 && br.BaseCredit != null) ? br.BaseCredit : br.BaseDebit) : br.BaseDebit,


                              //old code commented on 08/05/2019

                              //Ammount = br.Type == BankReconciliationValidation.Withdrawal ? -((br.DocType == BankReconciliationValidation.payments ||
                              //          br.DocType == BankReconciliationValidation.CashPayments ||
                              //         br.DocType == BankReconciliationValidation.Journal || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashSale || br.DocType == BankReconciliationValidation.receipt ||
                              //         (br.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br.DocCredit != 0 && br.DocCredit != null) ? br.DocCredit : br.DocDebit) : br.DocDebit) : (br.DocType == BankReconciliationValidation.payments ||
                              //          br.DocType == BankReconciliationValidation.CashPayments ||
                              //         br.DocType == BankReconciliationValidation.Journal || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashSale || br.DocType == BankReconciliationValidation.receipt ||
                              //         (br.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br.DocCredit != 0 && br.DocCredit != null) ? br.DocCredit : br.DocDebit) : br.DocDebit,

                              Ammount = isWithdrawal == true ? -(br.DocDebit != null ? br.DocDebit : br.DocCredit) : (br.DocDebit != null ? br.DocDebit : br.DocCredit),


                              ClearingDate = br.ClearingDate,
                              ClearingStatus = (br.ClearingDate != null && br.IsBankReconcile == true) ? "Reconciled" : br.ClearingDate != null ? "Cleared" : String.Empty,
                              DocumentId = br.DocumentId,
                              IsReconcile = br.IsBankReconcile,
                              IsChecked = br.ClearingDate != null,
                              JournalDetailId = br.Id
                          }).ToList();
                //    lstbrd1 = (from br1 in lstbankr1
                //               select new BankReconciliationDetailModel
                //               {
                //                   Id = br1.Id,
                //                   JournalId = br1.Id,
                //                   DocumentDate = br1.DocDate,
                //                   CompanyId = companyId,
                //                   DocumentType = br1.DocType,
                //                   DocRefNo = br1.DocNo,
                //                   BankReconciliationId = br1.ReconciliationId,
                //                   ReconciliationDate = br1.ReconciliationDate,
                //                   Withdrawal = br1.Type == "Withdrawal" ? true : false,
                //                   DocSubType = "Bank Recon",
                //                   COAId = chartid,
                //                   ServiceEntityId = subcompanyId,
                //                   //RefNo = br1.SystemRefNo,
                //                   //EntityId = e.Id,
                //                   //EntityName = e.Name,
                //                   Ammount = (br1.DocType == BankReconciliationValidation.payments ||
                //                           br1.DocType == BankReconciliationValidation.CashPayments ||
                //                           br1.DocType == BankReconciliationValidation.Payroll_Payment ||
                //                           br1.DocType == "Journal" || br1.DocType == "Withdrawal" ||
                //                           (br1.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br1.DocCredit != 0 && br1.DocCredit != null) ? br1.DocCredit : br1.DocDebit) : br1.DocDebit,
                //                   ClearingDate = br1.ClearingDate,
                //                   ClearingStatus = (br1.ClearingDate != null && br1.IsBankReconcile == true) ? "Reconciled" : br1.ClearingDate != null ? "Cleared" : String.Empty,
                //                   DocumentId = br1.DocumentId,
                //                   IsReconcile = br1.IsBankReconcile
                //               }).ToList();
            }
            //if (lstbrd1.Any())
            //    lstbrd.AddRange(lstbrd1);
            return lstbrd.OrderBy(a => a.DocumentDate).Where(x => x.Ammount != 0).AsQueryable();
        }
        public BankReconciliation GetBankRDetailsBychartid(Guid id, long companyId, long subcompanyId, long chartid, bool isclearing)
        {
            if (isclearing)
                return _bankReconciliationRepository.Query(a => a.Id == id && a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid).Include(c => c.BankReconciliationDetails).Select().LastOrDefault();
            else
                return _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == chartid).Include(c => c.BankReconciliationDetails).Select().LastOrDefault();

        }
        public BankReconciliation GetBankReconciliationBySid(Guid id, long companyId, long subcompanyId, long coaId, DateTime statementDate)
        {
            statementDate = statementDate.Date;
            return _bankReconciliationRepository.Query(s => s.Id != id && s.CompanyId == companyId && s.ServiceCompanyId == subcompanyId && s.COAId == coaId && s.StatementDate == statementDate && s.State != "Void").Select().FirstOrDefault();
        }
        public BankReconciliation GetAllBankReconciliations(Guid id, long subcompanyId)
        {
            return _bankReconciliationRepository.Query(s => s.Id == id && s.ServiceCompanyId == subcompanyId).Include(a => a.BankReconciliationDetails).Select().FirstOrDefault();
        }

        public BankReconciliation GetByDate(long companyId, long serviceCompanyId, long Coaid, DateTime bankReconciliationDate)
        {
            var bankRs = _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == Coaid);
            return bankRs.Select().Where(z => z.BankReconciliationDate.Date == bankReconciliationDate.Date).FirstOrDefault();
        }

        public List<BankReconciliation> GetAllBankById(Guid id)
        {
            return _bankReconciliationRepository.Query(s => s.Id == id).Select().ToList();
        }

        public BankReconciliation GetList(Guid guid, long companyId, long subcompanyId, long COAId)
        {

            var br = _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == COAId).Include(c => c.BankReconciliationDetails).Select().LastOrDefault();
            if (br != null)
            {
                var lst =
                    _bankReconciliationRepository.Query(
                            a => a.CompanyId == companyId && a.ServiceCompanyId == subcompanyId && a.COAId == COAId)
                        .Include(c => c.BankReconciliationDetails)
                        .Select()
                        .ToList();
                if (lst.Any())
                {
                    lst = lst.Where(c => c.Id != br.Id).ToList().OrderByDescending(c => c.StatementDate).ToList();
                }
                br = lst.LastOrDefault();
            }
            return br;
        }
        public List<BankReconciliation> GetAllBrs(long subCompantId, long coaId, DateTime? createdDate, string currency)
        {
            return _bankReconciliationRepository.Query(x => x.ServiceCompanyId == subCompantId && x.COAId == coaId && x.BankReconciliationDate > createdDate && x.Currency == currency).Select().ToList();
        }
        public DateTime? GetLastReocnciledDate(long companyId, long coaId, long serviceCompanyId, Guid recId, DateTime reconciledDate, bool? isEditMode)
        {
            BankReconciliation reconciliation = null;
            if (isEditMode == true)
                reconciliation = _bankReconciliationRepository.Query(a => a.Id != recId && a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.State == "Reconciled" && a.StatementDate < reconciledDate).Select().OrderByDescending(a => a.StatementDate).FirstOrDefault();
            else
                reconciliation = _bankReconciliationRepository.Query(a => a.Id != recId && a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.State == "Reconciled").Select().OrderByDescending(a => a.StatementDate).FirstOrDefault();
            return reconciliation != null ? (DateTime?)reconciliation.BankReconciliationDate : null;
        }

        public BankReconciliation GetBankReconciliationByIdandSIdAndCid(long companyId, long serviceCompanyId, long coaID, DateTime? reconciledDate, Guid bankRecId)
        {
            return _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaID && a.BankReconciliationDate == reconciledDate && a.Id == bankRecId).Include(a => a.BankReconciliationDetails).Select().FirstOrDefault();
        }

        public BankReconciliation GetListofbankRecDetail(Guid recId, long companyId, long coaId, long srevicecompanyId, DateTime? bankRecDate, DateTime? lastRecDate)
        {
            return _bankReconciliationRepository.Query(a => a.CompanyId == companyId && a.Id == recId && a.ServiceCompanyId == srevicecompanyId && a.BankReconciliationDate == bankRecDate).Include(a => a.BankReconciliationDetails).Select().FirstOrDefault();
        }
        public List<BankReconciliation> GetListOfClearingByCoaIdandScId(long serviceCompnayId, long coaId)
        {
            return _bankReconciliationRepository.Query(a => a.ServiceCompanyId == serviceCompnayId && a.COAId == coaId /*&& a.State != BRConstants.Draft*/).Include(a => a.BankReconciliationDetails).Select().ToList();
        }
        public BankReconciliation GetBRByCOAID(long? coaId, long? serviceCompanyId, long? companyId)
        {
            return _bankReconciliationRepository.Queryable().Where(c => c.COAId == coaId && c.ServiceCompanyId == serviceCompanyId && c.CompanyId == companyId && c.State != "Void" && c.IsReRunBR == true).OrderByDescending(x => x.BankReconciliationDate).FirstOrDefault();
        }

        public bool? IsBrcToBeReRun(long companyId, long serviceCompanyId, long coaId, DateTime? brcDate)
        {
            return _bankReconciliationRepository.Queryable().Where(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.BankReconciliationDate >= brcDate && a.State == "Reconciled").Any();
        }

        public List<BankReconciliation> GetListOfBankReconciliation(long companyId, long serviceCompanyId, long coaId)
        {
            return _bankReconciliationRepository.Queryable().Where(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.State == "Reconciled" && a.IsReRunBR != true).ToList();
        }
    }
}
