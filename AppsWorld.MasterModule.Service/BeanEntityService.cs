using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Entities.Models;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class BeanEntityService : Service<BeanEntity>, IBeanEntityService
    {
        private readonly IMasterModuleRepositoryAsync<BeanEntity> _beanEntityRepository;
        private readonly IMasterModuleRepositoryAsync<FinancialSetting> _financialRepository;
        private readonly IMasterModuleRepositoryAsync<TermsOfPayment> _termsOfPaymentRepository;
        private readonly IMasterModuleRepositoryAsync<SSICCodes> _sSiccodesRepository;
        public BeanEntityService(IMasterModuleRepositoryAsync<BeanEntity> beanEntityRepository, IMasterModuleRepositoryAsync<FinancialSetting> financialRepository, IMasterModuleRepositoryAsync<TermsOfPayment> termsOfPaymentRepository, IMasterModuleRepositoryAsync<SSICCodes> sSiccodesRepository)
            : base(beanEntityRepository)
        {
            _beanEntityRepository = beanEntityRepository;
            _financialRepository = financialRepository;
            _termsOfPaymentRepository = termsOfPaymentRepository;
            _sSiccodesRepository = sSiccodesRepository;
        }

        public IQueryable<BeanEntityModelk> GetAllBeanEntitysK(long companyId)
        {
            FinancialSetting financial = _financialRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault(); 
            IQueryable<BeanEntity> beanEntityRepository = _beanEntityRepository.Queryable().Where(a=>a.CompanyId==companyId);
            IQueryable <TermsOfPayment> termsOfPayments = _termsOfPaymentRepository.Queryable().Where(a => a.CompanyId == companyId);
            IQueryable<BeanEntityModelk> beanEntityDetails =
                                                 from b in beanEntityRepository
                                                 where (b.CompanyId == companyId /*&& b.IsShowPayroll == true*/)
                                                 select new BeanEntityModelk()
                                                 {
                                                     Id = b.Id,
                                                     CompanyId = b.CompanyId,
                                                     Name = b.Name,
                                                     CustNature = b.CustNature,
                                                     VenNature = b.VenNature,
                                                     Status = b.Status.ToString(),
                                                     CustCurrency = b.CustCurrency,
                                                     VenCurrency = b.VenCurrency,
                                                     IsCustomer = b.IsCustomer,
                                                     BaseCurrency = financial.BaseCurrency,
                                                     IsVendor = b.IsVendor,
                                                     UserCreated = b.UserCreated,
                                                     ModifiedBy = b.ModifiedBy,
                                                     ModifiedDate = b.ModifiedDate,
                                                     VendorType = b.VendorType,
                                                     CreditTerms = b.CustTOP == null && b.CustTOPId != null ? termsOfPayments.Where(a=> a.Id == b.CustTOPId).Select(a=>a.Name).FirstOrDefault() : b.CustTOP,
                                                     PaymentTerms = b.VenTOP == null && b.VenTOPId != null ? termsOfPayments.Where(a => a.Id == b.VenTOPId).Select(a => a.Name).FirstOrDefault() : b.VenTOP,
                                                     CustCreditLimit = (double)(b.CustCreditLimit),
                                                     VenCreditLimit = (double)(b.VenCreditLimit),
                                                     CreatedDate = b.CreatedDate,
                                                     CustBal = (double)(b.CustBal),
                                                     VenBal = b.VenBal,
                                                     IsExternalData = b.IsExternalData,

                                                     PeppolId=b.PeppolDocumentId

                                                 };
            return beanEntityDetails.OrderByDescending(a => a.CreatedDate).AsQueryable();

        }

        public IEnumerable<BeanEntity> GetAllBeanEntities(long companyId)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && a.Status < RecordStatusEnum.Disable).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }

        public async Task<BeanEntity> GetBeanEntities(long CompanyId, Guid id)
        {
            return await Task.Run(()=> _beanEntityRepository.Queryable().Where(a => a.CompanyId == CompanyId && a.Id == id).FirstOrDefault());
        }

        public BeanEntity GetBeanEntityByIdAndCompanyId(long CompanyId, Guid Id)
        {
            return _beanEntityRepository.Query(c => c.CompanyId == CompanyId && c.Id == Id).Select().FirstOrDefault();
        }

        public BeanEntity GetBeanEntityNameCheck(Guid Id, string Name, long CompanyId)
        {
            return _beanEntityRepository.Query(e => e.Id != Id && e.Name.ToLower() == Name.ToLower() && e.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public List<BeanEntity> GetBeanEntityNameChec(string Name, long CompanyId)
        {
            return _beanEntityRepository.Query(e => e.Name.ToLower() == Name.ToLower() && e.CompanyId == CompanyId).Select().ToList();
        }
        public async Task<List<BeanEntity>> GetAllBeanEntitys(long CompanyId)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.IsCustomer == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).Select().ToList());
        }
        public IQueryable<BeanEntity> GetAllBeanEntitysNew(long CompanyId)
        {
            return  _beanEntityRepository.Queryable().Where(a => a.IsCustomer == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).AsQueryable();
        }
        public async Task<List<BeanEntity>> GetAllBeanEntitiesExpInv(long CompanyId)
        {
            return await Task.Run(()=>_beanEntityRepository.Query(a => a.IsCustomer == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId && a.CustNature != "Interco").Select().ToList());
        }
        public IQueryable<BeanEntity> GetAllBeanEntitiesExpInvNew(long CompanyId)
        {
            return  _beanEntityRepository.Queryable().Where(a => a.IsCustomer == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId && a.CustNature != "Interco").AsQueryable();
        }
        public async Task<List<BeanEntity>> GetAllBeanEntity(Guid? Id, long CompanyId)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.IsCustomer == true && (a.Status == RecordStatusEnum.Active || a.Id == Id) && a.CompanyId == CompanyId).Select().ToList());
        }
        public async Task<List<BeanEntity>> GetAllBeanEntityExpInv(Guid? Id, long CompanyId)
        { 
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.IsCustomer == true && (a.Status == RecordStatusEnum.Active || a.Id == Id) && a.CompanyId == CompanyId && a.CustNature != "Interco").Select().ToList());
        }
        public async Task<List<BeanEntity>> GetBeanEntityByVendor(long CompanyId)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.IsVendor == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId).Select().ToList());
        }
        public async Task<List<BeanEntity>> GetBeanEntityByVendorExpBill(long CompanyId)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.IsVendor == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId && a.VenNature != "Interco").Select().ToList());
        }
        public List<BeanEntity> GetByEntityId(Guid? EntityId, long CompanyId)
        {
            return _beanEntityRepository.Query(a => a.IsVendor == true && (a.Status == RecordStatusEnum.Active || a.Id == EntityId) && a.CompanyId == CompanyId /*&& a.IsShowPayroll == true*/).Select().ToList();
        }
        public BeanEntity GetBeanEntityByDocumentId(Guid documentId)
        {
            return _beanEntityRepository.Query(a => a.DocumentId == documentId && a.IsExternalData == true).Select().FirstOrDefault();
        }
        public BeanEntity GetBeanEntityByDocumentIdCid(Guid documentId, long companyId, string name)
        {
            return _beanEntityRepository.Query(a => a.DocumentId != documentId && a.CompanyId == companyId && a.Name == name).Select().FirstOrDefault();
        }
        public BeanEntity GetBeanEntityByClientId(long CompanyId, Guid Id)
        {
            return _beanEntityRepository.Query(a => a.DocumentId == Id && a.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public bool? IfBeanEntityExists(Guid entityId, long companyId, string name)
        {
            return _beanEntityRepository.Query(a => a.Id != entityId && a.CompanyId == companyId && a.Name == name).Select().Any();
        }
        public List<BeanEntity> GetListOfEntity(long companyId)
        {
            return _beanEntityRepository.Query(a => a.CompanyId == companyId && a.IsVendor == true).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public BeanEntity GetBeanEntity(long CompanyId, long coaId)
        {
            return _beanEntityRepository.Queryable().Where(a => a.CompanyId == CompanyId && a.COAId == coaId).FirstOrDefault();
        }
        //public List<BeanEntity> GetAllVendorEntities(long CompanyId, Guid? id)
        //{
        //    string nature = string.Empty;
        //    if (id != Guid.Empty)
        //        nature = _beanEntityRepository.Query(c => c.Id == id).Select(d => d.VenNature).ToString();
        //    if (id != Guid.Empty && nature == "Interco")
        //        return _beanEntityRepository.Query(a => a.IsVendor == true && a.VenNature == nature && a.CompanyId == CompanyId).Select().ToList();
        //    else
        //        return _beanEntityRepository.Query(a => a.IsVendor == true && a.Status == RecordStatusEnum.Active && a.CompanyId == CompanyId && a.VenNature != "Interco").Select().ToList();

        //}
        public IQueryable<LinkedAccountsModel> GetLinkedAccountsK(long? companyId, string connectionString, string username)
        {
            using (SqlConnection DwhConn = new SqlConnection(connectionString))
            {
                if (DwhConn.State == ConnectionState.Closed)
                    DwhConn.Open();
                SqlCommand cmd = new SqlCommand("Bean_COALinkedAccountsList", DwhConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
                cmd.Parameters.AddWithValue("@userName", username);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                var accounts = (from row in dt.AsEnumerable()
                                select new LinkedAccountsModel
                                {
                                    Cursor = row.Field<string>(0),
                                    Subsection = row.Field<string>(1) != null ? row.Field<string>(1) : string.Empty,
                                    Feature = row.Field<string>(2),
                                    FeatureName = row.Field<string>(3),
                                    COAName = row.Field<string>(4),
                                    AccountType = row.Field<string>(5)
                                }).Distinct().ToList();
                return accounts.AsQueryable();
            }
        }



        public bool IfEntityNameExistsByNature(long companyId, Guid entityId, string name, string nature)
        {
            bool isExist = false;
            if (nature != "Interco")
                isExist = _beanEntityRepository.Query(a => a.CompanyId == companyId && a.Id != entityId && a.Name == name && a.CustNature != "Interco" && a.VenNature != "Interco").Select().Any();
            else if (nature == "Interco")
                isExist = _beanEntityRepository.Query(a => a.CompanyId == companyId && a.Id != entityId && a.Name == name && a.CustNature == "Interco" && a.VenNature == "Interco").Select().Any();
            return isExist;
        }
        public List<SSICCodes> GetSSICCodesByType(string type)
        {
            var ssicCodes = _sSiccodesRepository.Query(x => (x.Code.Contains(type) || x.Industry.Contains(type)) && x.Status == RecordStatusEnum.Active).Select().ToList();
            return ssicCodes;
        }



        public async Task<BeanEntity> GetBeanEntitiesAsync(long CompanyId, Guid id)
        {
            return await Task.Run(()=> _beanEntityRepository.Query(a => a.CompanyId == CompanyId && a.Id == id).Select().FirstOrDefault());
        }

      
    }
}
