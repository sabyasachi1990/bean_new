using AppsWorld.BankWithdrawalModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public class GSTDetailService:Service<GSTDetail>,IGSTDetailService
    {
        private readonly IBankWithdrawalModuleRepositoryAsync<GSTDetail> _gstDetailRepository;
        public GSTDetailService(IBankWithdrawalModuleRepositoryAsync<GSTDetail> gstDetailRepository)
            : base(gstDetailRepository)
        {
            this._gstDetailRepository = gstDetailRepository;
        }
        public List<GSTDetail> GetAllGstDetail(Guid id,string docType)
        {
            return _gstDetailRepository.Query(x => x.DocId == id && x.DocType == docType).Select().ToList();
        }
        public GSTDetail GetGSTById(Guid id)
        {
            return _gstDetailRepository.Queryable().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
