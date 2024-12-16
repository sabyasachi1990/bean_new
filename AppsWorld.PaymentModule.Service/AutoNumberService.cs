using AppsWorld.PaymentModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
namespace AppsWorld.PaymentModule.Service
{
    public class AutoNumberService : Service<AutoNumber>,IAutoNumberService
    {
        private readonly IPaymentModuleRepositoryAsync<AutoNumber> _autoNumberepository;

        public AutoNumberService(IPaymentModuleRepositoryAsync<AutoNumber> autoNumberRepository)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }
        public bool? GetAutoNumberIsEditable(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(c => c.IsEditable).FirstOrDefault();
        }
    }
}
