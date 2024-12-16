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
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
    public class ControlCodeCategoryService : Service<ControlCodeCategory>, IControlCodeCategoryService
    {
        private readonly IReceiptModuleRepositoryAsync<ControlCodeCategory> _controlCodeCategoryRepository;
        private readonly IControlCodeService _controlCodeService;
        public ControlCodeCategoryService(IReceiptModuleRepositoryAsync<ControlCodeCategory> controlCodeCategoryRepository, IControlCodeService controlCodeService)
            : base(controlCodeCategoryRepository)
        {
            _controlCodeCategoryRepository = controlCodeCategoryRepository;
            _controlCodeService = controlCodeService;
        }

        public  LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode, string codeKey)
        {
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Include(c => c.ControlCodes).Select().FirstOrDefault();
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
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
                    lookUpCategory.Lookups = controlcodeCategory.ControlCodes.Where(c => c.CodeKey == codeKey || (c.Status == RecordStatusEnum.Active || c.Status == RecordStatusEnum.Inactive)).Select(x => new LookUp<string>()
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
