using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Repository
{
    public static class ReceiptRepository
    {  
        public static IEnumerable<Receipt> GetReceiptById(this  IReceiptModuleRepositoryAsync<Receipt> repository, Guid Id)
        {
            return repository.Queryable().Where(x => x.Id == Id).AsEnumerable();
        }    
    }
}
