//using Service.Pattern;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using AppsWorld.ReceiptModule.Entities;
////using AppsWorld.ReceiptModule.Models;
//using AppsWorld.ReceiptModule.RepositoryPattern;
//using AppsWorld.Framework;

//namespace AppsWorld.ReceiptModule.Service
//{
//    public class AccountTypeService : Service<AccountType>, IAccountTypeService
//    {
//        private readonly IReceiptModuleRepositoryAsync<AccountType> _accountTypeRepository;
//		public AccountTypeService(IReceiptModuleRepositoryAsync<AccountType> accountTypeRepository)
//			: base(accountTypeRepository)
//        {
//			_accountTypeRepository = accountTypeRepository;
//        }
//		public AccountType GetById(long companyId,string name)
//		{
//            return _accountTypeRepository.Query(c => c.CompanyId == companyId && c.Name == name && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
//		}
//        public List<long> GetAllAccounyTypeByNameByID(long companyId, List<string> name)
//        {
//            return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) && c.Status == RecordStatusEnum.Active).Select(x => x.Id).ToList();
//        }
      
//    }
//}
