using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class BillService : Service<Bill>, IBillService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Bill> _billRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<OpeningBalance> _openingBalanceRepository;

        public BillService(IJournalVoucherModuleRepositoryAsync<Bill> billRepository, IJournalVoucherModuleRepositoryAsync<OpeningBalance> openingBalanceRepository) : base(billRepository)
        {
            _billRepository = billRepository;
            _openingBalanceRepository = openingBalanceRepository;
        }
        public List<Bill> GetAllBillModel(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId).Select().ToList();
        }
        public Bill GetBillById(Guid id, long companyId)
        {
            return _billRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Bill CreateBill(long companyId)
        {
            return _billRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Bill GetDocNo(string docNo, long companyId)
        {
            return _billRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Bill GetDocNoById(Guid Id, long companyId)
        {
            return _billRepository.Query(c => c.Id == Id && c.CompanyId == companyId).Select().FirstOrDefault();
        }


        public Bill GetByAllBillDetails(Guid id)
        {
            return _billRepository.Query(c => c.Id == id).Include(a => a.BillDetails).Include(a => a)
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

        public Bill GetByDocTypeId(Guid id, string DocType, long companyId)
        {
            return _billRepository.Queryable().Where(c => c.Id != id && c.DocNo == DocType && c.CompanyId == companyId).FirstOrDefault();

        }
        public void BillUpdate(Bill bill)
        {
            _billRepository.Update(bill);
        }
        public void BillInsert(Bill bill)
        {
            _billRepository.Insert(bill);
        }
        public Bill GetAllBillBuId(Guid Id)
        {
            return _billRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }
        public OpeningBalance GetOBByServiceCompanyId(long companyId, long? serviceCompanyId, string docType)
        {
            return _openingBalanceRepository.Query(c => c.CompanyId == companyId && c.ServiceCompanyId == serviceCompanyId && c.DocType == "Opening Bal").Select().FirstOrDefault();
        }
    }
}
