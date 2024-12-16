using System;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IContactDetailService : IService<ContactDetail>
    {
        ContactDetail GetClientContact(Guid contactId, Guid entityId);
        ContactDetail GetLeadPrimaryContact(Guid entityId);
        List<ContactDetail> GetAllClientContactsbyContactId(Guid contactId);
        Task<bool> IsContactExist(Guid id);
        List<ContactDetail> GetContactDetails(Guid id);
        bool GetAssociationsByContactIdDistinct(Guid id, Guid entityId);
    }
}
