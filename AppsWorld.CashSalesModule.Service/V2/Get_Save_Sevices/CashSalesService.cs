using System;
using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Service.Pattern;
using System.Linq;
using FrameWork;
using System.Collections.Generic;
using AppsWorld.Framework;
using AppsWorld.CashSalesModule.Infra;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public class CashSalesService : Service<CashSale>, ICashSalesService
    {
        private readonly ICashSalesMasterRepositoryAsync<CashSale> _cashSaleRepository;
        private readonly ICashSalesMasterRepositoryAsync<CashSaleDetail> _cashSaleDetailRepository;
        public CashSalesService(ICashSalesMasterRepositoryAsync<CashSale> cashSaleRepository, ICashSalesMasterRepositoryAsync<CashSaleDetail> cashSaleDetailRepository)
            : base(cashSaleRepository)
        {
            _cashSaleRepository = cashSaleRepository;
            _cashSaleDetailRepository = cashSaleDetailRepository;
        }

        public List<CashSale> GetAllCashSale(long companyId)
        {
            return _cashSaleRepository.Queryable().Where(c => c.CompanyId == companyId).AsEnumerable().OrderByDescending(c => c.CreatedDate).ToList();
        }

        public List<string> GetAutoNumber(long companyId)
        {
            return _cashSaleRepository.Queryable().Where(x => x.CompanyId == companyId).Select(x => x.CashSaleNumber).ToList();
        }
        public CashSale GetCashSaleDocNo(Guid id, string docNo, long companyId)
        {
            return _cashSaleRepository.Query(x => x.Id != id && x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState != CashSaleStatus.Void).Select().FirstOrDefault();
        }

        public CashSale CreateCashSales(long companyId, Guid id)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select().FirstOrDefault();
        }

        public CashSale GetAllCashSalesLUs(Guid cashsaleId, long companyId)
        {
            return _cashSaleRepository.Query(c => c.Id == cashsaleId && c.CompanyId == companyId).Include(c => c.CashSaleDetails).Select().FirstOrDefault();
        }


        public CashSale GetCashSaleByIdAndCompanyId(Guid id, long companyid)
        {
            return _cashSaleRepository.Query(e => e.Id == id && e.CompanyId == companyid).Include(a => a.CashSaleDetails).Select().FirstOrDefault();
        }

        public CashSale GetCashSaleLU(long companyId, Guid Id)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId && a.Id == Id).Include(z => z.CashSaleDetails).Select().FirstOrDefault();
        }

        public CashSale GetDocTypeAndCompanyid(string DocType, long companyId)
        {
            return _cashSaleRepository.Query(a => a.DocType == DocType && a.CompanyId == companyId && a.DocumentState != CashSaleStatus.Void).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();
        }

        public CashSale DuplicateCashsale(string DocNo, string docType, long companyId)
        {
            return _cashSaleRepository.Query(a => a.DocNo == DocNo && a.DocType == docType && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public DateTime GetCashsaleByCompanyId(long companyId)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).Select(c => c.DocDate).FirstOrDefault();
        }
        public void CashSaleDetailInsert(CashSaleDetail detail)
        {
            _cashSaleDetailRepository.Insert(detail);
        }
        public void CashSaleDetailUpdate(CashSaleDetail detail)
        {
            _cashSaleDetailRepository.Update(detail);
        }
        public Dictionary<DateTime?, DateTime> GetCashsaleCreatedDate(long companyId)
        {
            return _cashSaleRepository.Query(a => a.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).Select(c => new { CreatedDate = c.CreatedDate, DocDate = c.DocDate }).ToDictionary(d => d.CreatedDate, d => d.DocDate);
        }
    }
}
