using Service.Pattern;
using AppsWorld.MasterModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IDebitNoteService : IService<DebitNote>
    {
        List<DebitNote> GetDebitNoteByEntity(Guid? entityId);
    }
}
