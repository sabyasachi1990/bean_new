using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.Framework;
using FrameWork;

namespace AppsWorld.CommonModule.Service
{
    public class EmployeeService : Service<Employee>, IEmployeeService
    {
        private readonly ICommonModuleRepositoryAsync<Employee> _employeeRepository;
        public EmployeeService(ICommonModuleRepositoryAsync<Employee> employeeRepository)
            : base(employeeRepository)
        {
            this._employeeRepository = employeeRepository;
        }
        public async Task<List<Employee>> EntityLookUp(long companyId,Guid? entityId)
        {
            return await Task.Run(()=> _employeeRepository.Query(x=>x.CompanyId == companyId && (x.Status == RecordStatusEnum.Active || x.Id == entityId)).
                 Select().ToList());         
        }     
    }
}
