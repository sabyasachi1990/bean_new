using Service.Pattern;
using System.Collections.Generic;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ControlCodeService : Service<ControlCode>, IControlCodeService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<ControlCode> _controlCodeRepository;

        public ControlCodeService(IJournalVoucherModuleRepositoryAsync<ControlCode> controlCodeRepository)
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
			return _controlCodeRepository.Query(c => c.ControlCategoryId == CatId).Select().ToList();
		}
        public async Task<List<ControlCode>> GetControlCodesByCatIdAsync(long CatId)
        {
            return await Task.Run(()=> _controlCodeRepository.Query(c => c.ControlCategoryId == CatId).Select().ToList());
        }
    }
}
