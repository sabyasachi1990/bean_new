using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ClearingService:Service<GLClearing>,IClearingService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<GLClearing> _clearingRepository;
         
        public ClearingService(IJournalVoucherModuleRepositoryAsync<GLClearing> clearingRepository) :base(clearingRepository)
        {
            _clearingRepository = clearingRepository;
        }
        public GLClearing GetClearing(Guid id,long companyId)
        {
            return _clearingRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public GLClearing GetByCompanyId(long companyId)
        {
            return _clearingRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault();
        }
    }
}
