using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface ITaxCodeService : IService<TaxCode>
    {
        List<TaxCode> GetTaxCodeEdit(long? id, long CompanyId, DateTime date);
        List<TaxCode> GetTaxCodeNew(long CompanyId, DateTime date);

        TaxCode GetTaxCode(long id);
        List<TaxCode> GetTaxCodes(long companyId);

    }
}
