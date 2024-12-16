
using AppsWorld.MasterModule.Models;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class SegmentMasterCreated : IDomainEvent
    {
        public SegmentMasterDTO SegmentMaster{ get; private set; }

        public SegmentMasterCreated(SegmentMasterDTO segmentMaster)
        {
            SegmentMaster = segmentMaster;
        }
    }
}
