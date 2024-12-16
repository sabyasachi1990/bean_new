using System.Collections.Generic;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using AppsWorld.Framework;
using Repository.Pattern.Infrastructure;
using FrameWork;
//using Domain.Events.Model.Events;
using System.Configuration;
using System.Runtime.Remoting.Services;
using Serilog;
using Logger;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Entities.Models;
using AppsWorld.CommonModule.Infra;
using System.Collections.Specialized;
using Ziraff.FrameWork;
using System.Data.Entity.Validation;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using ServiceStack.Logging;
using System.Net;
//using Domain.Events.Model.Events;
namespace AppsWorld.CommonModule.Application
{
    public class CommonApplicationService
    {
        private readonly IAutoNumberService _autoNumberService;
        private readonly IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ICompanyService _companyService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IGSTSettingService _gSTSettingService;
        private readonly IDocRepositoryService _docRepositoryService;
        //private readonly IReferenceFilesService _refrenceFilesService;
        ICommonModuleUnitOfWorkAsync _unitOfWorkAsync;
        SqlConnection con = null;
        SqlCommand cmd = null;
        string query = string.Empty;
        SqlDataReader dr = null;
        public CommonApplicationService(IAutoNumberService autoNumberService, IAutoNumberCompanyService autoNumberCompanyService, IChartOfAccountService chartOfAccountService, ICompanyService companyService, IFinancialSettingService financialSettingService, IGSTSettingService gSTSettingService, IDocRepositoryService docRepositoryService, /*IReferenceFilesService refrenceFilesService,*/ ICommonModuleUnitOfWorkAsync unitOfWorkAsync)
        {
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._chartOfAccountService = chartOfAccountService;
            _companyService = companyService;
            this._financialSettingService = financialSettingService;
            this._gSTSettingService = gSTSettingService;
            _docRepositoryService = docRepositoryService;
            //_refrenceFilesService = refrenceFilesService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public string GenerateAutoNumberForType(AutoNumberViewModel autoNumberViewModel)
        {
            try
            {
                //Log.Logger.ZInfo(ValidationMessageConstants.Log_GenerateAutoNumberForType_Start_Message);
                long companyId = autoNumberViewModel.CompanyId;
                string Type = autoNumberViewModel.Type;
                List<string> lstSystemRefNos = autoNumberViewModel.SystemRefNo;
                string companyCode = autoNumberViewModel.CompanyCode;
                string serviceGroupCode = autoNumberViewModel.ServiceGroupCode;

                return _autoNumberService.GenerateAutoNumberForType(companyId, Type, lstSystemRefNos, companyCode, serviceGroupCode);
            }
            catch (Exception ex)
            {
                //Log.Logger.ZInfo(ValidationMessageConstants.Log_GenerateAutoNumberForType_Exception_Message);
                throw ex;
            }
        }

        #region GL_AR_AP_OR_OP_Reporting_Call
        public List<RepotringModel> GetGLAccounts(GLAccountM accounts, string ConnectionString)
        {
            List<RepotringModel> lstReportingModel = new List<RepotringModel>();
            List<string> Names = new List<string>();
            if (accounts.COANames == null || accounts.COANames == string.Empty)
            {
                List<ChartOfAccount> lstCOAs = _chartOfAccountService.GetAllChartOfAccounts(0);
                if (lstCOAs.Any())
                {
                    Names = lstCOAs.Select(s => s.Name).ToList();
                }
            }
            //List<string> deserielize = JsonConvert.DeserializeObject<List<string>>(coaNames);
            string coa = string.Empty;
            foreach (var item in Names)
            {
                if (item.Length > 0)
                {
                    coa += ",";
                }
                //coa += "'"+item + "'";
                coa += item;
            }
            if (coa != null && coa != string.Empty)
                coa = coa.Remove(0, 1);
            try
            {
                con = new SqlConnection(ConnectionString);
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd = new SqlCommand("Rpt_General_Ledger_with_FinancialYear_Updated_API", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyValue", accounts.CompanyId);
                cmd.Parameters.AddWithValue("@FromDate", accounts.FromDate.ToString());
                //cmd.Parameters.Add("@FromDate", SqlDbType.DateTime2).Value = fromDate;
                //cmd.Parameters.Add("@ToDate", SqlDbType.DateTime2).Value = toDate;
                cmd.Parameters.AddWithValue("@ToDate", accounts.ToDate.ToString());
                cmd.Parameters.AddWithValue("@COA", (accounts.COANames == null || accounts.COANames == string.Empty) ? coa : accounts.COANames);
                cmd.Parameters.AddWithValue("@ServiceCompany", accounts.ServiceEntityName);
                cmd.Parameters.AddWithValue("@ExcludeClearedItems", accounts.ExcludeCreatedItems);


                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                SqlDataReader value = cmd.ExecuteReader();
                int? count = value.FieldCount;
                int? count1 = value.VisibleFieldCount;
                while (value.Read())
                {
                    RepotringModel model = new RepotringModel();
                    FillRecurringModel(value, model);
                    lstReportingModel.Add(model);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstReportingModel;
        }


        #endregion GL_AR_AP_OR_OP_Reporting_Call

        #region GET History Document
        public List<DocumentHistoryModelVM> GetAllDocumentHistory(long companyId, string docType, Guid Id, string ConnectionString)
        {
            List<DocumentHistoryModelVM> lstHisDoc = new List<DocumentHistoryModelVM>();
            try
            {
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_Document_History", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@DocType", docType);
                    cmd.Parameters.AddWithValue("@Id", Id);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstHisDoc.Add(new DocumentHistoryModelVM()
                            {
                                CompanyId = Convert.ToInt64(dr["CompanyId"]),
                                DocSubType = dr["DocSubType"] != DBNull.Value ? Convert.ToString(dr["DocSubType"]) : null,
                                DocumentId = dr["DocumentId"] != DBNull.Value ? Guid.Parse(dr["DocumentId"].ToString()) : (Guid?)null,
                                TransactionId = dr["TransactionId"] != DBNull.Value ? Guid.Parse(dr["TransactionId"].ToString()) : (Guid?)null,
                                DocState = dr["DocState"] != DBNull.Value ? Convert.ToString(dr["DocState"]) : null,
                                DocAmount = Convert.ToDecimal(dr["DocAmount"]),
                                BaseAmount = Convert.ToDecimal(dr["BaseAmount"]),
                                DocBalanaceAmount = Convert.ToDecimal(dr["DocBalanaceAmount"]),
                                BaseBalanaceAmount = Convert.ToDecimal(dr["BaseBalanaceAmount"]),
                                ExchangeRate = dr["ExchangeRate"] != DBNull.Value ? Convert.ToDecimal(dr["ExchangeRate"]) : (decimal?)null,
                                DocCurrency = Convert.ToString(dr["DocCurrency"]),
                                StateChangedBy = Convert.ToString(dr["StateChangedBy"]),
                                StateChangedDate = dr["StateChangedDate"] != DBNull.Value ? Convert.ToDateTime(dr["StateChangedDate"]) : (DateTime?)null,
                                DocType = Convert.ToString(dr["DocType"]),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return lstHisDoc.OrderByDescending(s => s.StateChangedDate).ToList();
        }
        #endregion GET History Document

        #region Private_Block
        private static void FillRecurringModel(SqlDataReader value, RepotringModel model)
        {
            model.AccountName = Convert.ToString(value[1]);
            model.DocType = Convert.ToString(value[3]);
            model.DocSubType = Convert.ToString(value[4]);
            model.AccountClass = Convert.ToString(value[5]);
            model.AccountCategory = Convert.ToString(value[6]);
            model.AccountSubCategory = Convert.ToString(value[7]);
            model.AccountType = Convert.ToString(value[8]);
            model.AccountNature = Convert.ToString(value[2]);
            model.AccountCode = Convert.ToString(value[0]);
            model.DocDescription = Convert.ToString(value[9]);
            model.DocDate = value[10] != DBNull.Value ? Convert.ToDateTime(value[10]) : (DateTime?)null;
            model.DocNo = Convert.ToString(value[11]);
            model.SystemRefNo = Convert.ToString(value[12]);
            model.EntityName = Convert.ToString(value[14]);
            //var s =  value.GetValue(13);
            model.BaseDebit = value[15] != DBNull.Value ? Convert.ToDecimal(value["BaseDebit"]) : (decimal?)null;
            //var s1 =  value.GetName(13);
            model.BaseCredit = value[16] != DBNull.Value ? Convert.ToDecimal(value[16]) : (decimal?)null;
            model.BaseBalance = Convert.ToDecimal(value[17]);
            model.ExchangeRate = value[18] != DBNull.Value ? Convert.ToDecimal(value[18]) : (decimal?)null;
            model.BaseCurrency = Convert.ToString(value[19]);
            model.DocCurrency = Convert.ToString(value[20]);
            model.DocDebit = value[21] != DBNull.Value ? Convert.ToDecimal(value[21]) : (decimal?)null;
            model.DocCredit = value[22] != DBNull.Value ? Convert.ToDecimal(value[22]) : (decimal?)null;
            model.DocBalance = value[23] != DBNull.Value ? Convert.ToDecimal(value[23]) : (decimal?)null;
            model.CorrAccountName = Convert.ToString(value[28]);
            model.EntityType = Convert.ToString(value[29]);
            model.PONo = Convert.ToString(value[31]);
            model.Nature = Convert.ToString(value[32]);
            model.CreditTermsDays = value[33] != DBNull.Value ? Convert.ToInt64(value[33]) : (long?)null;
            model.DueDate = value[34] != DBNull.Value ? Convert.ToDateTime(value[34]) : (DateTime?)null;
            model.ItemCode = Convert.ToString(value[37]);
            model.ItemDescription = Convert.ToString(value[38]);
            model.Qty = value[39] != DBNull.Value ? Convert.ToDouble(value[39]) : (double?)null;
            model.Unit = Convert.ToString(value[40]);
            model.UnitPrice = value[41] != DBNull.Value ? Convert.ToDecimal(value[41]) : (decimal?)null;
            model.DiscountType = Convert.ToString(value[42]);
            model.Discount = value[43] != DBNull.Value ? Convert.ToDouble(value[43]) : (double?)null;
            model.BankClearing = value[46] != DBNull.Value ? Convert.ToDateTime(value[46]) : (DateTime?)null;
            model.ModeOfReceipt = Convert.ToString(value[47]);
            model.ReversalDate = value[48] != DBNull.Value ? Convert.ToDateTime(value[48]) : (DateTime?)null;
            model.ReversalDocRefNo = Convert.ToString(value[50]);
            model.ClearingStatus = Convert.ToString(value[51]);
            model.BankReconcile = Convert.ToString(value[49]);
            model.CreatedBy = Convert.ToString(value[52]);
            model.CreatedOn = value[53] != DBNull.Value ? Convert.ToDateTime(value[53]) : (DateTime?)null;
            model.ModifiedBy = Convert.ToString(value[54]);
            model.ModifiedOn = value[55] != DBNull.Value ? Convert.ToDateTime(value[55]) : (DateTime?)null;
        }
        #endregion Private_Block

        /// <summary>
        /// To Limit the Grid Data associated Based on Service Entity
        /// </summary>
        /// <param name="username"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public IQueryable<ISvcEntityFilter> LimitUserPermissionBySe(NameValueCollection headers, long companyId, IQueryable<ISvcEntityFilter> data)
        {
            string authInformation = headers.GetValues("AuthInformation").FirstOrDefault();
            AuthInformation authInfo = JsonConvert.DeserializeObject<AuthInformation>(authInformation.ToString());
            CompanyUser companyuser = _companyService.GetCompanyUserByCid_User(authInfo.userName, companyId);
            List<long> serviceCompIds = companyuser.ServiceEntities != null ? companyuser.ServiceEntities.Split(',').Select(z => long.Parse(z)).ToList() : null;
            List<string> compNameLst = serviceCompIds != null ? _companyService.GetServiceCompanyNameById(serviceCompIds) : null;
            return compNameLst != null ? data.Where(z => compNameLst.Contains(z.ServiceCompanyName)) : data;
            //return serviceCompIds;
        }

        #region Document Folder Rename

        public void ChangeFolderName(long companyId, string recordName, string oldname, string Entityname, string Screenname)
        {
            if (recordName.Equals(oldname + "."))
                return;
            try
            {
                FolderRenameModel folderModel = new FolderRenameModel();
                folderModel.FileShareName = Convert.ToInt32(companyId);
                folderModel.NewName = StringCharactersReplaceFunction(recordName);
                folderModel.CursorName = DocumentConstants.CursorName;
                folderModel.OldName = StringCharactersReplaceFunction(oldname);
                folderModel.Path = DocumentConstants.Entities + "/" + Entityname + "/" + Screenname + "/" + folderModel.OldName;
                var json = RestHelper.ConvertObjectToJason(folderModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureFuncUrl"];
                    var response = RestHelper.ZPost(url, "api/RenameFolder", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<string>(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    //return false;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            // return true;
        }

        public void ChangeFolderName(long companyId, string recordName, string oldname, string Screenname)
        {
            if (recordName.Equals(oldname + "."))
                return;
            try
            {
                FolderRenameModel folderModel = new FolderRenameModel();
                folderModel.FileShareName = Convert.ToInt32(companyId);
                folderModel.NewName = StringCharactersReplaceFunction(recordName);
                folderModel.CursorName = DocumentConstants.CursorName;
                folderModel.OldName = StringCharactersReplaceFunction(oldname);
                folderModel.Path = Screenname + "/" + folderModel.OldName;
                var json = RestHelper.ConvertObjectToJason(folderModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureFuncUrl"];
                    var response = RestHelper.ZPost(url, "api/RenameFolder", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<string>(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    //return false;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            // return true;
        }

        #endregion



        #region File Uplode
        //public void FillAttachements(List<DocRepositoryModel> attachments, Guid id, Guid? refernceId)
        //{
        //    if (attachments.Count > 0)
        //    {
        //        foreach (var attachment in attachments)
        //        {
        //            UpdateDocRepository(attachment, id);
        //            Referencefiles(attachments, id, refernceId.Value);
        //        }
        //    }
        //    _unitOfWorkAsync.SaveChanges();
        //}

        //public DocRepositoryModel UpdateDocRepository(DocRepositoryModel docRepModel, Guid id)
        //{
        //    try
        //    {
        //        if (docRepModel != null)
        //        {

        //            if (docRepModel.RecordStatus == "Added")
        //            {
        //                DocRepository docRepository = new DocRepository();
        //                FillRepositorymodelToEntity(docRepository, docRepModel);
        //                docRepository.Id = Guid.NewGuid();
        //                docRepository.TypeId = id;
        //                docRepository.ObjectState = ObjectState.Added;
        //                _docRepositoryService.InsertDoc(docRepository);

        //            }
        //            else if (docRepModel.RecordStatus == "Modified")
        //            {
        //                var doc = _docRepositoryService.GetAllDocRepIdCompanyDetailIdType(docRepModel.Id, id);
        //                if (doc != null)
        //                {

        //                    FillRepositorymodelToEntity(doc, docRepModel);

        //                    doc.ObjectState = ObjectState.Modified;
        //                    _docRepositoryService.UpdateDoc(doc);
        //                }
        //            }
        //            else if (docRepModel.RecordStatus == "Deleted")
        //            {

        //                var referenceFiles = _refrenceFilesService.GetReferenceFiles(docRepModel.MongoFilesId);
        //                if (referenceFiles != null)
        //                {
        //                    _refrenceFilesService.DeleteDoc(referenceFiles.Id.ToString());
        //                }
        //                if (docRepModel.TypeId != null)
        //                    DeleteDOCRepository(docRepModel.Id, docRepModel.TypeId, docRepModel.Type);

        //            }
        //            try
        //            {
        //                _unitOfWorkAsync.SaveChanges();
        //            }
        //            catch (DbEntityValidationException ex)
        //            {
        //                foreach (var eve in ex.EntityValidationErrors)
        //                {
        //                    Console.WriteLine(
        //                       "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                       eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                    foreach (var ve in eve.ValidationErrors)
        //                    {
        //                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                           ve.PropertyName, ve.ErrorMessage);
        //                    }
        //                }
        //                throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //    return docRepModel;
        //}

        //public void Referencefiles(List<DocRepositoryModel> lstdocRepositories, Guid id, Guid refernceId)
        //{

        //    foreach (var docrepository in lstdocRepositories)
        //    {


        //        List<ReferenceFiles> lstreferenceFiles = _refrenceFilesService.GetReferenceFiles(docrepository.MongoFilesId, id.ToString());
        //        if (lstreferenceFiles.Count() > 0)
        //        {
        //            if (docrepository.RecordStatus == "Modified")
        //            {
        //                foreach (var referenceFiles in lstreferenceFiles)
        //                {

        //                    referenceFiles.IsSystem = true;
        //                    referenceFiles.IsSuccess = true;
        //                    referenceFiles.FilePath = docrepository.DisplayFileName;
        //                    referenceFiles.CompanyId = docrepository.CompanyId;

        //                    referenceFiles.FileExt = docrepository.FileExt;
        //                    referenceFiles.FileSize = FilSize(docrepository);
        //                    referenceFiles.ModifiedBy = docrepository.ModifiedBy;

        //                    referenceFiles.ModifiedDate = DateTime.Now;
        //                    _refrenceFilesService.Update(referenceFiles);

        //                }

        //            }

        //        }
        //        else
        //        {
        //            if (docrepository.RecordStatus == "Added")
        //            {
        //                ReferenceFiles referenceFiles = new ReferenceFiles();
        //                referenceFiles.Id = ObjectId.GenerateNewId().ToString();
        //                referenceFiles.FileId = docrepository.MongoFilesId.ToString();
        //                referenceFiles.TabName = docrepository.TabName;
        //                referenceFiles.ModuleName = docrepository.ModuleName;
        //                referenceFiles.CompanyId = docrepository.CompanyId;
        //                referenceFiles.ReferenceId = id.ToString();
        //                referenceFiles.IsSystem = true;
        //                referenceFiles.Isfolder = false;
        //                referenceFiles.IsSuccess = true;

        //                referenceFiles.FileName = docrepository.DisplayFileName;
        //                referenceFiles.FilePath = docrepository.DisplayFileName;
        //                referenceFiles.FileSize = FilSize(docrepository);
        //                referenceFiles.FileExt = docrepository.FileExt;
        //                referenceFiles.FeatureId = refernceId.ToString();
        //                referenceFiles.UserCreated = docrepository.UserCreated;
        //                referenceFiles.CreatedDate = DateTime.Now;
        //                referenceFiles.Source = docrepository.TabName;
        //                _refrenceFilesService.Add(referenceFiles);
        //            }
        //        }

        //    }

        //}
        //private void FillRepositorymodelToEntity(DocRepository docRepository, DocRepositoryModel docRepModel)
        //{

        //    docRepository.CompanyId = docRepModel.CompanyId;
        //    docRepository.Type = docRepModel.TabName;
        //    docRepository.CreatedDate = docRepModel.CreatedDate;
        //    docRepository.Description = docRepModel.Description;
        //    docRepository.FilePath = docRepModel.FilePath;
        //    docRepository.UserCreated = docRepModel.UserCreated;
        //    docRepository.FileExt = docRepModel.FileExt;
        //    docRepository.FileSize = docRepModel.FileSize;
        //    docRepository.ModifiedBy = docRepModel.ModifiedBy;
        //    docRepository.ModifiedDate = docRepModel.ModifiedDate;
        //    docRepository.ModuleName = "Bean Cursor";
        //    docRepository.DisplayFileName = docRepModel.DisplayFileName;
        //    docRepository.NameofApprovalAuthority = docRepModel.NameofApprovalAuthority;
        //    docRepository.RecOrder = docRepModel.RecOrder;
        //    docRepository.Status = Framework.RecordStatusEnum.Active;
        //    docRepository.TypeIntId = docRepModel.TypeIntId;
        //    docRepository.MongoFilesId = docRepModel.MongoFilesId;
        //}

        //public void DeleteDOCRepository(Guid id, Guid companyDetailId, string type)
        //{
        //    try
        //    {
        //        var existimgDoc = _docRepositoryService.GetAllDocRepIdCompanyDetailIdType(id, companyDetailId);
        //        if (existimgDoc != null)
        //        {
        //            _docRepositoryService.DeleteDoc(existimgDoc);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        //private string FilSize(DocRepositoryModel fileinfo)
        //{
        //    double doub = 0.0;
        //    string a = null;
        //    double fileSize = Convert.ToDouble(fileinfo.FileSize);
        //    //double f1 = fileSize / 1024;
        //    double f3 = 0.0;
        //    if (fileSize >= 1024)
        //    {
        //        double f2 = fileSize / 1024;
        //        var files = Math.Round(f2, 0);
        //        a = (files + " " + "MB").ToString();
        //        if (f2 >= 1024)
        //        {
        //            f3 = f2 / 1024;
        //            var files2 = Math.Round(f3, 0);
        //            a = (files2 + " " + "GB").ToString();

        //        }
        //    }
        //    else
        //    {
        //        var files = Math.Round(fileSize, 0);
        //        a = (files + " " + "KB").ToString();
        //    }

        //    return a;
        //}
        #endregion

        #region Get All File Throw database
        //public List<DocRepositoryModel> GetAllDocRepByIdType(Guid typeId, string type)
        //{
        //    List<DocRepositoryModel> lst = new List<DocRepositoryModel>();
        //    try
        //    {
        //        lst = _docRepositoryService.GetAllDocRepByIdType(typeId, type);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lst;
        //}
        #endregion Get All File Throw database




        #region common_Auto_number_generatedNumber
        public string GetNextSequenceNo(long companyId, string entityType, string connectionString)
        {
            string docNo = null;
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
                        cmd.Parameters.AddWithValue("@IsAdd", false);
                        cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 100);
                        cmd.Parameters["@DocNo"].Direction = ParameterDirection.Output;
                        docNo = cmd.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return docNo;
        }
        #endregion


        #region Common_Edit_Save_For_All_Docs

        public CommonDocUpdateModel SaveCommonEditData(CommonDocUpdateModel model, string ConnectioString)
        {
            try
            {
                List<AccountDesc> desc = new List<AccountDesc>();
                if (model.AccountDescDetails != null)
                {
                    foreach (var item in model.AccountDescDetails)
                    {
                        AccountDesc d = new AccountDesc();
                        d.Id = item.Key;
                        d.Dec = item.Value;
                        desc.Add(d);
                    }
                }
                var Jobj = JsonConvert.SerializeObject(desc);
                using (con = new SqlConnection(ConnectioString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_State_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentId", model.Id);
                    cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                    cmd.Parameters.AddWithValue("@DocType", model.DocType);
                    cmd.Parameters.AddWithValue("@Description", (object)model.DocDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoSupportingDocument", model.NoSupportingDocument);
                    cmd.Parameters.AddWithValue("@IsNoSupportingDocs", model.IsNoSupportingDoc);
                    cmd.Parameters.AddWithValue("@PoNo", (object)model.PONo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModifiedBy", (object)model.ModifiedBy ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Version", (object)model.Version ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccountDescription", model.AccountDescDetails != null ? Jobj : string.Empty);
                    cmd.CommandTimeout = 30;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return model;
        }

        #endregion Common_Edit_Save_For_All_Docs

        public string StringCharactersReplaceFunction(string name)
        {
            name = name.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                  .Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
            Regex re = new Regex("[.]*(?=[.]$)");
            name = re.Replace(name, "");
            name = name.EndsWith(".") ? name.Remove(name.Length - 1) : name;
            name = name.Trim();
            return name;
        }

        #region CommonLockedCall
        public Guid SaveCommonLockedCall(CommonLockModel commonLockModel, string connectionString)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("Bean_Update_Locked_Flag", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentId", commonLockModel.Id);
                    cmd.Parameters.AddWithValue("@CompanyId", commonLockModel.CompanyId);
                    cmd.Parameters.AddWithValue("@DocType", commonLockModel.DocType);
                    cmd.Parameters.AddWithValue("@DocSubType", commonLockModel.DocSubType);
                    cmd.Parameters.AddWithValue("@IsLocked", commonLockModel.IsLocked);
                    cmd.Parameters.AddWithValue("@ModifiedBy", (object)commonLockModel.ModifiedBy ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Version", (object)commonLockModel.Version ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                    return commonLockModel.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Common_Void_Delete_Call
        public string DeleteTransaction(CommonDeleteModel commonDeleteModel,string connectionString)
        {
            try
            {
                using(con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_Delete_Transaction_History", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Listofids", commonDeleteModel.TransactionIds);
                    cmd.Parameters.AddWithValue("@CompanyId", commonDeleteModel.CompanyId);
                    cmd.Parameters.AddWithValue("@DocType", commonDeleteModel.DocType);
                    cmd.Parameters.AddWithValue("@DocSubType", commonDeleteModel.DocSubType);
                    cmd.Parameters.AddWithValue("@Deletedby", commonDeleteModel.DeletedBy);
                    cmd.ExecuteNonQuery();
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
                return "Deleted successfully";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

    }

    public class AuthInformation
    {
        public long? companyId { get; set; }
        public string userName { get; set; }
        public long? moduleDetailId { get; set; }
    }




}

