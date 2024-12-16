using AppsWorld.CommonModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.RepositoryPattern;
using Service.Pattern;
using Repository.Pattern.Infrastructure;
using System.Data.SqlClient;
using System.Data;

namespace AppsWorld.CommonModule.Service
{
    public class AutoNumberService : Service<AutoNumber>, IAutoNumberService
    {
        private readonly ICommonModuleRepositoryAsync<AutoNumber> _autoNumberepository;
        private readonly IAutoNumberCompanyService _autoNumberCompanyService;
        public AutoNumberService(ICommonModuleRepositoryAsync<AutoNumber> autoNumberRepository, IAutoNumberCompanyService autoNumberCompanyService)
            : base(autoNumberRepository)
        {
            _autoNumberepository = autoNumberRepository;
            _autoNumberCompanyService = autoNumberCompanyService;
        }

        public AutoNumber GetAutoNumber(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select().FirstOrDefault();
        }

        //string value = "";
        //string oppValue = "";


        //public IEnumerable<AutoNumber> GetAll(long CompanyId)
        //{
        //    //IEnumerable<AutoNumber> lstAutoNum = _redisService.GetDataFromRedisCache<AutoNumber>("GetAll");

        //    //if (!lstAutoNum.Any())
        //    //{
        //    //    lstAutoNum = _redisService.GetDataFromRedisCache<AutoNumber>("GetAll", new CacheModel<AutoNumber>() { Data = _autoNumberRepository.Queryable().AsEnumerable().ToList() }).ToList();
        //    //}

        //    //return lstAutoNum.Where(c => c.CompanyId == CompanyId);
        //    return _autoNumberepository.Queryable().Where(a => a.CompanyId == CompanyId).AsEnumerable().OrderByDescending(a => a.CreatedDate).AsEnumerable();
        //}
        //public AutoNumber Save(AutoNumber TObject)
        //{
        //   // Log.Logger.ZInfo(AutoNumberLoggingValidation.Log_AutoNumber_Save_Request_Message);
        //    //throw new NotImplementedException();
        //    string _errors = CommonValidation.ValidateObject(TObject);

        //    if (!string.IsNullOrEmpty(_errors))
        //    {
        //        throw new Exception(_errors);
        //    }

        //    //_redisService.InvalidateCache<AutoNumber>("GetAll");

        //    var _autoNumberSelect = _autoNumberepository.Query(e => e.Id == TObject.Id && e.CompanyId == TObject.CompanyId).Select();
        //    DateTime _date = DateTime.UtcNow;

        //    if (_autoNumberSelect.Any())
        //    {
        //       // Log.Logger.ZInfo(AutoNumberLoggingValidation.Log_AutoNumber_Save_UpdateRequest_Message);
        //        AutoNumber _autoNumberNew = _autoNumberSelect.FirstOrDefault();

        //        _autoNumberNew.ModuleMasterId = TObject.ModuleMasterId;
        //        _autoNumberNew.EntityType = TObject.EntityType;
        //        _autoNumberNew.Description = TObject.Description;

        //        _autoNumberNew.CounterLength = TObject.CounterLength;
        //        _autoNumberNew.MaxLength = TObject.MaxLength;
        //        _autoNumberNew.Preview = TObject.Preview;
        //        _autoNumberNew.Variables = TObject.Variables;

        //        _autoNumberNew.Status = TObject.Status;
        //        _autoNumberNew.ModifiedBy = TObject.ModifiedBy;
        //        _autoNumberNew.ModifiedDate = _date;

        //        //if (_autoNumberNew.Format != TObject.Format || _autoNumberNew.StartNumber != TObject.StartNumber)
        //        //{
        //        if (TObject.StartNumber == null)
        //        {
        //            _autoNumberNew.GeneratedNumber = 1.ToString();
        //        }
        //        else if (TObject.StartNumber == 0)
        //        {
        //            _autoNumberNew.GeneratedNumber = 1.ToString();
        //        }
        //        if (TObject.StartNumber != null && TObject.StartNumber != 0)
        //            _autoNumberNew.GeneratedNumber = (TObject.StartNumber).ToString();
        //        //}
        //        _autoNumberNew.Format = TObject.Format;
        //        _autoNumberNew.StartNumber = TObject.StartNumber;
        //        _autoNumberNew.Reset = TObject.Reset;

        //        bool strValue = TObject.Format.Contains("{companycode}");
        //        if (strValue)
        //        {
        //            _autoNumberNew.IsResetbySubsidary = true;
        //        }
        //        else
        //        {
        //            _autoNumberNew.IsResetbySubsidary = false;
        //        }

        //        _autoNumberNew.ObjectState = ObjectState.Modified;

        //        var _autonumberCompany = _autoNumberepository.GetRepository<AutoNumberCompany>().Query(a => a.AutonumberId == TObject.Id).Select();
        //        if (_autonumberCompany.Any())
        //        {
        //            AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
        //            _autoNumberCompanyNew.GeneratedNumber = TObject.GeneratedNumber;
        //            _autoNumberCompanyNew.AutonumberId = TObject.Id;

        //            _autoNumberCompanyNew.ObjectState = ObjectState.Modified;

        //        }
        //        else
        //        {
        //           // Log.Logger.ZInfo(AutoNumberLoggingValidation.Log_AutoNumber_Save_NewRequest_Message);
        //            AutoNumberCompany _autoNumberCompanyNew1 = new AutoNumberCompany();

        //            _autoNumberCompanyNew1.GeneratedNumber = TObject.GeneratedNumber;
        //            _autoNumberCompanyNew1.AutonumberId = TObject.Id;
        //            _autoNumberCompanyNew1.Id = Guid.NewGuid();

