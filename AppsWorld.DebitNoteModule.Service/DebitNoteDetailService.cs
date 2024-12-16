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
    public class DebitNoteDetailService:Service<DebitNoteDetail>,IDebitNoteDetailService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<DebitNoteDetail> _debitNoteDetailRepository;
        public DebitNoteDetailService(IDebitNoteMoluleRepositoryAsync<DebitNoteDetail> debitNoteDetailRepository):base(debitNoteDetailRepository)
        {
            _debitNoteDetailRepository = debitNoteDetailRepository;
        }
        public List<DebitNoteDetail> GetAllDebitNoteDetail(Guid debitNooteId)
        {
            return _debitNoteDetailRepository.Queryable().Where(x => x.DebitNoteId == debitNooteId).ToList();
        }
        public List<DebitNoteDetail> AllDebitNoteDetail(Guid debitNoteId)
        {
            return _debitNoteDetailRepository.Query(a => a.DebitNoteId == debitNoteId).Select().OrderBy(c => c.RecOrder).ToList();
        }
        public DebitNoteDetail GetDebitNoteDetail(Guid id,Guid debitNoteId)
        {
            return _debitNoteDetailRepository.Query(x => x.Id == id && x.DebitNoteId == debitNoteId).Select().FirstOrDefault();
        }
    }
}
