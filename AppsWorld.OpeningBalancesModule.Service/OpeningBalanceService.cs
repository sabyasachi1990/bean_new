using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.Models;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public class OpeningBalanceService : Service<OpeningBalance>, IOpeningBalanceService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<OpeningBalance> _OpeningBalancesRepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;

        public OpeningBalanceService(IOpeningBalancesModuleRepositoryAsync<OpeningBalance> OpeningBalancesRepository, IOpeningBalancesModuleRepositoryAsync<CompanyUser> companyUserRepository, IOpeningBalancesModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
            : base(OpeningBalancesRepository)
        {
            _OpeningBalancesRepository = OpeningBalancesRepository;
            _companyUserRepository = companyUserRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
        }

        #region Kendo
        public IQueryable<OpeningBalanceModelK> GetAllOpeningBalancessK(long companyId, string userName)
        {
            IQueryable<Company> companyRepository = _OpeningBalancesRepository.GetRepository<Company>().Queryable();
            IQueryable<OpeningBalance> OpeningBalancesModuleRepository = _OpeningBalancesRepository.Queryable().Where(a => a.CompanyId == companyId);
            //IQueryable<OpeningBalanceModelK> openingBalanceModelKDetails = from b in OpeningBalancesModuleRepository
            //                                                               join c in companyRepository on b.CompanyId equals c.Id
            //                                                               join user in _companyUserRepository.Queryable() on b.ServiceCompanyId equals user.CompanyId
            //                                                               //where b.CompanyId == companyId
            //                                                               //where b.ServiceCompanyId == c.Id
            //                                                               where b.CompanyId == companyId && (user.ServiceEntities != null ? user.ServiceEntities.Contains(user.Id.ToString()) : true) && user.Username == userName
            //                                                               //
            //                                                               select new OpeningBalanceModelK()
            //                                                               {
            //                                                                   Id = b.Id,
            //                                                                   CompanyId = b.CompanyId,
            //                                                                   ServiceCompanyId = b.ServiceCompanyId,
            //                                                                   ServiceCompanyName = c.ShortName,
            //                                                                   Date = b.Date,
            //                                                                   Currency = b.BaseCurrency,
            //                                                                   UserCreated = b.UserCreated,
            //                                                                   Status = b.Status,
            //                                                                   CreatedDate = b.CreatedDate,
            //                                                                   ModifiedBy = b.ModifiedBy,
            //                                                                   ModifiedDate = b.ModifiedDate,
            //                                                                   SaveType = b.SaveType
            //                                                               };
            //return openingBalanceModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();


            IQueryable<OpeningBalanceModelK> openingBalanceModelKDetails = from b in OpeningBalancesModuleRepository
                                                                           join c in companyRepository on b.ServiceCompanyId equals c.Id
                                                                           join user in _companyUserRepository.Queryable() on c.ParentId equals user.CompanyId
                                                                           join CUD in _companyUserDetailRepository.Queryable() on user.Id equals CUD.CompanyUserId
                                                                           where c.Id == CUD.ServiceEntityId
                                                                           where user.Username == userName && b.CompanyId == companyId && b.IsTemporary != true
                                                                           select new OpeningBalanceModelK()
                                                                           {
                                                                               Id = b.Id,
                                                                               CompanyId = b.CompanyId,
                                                                               ServiceCompanyId = b.ServiceCompanyId,
                                                                               ServiceCompanyName = c.ShortName,
                                                                               Date = b.Date,
                                                                               Currency = b.BaseCurrency,
                                                                               UserCreated = b.UserCreated,
                                                                               Status = b.Status,
                                                                               CreatedDate = b.CreatedDate,
                                                                               ModifiedBy = b.ModifiedBy,
                                                                               ModifiedDate = b.ModifiedDate,
                                                                               SaveType = b.SaveType,
                                                                               IsLocked = b.IsLocked
                                                                           };
            return openingBalanceModelKDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();



        }
        #endregion

        #region LookUp
        public OpeningBalance GetOpeningBalance(long companyId, long ServiceCompanyId)
        {
            return _OpeningBalancesRepository.Query(c => c.CompanyId == companyId && c.ServiceCompanyId == ServiceCompanyId).Include(c => c.OpeningBalanceDetails).Select().FirstOrDefault();
        }
        #endregion

        #region getcall
        public OpeningBalance GetServiceCompanyOpeningBalance(long companyId, long ServiceCompanyId)
        {
            return _OpeningBalancesRepository.Query(c => c.CompanyId == companyId && c.ServiceCompanyId == ServiceCompanyId && c.IsTemporary != true).Select().FirstOrDefault();
        }
        public OpeningBalance GetServiceCompanyOpeningBalanceNew(long companyId, long ServiceCompanyId)
        {
            return _OpeningBalancesRepository.Query(c => c.CompanyId == companyId && c.ServiceCompanyId == ServiceCompanyId && c.IsTemporary == true).Select().FirstOrDefault();
        }
        public OpeningBalance GetOpeningBalanceById(Guid Id)
        {
            return _OpeningBalancesRepository.Queryable().Where(c => c.Id == Id).FirstOrDefault();
        }
        public OpeningBalance CheckOpeningBalanceById(Guid Id)
        {
            return _OpeningBalancesRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }
        public OpeningBalance GetServiceCompanyOpeningBalance(long ServiceCompanyId)
        {
            return _OpeningBalancesRepository.Query(c => c.ServiceCompanyId == ServiceCompanyId).Include(c => c.OpeningBalanceDetails).Select().FirstOrDefault();
        }
        public List<OpeningBalance> lstopeningbalance(long companyid)
        {
            return _OpeningBalancesRepository.Query(a => a.CompanyId == companyid).Select().OrderByDescending(d => d.CreatedDate).ToList();
        }

        #endregion
    }
}
