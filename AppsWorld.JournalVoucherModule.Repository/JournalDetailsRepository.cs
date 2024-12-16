using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
namespace AppsWorld.JournalVoucherModule.Repository
{
   public static class JournalDetailsRepository
    {
	   public static IEnumerable<JournalDetail> GetJournalDeailsById(this IJournalVoucherModuleRepositoryAsync<JournalDetail> journalDeailsrepository, Guid Id)
        {
			return journalDeailsrepository.Queryable().Where(x => x.Id == Id).AsEnumerable();
        }
    }

}
