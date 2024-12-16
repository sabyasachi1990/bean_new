
using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class TaxCodeCreated : IDomainEvent
    {
        public TaxCode TaxCode { get; private set; }

        public TaxCodeCreated(TaxCode taxCode)
        {
            TaxCode = taxCode;
        }
    }
}
