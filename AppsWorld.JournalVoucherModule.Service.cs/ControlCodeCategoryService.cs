using Service.Pattern;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.CommonModule.Infra;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ControlCodeCategoryService : Service<ControlCodeCategory>, IControlCodeCategoryService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<ControlCodeCategory> _controlCodeCategoryRepository;
		private readonly IControlCodeService _controlCodeService;
        public ControlCodeCategoryService(IJournalVoucherModuleRepositoryAsync<ControlCodeCategory> controlCodeCategoryRepository, IControlCodeService controlCodeService)
			: base(controlCodeCategoryRepository)
        {
			_controlCodeCategoryRepository = controlCodeCategoryRepository;
			_controlCodeService = controlCodeService;
        }

		public  LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode)
		{
            var controlcodeCategory = _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status <= AppsWorld.Framework.RecordStatusEnum.Disable).Select().FirstOrDefault();
			var lookUpCategory = new LookUpCategory<string>();
			if (controlcodeCategory != null)
			{
				lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
				lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
				lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
				lookUpCategory.Id = controlcodeCategory.Id;
				List<ControlCode> lstCont = _controlCodeService.GetControlCodesByCatId(controlcodeCategory.Id);
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
        public async Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode)
        {
            var controlcodeCategory = await Task.Run(()=> _controlCodeCategoryRepository.Query(a => a.CompanyId == CompanyId && a.ControlCodeCategoryCode == CategoryCode && a.Status <= AppsWorld.Framework.RecordStatusEnum.Disable).Select().FirstOrDefault());
            var lookUpCategory = new LookUpCategory<string>();
            if (controlcodeCategory != null)
            {
                lookUpCategory.CategoryName = controlcodeCategory.ControlCodeCategoryDescription;
                lookUpCategory.DefaultValue = controlcodeCategory.DefaultValue;
                lookUpCategory.Code = controlcodeCategory.ControlCodeCategoryCode;
                lookUpCategory.Id = controlcodeCategory.Id;
                List<ControlCode> lstCont = await _controlCodeService.GetControlCodesByCatIdAsync(controlcodeCategory.Id);
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
    }
}
