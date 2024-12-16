using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Service.cs;
//using AppsWorld.MasterModule.Entities;
//using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ActivityHistoryService : Service<ActivityHistory>, IActivityHistoryService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<ActivityHistory> _activityHistoryRepository;
        public ActivityHistoryService(IJournalVoucherModuleRepositoryAsync<ActivityHistory> activityHistoryRepository)
            : base(activityHistoryRepository)
        {
            _activityHistoryRepository = activityHistoryRepository;
        }
        public List<ActivityHistory> GetByCIdAId(long companyid, Guid id, string type)
        {
            return _activityHistoryRepository.Query(a => a.CompanyId == companyid && a.DocumentId == id && a.Type == type).Select().ToList();
        }
    }
}
