using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    //public class SegmentDetailService : Service<SegmentDetail>, ISegmentDetailService
    //{
    //    private readonly IMasterModuleRepositoryAsync<SegmentDetail> _segmentDetailRepository;
    //    public SegmentDetailService(IMasterModuleRepositoryAsync<SegmentDetail> segmentDetailRepository)
    //        : base(segmentDetailRepository)
    //    {
    //        this._segmentDetailRepository = segmentDetailRepository;
    //    }
    //    public List<SegmentDetail> GetSegmentDetails(long segmentMasterId)
    //    {
    //        return _segmentDetailRepository.Query(a => a.Status < RecordStatusEnum.Disable && a.SegmentMasterId == segmentMasterId && a.ParentId == 0).Select().OrderBy(a => a.RecOrder).ToList();
    //    }
    //    public List<SegmentDetail> GetSegmentDetail(long segmentMasterId, long ParentId)
    //    {
    //        return _segmentDetailRepository.Query(a => a.Status < RecordStatusEnum.Disable && a.SegmentMasterId == segmentMasterId && a.ParentId == ParentId).Select().OrderBy(a => a.RecOrder).ToList();
    //    }
    //    public SegmentDetail Getby(long Id, long SegmentMasterId, string Name, long ParentId)
    //    {
    //        if (Id != -1)
    //        {
    //            //return _segmentDetailRepository.Query(e => e.Id != Id && e.SegmentMasterId == SegmentMasterId && e.Name == Name && e.ParentId == ParentId).Select().FirstOrDefault();
    //            var test = _segmentDetailRepository.Query(c => (Id != c.Id &&Id==c.Id) && SegmentMasterId == c.SegmentMasterId && Name == c.Name && ParentId == c.ParentId).Select().FirstOrDefault();
    //            return test;
    //        }
    //        else
    //            return _segmentDetailRepository.Query(e => e.Id == Id && e.SegmentMasterId == SegmentMasterId && e.Name == Name && e.ParentId == ParentId).Select().FirstOrDefault();
    //        //return _segmentDetailRepository.Queryable().Where(e => e.Id != Id && e.SegmentMasterId == SegmentMasterId && e.Name == Name && e.ParentId == ParentId).FirstOrDefault();
    //    }
    //    public SegmentDetail getByHuge(string Name, long ParentId, long SegmentMasterId)
    //    {
    //        return _segmentDetailRepository.Query(e => e.Name == Name && e.ParentId == ParentId && e.SegmentMasterId == SegmentMasterId && e.Status == RecordStatusEnum.Delete).Select().FirstOrDefault();
    //    }
    //    public SegmentDetail GetbyNameandParentId(long Id, long SegmentMasterId, long ParentId, string Name)
    //    {
    //        return _segmentDetailRepository.Query(e => e.Id != Id && e.SegmentMasterId == SegmentMasterId && e.ParentId == ParentId && e.Name == Name).Select().FirstOrDefault();
    //    }
    //    public SegmentDetail GetByIdAndCidAndParentId(string Name, long ParentId, long SegmentMasterId)
    //    {
    //        return _segmentDetailRepository.Query(e => e.Name == Name && e.ParentId == ParentId && e.SegmentMasterId == SegmentMasterId && e.Status == RecordStatusEnum.Delete).Select().FirstOrDefault();
    //    }
    //    public SegmentDetail GetById(long Id)
    //    {
    //        return _segmentDetailRepository.Query(e => e.Id == Id).Select().FirstOrDefault();
    //    }

    //}
}
