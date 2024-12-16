using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using Service.Pattern;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.DebitNoteModule.Service
{
    //public class ProvisionService : Service<Provision>, IProvisionService
    //{
    //    private readonly IDebitNoteMoluleRepositoryAsync<Provision> _provisionRepository;
    //    public ProvisionService(IDebitNoteMoluleRepositoryAsync<Provision> provisionRepository)
    //        : base(provisionRepository)
    //    {
    //        _provisionRepository = provisionRepository;
    //    }
    //    public List<Provision> GetProvisionByInvoiceId(Guid invoiceId)
    //    {
    //        return _provisionRepository.Query(x => x.RefDocumentId == invoiceId).Select().ToList();
    //    }
    //    public Provision GetProvisionById(Guid id)
    //    {
    //        return _provisionRepository.Query(x => x.Id == id).Select().FirstOrDefault();
    //    }
    //    public List<Provision> lstInvoiceProvision(long companyId)
    //    {
    //        return _provisionRepository.Query(c => c.CompanyId == companyId && c.RefDocType == DocTypeConstants.Invoice).Select().ToList();
    //    }
    //    public List<Provision> lstDebitProvision(long companyId)
    //    {
    //        return _provisionRepository.Query(c => c.CompanyId == companyId && c.RefDocType == DocTypeConstants.DebitNote).Select().ToList();
    //    }
    //    public Provision DocNoCheck(Guid id, long companyId, string docNo)
    //    {
    //        return _provisionRepository.Query(c => c.Id != id && c.CompanyId == companyId && c.DocNo == docNo).Select().FirstOrDefault();
    //    }
    //}
}
