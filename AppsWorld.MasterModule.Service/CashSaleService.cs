using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CashSaleService : Service<CashSale>, ICashSalesService
    {
        private readonly IMasterModuleRepositoryAsync<CashSale> _cashSaleServiceRepository;
        public CashSaleService(IMasterModuleRepositoryAsync<CashSale> cashSaleServiceRepository)
            : base(cashSaleServiceRepository)
        {
            _cashSaleServiceRepository = cashSaleServiceRepository;
        }
        public async Task<CashSale> GetALlCashSaleByItems(Guid? DocumentId)
        {
            return await Task.Run(()=> _cashSaleServiceRepository.Query(c => c.Id == DocumentId).Include(s => s.CashSaleDetails).Select().FirstOrDefault());
        }
    }
}
