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
    public class ControlCodeService : Service<ControlCode>, IControlCodeService
    {
        private readonly IBillModuleRepositoryAsync<ControlCode> _controlCodeRepository;

        public ControlCodeService(IBillModuleRepositoryAsync<ControlCode> controlCodeRepository)
			: base(controlCodeRepository)
        {
			_controlCodeRepository = controlCodeRepository;
        }
		public  ControlCode GetControlCodeCode(string code)
		{
			return _controlCodeRepository.Query(c => c.CodeKey == code).Select().FirstOrDefault();
		}
		public List<ControlCode> GetControlCodesByCatId(long CatId)
		{
			return _controlCodeRepository.Queryable().Where(c => c.ControlCategoryId == CatId && c.Status == RecordStatusEnum.Active).ToList();
		}
		public ControlCode GetControlCode(long controlCategoryId,string name)
		{
			return _controlCodeRepository.Query(c => c.ControlCategoryId == controlCategoryId && c.CodeKey == name).Select().FirstOrDefault();
		}
    }
}
