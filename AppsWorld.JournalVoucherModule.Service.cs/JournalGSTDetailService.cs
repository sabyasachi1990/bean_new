using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    //public  class JournalGSTDetailService:Service<JournalGSTDetail>,IJournalGSTDetailService
    //{
    //    private readonly IJournalVoucherModuleRepositoryAsync<JournalGSTDetail> _journalGSTDetailRepository;

    //    public JournalGSTDetailService(IJournalVoucherModuleRepositoryAsync<JournalGSTDetail> journalGSTDetailRepository):base(journalGSTDetailRepository)
    //    {
    //        this._journalGSTDetailRepository = journalGSTDetailRepository;
    //    }

    //    //public List<JournalGSTDetail> GetAllJournalGSTDetails(Guid jorunalId)
    //    //{
    //    //    return _journalGSTDetailRepository.Query(s => s.JournalId == jorunalId).Select().ToList();
    //    //}

    //    //public JournalGSTDetail GetJournalGSTDetailsById(Guid Id)
    //    //{
    //    //    return _journalGSTDetailRepository.Query(x => x.Id == Id).Select().FirstOrDefault();
    //    //}
    //}
}
