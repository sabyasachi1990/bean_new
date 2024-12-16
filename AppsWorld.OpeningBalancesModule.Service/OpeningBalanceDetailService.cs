using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public class OpeningBalanceDetailService : Service<OpeningBalanceDetail>, IOpeningBalanceDetailService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetail> _OpeningBalancesModuleRepository;

        public OpeningBalanceDetailService(IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetail> OpeningBalancesModuleRepository)
             : base(OpeningBalancesModuleRepository)
        {
            _OpeningBalancesModuleRepository = OpeningBalancesModuleRepository;
        }
        public List<OpeningBalanceDetail> GetServiceCompanyOpeningBalance(Guid openiningBalanceId)
        {
            return _OpeningBalancesModuleRepository.Query(c => c.OpeningBalanceId == openiningBalanceId).Include(c => c.OpeningBalanceDetailLineItems).Select().OrderBy(c => c.Recorder).ToList();
        }

        public OpeningBalanceDetail CheckOpeningBalanceId(Guid id)
        {
            return _OpeningBalancesModuleRepository.Queryable().FirstOrDefault(c => c.Id == id);
        }

        public List<OpeningBalanceDetail> openingBalanceDetailByCOAID(long COAId, Guid OpeningBalanceId)
        {
            return _OpeningBalancesModuleRepository.Query(c => c.COAId == COAId && c.OpeningBalanceId == OpeningBalanceId).Include(c => c.OpeningBalanceDetailLineItems).Select().ToList();
        }

        public OpeningBalanceDetail openingBalanceDetail(long COAId)
        {
            return _OpeningBalancesModuleRepository.Query(c => c.COAId == COAId).Select().FirstOrDefault();
        }

        //public Dictionary<long, bool?> GetOBlineItemIsEditableFlagValue(Guid openingBlanceId, List<long> coaIds)
        //{
        //    return _OpeningBalancesModuleRepository.Query(a=>a.OpeningBalanceId== openingBlanceId&&coaIds.Contains(a.COAId)).Include(a=>a.OpeningBalanceDetailLineItems).Select(a=>a.OpeningBalanceDetailLineItems.SelectMany(c => new { c.COAId, c.IsEditable })).ToDictionary(c=>c)
        //}
        public OpeningBalanceDetail GetOBDetail(Guid id, Guid openingBalanceId)
        {
            return _OpeningBalancesModuleRepository.Query(a => a.Id == id && a.OpeningBalanceId == openingBalanceId).Select().FirstOrDefault();
        }
        public List<OpeningBalanceDetail> GetOpeningBalanceDetail(Guid openiningBalanceId)
        {
            return _OpeningBalancesModuleRepository.Query(c => c.OpeningBalanceId == openiningBalanceId && c.DocCredit == null && c.DocDebit == null).Include(c => c.OpeningBalanceDetailLineItems).Select().OrderBy(c => c.Recorder).ToList();
        }

        public List<OpeningBalanceDetail> GetAllOBDetailAndLineItmsById(List<Guid> openiningBalanceIds)
        {
            return _OpeningBalancesModuleRepository.Query(c => openiningBalanceIds.Contains(c.Id)).Include(c => c.OpeningBalanceDetailLineItems).Select().OrderBy(c => c.Recorder).ToList();
        }
        

    }
}
