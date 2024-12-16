using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IDebitNoteService : IService<DebitNote>
    {
        List<DebitNote> GetTaggedDebitNotesByCustomerAndCurrency(Guid customerId, string currency, long companyId);
        DebitNote GetDebitNoteById(Guid Id);
        List<DebitNote> GetAllDebitNoteByIdAndNature(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docDate,string Nature);
        List<DebitNote> GetAllDebitNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docDate);
        DebitNote GetAllDebitNoteById(Guid Id);
        List<DebitNote> GetDebitNoteByEntity(Guid? entityId);
        List<DebitNote> GetDDByDebitNoteId(List<Guid> Ids);
        decimal? GetBalanceAmount(long companyId, Guid Id);

        bool IsVoid(long companyId, Guid id);
        List<string> GetDNStatusByIds(List<Guid> Ids);
    }
}
