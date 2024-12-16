using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.MasterModule.Service
{
    public class ContactService : Service<Contact>, IContactService
    {
        private readonly IMasterModuleRepositoryAsync<Contact> _contactRepository;
        private readonly IMasterModuleRepositoryAsync<ContactDetail> _contactDetailRepository;
        private readonly IMasterModuleRepositoryAsync<MediaRepository> _mediaRepository;
        public ContactService(IMasterModuleRepositoryAsync<Contact> contactRepository,
            IMasterModuleRepositoryAsync<ContactDetail> contactDetailRepository,
            IMasterModuleRepositoryAsync<MediaRepository> mediaRepository)
            : base(contactRepository)
        {
            _contactRepository = contactRepository;
            _contactDetailRepository = contactDetailRepository;
            _mediaRepository = mediaRepository;
        }



        public List<ContactDetail> GetContactForAccount(long companyId, Guid entityId, string entityType)
        {
            return _contactDetailRepository.Query(c => /*c.Contact.CompanyId == companyId &&*/ c.EntityId == entityId && c.EntityType == entityType && c.Contact.Status != RecordStatusEnum.Delete).Include(v => v.Contact).Select().ToList();
        }
        public Contact GetContact(Guid Id, long companyId)
        {
            return _contactRepository.Query(c => c.Id == Id & c.CompanyId == companyId).Select().FirstOrDefault();
        }

        public MediaRepository GetMediaRepo(Guid? photoId)
        {
            return _mediaRepository.Query(c => c.Id == photoId).Select().FirstOrDefault();
        }
        public List<MediaRepository> GetAllMediaRepo(List<Guid?> photoIds)
        {
            return _mediaRepository.Query(c => photoIds.Contains(c.Id)).Select().ToList();
        }
    }
}
