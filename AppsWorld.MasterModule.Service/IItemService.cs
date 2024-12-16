using System;
using System.Collections.Generic;
using Service.Pattern;
using AppsWorld.MasterModule.Entities;
using AppsWorld.Framework;
using System.Linq;
using AppsWorld.CommonModule.Infra;
using AppsWorld.MasterModule.Models;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public interface IItemService : IService<Item>
    {
        Task<IQueryable<ItemkModel>> GetByCompanyId(long companyId);
        Item GetByIdAndCompanyId(System.Guid id, long companyId);
        Item GetItemByCompanyId(long CompanyId);
        List<Item> GetAllItemByIdCompanyId(Guid id, long companyId);
        Item GetAllByItem(Guid Id, string Code, long companyId);
        Item GetAllItemByCodeAndCompanyId(string Code, long CompanyId);
        List<Item> GetIncidentalSetups(long CompanyId);
	   Item GetIncidentalSetup(long CompanyId);
	   List<Item> GetItemByDocumentId(long companyId, long documentId);
        List<Item> GetItemByIdandIncidental(Guid id, long companyId);
        List<Item> GetAllItemByDidAndCid(long companyId, long documentId);
        List<Item> GetAllItemName(long companyId, string code);
        List<Item> GetallItems(long companyId);
        Task<List<Item>> GetAllItems(long companyId);

        bool? IsDuplicateItem(long companyId, Guid itemId, string code);//item Duplicate check code
        Item GetItem(long compnayId, Guid itemId);//get Item by Id
        Item GetWFItem(long compnayId, long? documentId, Guid id);
        Item GetWorkFlowItemByRounding(long companyId);

        Item GetWFServiceItem(long companyId, long? documentId);//for wf service
        bool? IsExternalDuplicateItem(long companyId, long? documentId, string code, Guid itemId);
        Item GetBeanItem(long CompanyId, long coaId);
	   bool? IsDuplicateGroupByItem(long companyId, Guid itemId,  string groupBy);
	   bool GetIncidentalTypeExistOrNot(string incidentaltype, long companyId,Guid itemId);
        Task<List<Item>> GetAllItemNameAsync(long companyId, string code);


    }
}
