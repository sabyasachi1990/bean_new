using Service.Pattern;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class AutoNumberService : Service<AutoNumber>, IAutoNumberService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(IJournalVoucherModuleRepositoryAsync<AutoNumber> autoNumberRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetIsEditValue(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(s => s.IsEditable).FirstOrDefault();
        }

        public async Task<bool?> GetIsEditValueAsync(long companyId, string entityType)
        {
            return await Task.Run(()=> _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(s => s.IsEditable).FirstOrDefault());
        }

    }
}