        //            _autoNumberCompanyNew1.ObjectState = ObjectState.Added;
        //        }
        //        _autoNumberepository.Update(_autoNumberNew);

        //    }
        //    else
        //    {
        //        //long value = 0;
        //        //if (GetAll().Any())
        //        //{
        //        //    value = Convert.ToInt64(GetAll().Max(c => c.Id));
        //        //}

        //        TObject.UserCreated = TObject.UserCreated;
        //        TObject.CreatedDate = _date;

        //        TObject.Id = TObject.Id;
        //        TObject.ObjectState = ObjectState.Added;
        //        _autoNumberepository.Insert(TObject);

        //    }

        //    try
        //    {
        //        _unitOfWork.SaveChanges();
        //        //if (_autoNumberSelect.Any())
        //        //{
        //        //    DomainEventChannel.Raise(new AutoNumberUpdated(TObject));
        //        //}
        //        //else
        //        //{
        //        //    DomainEventChannel.Raise(new AutoNumberCreated(TObject));
        //        //}
        //        //Log.Logger.ZInfo(AutoNumberLoggingValidation.Log_AutoNumber_Save_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Logger.ZInfo(AutoNumberLoggingValidation.Log_AutoNumber_Save_Exception_Message);
        //       // Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //    return TObject;
        //} 

        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, List<string> lstBill, string companyCode, string serviceGroupCode)
        {

            AutoNumber _autoNo = _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == Type).Select().FirstOrDefault();
            string generatedAutoNumber = "";

            if (_autoNo != null)
                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                    _autoNo.GeneratedNumber, companyId, lstBill, companyCode, serviceGroupCode);

            FillAutoNumber(_autoNo);

            return generatedAutoNumber;
        }

        #region FillMethods
        private void FillAutoNumber(AutoNumber _autoNo)
        {
            if (_autoNo != null)
            {
                _autoNo.GeneratedNumber = value;
                _autoNo.ObjectState = ObjectState.Modified;
                _autoNumberepository.Update(_autoNo);

                var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
                if (_autonumberCompany.Any())
                {
                    AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AutoNumberCompany _autoNumberCompanyNew = new AutoNumberCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
            }
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, List<string> lstBill, string Companycode = null, string ServiceGroup = null)
        {
            string OutputNumber = "";
            string counter = "";
            string companyFormatHere = companyFormatFrom.ToUpper();

            if (companyFormatHere.Contains("{YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
            }
            else if (companyFormatHere.Contains("{MM/YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            if (companyFormatHere.Contains("{SERVICEGROUP}"))
            {
                companyFormatHere = companyFormatHere.Replace("{SERVICEGROUP}", ServiceGroup);
            }
            double val = 0;

            //lstBill = _caseService.GetAllCases(companyId);
            if (lstBill.Any())
            {
                AutoNumber autonumber = _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == Type).Select().FirstOrDefault();
                foreach (var bill in lstBill)
                {

                    if (bill != autonumber.Preview)
                        val = Convert.ToInt32(IncreamentVal);
                    else
                    {
                        val = Convert.ToInt32(IncreamentVal) + 1;
                        break;
                    }
                }
            }
            else
            {
                val = Convert.ToInt32(IncreamentVal);
            }

            if (counterLength == 1)
                counter = string.Format("{0:0}", val);
            else if (counterLength == 2)
                counter = string.Format("{0:00}", val);
            else if (counterLength == 3)
                counter = string.Format("{0:000}", val);
            else if (counterLength == 4)
                counter = string.Format("{0:0000}", val);
            else if (counterLength == 5)
                counter = string.Format("{0:00000}", val);
            else if (counterLength == 6)
                counter = string.Format("{0:000000}", val);
            else if (counterLength == 7)
                counter = string.Format("{0:0000000}", val);
            else if (counterLength == 8)
                counter = string.Format("{0:00000000}", val);
            else if (counterLength == 9)
                counter = string.Format("{0:000000000}", val);
            else if (counterLength == 10)
                counter = string.Format("{0:0000000000}", val);

            value = counter;
            OutputNumber = companyFormatHere + counter;

            return OutputNumber;
        }
        #endregion FillMethods


        #region Common_Auto_number
        public string GetAutonumber(long companyId, string entityType, string connectionString)
        {
            string docNo = null;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("Common_GenerateDocNo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@CursorName", "Bean Cursor");
                    cmd.Parameters.AddWithValue("@EntityType", entityType);
                    cmd.Parameters.AddWithValue("@IsAdd", false);
                    cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 100);
                    cmd.Parameters["@DocNo"].Direction = ParameterDirection.Output;
                    cmd.ExecuteScalar();
                    docNo = cmd.Parameters["@DocNo"].Value != DBNull.Value ? Convert.ToString(cmd.Parameters["@DocNo"].Value) : null;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return docNo;
        }
        #endregion

        public bool? GetAutoNumberIsEditable(long companyId, string entityType)
        {
            return _autoNumberepository.Query(e => e.CompanyId == companyId && e.EntityType == entityType).Select(c => c.IsEditable).FirstOrDefault();
        }

        public string GetAutonumberInAddMode(long companyId, string entityType, string connectionString)
        {
            string docNo = null;
            //bool? isEditable;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Common_GenerateDocNo", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        cmd.Parameters.AddWithValue("@CursorName", "Bean Cursor");
                        cmd.Parameters.AddWithValue("@EntityType", entityType);
                        cmd.Parameters.AddWithValue("@IsAdd", true);
                        cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@DocNo"].Direction = ParameterDirection.Output;
                        cmd.ExecuteScalar();
                        docNo = cmd.Parameters["@DocNo"].Value != DBNull.Value ? Convert.ToString(cmd.Parameters["@DocNo"].Value) : null;


                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return docNo;
        }
    }
}

