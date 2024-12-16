using AppsWorld.CommonModule.Entities;
using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.Models;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public class OpeningBalanceDetailLineItemService : Service<OpeningBalanceDetailLineItem>, IOpeningBalanceDetailLineItemService
    {
        private readonly IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetailLineItem> _OpeningBalancesRepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<OpeningBalance> _OpeningBalancesMasterRepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetail> _OpeningBalancesDetailRepository;
        private readonly IOpeningBalancesModuleRepositoryAsync<ChartOfAccount> _chartOfAccountRepository;
        public OpeningBalanceDetailLineItemService(IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetailLineItem> OpeningBalancesRepository, IOpeningBalancesModuleRepositoryAsync<ChartOfAccount> chartOfAccountRepository, IOpeningBalancesModuleRepositoryAsync<OpeningBalance> OpeningBalancesMasterRepository, IOpeningBalancesModuleRepositoryAsync<OpeningBalanceDetail> OpeningBalancesDetailRepository)
              : base(OpeningBalancesRepository)
        {
            _OpeningBalancesRepository = OpeningBalancesRepository;
            _chartOfAccountRepository = chartOfAccountRepository;
            _OpeningBalancesMasterRepository = OpeningBalancesMasterRepository;
            _OpeningBalancesDetailRepository = OpeningBalancesDetailRepository;
        }

        public List<OpeningBalanceDetailLineItem> GetLineItemsForCOA(long COAId, long ServiceCompanyId, string currency)
        {
            return _OpeningBalancesRepository.Queryable().Where(c => c.COAId == COAId && c.ServiceCompanyId == ServiceCompanyId && c.DocumentCurrency == currency).OrderBy(c => c.Recorder).ToList();
        }
        public IQueryable<OpeningBalanceLineItemModel> GetLineItemsForCOAs(long COAId, long ServiceCompanyId, string currency,long companyId)
        {
            return (from openingBalancelineitem in _OpeningBalancesRepository.Queryable().Where(c => c.COAId == COAId && c.ServiceCompanyId == ServiceCompanyId && c.DocumentCurrency == currency).AsQueryable()
                              join coa in _chartOfAccountRepository.Queryable() on openingBalancelineitem.COAId equals coa.Id
                              where (coa.CompanyId == companyId)
                              select (new OpeningBalanceLineItemModel() 
                              {
                                  Date = openingBalancelineitem.Date,
                                  BaseCurrency = openingBalancelineitem.BaseCurrency,
                                  Id = openingBalancelineitem.Id,
                                  OpeningBalanceDetailId = openingBalancelineitem.OpeningBalanceDetailId,
                                  COAId = openingBalancelineitem.COAId,
                                  AccountName = coa.Name,
                                  AccountCode = coa.Code,
                                  BaseCredit = openingBalancelineitem.BaseCredit,
                                  BaseDebit = openingBalancelineitem.BaseDebit,
                                  DocCurrency = openingBalancelineitem.DocumentCurrency,
                                  DocCredit = openingBalancelineitem.DocCredit,
                                  DocDebit = openingBalancelineitem.DoCDebit,
                                  Description = openingBalancelineitem.Description,
                                  ExchangeRate = openingBalancelineitem.ExchangeRate,
                                  EntityId = openingBalancelineitem.EntityId,
                                  ServiceCompanyId = openingBalancelineitem.ServiceCompanyId,
                                  DocumentReference = openingBalancelineitem.DocumentReference,
                                  IsDisAllow = openingBalancelineitem.IsDisAllow,
                                  UserCreated = openingBalancelineitem.UserCreated,
                                  CreatedDate = openingBalancelineitem.CreatedDate,
                                  ModifiedBy = openingBalancelineitem.ModifiedBy,
                                  ModifiedDate = openingBalancelineitem.ModifiedDate,
                                  DueDate = openingBalancelineitem.DueDate,
                                  IsEditable = openingBalancelineitem.IsEditable,
                                  RecOrder = openingBalancelineitem.Recorder,
                                  IsProcressed = openingBalancelineitem.IsProcressed,
                                  ProcressedRemarks = openingBalancelineitem.ProcressedRemarks
                              })).OrderBy(x=>x.RecOrder).Take(10).Skip(50).AsQueryable();
        }

        public OpeningBalanceDetailLineItem GetLineItemById(Guid id)
        {
            return _OpeningBalancesRepository.Queryable().FirstOrDefault(c => c.Id == id);
        }

        public List<OpeningBalanceDetailLineItem> GetLstLineItemById(Guid id)
        {
            return _OpeningBalancesRepository.Queryable().Where(c => c.OpeningBalanceDetailId == id).ToList();

        }

        public bool? IsLineItemExist(Guid lineItemId, Guid detailId)
        {
            return _OpeningBalancesRepository.Query(a => a.Id == lineItemId && a.OpeningBalanceDetailId == detailId).Select().Any();
        }

        public bool? IsLineItemDocDate(List<Guid> detailId, DateTime? obDate)
        {
            var q = _OpeningBalancesRepository.Query(a => detailId.Contains(a.Id) && a.Date >= obDate).Select().Any();
            return q;
        }

        public List<Guid> GetListOfLineItemId(List<long> coaids, long serviceCompanyId)
        {
            List<OpeningBalanceDetailLineItem> dlLineItem = _OpeningBalancesRepository.Query(a => coaids.Contains(a.COAId) && a.IsProcressed == false && a.IsEditable != false && a.ServiceCompanyId == serviceCompanyId).Select().ToList();
            return dlLineItem.Any() ? dlLineItem.Select(a => a.Id).ToList() : new List<Guid>();

        }
        public List<Guid> GetListOfLineItemId1(List<long> coaids, long serviceCompanyId,long companyId)
        {
            List<OpeningBalanceDetailLineItem> dlLineItem = (from ob in _OpeningBalancesMasterRepository.Queryable().Where(x => x.CompanyId == companyId && x.ServiceCompanyId == serviceCompanyId)
                                                            join obd in _OpeningBalancesDetailRepository.Queryable() on ob.Id equals obd.OpeningBalanceId
                                                            join li in _OpeningBalancesRepository.Queryable() on obd.Id equals li.OpeningBalanceDetailId
                                                            where coaids.Contains(li.COAId) && li.IsProcressed == false && li.IsEditable != false && li.ServiceCompanyId == serviceCompanyId && ob.IsTemporary == false
                                                            select li).ToList();
            return dlLineItem.Any() ? dlLineItem.Select(a => a.Id).ToList() : new List<Guid>();

        }
        public List<Guid?> ListOfEntityIds(List<long> coaIds, long serviceCompanyId)
        {
            return _OpeningBalancesRepository.Query(a => coaIds.Contains(a.COAId)).Select(a => a.EntityId).ToList();
        }
        public List<OpeningBalanceDetailLineItem> GetListOfOBDLineItemByCoaId(List<long> coaIds, long serviceCompanyId)
        {
            return _OpeningBalancesRepository.Query(a => coaIds.Contains(a.COAId) && a.ServiceCompanyId == serviceCompanyId && a.DocCredit > 0).Select().ToList();
        }
        public Dictionary<Guid, string> GetListOfDeleteTPOPlineItem(List<Guid> lstLineItemId, List<long> coaIds, long serviceCompanyId)
        {
            return _OpeningBalancesRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && lstLineItemId.Contains(a.Id) && coaIds.Contains(a.COAId) && a.DocCredit > 0).Select(c => new { Id = c.Id, DocNo = c.DocumentReference }).ToDictionary(Id => Id.Id, DocNo => DocNo.DocNo);
        }
        public List<OpeningBalanceDetailLineItem> GetListOfTPOPLineItemId(List<Guid> TPOPIds, long serviceCompanyId)
        {
            return _OpeningBalancesRepository.Query(a => TPOPIds.Contains(a.Id) && a.ServiceCompanyId == serviceCompanyId/* && a.DocCredit > 0*/).Select().ToList();
            //return dlLineItem.Any() ? dlLineItem.Select(a => a.Id).ToList() : new List<Guid>();

        }
        public OpeningBalanceDetail GetOpeningBalanceDetail(Guid openingBalanceId,Guid openingBalanceDetailId, long coaId)
        {
            return _OpeningBalancesDetailRepository.Queryable().Where(x => x.Id == openingBalanceDetailId && x.OpeningBalanceId == openingBalanceId && x.COAId == coaId).FirstOrDefault();
        }
        public int GetOpeningBalanceDetailRecOrder(Guid openingBalanceId)
        {
            var rec = _OpeningBalancesDetailRepository.Queryable().Where(x => x.OpeningBalanceId == openingBalanceId).Max(x=>x.Recorder);
            return rec;
        }
    }
}
