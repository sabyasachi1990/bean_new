using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;

namespace AppsWorld.DebitNoteModule.Service
{
    //public class DebitNoteGstDetailService:Service<DebitNoteGSTDetail>,IDebitNoteGstDetailService
    //{
    //    private readonly IDebitNoteMoluleRepositoryAsync<DebitNoteGSTDetail> _debitNoteGstDetailRepository;
    //    public DebitNoteGstDetailService(IDebitNoteMoluleRepositoryAsync<DebitNoteGSTDetail> debitNoteGstDetailRepository):base(debitNoteGstDetailRepository)
    //    {
    //        _debitNoteGstDetailRepository = debitNoteGstDetailRepository;
    //    }
    //    public List<DebitNoteGSTDetail> AllGstDetail(Guid debitNoteId)
    //    {
    //        return _debitNoteGstDetailRepository.Query(a => a.DebitNoteId == debitNoteId).Select().ToList();
    //    }
    //}
}
