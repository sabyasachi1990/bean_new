using Repository.Pattern.Ef6;


namespace AppsWorld.CashSalesModule.Entities.V2
{
    public partial class ChartOfAccountK : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
    }
}
