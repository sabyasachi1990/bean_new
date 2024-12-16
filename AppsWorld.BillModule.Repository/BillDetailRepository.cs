using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;

namespace AppsWorld.BillModule.Repository
{
  public static  class BillDetailRepository
    {
      public static IEnumerable<BillDetail> GetBillDetailById(this IBillModuleRepositoryAsync<BillDetail> repository,Guid Id)
      {
          return repository.Queryable().Where(y => y.Id == Id).AsEnumerable();
      }
        
    }
}
