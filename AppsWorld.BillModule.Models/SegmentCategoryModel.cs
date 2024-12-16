using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
   public class SegmentCategoryModel
    {
       public long? SegmentMasterId1 { get; set; }
       public long? SegmentCategoryId1 { get; set; }
       public string SegmentCategoryName1 { get; set; }

       public long? SegmentMasterId2 { get; set; }
       public long? SegmentCategoryId2 { get; set; }
       public string SegmentCategoryName2 { get; set; }
    }
}
