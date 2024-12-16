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
    //public class InvoiceGSTDetailService:Service<InvoiceGSTDetail>,IInvoiceGSTDetailService       
    //{
    //    private readonly IInvoiceModuleRepositoryAsync<InvoiceGSTDetail> _invoiceGSTDeilService;
    //    public InvoiceGSTDetailService(IInvoiceModuleRepositoryAsync<InvoiceGSTDetail> invoiceGSTDeilService)
    //         : base(invoiceGSTDeilService)
    //    {
    //        _invoiceGSTDeilService = invoiceGSTDeilService;
    //    }
    //    public ICollection<InvoiceGSTDetail> GetById(Guid Id)
    //    {
    //        return _invoiceGSTDeilService.Query(a => a.InvoiceId == Id).Select().ToList();
    //    }
    //    public InvoiceGSTDetail GetInvoiceGSTDetail(Guid invoiceId, Guid invoiceGSTDetailId)
    //    {
    //        return _invoiceGSTDeilService.Query(a => a.InvoiceId == invoiceId && a.Id == invoiceGSTDetailId).Select().FirstOrDefault();
    //    }
    //}
}
