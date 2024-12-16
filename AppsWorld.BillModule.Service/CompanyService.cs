using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.BillModule.Entities.Models;

namespace AppsWorld.BillModule.Service
{
    public class CompanyService : Service<Company>, ICompanyService
    {
        private readonly IBillModuleRepositoryAsync<Company> _companyRepository;
        private readonly IBillModuleRepositoryAsync<CompanyUser> _companyUserRepository;
        private readonly IBillModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;


        private readonly IBillModuleRepositoryAsync<PeppolInboundInvoice> _peppolInboundInvRepository;



        public CompanyService(IBillModuleRepositoryAsync<Company> companyRepository, IBillModuleRepositoryAsync<CompanyUser> companyUserRepository, IBillModuleRepositoryAsync<PeppolInboundInvoice> peppolInboundInvRepository, IBillModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
			: base(companyRepository)
        {
			_companyRepository = companyRepository;
            _companyUserRepository = companyUserRepository;
            _peppolInboundInvRepository = peppolInboundInvRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
        }
		public Company GetByNameByServiceCompany(long parentId)
		{
			return _companyRepository.Query(c => c.ParentId == parentId).Select().FirstOrDefault();
		}
		public List<Company> GetCompany( long companyId,long companyIdCheck,string userName)
		{
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId where c.Id == cud.ServiceEntityId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyIdCheck) && c.ParentId == companyId && cu.Username == userName
                    select c).ToList();

            //return _companyRepository.Queryable().Where(a => (a.Status == RecordStatusEnum.Active || a.ParentId == companyIdCheck) && a.ParentId == companyId).ToList();
        }
        public List<long> GetAllSubCompaniesId(string username, long companyId)
        {
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c.Id).ToList();
        }
        public Company GetById(long id)
        {
            return  _companyRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }






        public List<Company> GetCompanyByName( string ReciverPeppolId)
        {
           return  _companyRepository.Query(x => x.ParticipantPeppolId == ReciverPeppolId).Select().ToList();
        }

        public void InsertIntoPeppolInboundInvoice(PeppolInboundInvoice peppolInboundInvoice)
        {
            _peppolInboundInvRepository.Insert(peppolInboundInvoice);
        }

        public PeppolInboundInvoice GetInboundData(Guid id)
        {
            return _peppolInboundInvRepository.Query(x => x.Id==id).Select().FirstOrDefault();
        }
        public void UpdatetoPeppolInboundInvoice(PeppolInboundInvoice peppolInboundInvoice)
        {
            _peppolInboundInvRepository.Update(peppolInboundInvoice);
        }
    }
}
