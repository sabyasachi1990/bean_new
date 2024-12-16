using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
  //  public class SegmentMasterService:Service<SegmentMaster>,ISegmentMasterService
  //  {
  //     private readonly IJournalVoucherModuleRepositoryAsync<SegmentMaster> _segmentMasterRepository;
  //     public SegmentMasterService(IJournalVoucherModuleRepositoryAsync<SegmentMaster> segmentMasterRepository):base(segmentMasterRepository)
  //     {
  //         _segmentMasterRepository = segmentMasterRepository;
  //     }
  //     LookUpCategory<string> ISegmentMasterService.GetSegmentsEdit(long CompanyId, long? id)
  //     {
  //         var _segmentMasterReg = _segmentMasterRepository.Query().Include(x => x.SegmentDetails).Select().Where(a => a.CompanyId == CompanyId && a.Id == id).OrderBy(a => a.CreatedDate).FirstOrDefault();
  //         var lookUpCategorySingle = new LookUpCategory<string>();
  //         if (_segmentMasterReg != null)
  //         {
  //             lookUpCategorySingle.Id = _segmentMasterReg.Id;
  //             lookUpCategorySingle.CategoryName = _segmentMasterReg.Name;
  //             lookUpCategorySingle.Lookups = _segmentMasterReg.SegmentDetails.Where(a => a.Status == AppsWorld.Framework.RecordStatusEnum.Active).Select(x => new LookUp<string>()
  //             {
  //                 Id = x.Id,
  //                 Name = x.Name,
  //                 ParentId = x.ParentId,
  //                 RecOrder = x.RecOrder
  //             }).ToList();
  //         }
  //         return lookUpCategorySingle;
  //     }
  //     public IEnumerable<SegmentMaster> GetSegmentMastersById(long id)
  //      {
  //          return _segmentMasterRepository.Queryable().Where(x => x.Id == id).AsEnumerable();
  //      }

  //      List<LookUpCategory<string>> ISegmentMasterService.GetSegments(long CompanyId)
  //      {
  //          var _segmentMasterReg = _segmentMasterRepository.Query().Include(x => x.SegmentDetails).Select().Where(a => a.CompanyId == CompanyId && a.Status == AppsWorld.Framework.RecordStatusEnum.Active).OrderBy(a => a.CreatedDate).ToList();
  //          List<LookUpCategory<string>> lookUpCategory = new List<LookUpCategory<string>>();
  //          foreach (var segmentmaster in _segmentMasterReg)
  //          {
  //              var lookUpCategorySingle = new LookUpCategory<string>();
  //              lookUpCategorySingle.Id = segmentmaster.Id;
  //              lookUpCategorySingle.CategoryName = segmentmaster.Name;
  //              lookUpCategorySingle.Lookups = segmentmaster.SegmentDetails.Select(x => new LookUp<string>()
  //              {
  //                  Id = x.Id,
  //                  Name = x.Name,
  //                  ParentId = x.ParentId,
  //                  RecOrder = x.RecOrder
  //              }).ToList();

  //              lookUpCategory.Add(lookUpCategorySingle);
  //          }
  //          return lookUpCategory;
  //      }

		//public string GetSegmentName(long Id)
		//{
		//	return _segmentMasterRepository.Query(c => c.Id == Id).Select(c => c.Name).FirstOrDefault();
		//}

  //  }
}
