using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;
using AppsWorld.Framework;

namespace AppsWorld.ReceiptModule.Service
{
	public interface ICompanyService : IService<Company>
	{
		Company GetByNameByServiceCompany(long parentId);

		List<Company> GetCompany(string userName, long companyId,long companyIdCheck);

		Company GetById(long id);
        Dictionary<long, string> GetAllCompaniesName(List<long> Ids);
        Dictionary<long, string> GetAllCompanies(List<long> Ids);
		Dictionary<long, string> GetAllSubCompanies(List<long> Ids,string username,long companyId);
		Dictionary<long, RecordStatusEnum> GetAllCompaniesStatus(List<long> Ids);
		bool GetPermissionBasedOnUser(long? serviceEntityId, long? companyId, string username);
	}
}
