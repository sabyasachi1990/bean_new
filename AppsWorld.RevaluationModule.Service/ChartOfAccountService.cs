using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class ChartOfAccountService : Service<ChartOfAccount>, IChartOfAccountService
    {
        private readonly IRevaluationModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        public ChartOfAccountService(IRevaluationModuleRepositoryAsync<ChartOfAccount> journalRepository)
            : base(journalRepository)
        {
            _chartOfAccountRepository = journalRepository;
        }
        public List<ChartOfAccount> GetChartOfAccount(long id, long comapanyId)
        {
            return _chartOfAccountRepository.Query(x => x.Id == id && x.CompanyId == comapanyId).Select().ToList();
        }

        public bool? GetChartOfAccountRevaluation(long id, long comapanyId)
        {
            return
                _chartOfAccountRepository.Query(x => x.Id == id && x.CompanyId == comapanyId)
                    .Select(a => a.Revaluation).FirstOrDefault();
        }

        public ChartOfAccount GetChartOfAccountName(long id, long comapanyId)
        {
            return
                _chartOfAccountRepository.Query(x => x.Id == id && x.CompanyId == comapanyId)
                    .Select().FirstOrDefault();
        }
        public long GetChartOfAccountId(long CompanyId, string Name)
        {
            return _chartOfAccountRepository.Query(x => x.CompanyId == CompanyId && x.Name == Name)
                     .Select(c => c.Id).FirstOrDefault();
        }
        //public List<ChartOfAccount> GetAllChartOfAccountByCid(long companyId, List<long> coaId)
        //{
        //    return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && coaId.Contains(a.Id)).Select().ToList();
        //}
        public List<ChartOfAccount> GetAllChartOfAccountByCid(long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
        public List<ChartOfAccount> GetAllChartOfAccountByCIdAndId(List<long>coaIds,long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId&&coaIds.Contains(a.Id)).Select().ToList();
        }
    }
}
