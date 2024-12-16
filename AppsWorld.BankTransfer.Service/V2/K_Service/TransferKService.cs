using AppsWorld.BankTransferModule.Entities.V2;
using AppsWorld.BankTransferModule.Infra.Resources;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.BankTransferModule.RepositoryPattern.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service.V2
{
    public class TransferKService : Service<BankTransferK>, ITransferKService
    {
        readonly ITransferKRepositoryAsync<BankTransferK> _transferRepository;
        readonly ITransferKRepositoryAsync<CompanyCompact> _companyRepository;
        readonly ITransferKRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        public TransferKService(ITransferKRepositoryAsync<BankTransferK> transferRepository, ITransferKRepositoryAsync<CompanyCompact> companyRepository, ITransferKRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository) : base(transferRepository)
        {
            this._transferRepository = transferRepository;
            this._companyRepository = companyRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
        }
        public IQueryable<BankTransferModelK> GetAllBankTransferK(string username, long companyId)
        {
            //IQueryable<BankTransfer> banktransferRepository = _bankTransferRepository.Queryable().Where(c => c.CompanyId == companyId).AsQueryable();
            IQueryable<CompanyCompact> companyRepository = _companyRepository.Queryable().Where(c => c.ParentId == companyId);
            IQueryable<ChartOfAccountCompact> coaRepository = _chartOfAccountRepository.GetRepository<ChartOfAccountCompact>().Queryable().Where(d => d.CompanyId == companyId);
            return (from b in _transferRepository.Queryable().Where(c => c.CompanyId == companyId)
                    let name1 = b.BankTransferDetails.Where(c => c.Type == BankTransferConstants.Withdrawal).Select(x => x.ServiceCompanyId).FirstOrDefault()
                    let name2 = b.BankTransferDetails.Where(c => c.Type == BankTransferConstants.Deposit).Select(x => x.ServiceCompanyId).FirstOrDefault()
                    let coa1 = b.BankTransferDetails.Where(c => c.Type == BankTransferConstants.Withdrawal).Select(x => x.COAId).FirstOrDefault()
                    let coa2 = b.BankTransferDetails.Where(c => c.Type == BankTransferConstants.Deposit).Select(x => x.COAId).FirstOrDefault()
                    select new BankTransferModelK()
                    {
                        Id = b.Id,
                        CompanyId = b.CompanyId,
                        DocumentState = b.DocumentState,
                        DocNo = b.DocNo,
                        UserCreated = b.UserCreated,
                        CreatedDate = b.CreatedDate,
                        ModifiedBy = b.ModifiedBy,
                        ModeOfTransfer = b.ModeOfTransfer,
                        TransferRefNo = b.TransferRefNo,
                        WthAmt = (double) b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Withdrawal).Select(c => c.Amount).FirstOrDefault(),
                        DepAmt = (double)b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Deposit).Select(c => c.Amount).FirstOrDefault(),
                        WthCurr = b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Withdrawal).Select(c => c.Currency).FirstOrDefault(),
                        DepCurr = b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Deposit).Select(c => c.Currency).FirstOrDefault(),
                        ModifiedDate = b.ModifiedDate,
                        TransferDate = b.TransferDate,
                        WthCo = companyRepository.Where(c => c.Id == name1).Select(d => d.ShortName).FirstOrDefault(),
                        DepCo = companyRepository.Where(c => c.Id == name2).Select(d => d.ShortName).FirstOrDefault(),
                        WthCashBank = coaRepository.Where(c => c.Id == coa1).Select(c => c.Name).FirstOrDefault(),
                        DepCashBank = coaRepository.Where(c => c.Id == coa2).Select(c => c.Name).FirstOrDefault(),
                        WthBankClearing = b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Withdrawal).Select(c => c.BankClearingDate).FirstOrDefault(),
                        DepBankClearing = b.BankTransferDetails.Where(d => d.Type == BankTransferConstants.Deposit).Select(c => c.BankClearingDate).FirstOrDefault(),
                        ExchangeRate = (b.ExchangeRate).ToString()
                    }).OrderByDescending(a => a.CreatedDate).AsQueryable();

        }
    }
}
