using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using Service.Pattern;
using AppsWorld.BankReconciliationModule.Models;

namespace AppsWorld.BankReconciliationModule.Service
{
    public interface IBankReconciliationService : IService<BankReconciliation>
    {
        List<BankReconciliation> GetAllBankReconciliations(long companyId);
        BankReconciliation GetBankReconciliation(Guid id, long companyId);
        BankReconciliation GetBankReconciliationDate(long companyId);
        IQueryable<BankReconciliationModelk> GetAllBankReconciliationsK(string username, long companyId);

        BankReconciliation GetBankRDetailsBychartid(Guid id, long companyId, long subcompanyId, long chartid, bool isclearing);

        BankReconciliation GetBankReconciliationBySid(Guid id, long companyId, long subcompanyId, long coaId, DateTime statementDate);

        BankReconciliation GetAllBankReconciliations(Guid id, long subcompanyId);
        IQueryable<BankReconciliationDetailModel> GetClearingTransaction(string username, long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate);
        BankReconciliation GetByDate(long companyId, long serviceCompanyId, long cOAId, DateTime bankReconciliationDate);
        List<BankReconciliation> GetAllBankById(Guid id);

        BankReconciliation GetList(Guid guid, long companyId, long subcompanyId, long COAId);
        List<BankReconciliation> GetAllBrs(long subCompantId, long coaId, DateTime? createdDate, string currency);

        DateTime? GetLastReocnciledDate(long companyId, long coaId, long serviceCompanyId, Guid recId, DateTime reconciledDate, bool? isEditMode);//modified by Lokanath for Last Reconsiled Date
        BankReconciliation GetBankReconciliationByIdandSIdAndCid(long companyId, long serviceCompanyId, long coaID, DateTime? reconciledDate, Guid bankRecId);

        BankReconciliation GetListofbankRecDetail(Guid recId, long companyId, long coaId, long srevicecompanyId, DateTime? bankRecDate, DateTime? lastRecDate);

        //for List of Bank Reconciliation and there Details
        List<BankReconciliation> GetListOfClearingByCoaIdandScId(long serviceCompnayId, long coaId);
        BankReconciliation GetBRByCOAID(long? coaId, long? serviceCompanyId, long? companyId);

        bool? IsBrcToBeReRun(long companyId, long serviceCompanyId, long coaId, DateTime? brcDate);

        List<BankReconciliation> GetListOfBankReconciliation(long companyId, long serviceCompanyId, long coaId);

    }
}
