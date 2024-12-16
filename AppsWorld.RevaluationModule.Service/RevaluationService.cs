using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class RevaluationService : Service<Revaluation>, IRevaluationService
    {
        private readonly IRevaluationModuleRepositoryAsync<Revaluation> _revaluationRepository;
        private readonly IRevaluationModuleRepositoryAsync<Company> _companyRepository;
        private readonly IRevaluationModuleRepositoryAsync<CompanyUser> _compUserRepo;
        public RevaluationService(IRevaluationModuleRepositoryAsync<Revaluation> revaluationRepository, IRevaluationModuleRepositoryAsync<Company> companyRepository, IRevaluationModuleRepositoryAsync<CompanyUser> compUserRepo)
            : base(revaluationRepository)
        {
            _revaluationRepository = revaluationRepository;
            this._companyRepository = companyRepository;
            _compUserRepo = compUserRepo;

        }
        public IQueryable<RevaluationModelK> GetAllRevaluationsK(string username, long companyId)
        {
            //IQueryable<Company> companyRepository = _revaluationRepository.GetRepository<Company>().Queryable();

            IQueryable<Revaluation> revaluationRepository = _revaluationRepository.Query(x => x.CompanyId == companyId).Select().AsQueryable();
            IQueryable<RevaluationModelK> revaluationModelKDetails = from b in revaluationRepository
                                                                     join company in _companyRepository.Queryable() on b.ServiceCompanyId equals company.Id
                                                                     join compUser in _compUserRepo.Queryable() on company.ParentId equals compUser.CompanyId
                                                                     where (compUser.ServiceEntities != null ? compUser.ServiceEntities.Contains(company.Id.ToString()) : true) && compUser.Username == username
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
                                                                         DocState = b.DocState
                                                                     };
            return revaluationModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }
        public Revaluation GetAllRevaluationById(Guid id, long companyId)
        {
            return _revaluationRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<Revaluation> GetAllRevaluationCompanyId(long companyId)
        {
            return _revaluationRepository.Queryable().Where(x => x.CompanyId == companyId).ToList();
        }
        public List<Revaluation> GetAllPostedRevaluation(DateTime? revDate, long? serviceCompanyId)
        {
            return _revaluationRepository.Queryable().Where(x => x.RevalutionDate == revDate && x.ServiceCompanyId == serviceCompanyId && x.DocState == "Posted").ToList();
        }
        public Revaluation GetAllRevaluationAndDetail(Guid id, long companyId)
        {
            return _revaluationRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(d => d.RevalutionDetails).Select().FirstOrDefault();
        }
    }
}
