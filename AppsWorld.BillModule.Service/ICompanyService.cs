using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Entities.Models;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface ICompanyService : IService<Company>
	{
		Company GetByNameByServiceCompany(long parentId);

		List<Company> GetCompany( long companyId,long companyIdCheck, string userName);
		List<long> GetAllSubCompaniesId(string username, long companyId);

        Company GetById(long id);

		List<Company> GetCompanyByName( string reciverPeppolId);


		void InsertIntoPeppolInboundInvoice(PeppolInboundInvoice peppolInboundInvoice);
		PeppolInboundInvoice GetInboundData(Guid id);
		void UpdatetoPeppolInboundInvoice(PeppolInboundInvoice peppolInboundInvoice);
	}
}
