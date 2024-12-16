using Service.Pattern;
using System.Collections.Generic;
using System;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class TaxCodeService : Service<TaxCode>, ITaxCodeService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<TaxCode> _taxCodeRepository;

        public TaxCodeService(IJournalVoucherModuleRepositoryAsync<TaxCode> taxRepository)
            : base(taxRepository)
        {
            _taxCodeRepository = taxRepository;
        }

        public List<TaxCode> GetTaxCodeEdit(long id, long CompanyId, DateTime date)
        {
            return _taxCodeRepository.Queryable().Where(c => c.EffectiveFrom <= date && (c.EffectiveTo >= date || c.EffectiveTo == null) && (c.Status == RecordStatusEnum.Active || c.Status == RecordStatusEnum.Inactive || c.Id == id) && c.CompanyId == CompanyId).ToList();
        }
        public List<TaxCode> GetTaxCodeNew(long CompanyId, DateTime date)
        {
            return _taxCodeRepository.Queryable().Where(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).ToList();
        }
        public TaxCode GetTaxById(long? Id)
        {
            return _taxCodeRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }

        public TaxCode GetTaxiId(long? Id)
        {
            return _taxCodeRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }
        public TaxCode GetTaxId(long? Id)
        {
            return _taxCodeRepository.Query(c => c.Id == Id && c.Code != "NA").Select().FirstOrDefault();
        }
        public List<TaxCode> GetListOfTaxCode(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
        public async Task<List<TaxCode>> GetTaxCodes(long companyId)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.CompanyId == companyId && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active)).Select().ToList());
        }
        public async Task<List<TaxCode>> GetTaxAllCodes(long companyId, DateTime? date)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == companyId).Select().ToList());
        }
        public List<TaxCode> GetTaxCodesByIds(List<long> Ids)
        {
            return _taxCodeRepository.Query(a => Ids.Contains(a.Id)).Select().ToList();
        }

        public long GetTaxId(string code, long companyId)
        {
            return _taxCodeRepository.Query(a => a.Code == code && a.CompanyId == companyId).Select(a => a.Id).FirstOrDefault();
        }
    }
}
