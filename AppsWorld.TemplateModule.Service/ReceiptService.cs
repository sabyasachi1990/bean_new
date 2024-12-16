using AppsWorld.TemplateModule.Entities.Models;
using AppsWorld.TemplateModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Service
{
    public class ReceiptService : Service<Receipt>, IReceiptService
    {
        private ITemplateModuleRepositoryAsync<Receipt> _receiptRepository;

        public ReceiptService(ITemplateModuleRepositoryAsync<Receipt> receiptRepository) : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
    }
}
