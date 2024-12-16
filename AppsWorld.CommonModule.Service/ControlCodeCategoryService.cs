using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public class ControlCodeCategoryService : Service<ControlCodeCategory>, IControlCodeCategoryService
    {
        private readonly ICommonModuleRepositoryAsync<ControlCodeCategory> _controlCodeCategoryRepository;
        private readonly IControlCodeService _controlCodeService;

        public ControlCodeCategoryService(ICommonModuleRepositoryAsync<ControlCodeCategory> controlCodeCategoryRepository, IControlCodeService controlCodeService)
            : base(controlCodeCategoryRepository)
        {
            _controlCodeCategoryRepository = controlCodeCategoryRepository;
            _controlCodeService = controlCodeService;
        }

        public LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                List<ControlCode> lstCont = controlcodeCategory.ControlCodeCategoryCode == "ModeOfTransfer" ? _controlCodeService.GetControlCodeByCategoryId(controlcodeCategory.Id) : _controlCodeService.GetControlCodesByCatId(controlcodeCategory.Id);
                lookUpCategory.Lookups = lstCont.Select(x => new LookUp<string>()
                //lookUpCategory.Lookups = _controlCodeRepository.Query(a => a.ControlCategoryId == controlcodeCategory.Id).Select(x => new LookUp<string>()
                {
                    Code = x.CodeKey,
                    Name = x.CodeValue,
                    Id = x.Id,
                    RecOrder = x.RecOrder
                }).OrderBy(x => x.RecOrder).ToList();
            }
            return lookUpCategory;
        }

        public async Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode)
        {
            var controlcodeCategory = await Task.Run(() => _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault());
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                List<ControlCode> lstCont = controlcodeCategory.ControlCodeCategoryCode == "ModeOfTransfer" ? await _controlCodeService.GetControlCodeByCategoryIdAsync(controlcodeCategory.Id) : await _controlCodeService.GetControlCodesByCatIdAsync(controlcodeCategory.Id);
                lookUpCategory.Lookups = lstCont.Select(x => new LookUp<string>()
                {
                    Code = x.CodeKey,
                    Name = x.CodeValue,
                    Id = x.Id,
                    RecOrder = x.RecOrder
                }).OrderBy(x => x.RecOrder).ToList();
            }
            return lookUpCategory;
        }
        public LookUp<string> GetInactiveControlcode(long CompanyId, string CategoryCode, string controlCodeKey)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select().FirstOrDefault();
            if (controlcodeCategory != null)
            {
                var controlCode =
                    _controlCodeService.Query(
                            a => a.ControlCategoryId == controlcodeCategory.Id && a.Status == AppsWorld.Framework.RecordStatusEnum.Inactive && a.CodeKey == controlCodeKey)
                        .Select()
                        .FirstOrDefault();
                if (controlCode != null)
                {
                    LookUp<string> lookUp = new LookUp<string>();
                    var lu = this.GetByCategoryCodeCategory(CompanyId, CategoryCode).Lookups.Where(a => a.Id == controlCode.Id).FirstOrDefault();
                    if (lu == null)
                    {
                        lookUp.Code = controlCode.CodeKey;
                        lookUp.Name = controlCode.CodeValue;
                        lookUp.Id = controlCode.Id;
                        lookUp.RecOrder = controlCode.RecOrder;
                        this.GetByCategoryCodeCategory(CompanyId, CategoryCode).Lookups.Add(lookUp);
                    }
                    return lookUp;
                }
            }
            return null;
        }
        public LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode, string codeKey)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault();
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                List<ControlCode> lstCont = _controlCodeService.GetControlCodesByCatId(controlcodeCategory.Id);
                if (codeKey == string.Empty)
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    //lookUpCategory.Lookups = _controlCodeRepository.Query(a => a.ControlCategoryId == controlcodeCategory.Id).Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        Status = x.Status,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
                else
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.CodeKey == codeKey || c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    //lookUpCategory.Lookups = _controlCodeRepository.Query(a => a.ControlCategoryId == controlcodeCategory.Id).Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        Status = x.Status,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
            }
            return lookUpCategory;
        }

        public async Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode, string codeKey)
        {
            var controlcodeCategory = await Task.Run(()=> _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault());
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                List<ControlCode> lstCont = await _controlCodeService.GetControlCodesByCatIdAsync(controlcodeCategory.Id);
                if (codeKey == string.Empty)
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        Status = x.Status,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
                else
                {
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.CodeKey == codeKey || c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    {
                        Code = x.CodeKey,
                        Name = x.CodeValue,
                        Id = x.Id,
                        Status = x.Status,
                        RecOrder = x.RecOrder
                    }).OrderBy(x => x.RecOrder).ToList();
                }
            }
            return lookUpCategory;
        }
    }
}
