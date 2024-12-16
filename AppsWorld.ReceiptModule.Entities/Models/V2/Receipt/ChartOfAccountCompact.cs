using Repository.Pattern.Ef6;

namespace AppsWorld.ReceiptModule.Entities.Models.V2.Receipt
{
    public partial class ChartOfAccountCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public long AccountTypeId { get; set; }
        public long? SubsidaryCompanyId { get; set; }
    }
}
