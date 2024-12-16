using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.RepositoryPattern;
using AppsWorld.GLClearingModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.GLClearingModule.Models;
using AppsWorld.GLClearingModule.Infra;

namespace AppsWorld.GLClearingModule.Service
{
    public class ClearingService : Service<GLClearing>, IClearingService
    {
        private readonly IClearingModuleRepositoryAsync<GLClearing> _clearingRepository;
        private readonly IClearingModuleRepositoryAsync<GLClearingDetail> _clearingDetailRepository;
        private readonly IClearingModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        private readonly IClearingModuleRepositoryAsync<Company> _companyRepository;
        private readonly IClearingModuleRepositoryAsync<CompanyUser> _compUserRepo;
        private readonly IClearingModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public ClearingService(IClearingModuleRepositoryAsync<GLClearing> clearingRepository, IClearingModuleRepositoryAsync<GLClearingDetail> clearingDetailRepository, IClearingModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, IClearingModuleRepositoryAsync<Company> companyRepository, IClearingModuleRepositoryAsync<CompanyUser> compUserRepo, IClearingModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo) : base(clearingRepository)
        {
            _clearingRepository = clearingRepository;
            _clearingDetailRepository = clearingDetailRepository;
            _chartOfAccountRepository = chartOfAccountRepository;
            _companyRepository = companyRepository;
            _compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public GLClearing GetClearing(Guid id, long companyId)
        {
            return _clearingRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(c => c.GLClearingDetails).Select().FirstOrDefault();
        }
        public GLClearing GetByCompanyId(long companyId)
        {
            return _clearingRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public IQueryable<ClearingModelK> GetAllClearingK(string username, long companyId)
        {
            IQueryable<GLClearing> lstClearing = _clearingRepository.Queryable();

            IQueryable<ClearingModelK> lstClearingModel = from gl in lstClearing
                                                          join coa in _chartOfAccountRepository.Queryable() on gl.COAId equals coa.Id
                                                          join company in _companyRepository.Queryable() on gl.ServiceCompanyId equals company.Id
                                                          join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId
                                                          where gl.CompanyId == companyId
                                                          && (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
                                                          select new ClearingModelK()
                                                          {
                                                              Id = gl.Id,
                                                              CompanyId = gl.CompanyId,
                                                              //DocDate = gl.DocDate,
                                                              DocNo = gl.DocNo,
                                                              DocType = gl.DocType,
                                                              ServiceCompanyName = company.ShortName,
                                                              AccountName = coa.Name,
                                                              SystemRefNo = gl.SystemRefNo,
                                                              DocDescription = gl.DocDescription,
                                                              DocumentState = gl.DocumentState,
                                                              UserCreated = gl.UserCreated,
                                                              ModifiedBy = gl.ModifiedBy,
                                                              ModifiedDate = gl.ModifiedDate,
                                                              CreatedDate = gl.CreatedDate,
                                                              IsLocked=gl.IsLocked
                                                          };
            return lstClearingModel.OrderByDescending(c => c.CreatedDate).AsQueryable();
        }

        public IQueryable<ClearedModelK> GetAllClrearedK(long companyId, string username)
        {
            IQueryable<ClearedModelK> clearedLst = from gl in _clearingRepository.Queryable()
                                                   join ch in _chartOfAccountRepository.Queryable()
                                                   on gl.COAId equals ch.Id
                                                   join c in _companyRepository.Queryable() on gl.CompanyId equals c.ParentId where c.ParentId == companyId && gl.ServiceCompanyId == c.Id
                                                   join cu in _compUserRepo.Queryable() on c.ParentId equals cu.CompanyId
                                                   join cud in _compUserDetailRepo.Queryable() on cu.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                                                   where gl.DocumentState != ClearingState.Unclear && cu.Username == username && gl.CompanyId == companyId
                                                   select new ClearedModelK()
                                                   {
                                                       Id = gl.Id,
                                                       AccountName = ch.Name,
                                                       ClearingDate = gl.ClearingDate,
                                                       TransactionCount = gl.TransactionCount,
                                                       CheckAmount = gl.CheckAmount,
                                                       UserCreated = gl.UserCreated,
                                                       CreatedDate = gl.CreatedDate,
                                                       RowVersion = gl.Version,
                                                       IsLocked = gl.IsLocked

                                                   };
            return clearedLst.OrderByDescending(s => s.CreatedDate).AsQueryable();
        }
        public List<GLClearing> GelAllClearings(long companyId)
        {
            return _clearingRepository.Query(c => c.CompanyId == companyId).Select().ToList();
        }
        public bool IsDocNoExists(long companyId, Guid id, string docNo)
        {
            GLClearing isExists = _clearingRepository.Query(a => a.CompanyId == companyId && a.Id != id && a.DocNo == docNo).Select().FirstOrDefault();
            return isExists != null ? true : false;
        }
        public Guid GetClearingByDetailId(Guid detailId, long companyId)
        {
            return _clearingDetailRepository.Query(a => a.JournalDetailId == detailId).Select(c => c.GLClearingId).FirstOrDefault();
        }
        //public List<GLClearing> GetAllClearings(List<Guid> ids,long? companyId)
        //{
        //    return _clearingRepository.Queryable().Where(d => ids.Contains(d.Id) && d.CompanyId == companyId).ToList();
        //}
        public GLClearing GetClearingById(Guid id, long companyId)
        {
            return _clearingRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
    }
}
