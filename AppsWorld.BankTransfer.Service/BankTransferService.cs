using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.RepositoryPattern;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CashSalesModule.Infra;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Entities.Models;
using System.Data.Entity;


namespace AppsWorld.BankTransferModule.Service
{
    public class BankTransferService : Service<BankTransfer>, IBankTransferService
    {
        private readonly IBankTransferModuleRepositoryAsync<BankTransfer> _bankTransferRepository;
        private readonly IBankTransferModuleRepositoryAsync<BankTransferDetail> _bankTransferDetailRepository;
        private readonly IBankTransferModuleRepositoryAsync<Company> _companyRepository;
        private readonly IBankTransferModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly IBankTransferModuleRepositoryAsync<CompanyUserDetail> _companyUserDetailRepository;
        public BankTransferService(IBankTransferModuleRepositoryAsync<BankTransfer> bankTransferRepository, IBankTransferModuleRepositoryAsync<Company> companyRepository, IBankTransferModuleRepositoryAsync<CompanyUser> companyUserRepository, IBankTransferModuleRepositoryAsync<CompanyUserDetail> companyUserDetailRepository, IBankTransferModuleRepositoryAsync<BankTransferDetail> bankTransferDetailRepository)
            : base(bankTransferRepository)
        {
            _bankTransferRepository = bankTransferRepository;
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
            _bankTransferDetailRepository = bankTransferDetailRepository;
        }

        public async Task<IQueryable<BankTransferModelK>> GetAllBankTransferKAsync(string username, long requestCompanyId)
        {
            try
            {
                var bankTransfers = await _bankTransferRepository.Queryable()
                    .Include(bt => bt.BankTransferDetails)
                    .Where(c => c.CompanyId == requestCompanyId)
                    .ToListAsync();

                if (!bankTransfers.Any())
                {
                    return Enumerable.Empty<BankTransferModelK>().AsQueryable();
                }

                var authorizedTransferIds = await (from btd in _bankTransferDetailRepository.Queryable()
                                                   join comp in _companyRepository.Queryable() on btd.ServiceCompanyId equals comp.Id
                                                   join cu in _companyUserRepository.Queryable() on comp.ParentId equals cu.CompanyId
                                                   join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                                                   where comp.Id == cud.ServiceEntityId
                                                         && cu.CompanyId == requestCompanyId
                                                         && cu.Username == username
                                                   select btd.BankTransferId)
                                                 .Distinct()
                                                 .ToListAsync();

                var serviceCompanyIds = bankTransfers
                    .Where(bt => authorizedTransferIds.Contains(bt.Id))
                    .SelectMany(bt => bt.BankTransferDetails)
                    .Select(btd => btd.ServiceCompanyId)
                    .Distinct()
                    .ToList();

                var companies = await _companyRepository.Queryable()
                    .Where(c => serviceCompanyIds.Contains(c.Id))
                    .Select(c => new { c.Id, c.ShortName })
                    .ToListAsync();

                var companiesLookup = companies.ToDictionary(c => c.Id, c => c.ShortName);

                var chartOfAccounts = await _bankTransferRepository.GetRepository<ChartOfAccount>().Queryable()
                    .Where(d => d.CompanyId == requestCompanyId)
                    .Select(c => new { c.Id, c.Name })
                    .ToListAsync();

                var coaLookup = chartOfAccounts.ToDictionary(c => c.Id, c => c.Name);

                var result = bankTransfers
                    .Where(b => authorizedTransferIds.Contains(b.Id))
                    .Select(b =>
                    {
                        var withdrawalDetail = b.BankTransferDetails?.FirstOrDefault(d => d.Type == "Withdrawal");
                        var depositDetail = b.BankTransferDetails?.FirstOrDefault(d => d.Type == "Deposit");

                        string GetCompanyName(long? companyId) =>
                            companyId.HasValue && companiesLookup.ContainsKey(companyId.Value)
                                ? companiesLookup[companyId.Value]
                                : null;

                        string GetCoaName(long? coaId) =>
                            coaId.HasValue && coaLookup.ContainsKey(coaId.Value)
                                ? coaLookup[coaId.Value]
                                : null;

                        return new BankTransferModelK
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
                            WthAmt = withdrawalDetail?.Amount != null ? (double?)Convert.ToDouble(withdrawalDetail.Amount) : 0,
                            DepAmt = depositDetail?.Amount != null ? (double?)Convert.ToDouble(depositDetail.Amount) : 0,
                            WthCurr = withdrawalDetail?.Currency,
                            DepCurr = depositDetail?.Currency,
                            ModifiedDate = b.ModifiedDate,
                            TransferDate = b.TransferDate,
                            WthCo = GetCompanyName(withdrawalDetail?.ServiceCompanyId),
                            DepCo = GetCompanyName(depositDetail?.ServiceCompanyId),
                            WthCashBank = GetCoaName(withdrawalDetail?.COAId),
                            DepCashBank = GetCoaName(depositDetail?.COAId),
                            WthBankClearing = withdrawalDetail?.BankClearingDate,
                            DepBankClearing = depositDetail?.BankClearingDate,
                            ExchangeRate = b.ExchangeRate.ToString(),
                            IsLocked = b.IsLocked,
                            DocType = b.DocType
                        };
                    })
                    .OrderByDescending(a => a.CreatedDate)
                    .AsQueryable();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving bank transfer data. Please see logs for details.", ex);
            }
        }

        public BankTransfer GetBankTransferById(Guid id, long companyId)
        {
            return _bankTransferRepository.Query(c => c.Id == id && c.CompanyId == companyId).Include(a => a.BankTransferDetails).Include(c => c.SettlementDetails).Select().FirstOrDefault();
        }
        public BankTransfer GetCompanyIdById(long companyId)
        {
            return _bankTransferRepository.Query(a => a.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public BankTransfer GetDocTypeAndCompanyid(string DocType, long companyId)
        {
            return _bankTransferRepository.Query(a => a.DocType == DocType && a.CompanyId == companyId && a.DocumentState != BanktransferStatus.Void).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }
        public BankTransfer DuplicateBankTransfer(string DocNo, string docType, long companyId)
        {
            return _bankTransferRepository.Query(a => a.DocNo == DocNo && a.DocType == docType && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public BankTransfer GetBankTransferLU(Guid banktransferId, long companyId)
        {
            return _bankTransferRepository.Query(c => c.Id == banktransferId && c.CompanyId == companyId).Include(a => a.BankTransferDetails).Select().FirstOrDefault();
        }
        public BankTransfer GetBankTransferDocNo(Guid id, string docNo, long companyId)
        {
            return _bankTransferRepository.Query(x => x.Id != id && x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState != CashSaleStatus.Void).Select().FirstOrDefault();
        }
        public List<string> GetAutoNumber(long companyId)
        {
            return _bankTransferRepository.Queryable().Where(x => x.CompanyId == companyId).Select(x => x.SystemRefNo).ToList();
        }
        public List<BankTransfer> GetAllBankTransfer(long companyId)
        {
            return _bankTransferRepository.Queryable().Where(c => c.CompanyId == companyId).AsEnumerable().OrderByDescending(c => c.CreatedDate).ToList();
        }

        public bool IsVoid(long companyId, Guid id)
        {
            return _bankTransferRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == InvoiceStates.Void).FirstOrDefault() == true;
        }
        public List<long> lstServiceCompanyIds(long companyId, string username)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where cu.CompanyId == companyId && cu.Username == username
                    select c.Id).ToList();
        }

    }
}
