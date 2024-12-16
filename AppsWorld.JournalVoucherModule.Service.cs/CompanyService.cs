using Service.Pattern;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using FrameWork;
using AppsWorld.JournalVoucherModule.Entities.Models;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CompanyService : Service<Company>, ICompanyService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Company> _companyRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly IJournalVoucherModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;
        public CompanyService(IJournalVoucherModuleRepositoryAsync<Company> companyRepository, IJournalVoucherModuleRepositoryAsync<CompanyUser> companyUserRepository, IJournalVoucherModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
            : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
        }
        public Company GetByNameByServiceCompany(long? parentId)
        {
            return _companyRepository.Query(c => c.ParentId == parentId).Select().FirstOrDefault();
        }
        public async Task<List<Company>> GetCompany(long companyId, long companyIdCheck,string userName)
        {
            return (from c in await Task.Run(()=> _companyRepository.Queryable())
                    join cu in await Task.Run(()=> _companyUserRepository.Queryable()) on c.ParentId equals cu.CompanyId
                    join cud in await Task.Run(()=> _companyUserDetailRepository.Queryable()) on cu.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyIdCheck) && c.ParentId == companyId && cu.Username == userName
                    select c).ToList();
          
        }
        public Company GetById(long? id)
        {
            return _companyRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }


        public List<FrameWork.LookUps.LookUp<long>> GetAllLookup(long companyId, string Username)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId 
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyId) && c.ParentId == companyId
                    && cu.Username == Username
                    select c).Select(m => new FrameWork.LookUps.LookUp<long>()
            {
                Code = m.ShortName,
                Id = m.Id,
                Name = m.Name,
                Status = m.Status
            }).OrderBy(x=>x.Name).ToList();

        }

        public string GetCompanyName(long? id)
        {
            return _companyRepository.Query(a => a.Id == id).Select(a => a.ShortName).FirstOrDefault();
        }
    }
}
