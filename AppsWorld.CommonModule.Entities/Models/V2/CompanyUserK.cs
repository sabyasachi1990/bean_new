using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities.V2
{
    public partial class CompanyUserK : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Username { get; set; }
        public string ServiceEntities { get; set; }
    }
}
