

using AppsWorld.MasterModule.Entities;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class TaxCodeUpdated : IDomainEvent
    {
        public TaxCode TaxCode { get; private set; }

        public TaxCodeUpdated(TaxCode taxCode)
        {
            TaxCode = taxCode;
        }
    }
}
