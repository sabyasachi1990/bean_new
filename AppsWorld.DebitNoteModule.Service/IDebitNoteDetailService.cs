using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IDebitNoteDetailService:IService<DebitNoteDetail>
    {
        List<DebitNoteDetail> GetAllDebitNoteDetail(Guid debitNooteId);
        List<DebitNoteDetail> AllDebitNoteDetail(Guid debitNoteId);
        DebitNoteDetail GetDebitNoteDetail(Guid id, Guid debitNoteId);
    }
}
