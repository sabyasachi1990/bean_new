using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.BillModule.Service
{
    public class AutoNumberService : Service<AutoNumber> /*Service<BeanAutoNumber>*/, IAutoNumberService
    {
        private readonly IBillModuleRepositoryAsync<AutoNumber> _autoNumberepository;
        //private readonly IBillModuleRepositoryAsync<BeanAutoNumber> _autoNumberepository;

        public AutoNumberService(IBillModuleRepositoryAsync<AutoNumber> autoNumberRepository/*IBillModuleRepositoryAsync<BeanAutoNumber> autoNumberRepository*/)
			: base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
            //_autoNumberepository = autoNumberRepository;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }

        //public BeanAutoNumber GetAutoNumber(long companyId, string entityType)
        //{
        //    return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        //}
    }
}
