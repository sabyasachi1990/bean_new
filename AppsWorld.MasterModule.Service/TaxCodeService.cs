using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using AppsWorld.MasterModule.Models;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.MasterModule.Service
{
    public class TaxCodeService : Service<TaxCode>, ITaxCodeService
    {
        private readonly IMasterModuleRepositoryAsync<TaxCode> _taxCodeRepository;
        public TaxCodeService(IMasterModuleRepositoryAsync<TaxCode> taxCodeRepository)
            : base(taxCodeRepository)
        {
            _taxCodeRepository = taxCodeRepository;
        }


        public IQueryable<TaxCodeModelK> GetAllTaxCodeModelsK(long companyId, string jur)
        {
            if (companyId != 0)
            {
                List<TaxCode> taxCodes = _taxCodeRepository.Queryable().Where(x => x.CompanyId == 0 || x.CompanyId == companyId).ToList();
                IQueryable<TaxCode> taxZero = taxCodes.Where(x => x.CompanyId == 0).AsQueryable();
                IQueryable<TaxCode> taxComp = taxCodes.Where(x => x.CompanyId == companyId).AsQueryable();
                return (from taxcode in taxZero
                        join tax in taxComp
                        on taxcode.Code equals tax.Code into eGrp
                        from d in eGrp.DefaultIfEmpty()
                        select new TaxCodeModelK()
                        {
                            Id = d != null ? d.Id : taxcode.Id,
                            CompanyId = d != null ? d.CompanyId : taxcode.CompanyId,
                            Name = d != null ? d.Name : taxcode.Name,
                            Code = d != null ? d.Code : taxcode.Code,
                            AppliesTo = d != null ? d.AppliesTo : taxcode.AppliesTo,
                            TaxType = d != null ? d.TaxType : taxcode.TaxType,
                            TaxRate = d != null ? d.TaxRate : taxcode.TaxRate,
                            EffectiveFrom = d != null ? d.EffectiveFrom : taxcode.EffectiveFrom,
                            UserCreated = d != null ? d.UserCreated : taxcode.UserCreated,
                            CreatedDate = d != null ? d.CreatedDate : taxcode.CreatedDate,
                            ModifiedBy = d != null ? d.ModifiedBy : taxcode.ModifiedBy,
                            Applicable = d != null ? d.IsApplicable == true ? "Appl" : "N/A" : taxcode.IsApplicable == true ? "Appl" : "N/A",
                            ModifiedDate = d != null ? d.ModifiedDate : taxcode.ModifiedDate,
                            Version = d != null ? d.Version : taxcode.Version,
                            TaxcodeStatus = d != null ? d.Status : taxcode.Status == RecordStatusEnum.Active ? RecordStatusEnum.Inactive : RecordStatusEnum.Inactive,
                            Status = (d != null ? d.Status : taxcode.Status == RecordStatusEnum.Active ? RecordStatusEnum.Inactive : RecordStatusEnum.Inactive).ToString(),
                            Description = d != null ? d.Description : taxcode.Description,
                            IsSeedData = d != null ? d.IsSeedData : taxcode.IsSeedData,
                            Jurisdiction = d != null ? d.Jurisdiction : taxcode.Jurisdiction
                        }).Where(x => x.Jurisdiction == jur).AsQueryable().OrderBy(c => c.CreatedDate);
            }
            else
            {
                List<TaxCode> taxCodes = _taxCodeRepository.Queryable().Where(x => x.CompanyId == 0).ToList();
                return (from tax in taxCodes
                        select new TaxCodeModelK()
                        {
                            Id = tax.Id,
                            CompanyId = tax.CompanyId,
                            Name = tax.Name,
                            Code = tax.Code,
                            AppliesTo = tax.AppliesTo,
                            TaxRate = tax.TaxRate,
                            TaxType = tax.TaxType,
                            EffectiveFrom = tax.EffectiveFrom,
                            UserCreated = tax.UserCreated,
                            CreatedDate = tax.CreatedDate,
                            ModifiedDate = tax.ModifiedDate,
                            ModifiedBy = tax.ModifiedBy,
                            Applicable = tax.IsApplicable == true ? "Appl":"N/A",
                            Version = tax.Version,
                            Status = (tax.Status).ToString(),
                            TaxcodeStatus = tax.Status,
                            Description = tax.Description,
                            IsSeedData = tax.IsSeedData,
                            Jurisdiction = tax.Jurisdiction
                        }).AsQueryable().OrderBy(c => c.CreatedDate); 
            }
        }

        public IEnumerable<TaxCode> GetAllCompanyById(long id, long CompanyId)
        {
            return _taxCodeRepository.Query(e => e.Id == id && e.CompanyId == CompanyId).Select();
        }
        public IEnumerable<TaxCode> GetAllTaxCodeCodeAndCompanyId(string code, DateTime Datetime, long companyId)
        {
            return _taxCodeRepository.Query(e => e.Code == code && e.EffectiveFrom == Datetime && e.CompanyId == companyId).Select().AsQueryable();//Change by Sreenivas
        }
        public IEnumerable<TaxCode> GetAllTaxcodeCodeAndCompanyId(long Id, string Code, long CompanyId)
        {
            return _taxCodeRepository.Query(e => e.Id == Id && e.Code == Code && e.CompanyId == CompanyId).Select();
        }
        public List<TaxCode> GetAllTaxCodeByCompany(long companyId)
        {
            return _taxCodeRepository.Queryable().Where(c => c.CompanyId == companyId).ToList();
        }
        public TaxCode GetByTaxId(long? Id)
        {
            return _taxCodeRepository.Query(t => t.Id == Id).Select().FirstOrDefault();
        }

        public List<LookUpCategory<string>> GetTaxCodeByIdandCid(long CompanyId, long? id)
        {
            CompanyId = 0;
            var _taxCode = _taxCodeRepository.Query().Select().Where(a => a.CompanyId == CompanyId && (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive)/*&& a.Id == id*/).ToList();
            List<LookUpCategory<string>> lookupcategory = new List<LookUpCategory<string>>();
            if (_taxCode.Any())
            {
                var lookUpCategorySingle = new LookUpCategory<string>();
                lookUpCategorySingle.CategoryName = "Default Tax Code";
                lookUpCategorySingle.Lookups = _taxCode.Select(x => new LookUp<string>()
                {
                    Id = x.Id,
                    Code = (x.Code + "-" + x.Name),
                    TaxCode = x.Code,
                    //Code = x.Code,
                    Name = x.Name,
                    RecOrder = x.RecOrder
                }).AsEnumerable().OrderBy(x => x.Name).Distinct().ToList();
                lookupcategory.Add(lookUpCategorySingle);
            }
            return lookupcategory;
        }
        public List<LookUpCategory<string>> GetById(long CompanyId)
        {
            CompanyId = 0;
            var _taxCode = _taxCodeRepository.Query().Select().Distinct().Where(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).ToList();
            List<LookUpCategory<string>> lookUpCategory2 = new List<LookUpCategory<string>>();
            if (_taxCode.Any())
            {
                var lookUpCategorySingle = new LookUpCategory<string>();
                lookUpCategorySingle.CategoryName = "Default Tax Code";
                lookUpCategorySingle.Lookups = _taxCode.Select(x => new LookUp<string>()
                {
                    Id = x.Id,
                    Code = (x.Code + "-" + x.Name),
                    TaxCode = x.Code,
                    //Code = x.Code,
                    Name = x.Name,
                    RecOrder = x.RecOrder
                }).AsEnumerable().OrderBy(x => x.Name).ToList();
                lookUpCategorySingle.Lookups = lookUpCategorySingle.Lookups.GroupBy(c => c.Name).Select(x => x.First()).ToList();
                lookUpCategory2.Add(lookUpCategorySingle);
            }
            return lookUpCategory2.ToList();
        }
        public IEnumerable<TaxCode> GetCodes(string code)
        {
            return _taxCodeRepository.Queryable().Where(x => x.Code == code).ToList();
        }
        public List<TaxCodeLookUp<string>> GetAllTaxCode(long companyId)
        {
            List<TaxCodeLookUp<string>> TaxCodeLu = new List<TaxCodeLookUp<string>>();
            List<TaxCode> lstTaxCode = _taxCodeRepository.Queryable().Where(c => c.CompanyId == companyId).ToList();
            if (lstTaxCode.Any())
            {
                TaxCodeLu = lstTaxCode.Select(x => new TaxCodeLookUp<string>
                {
                    Id = x.Id,
                    Code = (x.Code + "-" + x.Name),
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    RecOrder = x.RecOrder
                }).OrderBy(a => a.Name).ToList();
            }
            return TaxCodeLu;
        }
        public string GetTaxCode(long taxId, long companyId)
        {
            //string TaxIdCode = null;
            TaxCode taxCode = _taxCodeRepository.Query(a => a.CompanyId == companyId && a.Id == taxId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
            return taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") + "(" + taxCode.TaxType[0] + ")" : taxCode.Code;
        }
        public List<EntityTaxCodeLookUp<string>> GetAllTaxCodes(long companyId, bool isEdit)
        {
            List<TaxCode> lstTaxCode = new List<TaxCode>();
            List<EntityTaxCodeLookUp<string>> TaxCodeLu = new List<EntityTaxCodeLookUp<string>>();
            if (isEdit)
                lstTaxCode = _taxCodeRepository.Queryable().Where(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).ToList();
            else
                lstTaxCode = _taxCodeRepository.Queryable().Where(c => c.CompanyId == companyId).ToList();
            var groupByTaxcode = lstTaxCode.GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() });
            groupByTaxcode = groupByTaxcode.ToList();
            if (lstTaxCode.Any())
            {
                TaxCodeLu = groupByTaxcode.Select(x => new EntityTaxCodeLookUp<string>
                {
                    Id = x.lstTax.Id,
                    Code = x.code + "-" + x.lstTax.Name,
                    Name = x.lstTax.Name,
                    TaxRate = x.lstTax.TaxRate,
                    TaxType = x.lstTax.TaxType,
                    Status = x.lstTax.Status,
                    TaxIdCode = x.code != "NA" ? x.code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") + "(" + x.lstTax.TaxType[0] + ")" : x.code,
                    TaxCode = x.code
                }).OrderBy(a => a.Name).ToList();
            }
            return TaxCodeLu;
        }

        public List<TaxCode> GetTaxCode(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select().ToList();
        }

        public async Task<List<TaxCode>> GetTaxCodeAsync(long companyId)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select().ToList());
        }
        public List<EntityTaxCodeLookUp<string>> GetAllTaxCodesBydocDate(long companyId, DateTime? docDate)
        {
            List<EntityTaxCodeLookUp<string>> TaxCodeLu = new List<EntityTaxCodeLookUp<string>>();
            List<TaxCode> lstTaxCode = _taxCodeRepository.Query(c => c.CompanyId == companyId && (c.EffectiveFrom <= docDate && c.EffectiveTo >= docDate || c.EffectiveTo == null) && c.Status == RecordStatusEnum.Active).Select().ToList();
            var groupByTaxcode = lstTaxCode.GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() });
            groupByTaxcode = groupByTaxcode.ToList();
            if (lstTaxCode.Any())
            {
                TaxCodeLu = groupByTaxcode.Select(x => new EntityTaxCodeLookUp<string>
                {
                    Id = x.lstTax.Id,
                    Code = x.code + "-" + x.lstTax.Name,
                    Name = x.lstTax.Name,
                    TaxRate = x.lstTax.TaxRate,
                    TaxType = x.lstTax.TaxType,
                    Status = x.lstTax.Status,
                    TaxIdCode = x.code != "NA" ? x.code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.code,
                    TaxCode = x.code
                }).OrderBy(a => a.Name).ToList();
            }
            return TaxCodeLu;
        }
        public List<TaxCode> GetTaxCodes(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
        public async Task<List<TaxCode>> GetTaxCodesAsync(long companyId)
        {
            return await Task.Run(()=> _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList());
        }
        public List<TaxCode> GetTaxAllCodes(long companyId, DateTime? date)
        {
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == 0).Select().ToList();
        }
    }
}
