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
    //public class SegmentMasterService : Service<SegmentMaster>, ISegmentMasterService
    //{
    //    private readonly IMasterModuleRepositoryAsync<SegmentMaster> _segmentMasterRepository;
    //    public SegmentMasterService(IMasterModuleRepositoryAsync<SegmentMaster> segmentMasterRepository)
    //        : base(segmentMasterRepository)
    //    {
    //        this._segmentMasterRepository = segmentMasterRepository;
    //    }
    //    public IEnumerable<SegmentMaster> GetAllSegmentMasters(long CompanyId)
    //    {
    //        return _segmentMasterRepository.Queryable().Where(c => c.CompanyId == CompanyId).OrderBy(a => a.RecOrder).AsEnumerable();
    //    }
    //    public SegmentMaster GetByCidAndId(long Id, long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.Id == Id && e.CompanyId == CompanyId).Select().FirstOrDefault();
    //    }
    //    public List<SegmentMaster> GetByCidAndname(string Name, long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.Name == Name && e.CompanyId == CompanyId && e.Status == RecordStatusEnum.Active).Select().ToList();
    //    }
    //    public List<SegmentMaster> GetByIdAndNameAndCid(long Id, string Name, long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.Id == Id && e.Name == Name && e.CompanyId == CompanyId && e.Status == RecordStatusEnum.Active).Select().ToList();
    //    }
    //    public List<SegmentMaster> GetSegmentS(long Id, long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.CompanyId == CompanyId && e.Status == RecordStatusEnum.Active && e.Id != Id).Select().ToList();
    //    }
    //    public List<SegmentMaster> GetByCid(long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.CompanyId == CompanyId && e.Status == RecordStatusEnum.Active).Select().ToList();
    //    }
    //    public int GetAllSegmentMaster(long companyId)
    //    {
    //        return _segmentMasterRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select().Count();
    //    }
    //    public List<SegmentMaster> GetAllSegmentName(string Name, long CompanyId)
    //    {
    //        return _segmentMasterRepository.Query(e => e.Name == Name && e.CompanyId == CompanyId&&e.Status==RecordStatusEnum.Inactive).Select().ToList();
    //    }
    //}
}
