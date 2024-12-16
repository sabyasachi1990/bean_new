using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.MasterModule.Service
{
    public class ReceiptService : Service<Receipt>, IReceiptService
    {
        private readonly IMasterModuleRepositoryAsync<Receipt> _receiptRepository;
        public ReceiptService(IMasterModuleRepositoryAsync<Receipt> receiptRepository)
           : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
        public List<Receipt> GetAllReceiptByEntity(Guid? entityId)
        {
            return _receiptRepository.Queryable().Where(x => x.EntityId == entityId && x.DocumentState != ReceiptState.Void).ToList();
        }
    }
}
