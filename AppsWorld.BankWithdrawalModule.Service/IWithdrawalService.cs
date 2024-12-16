using Service.Pattern;
using System;
using AppsWorld.BankWithdrawalModule.Entities;
using System.Linq;
using System.Collections.Generic;
using AppsWorld.BankWithdrawalModule.Models;
using System.Threading.Tasks;


namespace AppsWorld.BankWithdrawalModule.Service
{
    public interface IWithdrawalService : IService<Withdrawal>
    {
         Task<IQueryable<BankWithdrawalModelK>> GetAllBankWithdralK(string username, long companyId, string docType);

      
        Withdrawal GetWithdrawal(Guid id, long companyId, string docType);
        Task<Withdrawal> GetWithdrawalById(Guid withdrawalId, long companyId);
        Task<Withdrawal> CreateWithdrawal(long companyId, string docType);
        Withdrawal GetDocNo(string docNo, long companyId, string docType);
        Withdrawal GetWithdrawalDocNo(Guid id, string docNo, long companyId, string docType);
        Withdrawal GetWithdraw(Guid id, long companyId, string docType);
        List<string> GetAutoNumber(long companyId, string docType);
        Withdrawal CreateWithdrawalforDocNo(long companyId, string docType);
        List<Withdrawal> GetAllPaymentModel(long companyId);

        bool IsVoid(long companyId, Guid id);

        Task<Withdrawal> GetWithdrawAsync(Guid id, long companyId, string docType);
        Task<Withdrawal> GetWithdrawalByIdAsync(Guid withdrawalId, long companyId);
    }
}
