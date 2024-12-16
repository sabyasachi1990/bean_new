using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class GeneralSettingFeatureModel
    {
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsFinancial { get; set; }
        public bool? IsRevaluation { get; set; }
        public bool? IsNoSupportingDocuments { get; set; }
        public DateTime? PeriodLockStartDate { get; set; }
        public DateTime? PeriodLockEndDate { get; set; }
        public string PeriodLockDatePassword { get; set; }
        public List<CompanyFeaturesModel> Feature { get; set; }
    }
}
