using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
namespace AppsWorld.PaymentModule.Service
{
    public interface ICompanyFeatureService : IService<CompanyFeature>
    {
        List<CompanyFeature> GetCompanyFeature(long companyId);
    }
}
