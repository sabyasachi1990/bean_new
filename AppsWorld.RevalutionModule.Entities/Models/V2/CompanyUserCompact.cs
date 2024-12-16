using Repository.Pattern.Ef6;

namespace AppsWorld.RevaluationModule.Entities.V2
{
    public partial class CompanyUserCompact : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
        public string ServiceEntities { get; set; }
    }
}
