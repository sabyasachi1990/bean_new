using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IJournalDetailService : IService<JournalDetail>
    {
        List<JournalDetail> GetAllDetailByCOAId(long coaId);
        bool? GetAllByCoaId(List<long> coaId);
        bool? GetAllJDetailByCid(long coaId);
    }
}
