using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;
using System.Data.Entity;

namespace AppsWorld.ReceiptModule.Service
{
    public class CompanyService : Service<Company>, ICompanyService
    {
        private readonly IReceiptModuleRepositoryAsync<Company> _companyRepository;
        private readonly IReceiptModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly IReceiptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;

        public CompanyService(IReceiptModuleRepositoryAsync<Company> companyRepository, IReceiptModuleRepositoryAsync<CompanyUser> companyUserRepository, IReceiptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
            : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
        }
        public Company GetByNameByServiceCompany(long parentId)
        {
            return _companyRepository.Query(c => c.Id == parentId).Select().FirstOrDefault();
        }
        public List<Company> GetCompany(string userName, long companyId, long companyIdCheck)
        {
            return (from c in _companyRepository.Queryable()
                                  join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                                  join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                                  where c.Id == cud.ServiceEntityId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyIdCheck) && c.ParentId == companyId && cu.Username == userName
                    select c).ToList();
        }
        public Dictionary<long, string> GetAllCompaniesName(List<long> Ids)
        {
            return _companyRepository.Query(c => Ids.Contains(c.Id) && c.Status == RecordStatusEnum.Active).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
        }
        public Dictionary<long, string> GetAllCompanies(List<long> Ids)
        {
            return _companyRepository.Query(c => Ids.Contains(c.Id)).Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, Name => Name.Code);
        }
        public Dictionary<long, string> GetAllSubCompanies(List<long> Ids, string username, long companyId)
        {
            return (from c in _companyRepository.Query(c => Ids.Contains(c.Id)).Select().AsQueryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c).ToList().Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, nameof => nameof.Code);
        }
        public Dictionary<long, RecordStatusEnum> GetAllCompaniesStatus(List<long> Ids)
        {
            return _companyRepository.Query(c => Ids.Contains(c.Id)).Select(c => new { Ids = c.Id, Status = c.Status }).ToDictionary(Id => Id.Ids, Status => Status.Status);
        }
        public Company GetById(long id)
        {
            return _companyRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public bool GetPermissionBasedOnUser(long? serviceEntityId, long? companyId, string username)
        {
            return (from cu in _companyUserRepository.Queryable()
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where cud.ServiceEntityId == serviceEntityId && cu.CompanyId == companyId && cu.Username == username
                    select cud.ServiceEntityId).Any();
        }
    }
}
