using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Pattern;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using AppsWorld.Framework;
using System.Configuration;
using Newtonsoft.Json;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;

namespace AppsWorld.DebitNoteModule.Service
{
    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<Invoice> _invoiceRepository;
        public InvoiceService(IDebitNoteMoluleRepositoryAsync<Invoice> invoiceRepository)
            : base(invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public Invoice GetInvoice(Guid id)
        {
            return _invoiceRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
        public Invoice GetInvoiceByCompany(long companyId, string docType)
        {
            return _invoiceRepository.Query(x => x.CompanyId == companyId && x.DocType == docType).Select().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }
        public Invoice GetDuplicateInvoice(long companyId, string docType, string docNo)
        {
            return _invoiceRepository.Query(x => x.CompanyId == companyId && x.DocType == docType && x.DocNo == docNo).Select().FirstOrDefault();
        }
    }
}
