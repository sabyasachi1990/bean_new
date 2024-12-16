
using AppsWorld.TemplateModule.Entities.Models.V2;
using AppsWorld.TemplateModule.Models.V2;
using AppsWorld.WF.Invoice.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice = AppsWorld.TemplateModule.Entities.Models.V2.Invoice;

namespace AppsWorld.TemplateModule.Service.V2
{
    public interface IInvoiceKService : IService<Entities.Models.V2.Invoice>
    {
        //GenericTemplateModel GetTemplateheaderInvoice(long companyId);
        Invoice GetClientsByInvoice(Guid invoiceid);
        //GenericTemplateModel GetTemplateheaderCreditNote(long companyId);
        CompanyTemplateSettings GetCompanyTemplateSettings(long companyId);
        //Tuple<Entities.Models.V2.Invoice, Entities.Models.V2.Company> GetInvoicesandsubsudaryCompanyById(Guid invoiceId);
        //string GetIdentificationType(long companyId);
        //string GetGSTNumber(long companyId, long serviceCompanyId);
        //Invoice GetInvoicesById(Guid invoiceid);
        //string GetCreditTerms(long companyId, long? creditTermId);
        //string GetEntityName(long companyId, Guid? entityId);




        ///////////////////
        BeanEntity GetEntity(Guid entityId);
        Company GetServiceCompany(long companyId);
        List<Address> GetAddress(Guid id);
        TaxCode GetTaxCode(long? taxId);
        List<Journal> GetJournal(Guid entityId);
        GenericTemplate GetGenerictemplate(long companyId, string templateType);
        Receipt GetReceipt(Guid Id);
        Receipt GetReceiptDetails(long? companyId);
        Guid GetEntityNameById(string entityName);
    }
}
