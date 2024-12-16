using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
   //public class SegmentMasterService:Service<SegmentMaster>,ISegmentMasterService
   // {
   //    private readonly IBillModuleRepositoryAsync<SegmentMaster> _segmentMasterRepository;
   //    public SegmentMasterService(IBillModuleRepositoryAsync<SegmentMaster> segmentMasterRepository):base(segmentMasterRepository)
   //    {
   //        _segmentMasterRepository = segmentMasterRepository;
   //    }
   //     LookUpCategory<string> ISegmentMasterService.GetSegmentsEdit(long CompanyId, long? id)
   //     {
   //         var _segmentMasterReg = _segmentMasterRepository.Query().Include(x => x.SegmentDetails).Select().Where(a => a.CompanyId == CompanyId && a.Id == id).OrderBy(a => a.CreatedDate).ToList();
   //         LookUpCategory<string> lookUpCategory = new LookUpCategory<string>();
   //         foreach (var segmentmaster in _segmentMasterReg)
   //         {
   //             var lookUpCategorySingle = new LookUpCategory<string>();
   //             lookUpCategorySingle.Id = segmentmaster.Id;
   //             lookUpCategorySingle.CategoryName = segmentmaster.Name;
   //             lookUpCategorySingle.Lookups = segmentmaster.SegmentDetails.Where(a => a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select(x => new LookUp<string>()
   //             {
   //                 Id = x.Id,
   //                 Name = x.Name,
   //                 ParentId = x.ParentId,
   //                 RecOrder = x.RecOrder
   //             }).ToList();

   //             lookUpCategory = lookUpCategorySingle;
   //         }
   //         return lookUpCategory;
   //     }
   //     public IEnumerable<SegmentMaster> GetSegmentMastersById(long id)
   //     {
   //         return _segmentMasterRepository.Queryable().Where(x => x.Id == id).AsEnumerable();
   //     }

   //     List<LookUpCategory<string>> ISegmentMasterService.GetSegments(long CompanyId)
   //     {
   //         var _segmentMasterReg = _segmentMasterRepository.Query().Include(x => x.SegmentDetails).Select().Where(a => a.CompanyId == CompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).OrderBy(a => a.CreatedDate).ToList();
   //         List<LookUpCategory<string>> lookUpCategory = new List<LookUpCategory<string>>();
   //         foreach (var segmentmaster in _segmentMasterReg)
   //         {
   //             var lookUpCategorySingle = new LookUpCategory<string>();
   //             lookUpCategorySingle.Id = segmentmaster.Id;
   //             lookUpCategorySingle.CategoryName = segmentmaster.Name;
   //             lookUpCategorySingle.Lookups = segmentmaster.SegmentDetails.Where(a => a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select(x => new LookUp<string>()
   //             {
   //                 Id = x.Id,
   //                 Name = x.Name,
   //                 ParentId = x.ParentId,
   //                 RecOrder = x.RecOrder
   //             }).ToList();

   //             lookUpCategory.Add(lookUpCategorySingle);
   //         }
   //         return lookUpCategory;
   //     }
   // }
}
