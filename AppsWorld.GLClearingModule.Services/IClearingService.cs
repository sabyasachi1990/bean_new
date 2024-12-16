using AppsWorld.GLClearingModule.Entities;
using AppsWorld.GLClearingModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Service
{
    public interface IClearingService : IService<GLClearing>
    {
        GLClearing GetClearing(Guid id, long companyId);
        GLClearing GetByCompanyId(long companyId);
        IQueryable<ClearingModelK> GetAllClearingK(string username, long companyId);
        IQueryable<ClearedModelK> GetAllClrearedK(long companyId, string username);
        List<GLClearing> GelAllClearings(long companyId);
        bool IsDocNoExists(long companyId, Guid id, string docNo);
        Guid GetClearingByDetailId(Guid detailId, long companyId);
        //List<GLClearing> GetAllClearings(List<Guid> ids, long? companyId);
        GLClearing GetClearingById(Guid id, long companyId);
    }
}
