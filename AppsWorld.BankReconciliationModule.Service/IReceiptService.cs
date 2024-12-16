using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.Models;
using Service.Pattern;

namespace AppsWorld.BankReconciliationModule.Service
{
	public interface IReceiptService : IService<Receipt>
	{
		List<Receipt> GetAllReceiptModel(long companyId);
		Receipt GetReceipt(Guid id, long companyId);

		Receipt CreateReceipt(long companyId);
        //Receipt GetReceipts(Guid id, long companyId);
		Receipt GetDocNo(string docNo, long companyId);
		Receipt CheckDocNo(Guid id, string docNo, long companyId);
        //Receipt CheckReceiptById(Guid id);

		void UpdateReceipt(Receipt receipt);
		void InsertReceipt(Receipt receipt);

        List<Receipt> lstReceiptJV(DateTime statementDate, DateTime reconciliationDate);

        //List<ClearingDateModel> GetClearingReceipts(long companyId, long serviceCompanyId, long coaId, DateTime clearingDate,string fromDate);

       
	}
}
