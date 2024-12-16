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
    public class ControlCodeService : Service<ControlCode>, IControlCodeService
    {
        private readonly IReceiptModuleRepositoryAsync<ControlCode> _controlCodeRepository;

        public ControlCodeService(IReceiptModuleRepositoryAsync<ControlCode> controlCodeRepository)
            : base(controlCodeRepository)
        {
            _controlCodeRepository = controlCodeRepository;
        }
        public ControlCode GetControlCodeCode(string code)
        {
            return _controlCodeRepository.Query(c => c.CodeKey == code).Select().FirstOrDefault();
        }
        public List<ControlCode> GetControlCodesByCatId(long CatId)
        {
            return _controlCodeRepository.Queryable().Where(c => c.ControlCategoryId == CatId && c.Status == Framework.RecordStatusEnum.Active).ToList();
        }
    }
}
