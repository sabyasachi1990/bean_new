using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AppsWorld.GLClearingModule.Entities;

namespace AppsWorld.GLClearingModule.Service
{
    public interface IClearingDetailService:IService<GLClearingDetail>
    {
        List<GLClearingDetail> GetAllClearing(Guid clearingId);
        Guid GetAllClearingDetail(Guid Id);
        GLClearingDetail GetAllClearingByDetail(Guid Id);
        GLClearingDetail GetAllClearingByDetailId(Guid Id);
        List<GLClearingDetail> GetAllClearingDetails(List<Guid?> lstDetails);
        List<Guid> GetAllClearingDetailsByIds(List<Guid?> lstDetails);
    }
}
