using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class InvoiceNoteService:Service<InvoiceNote>,IInvoiceNoteService
    {
        
         private readonly IInvoiceModuleRepositoryAsync<InvoiceNote> _invoiceNoteService;
         public InvoiceNoteService(IInvoiceModuleRepositoryAsync<InvoiceNote> invoiceNoteService)
             : base(invoiceNoteService)
        {
            _invoiceNoteService = invoiceNoteService;
        }

         public ICollection<InvoiceNote> GetInvoiceByid(Guid Id)
         { 
         return _invoiceNoteService.Query(a => a.InvoiceId == Id).Select().ToList();
         }

         public InvoiceNote GetInvoiceNote(Guid invoiceId, Guid invoiceNoteId)
         {
             return _invoiceNoteService.Query(a => a.InvoiceId == invoiceId && a.Id == invoiceNoteId).Select().FirstOrDefault();
         }

   

    }
}
