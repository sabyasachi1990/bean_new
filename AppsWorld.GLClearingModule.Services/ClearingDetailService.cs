using AppsWorld.GLClearingModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.RepositoryPattern;

namespace AppsWorld.GLClearingModule.Service
{
    public class ClearingDetailService : Service<GLClearingDetail>,IClearingDetailService
    {
        private readonly IClearingModuleRepositoryAsync<GLClearingDetail> _clearingDetailRepository;
        public ClearingDetailService(IClearingModuleRepositoryAsync<GLClearingDetail> clearingDetailRepository) : base(clearingDetailRepository)
        {
            _clearingDetailRepository = clearingDetailRepository;
        }
        public List<GLClearingDetail>GetAllClearing(Guid  clearingId)
        {
            return _clearingDetailRepository.Query(a=>a.GLClearingId==clearingId).Select().ToList();
        }
        public Guid GetAllClearingDetail(Guid Id)
        {
            return _clearingDetailRepository.Query(a => a.JournalDetailId == Id).Select(s=>s.GLClearingId).FirstOrDefault();
            //return result.FirstOrDefault();
        }
        public GLClearingDetail GetAllClearingByDetail(Guid Id)
        {
            return _clearingDetailRepository.Query(a => a.DocumentId == Id).Select().FirstOrDefault();
            //return result.FirstOrDefault();
        }
        public GLClearingDetail GetAllClearingByDetailId(Guid Id)
        {
            return _clearingDetailRepository.Query(a => a.JournalDetailId == Id).Select().FirstOrDefault();
            //return result.FirstOrDefault();
        }
        public List<GLClearingDetail> GetAllClearingDetails(List<Guid?> lstDetails)
        {
            return _clearingDetailRepository.Queryable().Where(d => lstDetails.Contains(d.JournalDetailId)).ToList();
        }
        public List<Guid> GetAllClearingDetailsByIds(List<Guid?> lstDetails)
        {
            return _clearingDetailRepository.Queryable().Where(d => lstDetails.Contains(d.JournalDetailId)).Select(f=>f.GLClearingId).ToList();
        }
    }
}


