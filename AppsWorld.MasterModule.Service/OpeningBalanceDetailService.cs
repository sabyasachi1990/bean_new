using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{

    public class OpeningBalanceDetailService : Service<OpeningBalanceDetail>, IOpeningBalanceDetail
    {
        private readonly IMasterModuleRepositoryAsync<OpeningBalanceDetail> _openingBalanceDetailRepository;
        public OpeningBalanceDetailService(IMasterModuleRepositoryAsync<OpeningBalanceDetail> openingBalanceDetailRepository)
            : base(openingBalanceDetailRepository)
        {
            _openingBalanceDetailRepository = openingBalanceDetailRepository;
        }
        public OpeningBalanceDetail GetOpenaningBalance(long coaId)
        {
            return _openingBalanceDetailRepository.Queryable().Where(s => s.COAId == coaId)/*&& (s.DocCredit == null|| s.DocDebit == null))*/.FirstOrDefault();
        }
    }
}
