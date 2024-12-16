using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.Service;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using AppsWorld.BankReconciliationModule.Models;

namespace AppsWorld.BankReconciliationModule.Service
{
	public class ReceiptService : Service<Receipt>, IReceiptService
	{
		private readonly IBankReconciliationModuleRepositoryAsync<Receipt> _receiptRepository;

		public ReceiptService(IBankReconciliationModuleRepositoryAsync<Receipt> receiptRepository)
			: base(receiptRepository)
		{
			_receiptRepository = receiptRepository;
		}

		public List<Receipt> GetAllReceiptModel(long companyId)
		{
			return _receiptRepository.Query(c => c.CompanyId == companyId).Select().ToList();
		}

		public Receipt GetReceipt(Guid id, long companyId)
		{
			return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
		}

		////public Receipt GetReceipts(Guid id, long companyId)
		////{
		////    return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(c => c.ReceiptDetails).Include(c => c.ReceiptGSTDetails).Include(c => c.ReceiptBalancingItems).Select().FirstOrDefault();
		////}

		public Receipt CreateReceipt(long companyId)
		{
			return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
		}
		public Receipt GetDocNo(string docNo, long companyId)
		{
			return _receiptRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
		}
		public Receipt CheckDocNo(Guid id, string docNo, long companyId)
		{
			return _receiptRepository.Query(c => c.Id != id && c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
		}

		//public Receipt CheckReceiptById(Guid id)
		//{
		//    return
		//        _receiptRepository.Query(c => c.Id == id)
		//            .Include(c => c.ReceiptDetails)
		//            .Include(c => c.ReceiptBalancingItems)
		//            .Include(c => c.ReceiptGSTDetails)
		//            .Select()
		//            .FirstOrDefault();
		//}

		public void UpdateReceipt(Receipt receipt)
		{
			_receiptRepository.Update(receipt);
		}
		public void InsertReceipt(Receipt receipt)
		{
			_receiptRepository.Insert(receipt);
		}

		public List<Receipt> lstReceiptJV(DateTime statementDate, DateTime reconciliationDate)
		{
			return _receiptRepository.Query(c => c.DocDate <= statementDate && c.BankClearingDate == null && c.ModeOfReceipt == "Cheque").Select().ToList();
		}

        //public List<ClearingDateModel> GetClearingReceipts(long companyId, long serviceCompanyId, long coaId, DateTime clearingDate, string fromDate)
        // {

			 
        //     IQueryable<BeanEntity> beanEntityRepository = _receiptRepository.GetRepository<BeanEntity>().Queryable();
        //     IQueryable<Receipt> receiptRepository = _receiptRepository.Queryable();
        //     if (fromDate == "null")
        //     {
        //         IQueryable<ClearingDateModel> receipts = from b in receiptRepository
        //                                                  from e in beanEntityRepository
        //                                                  where (b.EntityId == e.Id)
        //                                                  where (b.CompanyId == companyId)
        //                                                  where (b.ServiceCompanyId == serviceCompanyId)
        //                                                  where (b.COAId == coaId)
        //                                                  where (b.DocDate <= clearingDate)
        //                                                  select new ClearingDateModel()
        //                                                  {
        //                                                      DocNo = b.DocNo,
        //                                                      DocumentId = b.Id,
        //                                                      CompanyId = companyId,
        //                                                      DocumentType = "Receipt",
        //                                                      EntityName = e.Name,
        //                                                      DocRefNo = b.SystemRefNo,
        //                                                      Amount = b.GrandTotal,
        //                                                      BankClearingDate = b.BankClearingDate,
        //                                                      ClearingStatus = b.BankClearingDate != null ? "Cleared" : null,

        //                                                  };
        //         return receipts.ToList();

        //     }
        //    else
        //     {
        //         DateTime date1 = Convert.ToDateTime(fromDate);

        //         IQueryable<ClearingDateModel> receipts = from b in receiptRepository
        //                                                  from e in beanEntityRepository
        //                                                  where (b.EntityId == e.Id)
        //                                                  where (b.CompanyId == companyId)
        //                                                  where (b.ServiceCompanyId == serviceCompanyId)
        //                                                  where (b.COAId == coaId)
        //                                                  where (b.DocDate >= date1 &&
        //                                                   b.DocDate <= clearingDate)
        //                                                  select new ClearingDateModel()
        //                                                  {
        //                                                      DocNo = b.DocNo,
        //                                                      DocumentId = b.Id,
        //                                                      CompanyId = companyId,
        //                                                      DocumentType = "Receipt",
        //                                                      EntityName = e.Name,
        //                                                      DocRefNo = b.SystemRefNo,
        //                                                      Amount = b.GrandTotal,
        //                                                      BankClearingDate = b.BankClearingDate,
        //                                                      ClearingStatus = b.BankClearingDate != null ? "Cleared" : null,
        //                                                  };
        //         return receipts.ToList();
        //     }
        // }

	}
}
