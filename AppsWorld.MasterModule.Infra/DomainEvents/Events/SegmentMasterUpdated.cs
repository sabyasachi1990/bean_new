
using AppsWorld.MasterModule.Models;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class SegmentMasterUpdated : IDomainEvent
    {
        public SegmentMasterDTO SegmentMaster{ get; private set; }

        public SegmentMasterUpdated(SegmentMasterDTO segmentMaster)
        {
            SegmentMaster = segmentMaster;
        }
    }
}
