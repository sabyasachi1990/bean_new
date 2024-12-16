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
    public class ControlCodeCategoryService : Service<ControlCodeCategory>, IControlCodeCategoryService
    {
        private readonly IBillModuleRepositoryAsync<ControlCodeCategory> _controlCodeCategoryRepository;
		private readonly IControlCodeService _controlCodeService;
        public ControlCodeCategoryService(IBillModuleRepositoryAsync<ControlCodeCategory> controlCodeCategoryRepository, IControlCodeService controlCodeService)
			: base(controlCodeCategoryRepository)
        {
			_controlCodeCategoryRepository = controlCodeCategoryRepository;
			_controlCodeService = controlCodeService;
        }

		public LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode)
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

	    public ControlCodeCategory GetByCategoryCode(long CompanyId, string CategoryCode)
	    {
		    return
			    _controlCodeCategoryRepository.Query(c => c.CompanyId == CompanyId && c.ControlCodeCategoryCode == CategoryCode)
				    .Select()
				    .FirstOrDefault();
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



    }
}
