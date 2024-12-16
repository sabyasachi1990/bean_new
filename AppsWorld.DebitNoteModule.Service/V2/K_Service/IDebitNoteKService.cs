using AppsWorld.DebitNoteModule.Entities.V2;
using AppsWorld.DebitNoteModule.Models;
using Service.Pattern;
using System.Linq;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public interface IDebitNoteKService : IService<DebitNoteK>
    {
        IQueryable<DebitNoteKModel> GetAllDebitNotesK(string username, long companyId);
    }
}
