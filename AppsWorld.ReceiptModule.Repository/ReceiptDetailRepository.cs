using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Repository
{
    public static class ReceiptDetailRepository
    {
        public static IEnumerable<ReceiptDetail> GetReceiptById(this IReceiptModuleRepositoryAsync<ReceiptDetail> repository, Guid Id)
        {
            return repository.Queryable().Where(x => x.Id == Id).AsEnumerable();
        }
    }
}
