using System.ComponentModel.DataAnnotations;
using AppsWorld.Framework;
using Newtonsoft.Json;
using FrameWork;
using Newtonsoft.Json.Converters;
using Repository.Pattern.Ef6;

namespace AppsWorld.CashSalesModule.Entities.V2
{
    public partial class BeanEntityK : Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
         
    }
}
