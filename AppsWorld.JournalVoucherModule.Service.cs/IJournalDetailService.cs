using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;

namespace AppsWorld.JournalVoucherModule.Service
{
	public interface IJournalDetailService : IService<JournalDetail>
	{
		List<JournalDetail> GetAllJournalDetailsByid(Guid jorunalId);
		List<JournalDetail> GetAllJournalDetailsByidForView(Guid jorunalId);
		List<JournalDetail> GetAllOnlyJournalDetails(Guid jorunalId);
		JournalDetail GetJournalDetailsById(Guid Id);
		JournalDetail GetJDdetailJournalId(Guid Id, Guid journalId);
		List<JournalDetail> GetJDdetailJournals(Guid journalId);
        JournalDetail GetJournalDetailById(Guid journalId);
        //List<JournalDetail> GetAllBTJournalDetails(string systemRefNo);
    }
}
