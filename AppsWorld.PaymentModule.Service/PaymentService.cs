using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
using AppsWorld.PaymentModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.PaymentModule.Infra;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using AppsWorld.PaymentModule.Infra.Resources;
namespace AppsWorld.PaymentModule.Service
{
    public class PaymentService : Service<Payment>, IPaymentService
    {
        private readonly IPaymentModuleRepositoryAsync<Payment> _paymentRepository;
        private readonly IPaymentModuleRepositoryAsync<Company> _companyRepository;
        private readonly IPaymentModuleRepositoryAsync<ChartOfAccount> _coaRepository;
        private readonly IPaymentModuleRepositoryAsync<CompanyUser> _compUsersRepo;
        private readonly IPaymentModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUsersDetailRepo;

        public PaymentService(IPaymentModuleRepositoryAsync<Payment> paymentRepository, IPaymentModuleRepositoryAsync<Company> companyRepository, IPaymentModuleRepositoryAsync<ChartOfAccount> coaRepository, IPaymentModuleRepositoryAsync<CompanyUser> compUserRepo, IPaymentModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(paymentRepository)
        {
            _paymentRepository = paymentRepository;
            _coaRepository = coaRepository;
            _companyRepository = companyRepository;
            _compUsersRepo = compUserRepo;
            _compUsersDetailRepo = compUserDetailRepo;
        }

        public Payment GetPayment(Guid id, long companyId)
        {
            return _paymentRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(c => c.PaymentDetails).Select().FirstOrDefault();
        }
        public List<Payment> GetAllPaymentModel(long companyId)
        {
            return _paymentRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public Payment GetPayments(Guid id, long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.Id == id && c.CompanyId == companyId && (docType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") /*c.DocSubType == docType*/).Include(c => c.PaymentDetails).Select().FirstOrDefault();
        }
        public Payment CreatePayment(long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.CompanyId == companyId && /*c.DocSubType == docType*/(docType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll")).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Payment CreatePaymentDocNo(long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.CompanyId == companyId && c.DocumentState != PaymentState.Void && c.DocSubType == docType).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Payment GetDocNo(string docNo, long companyId)
        {
            return _paymentRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Payment CheckDocNo(Guid id, string docNo, long companyId, string docType)
        {
            return _paymentRepository.Query(c => c.Id != id && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != PaymentState.Void && c.DocSubType == docType).Select().FirstOrDefault();
        }
        public Payment CheckPaymentById(Guid id, string docType)
        {
            return _paymentRepository.Query(c => c.Id == id && c.DocSubType == docType).Include(c => c.PaymentDetails).Select().FirstOrDefault();
        }
        public void UpdatePayment(Payment payment)
        {
            _paymentRepository.Update(payment);
        }
        public void InsertPayment(Payment payment)
        {
            _paymentRepository.Insert(payment);
        }
        public IQueryable<PaymentModelK> GetAllPaymentK(string username, long companyId, string docType)
        {
            IQueryable<BeanEntity> beanEntityRepository = _paymentRepository.GetRepository<BeanEntity>().Queryable();
            IQueryable<Payment> paymentRepository = _paymentRepository.Queryable();
            IQueryable<PaymentModelK> paymentModelKDetails = from b in paymentRepository
                                                             join e in beanEntityRepository on b.EntityId equals e.Id
                                                             join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                                                             join coa in _coaRepository.Queryable() on b.COAId equals coa.Id
                                                             //from e in beanEntityRepository
                                                             //where (b.EntityId == e.Id)
                                                             join compUser in _compUsersRepo.Queryable() on company.ParentId equals compUser.CompanyId
                                                             join cud in _compUsersDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where company.Id == cud.ServiceEntityId
                                                             where b.CompanyId == companyId &&/* b.DocSubType == docType*/(docType == "General" ? b.DocSubType != "Payroll" : b.DocSubType == "Payroll") && compUser.Username == username
                                                             select new PaymentModelK()
                                                             {
                                                                 Id = b.Id,
                                                                 CompanyId = b.CompanyId,
                                                                 DocNo = b.DocNo,
                                                                 DocDate = b.DocDate,
                                                                 EntityName = e.Name,
                                                                 //SystemRefNo = b.SystemRefNo,
                                                                 DocumentState = b.DocumentState,
                                                                 GrandTotal = (double)(b.GrandTotal),
                                                                 ModeOfPayment = b.ModeOfPayment,
                                                                 CreatedDate = b.CreatedDate,
                                                                 ExchangeRate = (b.ExchangeRate).ToString(),
                                                                 PaymentRefNo = b.PaymentRefNo,
                                                                 BankPaymentAmmount = (double)(b.PaymentApplicationAmmount),
                                                                 BankPaymentAmmountCurrency = b.PaymentApplicationCurrency,
                                                                 // DocDescription = b.Remarks,
                                                                 ServiceCompanyName = company.ShortName,
                                                                 CashBankAccount = coa.Name,
                                                                 PaymentApplicationCurrency = b.PaymentApplicationCurrency,
                                                                 PaymentApplicationAmmount = (double)(b.PaymentApplicationAmmount),
                                                                 ModifiedBy = b.ModifiedBy,
                                                                 UserCreated = b.UserCreated,
                                                                 ModifiedDate = b.ModifiedDate,
                                                                 //NoSupportingDocument = b.NoSupportingDocs,
                                                                 BankClearingDate = b.BankClearingDate,
                                                                 IsLocked = b.IsLocked,
                                                                 DocType = b.DocType
                                                             };
            return paymentModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
        public Payment GetPaymentsById(Guid id, long companyId, string docType, Guid entityId)
        {
            return _paymentRepository.Query(c => c.Id == id && c.CompanyId == companyId && (docType == "Payroll" ? c.DocSubType == "Payroll" : c.DocSubType != "Payroll") && c.EntityId == entityId /*c.DocSubType == docType*/).Include(c => c.PaymentDetails).Select().FirstOrDefault();
        }
        public bool? CheckDocumentState(Guid id, string docType)
        {
            return _paymentRepository.Query(s => s.Id == id /*&& s.DocType == docType*/).Select((s => s.DocumentState == "Void" || s.DocumentState == "Cleared")).FirstOrDefault() == true;
        }
    }
}
