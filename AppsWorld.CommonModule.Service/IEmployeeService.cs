using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using FrameWork;
using AppsWorld.Framework;

namespace AppsWorld.CommonModule.Service
{
    public interface IEmployeeService : IService<Employee>
    {
        Task<List<Employee>> EntityLookUp(long companyId,Guid? entityId);
    }
}
