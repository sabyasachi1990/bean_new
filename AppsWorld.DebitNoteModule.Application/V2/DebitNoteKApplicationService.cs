using AppsWorld.DebitNoteModule.Models;
using AppsWorld.DebitNoteModule.Service.V2;
using System;
using System.Linq;

namespace AppsWorld.DebitNoteModule.Application.V2
{
    public class DebitNoteKApplicationService
    {
        readonly IDebitNoteKService _debitNoteService;
        public DebitNoteKApplicationService(IDebitNoteKService debitNoteService)
        {
            this._debitNoteService = debitNoteService;
        }

        #region Kendo_Call
        public IQueryable<DebitNoteKModel> GetAllDebitNotesK(string username, long companyId)
        {
            return _debitNoteService.GetAllDebitNotesK(username, companyId);
        }
        #endregion
    }
}
