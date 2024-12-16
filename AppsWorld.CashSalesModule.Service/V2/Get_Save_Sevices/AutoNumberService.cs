using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Service.Pattern;
using AppsWorld.CashSalesModule.Entities.V2;
namespace AppsWorld.CashSalesModule.Service.V2
{
    public class AutoNumberService : Service<AutoNumberCompact>, IAutoNumberService
    {
        private readonly ICashSalesMasterRepositoryAsync<AutoNumberCompact> _autoNumberepository;
        private readonly ICashSalesMasterRepositoryAsync<AutoNumberComptCompany> _autoNumbeCompanyRepository;

        public AutoNumberService(ICashSalesMasterRepositoryAsync<AutoNumberCompact> autoNumberRepository, ICashSalesMasterRepositoryAsync<AutoNumberComptCompany> autoNumbeCompanyRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
            this._autoNumbeCompanyRepository = autoNumbeCompanyRepository;
        }

        public AutoNumberCompact GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoNumberFlag(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(x => x.IsEditable).FirstOrDefault();
        }
        public List<AutoNumberComptCompany> GetAutoNumberCompany(Guid AutoNumberId)
        {
            return _autoNumbeCompanyRepository.Query(a => a.AutonumberId == AutoNumberId).Select().ToList();
        }
    }
}
