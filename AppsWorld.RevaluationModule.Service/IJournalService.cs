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
    public interface IJournalService:IService<Journal>
    {
        List<Journal> GetPostingJournal(DateTime dateTime,long companyId, long serviceCompanyId);
        Journal GetJournal(long companyId, Guid documentId);
        List<Journal> GetAllJournal(long companyId, Guid documentId);
        Journal GetJournalById(long companyId, Guid id);
    }
}
