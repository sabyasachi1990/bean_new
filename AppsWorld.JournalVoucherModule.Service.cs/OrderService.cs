using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class OrderService : Service<Order>, IOrderService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Order> _OrderRepository;
        public OrderService(IJournalVoucherModuleRepositoryAsync<Order> OrderRepository)
            : base(OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }



        public List<Order> GetOrderByEid(long companyid)
        {
            return _OrderRepository.Query(c => c.CompanyId == companyid).Select().ToList();
        }
        public Order GetOrderByEidLeadSheetType(long companyId, string leadSheetType, string accountClass)
        {
            accountClass = accountClass != null ? accountClass.ToUpper() : null;

            return _OrderRepository.Queryable().Where(a => a.CompanyId == companyId && a.LeadSheetType == leadSheetType && a.AccountClass.ToUpper() == accountClass).FirstOrDefault();
        }

        public Order GetIncomeStatementOrderByCompanyId(long companyId)
        {
            return _OrderRepository.Query(c => c.CompanyId == companyId && c.LeadSheetType == "Income Statement" && c.AccountClass == "Income Statement").Select().FirstOrDefault();
        }
    }
}

