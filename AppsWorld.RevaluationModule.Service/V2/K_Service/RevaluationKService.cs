using System.Linq;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.V2;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public class RevaluationKService : Service<RevaluationK>, IRevaluationKService
    {
        private readonly IRevaluationKRepositoryAsync<RevaluationK> _revaluationRepository;
        private readonly IRevaluationKRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IRevaluationKRepositoryAsync<CompanyUserCompact> _compUserRepo;
        private readonly IRevaluationKRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _compUserDetailRepo;
        public RevaluationKService(IRevaluationKRepositoryAsync<RevaluationK> revaluationRepository, IRevaluationKRepositoryAsync<CompanyCompact> companyRepository, IRevaluationKRepositoryAsync<CompanyUserCompact> compUserRepo, IRevaluationKRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> compUserDetailRepo)
            : base(revaluationRepository)
        {
            this._revaluationRepository = revaluationRepository;
            this._companyRepository = companyRepository;
            this._compUserRepo = compUserRepo;
            _compUserDetailRepo = compUserDetailRepo;
        }
        public IQueryable<RevaluationModelK> GetAllRevaluationsK(string username, long companyId)
        {
            return (from b in _revaluationRepository.Queryable()
                    join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                    join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId
                    join cud in _compUserDetailRepo.Queryable() on compUser.Id equals cud.CompanyUserId where company.Id == cud.ServiceEntityId
                    where compUser.Username == username && b.CompanyId == companyId
                    select new RevaluationModelK
                    {
                        Id = b.Id,
                        CompanyId = b.CompanyId,
                        ServiceCompanyName = company.ShortName != null ? company.ShortName : string.Empty,
                        UserCreated = b.UserCreated,
                        ServiceCompanyId = b.ServiceCompanyId != null ? b.ServiceCompanyId : 0,
                        RevaluationDate = b.RevalutionDate,
                        CreatedDate = b.CreatedDate,
                        ModifiedBy = b.ModifiedBy,
                        ModifiedDate = b.ModifiedDate,
                        DocState = b.DocState,
                        DocNo = b.SystemRefNo,
                        NetAmount = (double) b.NetAmount,
                        IsLocked = b.IsLocked,
                        DocType = "Revaluation"
                    }).OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
    }
}
