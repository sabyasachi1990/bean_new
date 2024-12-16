using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEmtityService
    {
        private readonly ITemplateModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly ITemplateModuleRepositoryAsync<Company> _companyRepository;
        private readonly ITemplateModuleRepositoryAsync<Address> _addressesRepository;


        public BeanEntityService(ITemplateModuleRepositoryAsync<BeanEntity> beanEntityRepository, ITemplateModuleRepositoryAsync<Company> companyRepository, ITemplateModuleRepositoryAsync<Address> addressesRepository) : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
            _companyRepository = companyRepository;
            _addressesRepository = addressesRepository;

        }

        public List<Address> GetAddress(Guid id)
        {
            List<Address> addresses= _addressesRepository.Query(b => b.AddTypeId == id).Include(b => b.AddressBook).Select().ToList();
            return addresses;
        }

        public BeanEntity GetEntity(Guid entityId)
        {
            BeanEntity beanEntity = _beanEntityRepository.Query(v => v.Id == entityId).Include(v => v.Invoices).Include(v=>v.Invoices.Select(b=>b.InvoiceDetails)).Select().FirstOrDefault();
            return beanEntity;
        }



        public Company GetServiceCompany(long companyId)
        {
            Company company = _companyRepository.Query(b => b.Id == companyId).Include(b => b.Bank).Select().FirstOrDefault();
            return company;
        }
    }
}
