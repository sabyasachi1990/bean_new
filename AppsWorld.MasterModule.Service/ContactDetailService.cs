using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class ContactDetailService : Service<ContactDetail>, IContactDetailService
    {
        private readonly IMasterModuleRepositoryAsync<ContactDetail> _contactDetailRepository;
        private readonly IMasterModuleRepositoryAsync<Contact> _contactRepository;
        private readonly IMasterModuleRepositoryAsync<BeanEntity> _entityRepository;
        public ContactDetailService(IMasterModuleRepositoryAsync<ContactDetail> contactDetailRepository, IMasterModuleRepositoryAsync<Contact> contactRepository, IMasterModuleRepositoryAsync<BeanEntity> entityRepository)
            : base(contactDetailRepository)
        {
            _contactDetailRepository = contactDetailRepository;
            _contactRepository = contactRepository;
            _entityRepository = entityRepository;

        }

        public ContactDetail GetClientContact(Guid contactId, Guid entityId)
        {
            return _contactDetailRepository.Query(c => c.ContactId == contactId && c.EntityId == entityId).Select().FirstOrDefault();
        }
        public ContactDetail GetLeadPrimaryContact(Guid entityId)
        {
            return _contactDetailRepository.Query(c => c.EntityId == entityId && c.IsPrimaryContact == true).Select().FirstOrDefault();
        }
        public List<ContactDetail> GetAllClientContactsbyContactId(Guid contactId)
        {
            return _contactDetailRepository.Query(c => c.ContactId == contactId && c.IsCopy == true).Select().ToList();
        }

        public async Task<bool> IsContactExist(Guid id)
        {
            return await Task.Run(()=> _contactDetailRepository.Queryable().Where(c => c.EntityId == id).Any());
        }
        public List<ContactDetail> GetContactDetails(Guid id)
        {
            return _contactDetailRepository.Queryable().Where(c => c.EntityId == id).ToList();
        }

        public bool GetAssociationsByContactIdDistinct(Guid id, Guid entityId)
        {
            var a= (from cd in _contactDetailRepository.Queryable()
                    join c in _contactRepository.Queryable() on cd.ContactId equals c.Id
                    join be in _entityRepository.Queryable() on cd.EntityId equals be.Id
                    where (c.Id == id && cd.EntityId != entityId)
                    select cd).Any();
            return a;
        }
    }
}
