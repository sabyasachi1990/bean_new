using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
    public class TaxCodeService : Service<TaxCode>, ITaxCodeService
    {
        private readonly IBillModuleRepositoryAsync<TaxCode> _taxCodeRepository;

        public TaxCodeService(IBillModuleRepositoryAsync<TaxCode> taxRepository)
			: base(taxRepository)
        {
			_taxCodeRepository = taxRepository;
        }

		public List<TaxCode> GetTaxCodeEdit(long id, long companyId,DateTime date)
		{
			companyId = 0;
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Active || a.Id == id) && a.CompanyId == companyId).Select().ToList();
		}
		public List<TaxCode> GetTaxCodeNew(long CompanyId,DateTime date)
		{
			CompanyId = 0;
			return _taxCodeRepository.Queryable().Where(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null)&& a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).ToList();
		}
        public TaxCode GetTaxById(long? Id)
        {
            return _taxCodeRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }

        public TaxCode GetTaxiId(long? Id)
        {
            return _taxCodeRepository.Query(c => c.Id == Id).Select().FirstOrDefault();
        }
        public TaxCode GetTaxCode(long taxId)
        {
            return _taxCodeRepository.Query(x => x.Id == taxId).Select().FirstOrDefault();
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
                var lstTaxes = Taxcode(lstTaxcodes);
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
                Status=x.Status,
                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
            }).OrderBy(c => c.Code).ToList();
            return lstTaxes;
        }
        public List<TaxCode> GetAllTaxs(long companyId)
        {
            companyId = 0;
            return _taxCodeRepository.Query(c => c.CompanyId == companyId).Select().ToList();
        }
        public TaxCode GetTaxByCode(string taxCode)
        {
            return _taxCodeRepository.Query(c => c.Code == taxCode).Select().FirstOrDefault();
        }
        public List<TaxCode> GetTaxCodes(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
        public List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date)
        {
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == companyId).Select().ToList();
        }
        public List<TaxCode> GetTaxAllCodesByIds(List<long?> ids)
        {
            return _taxCodeRepository.Queryable().Where(c=>ids.Contains(c.Id)).ToList();
        }
    }
}
