using AppsWorld.JournalVoucherModule.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Repository
{
  public static  class JournalRepository
    {
        public static IEnumerable<Journal> GetJournalById(this IJournalVoucherModuleRepositoryAsync<Journal>  repository,Guid Id)
        {
            return repository.Queryable().Where(x => x.Id == Id).AsEnumerable();
        }
    }
}
