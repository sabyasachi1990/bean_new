using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using AppsWorld.MasterModule.Models;

namespace AppsWorld.MasterModule.Service
{
    public class ItemService : Service<Item>, IItemService
    {
	   private readonly IMasterModuleRepositoryAsync<Item> _itemRepository;
	   private readonly IMasterModuleRepositoryAsync<TaxCode> _taxCodeRepository;
	   private readonly IMasterModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;

	   public ItemService(IMasterModuleRepositoryAsync<Item> itemRepository, IMasterModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, IMasterModuleRepositoryAsync<TaxCode> taxCodeRepository)
		  : base(itemRepository)
	   {
		  _itemRepository = itemRepository;
		  _chartOfAccountRepository = chartOfAccountRepository;
		  _taxCodeRepository = taxCodeRepository;

	   }

	   public async Task<IQueryable<ItemkModel>> GetByCompanyId(long companyId)
	   {
		  IQueryable<Item> itemRepository = await Task.Run(()=> _itemRepository.Queryable().Where(a => a.CompanyId == companyId && a.Status <= RecordStatusEnum.Disable));
		  IQueryable<ChartOfAccount> chartOfAccountRepository =await Task.Run(()=> _chartOfAccountRepository.Queryable().Where(d => d.CompanyId == companyId));
		  IQueryable<TaxCode> lstTaxCodes = await Task.Run(()=> _taxCodeRepository.Queryable());
		  IQueryable<ItemkModel> itemkModelDetails =
									    from b in itemRepository
									    from c in chartOfAccountRepository
									    where c.Id == b.COAId
									    select new ItemkModel()
									    {
										   Id = b.Id,
										   CompanyId = b.CompanyId,
										   Code = b.Code,
										   Description = b.Description,
										   DefaultTaxcodeId = b.DefaultTaxcodeId == null ? 0 : b.DefaultTaxcodeId,
										   UnitPrice = b.UnitPrice != null ? (double?)b.UnitPrice.Value : null,
										   UOM = b.UOM,
										   COAName = c.Name,
										   Status = b.Status.ToString(),
										   CreatedDate = b.CreatedDate,
										   TaxCode = b.DefaultTaxcodeId != null ? lstTaxCodes.Where(d => d.Id == b.DefaultTaxcodeId).Select(c => c.Code).FirstOrDefault() : string.Empty,
										   ModifiedBy = b.ModifiedBy,
										   UserCreated = b.UserCreated,
										   ModifiedDate = b.ModifiedDate,
										   AllowableDis = b.AllowableDis,
										   IsExternalData = b.IsExternalData,
										   IsIncidental = b.IsIncidental
									    };
		  return itemkModelDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();

	   }
	   public Item GetByIdAndCompanyId(System.Guid id, long companyId)
	   {
		  return _itemRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
	   }

	   public Item GetItemByCompanyId(long CompanyId)
	   {
		  return _itemRepository.Query(c => c.CompanyId == CompanyId).Select().FirstOrDefault();
	   }

	   public List<Item> GetAllItemByIdCompanyId(Guid id, long companyId)
	   {
		  return _itemRepository.Queryable().Where(c => c.Id == id && c.CompanyId == companyId).ToList();
	   }
	   public Item GetAllByItem(Guid Id, string Code, long companyId)
	   {
		  return _itemRepository.Query().Select().Where(a => a.Id != Id && a.Code.ToLower() == Code.ToLower() && a.CompanyId == companyId).FirstOrDefault();
	   }

	   public Item GetAllItemByCodeAndCompanyId(string Code, long CompanyId)
	   {
		  return _itemRepository.Query().Select().Where(a => a.Code.ToLower() == Code.ToLower() && a.CompanyId == CompanyId).FirstOrDefault();
	   }

	   public List<Item> GetIncidentalSetups(long CompanyId)
	   {
		  return _itemRepository.Query(x => x.CompanyId == CompanyId && x.IsIncidental == true && x.Status == RecordStatusEnum.Active).Select().OrderBy(c => c.CreatedDate).ToList();
	   }

