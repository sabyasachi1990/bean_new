using System;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System.Collections.Generic;

namespace AppsWorld.MasterModule.Service
{
    public interface IContactService : IService<Contact>
    {
        List<ContactDetail> GetContactForAccount(long companyId, Guid entityId, string entityType);
        Contact GetContact(Guid Id, long companyId);
        MediaRepository GetMediaRepo(Guid? photoId);
        List<MediaRepository> GetAllMediaRepo(List<Guid?> photoIds);
    }
}
