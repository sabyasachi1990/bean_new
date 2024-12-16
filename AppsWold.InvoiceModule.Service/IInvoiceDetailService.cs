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
    public interface IInvoiceDetailService : IService<InvoiceDetail>
    {
        ICollection<InvoiceDetail> GetById(Guid Id);

       List<InvoiceDetail> GetInvoiceDetailById(Guid Id);
       InvoiceDetail GetAllInvoiceIdAndId(Guid invoiceId, Guid invoiceDetalId);
    }
}