	   public Item GetIncidentalSetup(long CompanyId)
	   {
		  return _itemRepository.Query(x => x.CompanyId == CompanyId && x.IsIncidental == true && x.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
	   }
	   public List<Item> GetItemByDocumentId(long companyId, long documentId)
	   {
		  return _itemRepository.Queryable().Where(a => a.DocumentId == documentId && a.CompanyId == companyId).ToList();
	   }

	   public List<Item> GetItemByIdandIncidental(Guid id, long companyId)
	   {
		  return _itemRepository.Query(a => a.CompanyId == companyId && a.Id == id && a.IsIncidental == true).Select().ToList();
	   }

	   public List<Item> GetAllItemByDidAndCid(long companyId, long documentId)
	   {
		  return _itemRepository.Query(a => a.CompanyId == companyId && (a.DocumentId == documentId || a.IsIncidental == true)).Select().ToList();
		}

		public List<Item> GetAllItemName(long companyId, string code)
		{
			return _itemRepository.Query(a => a.CompanyId == companyId && a.Code == code).Select().ToList();
		}
        public async Task<List<Item>> GetAllItemNameAsync(long companyId, string code)
        {
            return await Task.Run(()=> _itemRepository.Query(a => a.CompanyId == companyId && a.Code == code).Select().ToList());
        }

        public List<Item> GetallItems(long companyId)
	   {
		  return _itemRepository.Queryable().Where(x => x.CompanyId == companyId && (x.IsExternalData == null || x.IsExternalData == false)).ToList();
	   }
	   public async Task<List<Item>> GetAllItems(long companyId)
	   {
		  return await Task.Run(()=> _itemRepository.Queryable().Where(x => x.CompanyId == companyId && x.Code != "Opening Balance").ToList());
	   }
	   public bool? IsDuplicateItem(long companyId, Guid itemId, string code)
	   {
		  return _itemRepository.Query(a => a.Id != itemId && a.CompanyId == companyId && a.Code == code).Select().Any();
	   }
	   public Item GetItem(long compnayId, Guid itemId)
	   {
		  return _itemRepository.Query(a => a.CompanyId == compnayId && a.Id == itemId).Select().FirstOrDefault();
	   }
	   public Item GetWFItem(long compnayId, long? documentId, Guid id)
	   {
		  return _itemRepository.Query(a => a.Id == id && a.CompanyId == compnayId && a.DocumentId == documentId && a.IsExternalData == true).Select().FirstOrDefault();
	   }
	   public Item GetWFServiceItem(long companyId, long? documentId)
	   {
		  return _itemRepository.Query(a => a.CompanyId == companyId && a.DocumentId == documentId && a.IsExternalData == true).Select().FirstOrDefault();
	   }
	   public Item GetWorkFlowItemByRounding(long companyId)
	   {
		  return _itemRepository.Query(c => c.CompanyId == companyId && c.Code == "Rounding (GST)").Select().FirstOrDefault();
	   }
	   public bool? IsExternalDuplicateItem(long companyId, long? documentId, string code, Guid itemId)
	   {
		  Item isItemExist = _itemRepository.Queryable().Where(a => a.DocumentId == documentId && a.CompanyId == companyId).FirstOrDefault();
		  if (isItemExist != null)
			 return _itemRepository.Query(a => a.Id != isItemExist.Id && a.CompanyId == companyId && a.Code == code).Select().Any();
		  else
			 return _itemRepository.Query(a => a.Id != itemId && a.CompanyId == companyId && a.Code == code).Select().Any();
	   }
	   public Item GetBeanItem(long CompanyId, long coaId)
	   {
		  return _itemRepository.Queryable().Where(a => a.CompanyId == CompanyId && a.COAId == coaId && a.Status == RecordStatusEnum.Active).FirstOrDefault();
	   }
	   public bool? IsDuplicateGroupByItem(long companyId, Guid itemId, string groupBy)
	   {
		  return _itemRepository.Queryable().Where(x => x.Id != itemId && x.CompanyId == companyId && x.IncidentalType == groupBy).Any();
	   }
	   public bool GetIncidentalTypeExistOrNot(string incidentaltype, long companyId,Guid itemId)
	   {
		  return _itemRepository.Queryable().Where(x =>x.Id != itemId && x.IncidentalType == incidentaltype && x.CompanyId == companyId).Any();
	   }
    }
}







