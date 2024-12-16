using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.Models;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IDebitNoteService : IService<DebitNote>
    {
        DebitNote GetDebitNoteById(Guid id, long companyId);
        DebitNote GetDebitNote(Guid id, long companyId);
        DebitNote CreateDebitNote(long companyId);
        DebitNote CreateDebitNoteForDocNo(long companyId);
        DebitNote GetDocNo(string docNo, long companyId);
        Task<IQueryable<DebitNoteKModel>> GetAllDebitNotesK(string userName, long companyId);
        DebitNote GetDebitNoteDocNo(Guid id, string docNo, long companyId);
        List<DebitNote> GetAllDebitModel(long companyId);
        DebitNote GetDebittNote(Guid id);
        DateTime? GetDNLastPostedDate(long companyId);

        //To check if it is void or not
        bool IsVoid(long companyId, Guid id);
    }
}
