using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;

namespace AppsWorld.BankReconciliationModule.Repository
{
   public static class BankReconciliationClearingDetailRepository
    {
       public static BankReconciliationClearingDetail GetBankReconciliation(this IBankReconciliationModuleRepositoryAsync<BankReconciliationClearingDetail> repository, Guid id)
       {
           return repository.Queryable().FirstOrDefault(s => s.Id == id);
       }
    }
}
