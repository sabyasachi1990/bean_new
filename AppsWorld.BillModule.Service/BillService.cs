using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.BillModule.Infra;

namespace AppsWorld.BillModule.Service
{
    public class BillService : Service<Bill>, IBillService
    {
        private readonly IBillModuleRepositoryAsync<Bill> _billRepository;
        private readonly IBillModuleRepositoryAsync<Company> _companyService;
        private readonly IBillModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IBillModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public BillService(IBillModuleRepositoryAsync<Bill> billRepository, IBillModuleRepositoryAsync<Company> companyService, IBillModuleRepositoryAsync<CompanyUser> compUserRepo, IBillModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(billRepository)
        {
            _billRepository = billRepository;
            _companyService = companyService;
            _compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public List<Bill> GetAllBillModel(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public Bill GetBillById(Guid id, long companyId, string docType)
        {
            return _billRepository.Query(c => c.Id == id && c.CompanyId == companyId && c.DocSubType == docType).Include(c => c.BillDetails).Select().FirstOrDefault();
        }
        public Bill CreateBill(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId && c.DocSubType != "Opening Bal").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Bill GetDocNo(string docNo, long companyId)
        {
            return _billRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Bill GetDocNoById(Guid Id, long companyId)
        {
            return _billRepository.Query(c => c.Id == Id && c.CompanyId == companyId).Include(d => d.BillDetails).Select().FirstOrDefault();
        }


        public Bill GetByAllBillDetails(Guid id, string docType)
        {
            return _billRepository.Query(c => c.Id == id && c.DocSubType == docType).Include(a => a.BillDetails)
                .Select().FirstOrDefault();
        }

        //Bill IBillService.GetByAllBillDetails(Guid id)
        //{
        //    throw new NotImplementedException();
        //}


        //public Bill GetUpdate(Guid id)
        //{
        //   return _billRepository.Update(id);
        //}

        public bool GetByDocTypeId(Guid id, string docNo, long companyId, string docType, Guid entityId)
        {
            return _billRepository.Queryable().Any(c => c.Id != id && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != BillNoteState.Void && c.EntityId == entityId);
        }
        public void BillUpdate(Bill bill)
        {
            _billRepository.Update(bill);
        }
        public void BillInsert(Bill bill)
        {
            _billRepository.Insert(bill);
        }
        public IQueryable<Bill> GetAllBills(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId).Include(c => c.BillDetails).Select().AsQueryable();
        }

        public IQueryable<BillModelK> GetAllBillsK(string username, long companyId, string type)
        {
            IQueryable<BeanEntity> beanEntityRepository = _billRepository.GetRepository<BeanEntity>().Queryable().Where(x => x.CompanyId == companyId);
            IQueryable<Bill> billRepository = _billRepository.Queryable().Where(c => c.CompanyId == companyId);
            IQueryable<BillModelK> billDetails = from b in billRepository
                                                 join e in beanEntityRepository on b.EntityId equals e.Id
                                                 join serviceComp in _companyService.Queryable() on b.ServiceCompanyId equals serviceComp.Id
                                                 join compUser in _compUserRepo.Queryable() on serviceComp.ParentId equals compUser.CompanyId
                                                 join cud in _compUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId
                                                 where serviceComp.Id == cud.ServiceEntityId
                                                 where (b.CompanyId == companyId && (type == "Bill" ? b.DocSubType != "Payroll" : b.DocSubType == "Payroll"))
                                                 && compUser.Username == username
                                                 select new BillModelK()
                                                 {
                                                     Id = b.Id,
                                                     CompanyId = b.CompanyId,
                                                     DocNo = b.DocNo,
                                                     DocDate = b.DocumentDate,
                                                     BalanceAmount = (double)(b.BalanceAmount),
                                                     Nature = b.Nature,
                                                     ExchangeRate = (b.ExchangeRate).ToString(),
                                                     DueDate = b.DueDate,
                                                     DocCurrency = b.DocCurrency,
                                                     DocumentState = b.DocumentState,
                                                     EntityName = e.Name,
                                                     GrandTotal = (double)(b.GrandTotal),
                                                     CreatedDate = b.CreatedDate,
                                                     ModifiedDate = b.ModifiedDate,
                                                     ModifiedBy = b.ModifiedBy,
                                                     UserCreated = b.UserCreated,
                                                     BaseTotal = (double)(b.BaseGrandTotal),
                                                     BaseBal = (double)(b.BaseBalanceAmount),
                                                     ServiceCompanyName = serviceComp.ShortName,
                                                     DocSubType = b.DocSubType,
                                                     IsExternal = b.IsExternal,
                                                     PostingDate = b.PostingDate,
                                                     IsLocked = b.IsLocked,
                                                     DocType = b.DocType
                                                 };
            return billDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }


        public List<Bill> GetAllPayrollBill(Guid? payrollId, long companyId)
        {
            return _billRepository.Queryable().Where(x => x.PayrollId == payrollId & x.CompanyId == companyId && x.DocumentState != "Void").ToList();
        }
        public Bill GetbillById(Guid id)
        {
            return _billRepository.Query(a => a.Id == id).Include(c => c.BillDetails).Select().FirstOrDefault();
        }
        public DateTime? GetLastBillCreatedDate(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId && c.DocSubType == "Bill").Select().OrderByDescending(a => a.CreatedDate).Select(a => a.PostingDate).FirstOrDefault();
        }
        public Bill GetBillById(long companyId, Guid billId)
        {
            return _billRepository.Query(a => a.CompanyId == companyId && a.Id == billId).Include(a => a.BillDetails).Select().FirstOrDefault();
        }

        public bool? GetClaimsBill(Guid id, long? companyId)
        {
            return _billRepository.Query(a => a.PayrollId == id && a.CompanyId == companyId && a.DocumentState != "Not Paid").Select().FirstOrDefault() != null ? true : false; ;
        }
        public decimal? GetBillBalAmount(Guid billId, long companyId)
        {
            return _billRepository.Query(c => c.Id == billId && c.CompanyId == companyId).Select(c => c.BalanceAmount).FirstOrDefault();
        }
        public List<Bill> GetListOfBill(long companyId)
        {
            return _billRepository.Query(a => a.CompanyId == companyId /*&& a.DocSubType != "Opening Bal"*/).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public List<Bill> GetListOfBills(List<Guid> lstBillIds, long companyId, string docSubType)
        {
            return _billRepository.Query(c => lstBillIds.Contains(c.Id) && c.DocSubType == docSubType && c.CompanyId == companyId).Include(a => a.BillDetails).Select().ToList();
        }


        public bool IsVoid(long companyId, Guid id)
        {
            return _billRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == BillNoteState.Void).FirstOrDefault();
        }
        public bool GetByDocSubTypeId(Guid id, string docNo, long companyId, string docType, Guid entityId, string docSubType)
        {
            return _billRepository.Queryable().Any(c => c.Id != id && c.DocNo == docNo && c.CompanyId == companyId && c.DocumentState != BillNoteState.Void && c.EntityId == entityId && c.DocSubType == docSubType);
        }
    }
}
