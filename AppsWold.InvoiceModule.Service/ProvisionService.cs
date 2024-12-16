using AppsWorld.CommonModule.Infra;
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
    //public class ProvisionService : Service<Provision>, IProvisionService
    //{
    //    private readonly IInvoiceModuleRepositoryAsync<Provision> _provisionService;
    //    public ProvisionService(IInvoiceModuleRepositoryAsync<Provision> provisionService)
    //        : base(provisionService)
    //    {
    //        _provisionService = provisionService;
    //    }

    //    public List<Provision> GetProvisionByInvoiceId(Guid invoiceId)
    //    {
    //        return _provisionService.Query(x => x.RefDocumentId == invoiceId).Select().ToList();
    //    }
    //    public Provision GetProvisionById(Guid id)
    //    {
    //        return _provisionService.Query(x => x.Id == id).Select().FirstOrDefault();
    //    }
    //    public List<Provision> lstInvoiceProvision(long companyId)
    //    {
    //        return _provisionService.Query(c => c.CompanyId == companyId && c.RefDocType == DocTypeConstants.Invoice).Select().ToList();
    //    }
    //    public List<Provision> lstDebitProvision(long companyId)
    //    {
    //        return _provisionService.Query(c => c.CompanyId == companyId && c.RefDocType == DocTypeConstants.DebitNote).Select().ToList();
    //    }
    //    public Provision DocNoCheck(Guid id, long companyId, string docNo)
    //    {
    //        return _provisionService.Query(c => c.Id != id && c.CompanyId == companyId && c.DocNo == docNo).Select().FirstOrDefault();
    //    }
    //    public Provision GetProvision(long CompanyId, string RefDocType)
    //    {
    //        return _provisionService.Query(a => a.CompanyId == CompanyId && a.RefDocType == DocTypeConstants.Invoice).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
    //    }
    //    public Provision GetProvisionByDocNoAndCompanyId(string strNewDocNo, long CompanyId)
    //    {
    //        return _provisionService.Query(a => a.DocNo == strNewDocNo && a.CompanyId == CompanyId).Select().FirstOrDefault();
    //    }

    //}
}
