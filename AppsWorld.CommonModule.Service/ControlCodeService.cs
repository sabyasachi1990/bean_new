using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
//using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.RepositoryPattern;

namespace AppsWorld.CommonModule.Service
{
    public class ControlCodeService : Service<ControlCode>, IControlCodeService
    {
        private readonly ICommonModuleRepositoryAsync<ControlCode> _controlCodeRepository;

        public ControlCodeService(ICommonModuleRepositoryAsync<ControlCode> controlCodeRepository)
            : base(controlCodeRepository)
        {
            _controlCodeRepository = controlCodeRepository;
        }

        public List<ControlCode> GetControlCodeByCategoryId(long categoryId)
        {
            return _controlCodeRepository.Queryable().Where(a => a.ControlCategoryId == categoryId && a.Status == Framework.RecordStatusEnum.Active).ToList();
        }
        public async Task<List<ControlCode>> GetControlCodeByCategoryIdAsync(long categoryId)
        {
            return await Task.Run(()=> _controlCodeRepository.Queryable().Where(a => a.ControlCategoryId == categoryId && a.Status == Framework.RecordStatusEnum.Active).ToList());
        }

        public ControlCode GetControlCodeCode(string code)
        {
            return _controlCodeRepository.Query(c => c.CodeKey == code).Select().FirstOrDefault();
        }
        public List<ControlCode> GetControlCodesByCatId(long CatId)
        {
            return _controlCodeRepository.Queryable().Where(c => c.ControlCategoryId == CatId).ToList();
        }
        public async Task<List<ControlCode>> GetControlCodesByCatIdAsync(long CatId)
        {
            return  await Task.Run(()=> _controlCodeRepository.Queryable().Where(c => c.ControlCategoryId == CatId).ToList());
        }
    }
}
