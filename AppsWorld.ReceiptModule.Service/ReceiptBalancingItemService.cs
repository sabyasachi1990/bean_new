using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
    public class ReceiptBalancingItemService : Service<ReceiptBalancingItem>, IReceiptBalancingItemService
    {
        private readonly IReceiptModuleRepositoryAsync<ReceiptBalancingItem> _receiptBalancingItemRepository;

        public ReceiptBalancingItemService(IReceiptModuleRepositoryAsync<ReceiptBalancingItem> receiptBalancingItemRepository)
            : base(receiptBalancingItemRepository)
        {
            _receiptBalancingItemRepository = receiptBalancingItemRepository;
        }

        public List<ReceiptBalancingItem> GetReceiptBalancingItems(Guid receiptId)
        {
			return _receiptBalancingItemRepository.Query(c => c.ReceiptId == receiptId).Select().ToList();
        }
		public ReceiptBalancingItem GetRBI(Guid id, Guid receiptId)
		{
			return _receiptBalancingItemRepository.Query(c => c.Id == id && c.ReceiptId == receiptId).Select().FirstOrDefault();
		}
		
    }
}
