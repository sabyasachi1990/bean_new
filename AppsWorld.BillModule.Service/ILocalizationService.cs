using AppsWorld.BillModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public interface ILocalizationService:IService<Localization>
    {
        string GetLocalization(long companyId);

    }
}
