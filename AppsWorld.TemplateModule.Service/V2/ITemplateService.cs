using AppsWorld.TemplateModule.Entities.Models.V2;
using AppsWorld.TemplateModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service.V2
{
    public interface ITemplateService
    {
        Invoice GetClientsByInvoice(Guid invoiceid);
        CompanyTemplateSettings GetCompanyTemplateSettings(long companyId);
        List<Address> GetAddress(Guid id);
        BeanEntity GetEntity(Guid entityId);
        Company GetServiceCompany(long companyId);
        List<Journal> GetJournal(Guid entityId, long companyId);
        TaxCode GetTaxCode(long? taxId);
        GenericTemplate GetGenerictemplate(long companyId, string templateType);
        Receipt GetReceipt(Guid Id);
        Receipt GetReceiptDetails(long? companyId);
        Guid GetEntityNameById(string entityName);
        Template GetTemplateById(long companyId, string menuType);
        Invoice GetInvoiceById(Guid invoiceId, string Type);
        Receipt GetReceiptById(Guid receipt);
        Localization GetLocalizationByCompanyId(long companyId);
        List<ContactEmailModel> GetContactByClienId(Guid id); //check;
        List<Contact> GetContactsById(Guid id);
        List<Contact> GetContactsById1(Guid id);
        string GetGenericEmailBody(long companyId, string templateName);
        string GetGenericEmailBody1(long companyId, string templateType, string templateName);
        string GetGenericEmailBody1(long companyId, string templateType, string templateName,long serviceCompanyId);
        string GetFirstName(long companyId,string UserName);
        ChartOfAccount GetChartOfAccount(long? coaId);
        string GetGSTnumber(long companyId);
        string GetTermsOfPaymentById(long? id, long companyId);
        Bank GetBank(long companyId);
        Bank GetBank(long companyId, string screenName);
        Bank GetInvoiceBank(long companyId);
        string GetIdType(long? id);
        List<Invoice> GetAllInvoice(Guid entityId, long companyId);
        DebitNote GetDebitNoteById(Guid invoiceId, string Type);
        List<Item> GetAllItems(long companyId);
        CashSale GetCashSaleById(Guid invoiceId, string Type);
        bool? GetServiceCompanyByGst(long companyId);
        MediaRepository GetPhoto(Guid?logoId);
        List<Address> GetAddressForCompany(long id);
        List<Company> GetServiceEntityNameById(long id);
        Company GetServiceCompanyForSOA(long companyId);
        string GetGenerictemplateForCcMail(long value, string templateName);
    }
}
