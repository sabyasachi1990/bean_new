using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Models;

namespace AppsWorld.CommonModule.Service
{
    public class TaxCodeService : Service<TaxCode>, ITaxCodeService
    {
        private readonly ICommonModuleRepositoryAsync<TaxCode> _taxCodeRepository;

        public TaxCodeService(ICommonModuleRepositoryAsync<TaxCode> taxCodeRepository)
            : base(taxCodeRepository)
        {
            this._taxCodeRepository = taxCodeRepository;

        }
        public List<TaxCode> GetAllTaxCode(long taxId, long companyId, DateTime date)
        {
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Active || a.Id == taxId) && a.CompanyId == companyId).Select().ToList();
        }
        public TaxCode GetTaxCode(long taxId)
        {
            return _taxCodeRepository.Query(x => x.Id == taxId).Select().FirstOrDefault();
        }
        public IEnumerable<TaxCode> GetTaxCodesById(long id)
        {
            return _taxCodeRepository.Queryable().Where(c => c.Id == id).AsEnumerable();
        }
        public List<TaxCode> GetTaxCodes(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }

        public List<TaxCodeLookUp<string>> Listoftaxes(DateTime? date, bool isEdit, long companyId)
        {
            if (isEdit)
            {

                List<TaxCode> lstTaxcodes = _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == 0).Select().ToList();
                var lstgroupTaxcodes = lstTaxcodes.GroupBy(c => c.Code);
                var lstTaxes = Taxcode(lstTaxcodes);
                return lstTaxes;
            }
            else
            {
                List<TaxCode> lstTaxcodes = _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Active) && a.CompanyId == 0).Select().ToList();
                var lstgroupTaxcodes = lstTaxcodes.GroupBy(c => c.Code).ToList();
                List<TaxCodeLookUp<string>> lstTaxes = Taxcode(lstTaxcodes);
                return lstTaxes;
            }
        }
        private List<TaxCodeLookUp<string>> Taxcode(List<TaxCode> TaxCode)
        {
            var lstTaxes = TaxCode.Select(x => new TaxCodeLookUp<string>()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                TaxRate = x.TaxRate,
                TaxType = x.TaxType,
                Status = x.Status,
                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")" */: x.Code
            }).OrderBy(c => c.Code).ToList();
            return lstTaxes;
        }
        public List<TaxCodeLookUp<string>> EditTaxCodes(List<long?> Id)
        {
            return _taxCodeRepository.Queryable().Where(x => Id.Contains(x.Id) && x.CompanyId == 0).Select(x => new TaxCodeLookUp<string>()
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                TaxRate = x.TaxRate,
                TaxType = x.TaxType,
                Status = x.Status,
                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
            }).OrderBy(c => c.Code).ToList();
        }
       
        public string GetTaxCode(long taxId, long companyId)
        {
         
            TaxCode taxCode = _taxCodeRepository.Query(a => a.CompanyId == companyId && a.Id == taxId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
            return taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") /*+ "(" + taxCode.TaxType[0] + ")"*/ : taxCode.Code;
        }
        public async Task<List<TaxCode>> GetListOfTaxCode(long companyId)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select().ToList());
        }
        public List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date)
        {
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == companyId).Select().ToList();
        }

        public async Task<List<TaxCode>> GetTaxAllCodesAsync(long companyId, DateTime? date)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == companyId).Select().ToList());
        }
        public List<TaxCode> GetTaxAllCodesByIds(List<long?> ids)
        {
            return _taxCodeRepository.Queryable().Where(c => ids.Contains(c.Id)).ToList();
        }
        public List<TaxCode> GetTaxAllCodesNew(long companyId, DateTime? date, List<long?> ids)
        {
            if (ids == null)
                return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == companyId).Select().ToList();
            else
                return _taxCodeRepository.Query
                (
                    a =>
                    (
                        (a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && a.Status == RecordStatusEnum.Active)
                        ||
                        (ids.Contains(a.Id))
                    )
                    && a.CompanyId == 0
                ).Select().ToList();
        }
        public long GetTaxID(string code, long companyId)
        {
            return _taxCodeRepository.Query(a => a.Code == "NA" && a.CompanyId == 0).Select(a => a.Id).FirstOrDefault();
        }
        public long GetTaxCodeByName(string code, long companyId)
        {
            return _taxCodeRepository.Query(x => x.Code == code && x.CompanyId == companyId).Select(s => s.Id).FirstOrDefault();
        }
        public Dictionary<long, string> GetTaxCodes(List<long?> taxIds, long companyId)
        {
            //string TaxIdCode = null;
            return _taxCodeRepository.Query(a => a.CompanyId == companyId && taxIds.Contains(a.Id)).Select(c => new { c.Id, c.Code }).ToDictionary(c => c.Id, c => c.Code);
            //return taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") /*+ "(" + taxCode.TaxType[0] + ")"*/ : taxCode.Code;
        }
    }
}
