using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.ReceiptModule.Service
{
    public class TaxCodeService : Service<TaxCode>, ITaxCodeService
    {
        private readonly IReceiptModuleRepositoryAsync<TaxCode> _taxCodeRepository;

        public TaxCodeService(IReceiptModuleRepositoryAsync<TaxCode> taxRepository)
            : base(taxRepository)
        {
            _taxCodeRepository = taxRepository;
        }

        public List<TaxCode> GetTaxCodeEdit(long? id, long CompanyId, DateTime date)
        {
            CompanyId = 0;
            return _taxCodeRepository.Queryable().Where(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive || a.Id == id) && a.CompanyId == CompanyId).ToList();
        }
        public List<TaxCode> GetTaxCodeNew(long CompanyId, DateTime date)
        {
            CompanyId = 0;
            return _taxCodeRepository.Queryable().Where(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).ToList();
        }

        public TaxCode GetTaxCode(long id)
        {
            return _taxCodeRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public List<TaxCode> GetTaxCodes(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
    }
}
