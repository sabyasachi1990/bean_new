using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public interface IJournalDetailService:IService<JournalDetail>
    {
        List<JournalDetail> GetPostingJournalDetail(DateTime dateTime);
        List<JournalDetail> GetAllOnlyJournalDetails(Guid jorunalId);
        JournalDetail GetAllJournalDetails(Guid jorunalId);
    }
}
