using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IControlCodeCategoryService : IService<ControlCodeCategory>
    {
        LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode);
        LookUp<string> GetInactiveControlcode(long CompanyId, string CategoryCode,string controlCodeKey);
        ControlCodeCategory GetcontrolCodecategory(long companyId, string control_Codes_CommunicationType);

        FrameWork.LookUpCategory<string> GetByCategoryCodeCategory1(long CompanyId, string CategoryCode);

        Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long companyId, string categoryCode);

        Task<FrameWork.LookUpCategory<string>> GetByCategoryCodeCategory1Asnc(long companyId, string CategoryCode);

        Task<LookUp<string>> GetInactiveControlCodeAsync(long companyId, string categoryCode, string controlCodeKey);
    }
}
