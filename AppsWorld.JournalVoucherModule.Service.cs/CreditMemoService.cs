using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CreditMemoService : Service<CreditMemo>, ICreditMemoService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CreditMemo> _memoRepository;

        public CreditMemoService(IJournalVoucherModuleRepositoryAsync<CreditMemo> memoRepository)
            : base(memoRepository)
        {
            _memoRepository = memoRepository;
        }
        public CreditMemo GetMemo(Guid? id)
        {
            return _memoRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
