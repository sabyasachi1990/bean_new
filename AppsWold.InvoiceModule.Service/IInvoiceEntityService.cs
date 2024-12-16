using AppsWorld.CommonModule.Entities;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Entities.Models;
using AppsWorld.InvoiceModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IInvoiceEntityService : IService<Invoice>
    {
        Task<IQueryable<InvoiceModelK>> GetAllInvoicesKOld(string userName, long companyId, string internalState);
        Task<Tuple<IQueryable<InvoiceModelK>, int>> GetAllInvoicesK(string userName, long companyId, string internalState);
        Task<IQueryable<CreditNoteModelK>> GetAllCreditNoteK(string userName, long companyId);
        Invoice GetCompanyAndId(long companyId, Guid id);
        Invoice GetByCompanyId(long companyId);
        Invoice GetinvoiceById(Guid Id);
        Invoice GetAllInvoiceLu(long companyId, Guid Id);
        Invoice GetAllInvoiceByIdDocType(Guid Id);
        Invoice GetAllInvoiceByDoctypeAndCompanyId(string docType, long companyId);
        Invoice GetAllInvovoice(string strNewDocNo, string docType, long CompanyId);
        Invoice GetAllInvoice(Guid Id, string docType, string docNo, long companyId, string DocumentVoidState);
        List<Invoice> GetCompanyIdAndDocType(long companyId);
        List<Invoice> GetCompanyIdByDoubtFulDbt(long companyId);
        List<Invoice> GetCompanyIdByCreditNote(long companyId);
        List<Invoice> GetCompanyIdAndId(long CompanyId, string DocType);
        Invoice GetInvoiceByIdAndComapnyId(long companyId, Guid Id);
        Invoice GetCompanyById(long companyId);
        Invoice GetAllInvoiceById(Guid Id);
        Invoice GetAllDebtNoteByCompanyId(long companyId);
        Invoice GetCreditNoteByCompanyIdAndId(long companyid, Guid id);
        Invoice GetCreditNoteByCompanyId(long companyId);
        Invoice GetCreditNoteById(Guid id);
        Invoice GetDoubtfuldebtByCompanyIdAndId(Guid Id, long companyId);
        Invoice GetCreditNoteByDocumentId(Guid Id);
        List<Invoice> GetAllCreditNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime date);
        Invoice GetDoubtfulDebtByIdAndCompanyId(long companyid, Guid Id);
        Invoice GetDoubtFuldbtByCompanyId(long companyId);
        Invoice GetDoubtfulDebtNoteById(Guid id);
        Task<IQueryable<DoubtfullDebitModelK>> GetAllDebitfulldebitK(string userName, long companyId);
        List<Invoice> GetAllInvoice(long companyId, Guid EntityId, string doccurrency, long servicecompanyid, DateTime date, string Nature);
        Invoice DoubtFulDebtbyId(Guid id);
        List<Invoice> GetAllByEntityId(Guid id, Guid invoiceId);
        List<Invoice> GetAllCnByEntityId(Guid id);
        List<Invoice> GetAllInvoiceByCidAndInvno(long companyId, string docType, string invNumber);

        List<string> GetInvoiceStatusByIds(List<Guid> Ids);

        Invoice GetInvoiceByIdAndDocumentId(Guid documentId, long companyId);

        List<Invoice> GetInvoiceNumber(long companyId, string invNumber);
        List<Invoice> GetDocNumber(long companyId, string docNo);
        Task<IQueryable<RecurringInvoiceK>> GetAllRecurringInvoicesK(string userName, long companyId);
        List<string> GetListofPotedDocNo(long companyId, string invState);
        IQueryable<InvoiceVoidK> GetAllVoidInvoicesK(long companyId);
        Invoice GetInvoiceByRecurId(long companyId, Guid recurInvId);
        List<string> GetListOfPostedDocNo(long companyId, string internalState);
        Invoice GetRecInvoiceByIStateAndCId(string docType, string internalSate, long companyId);
        List<Invoice> GetAllInvoiceByServiceCompany(long serviceCompanyId, long? companyId);
        bool GetRecurringDocNo(long companyId, Guid id, string internalState, string docNo);
        bool GetRecurringDocNo_modified(long companyId, Guid id, string docNo);
        IQueryable<InvoiceModelK> GetAllRecuurringPostedInvoicesK(long companyId, string internalState, Guid id);

        //for recurring master
        Invoice GetRecurringMaster(long companyId, Guid recId);

        //for invoice systemrefrence no
        List<Invoice> GetAllInvoiceByCIDandType(long companyId, string docType);
        Invoice GetAllIvoiceByCidAndDocSubtype(string docType, long companyId, string docSubType);
        IEnumerable<dynamic> GetLastInvoiceDocDate(long companyId);
        List<Invoice> GetAllDDByInvoiceId(List<Guid> Ids);
        decimal? GetBalanceAmount(long companyId, Guid id);
        IQueryable<InvoiceStateModel> GetDeletedAuditTrail(Guid invoiceId);
        Invoice GetDoubtfulDebtByCompanyId(long companyId);
        Invoice GetInvoice(Guid invoiceId, string templateType);

        List<Address> GetAddress(Guid entityId);
        Bank GetBankDetails(long companyId);
        Invoice GetInvoicebyExtenctionType(Guid id);
        Invoice GetInvoiceByCIdandId(Guid invoiceId, long companyId);
        List<Invoice> GetListOfWFInvoice(long companyId);


        //need to delete
        IQueryable<InvoiceModelK> GetAllParkedInvoice(string username, long companyId, string internalState);

        //newlly added for geting last docdate
        DateTime? GetLastPostedDate(long companyId);
        //to check if invoice documentstate is void or not
        bool IsVoid(long companyId, Guid id);
        Invoice GetByCompanyIdForInvoice(long companyId, string doctype);
        Invoice GetByCompanyIdForInvoiceWithDocSubType(long companyId, string doctype, string docsubtype);

        Invoice GetIntercoCreditNote(long companyId, Guid creditNoteId, string docSubType);

        Dictionary<Guid, long> GetListOsServiceEntityByInvoiceId(long companyId, List<Guid> invoiceIds, string docType);
        Bank GetBankDetailsByCompanyId(long serviceComanyId);

        Task<Invoice> GetByCompanyIdForInvoiceAsync(long companyId, string docType);

        Task<Invoice> GetAllInvoiceLuAsync(long companyId, Guid Id);

        Task<Invoice> GetCompanyAndIdAsync(long companyId, Guid id);
    }
}
