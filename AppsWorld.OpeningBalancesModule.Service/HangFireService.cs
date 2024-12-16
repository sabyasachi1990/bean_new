using AppsWorld.CommonModule.Infra;
using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public class HangFireService
    {
        //OpeningBalancesContext contex = new OpeningBalancesContext();
        public void UpdateCustBalance(long companyId, string entityIds, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand cmd = new SqlCommand("Bean_Update_CustBalance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", companyId.ToString());
            cmd.Parameters.AddWithValue("@entitIds", entityIds);
            int res = cmd.ExecuteNonQuery();
            con.Close();
        }
        public void SaveScreenFolders(List<OpeningBalanceDetailLineItem> listOfDetailLineItems, bool isAdd, long companyId, List<OpeningBalanceLineItemModel> lstOBDLitemsModel)
        {
            foreach (OpeningBalanceDetailLineItem lItems in listOfDetailLineItems)
            {
                if (lstOBDLitemsModel.Any())
                {
                    var oldLineItem = lstOBDLitemsModel.Where(a => a.Id == lItems.Id).FirstOrDefault();
                    if (oldLineItem != null)
                    {
                        if (lItems.DocCredit > 0)
                        {
                            if (oldLineItem.EntityId != lItems.EntityId)
                                SaveScreenRecords(lItems.Id.ToString(), oldLineItem.EntityId.ToString(), lItems.EntityId.ToString(), lItems.DocumentReference, lItems.UserCreated, DateTime.UtcNow, false, companyId, oldLineItem.DocumentReference, oldLineItem.EntityId.ToString());
                            if (oldLineItem.DocumentReference != lItems.DocumentReference)
                                SaveScreenRecords(lItems.Id.ToString(), lItems.EntityId.ToString(), lItems.EntityId.ToString(), lItems.DocumentReference, lItems.UserCreated, DateTime.UtcNow, false, companyId, oldLineItem.DocumentReference, oldLineItem.EntityId.ToString());
                            if (oldLineItem.DocCredit < 0)
                            {
                                SaveScreenRecords(lItems.Id.ToString(), lItems.EntityId.ToString(), lItems.EntityId.ToString(), lItems.DocumentReference, lItems.UserCreated, DateTime.UtcNow, true, companyId, "Bills", oldLineItem.EntityId.ToString());
                            }
                        }
                        else
                        {
                            if (oldLineItem.DocumentReference != lItems.DocumentReference)
                                DeleteObLineItem(lItems.Id, oldLineItem.DocumentReference, companyId);
                            else
                                DeleteObLineItem(lItems.Id, lItems.DocumentReference, companyId);
                        }
                    }
                }
                else
                {
                    if (lItems.DocCredit > 0)
                        SaveScreenRecords(lItems.Id.ToString(), lItems.EntityId.ToString(), lItems.EntityId.ToString(), lItems.DocumentReference, lItems.UserCreated, DateTime.UtcNow, true, companyId, "Bills", lItems.EntityId.ToString());

                    //saveScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, isAdd, Tobject.CompanyId, "Bills", Tobject.EntityId.ToString());

                }
            }
        }

        public void SaveScreenFoldersNewRecent(Dictionary<Guid, string> lstEntity, List<OpeningBalanceDetailLineItem> listOfDetailLineItems, bool isAdd, long companyId)
        {
            foreach (OpeningBalanceDetailLineItem lItems in listOfDetailLineItems)
            {
                if (lItems != null)
                {
                    if (lItems.DocCredit < 0)
                    {
                        DeleteAzureFiles(lstEntity, companyId, lItems.EntityId, lItems.DocumentReference);
                    }
                }
            }
        }

        public void SaveScreenFoldersNew(Dictionary<Guid, string> lstEntity, List<OpeningBalanceDetailLineItem> listOfDetailLineItems, bool isAdd, long companyId, List<OpeningBalanceLineItemModel> lstOBDLitemsModel)
        {
            foreach (OpeningBalanceDetailLineItem lItems in listOfDetailLineItems)
            {
                if (lstOBDLitemsModel.Any())
                {
                    var oldLineItem = lstOBDLitemsModel.Where(a => a.Id == lItems.Id).FirstOrDefault();
                    if (oldLineItem != null)
                    {
                        if (lItems.DocCredit > 0)
                        {
                        }
                        else
                        {
                            if (oldLineItem.DocumentReference != lItems.DocumentReference)
                            {
                                DeleteAzureFiles(lstEntity, companyId, oldLineItem.EntityId, oldLineItem.DocumentReference);
                            }
                            else
                            {
                                DeleteAzureFiles(lstEntity, companyId, lItems.EntityId, lItems.DocumentReference);
                            }
                        }
                    }
                }
            }
        }

        private static void DeleteAzureFiles(Dictionary<Guid, string> lstEntity, long companyId, Guid? entityId, string docNo)
        {
            DeleteFileModel deleteViewModel = new DeleteFileModel();
            deleteViewModel.FileShareName = companyId.ToString("000");
            deleteViewModel.CompanyId = companyId;
            deleteViewModel.CursorName = CommonConstant.BeanCursor;
            deleteViewModel.Path = "Entities" + "/" + lstEntity.Where(s => s.Key == (entityId)).Select(a => a.Value) + "/" + "Bills" + docNo;
            var json = RestSharpHelper.ConvertObjectToJason(deleteViewModel);
            string url = ConfigurationManager.AppSettings["AzureUrl"];
            var response = RestSharpHelper.ZPost(url, "api/storage/deleteccfolder", json);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<DeleteViewModel>(response.Content);
            }
        }
        public void DeleteOBDeleteLineItemNew(Dictionary<Guid, string> lstEntity, long companyId, List<OpeningBalanceLineItemModel> listOfDetailLineItems)
        {
            foreach (var item in listOfDetailLineItems)
            {
                var oldLineItem = listOfDetailLineItems.Where(a => a.Id == item.Id).FirstOrDefault();
                if (oldLineItem != null)
                {
                    DeleteFileModel deleteViewModel = new DeleteFileModel();
                    deleteViewModel.CompanyId = companyId;
                    deleteViewModel.FileShareName = companyId.ToString("000");
                    deleteViewModel.Path = "Entities" + "/" + lstEntity.Where(s => s.Key == (oldLineItem.EntityId)).Select(a => a.Value) + "/" + "Bills" + oldLineItem.DocumentReference;
                    deleteViewModel.CursorName = CommonConstant.BeanCursor;
                    var json = RestSharpHelper.ConvertObjectToJason(deleteViewModel);
                    try
                    {
                        object obj = deleteViewModel;
                        string url = ConfigurationManager.AppSettings["AzureUrl"];
                        var response = RestSharpHelper.ZPost(url, "api/storage/deleteccfolder", json);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var data = JsonConvert.DeserializeObject<DeleteViewModel>(response.Content);
                        }
                        else
                        {
                            throw new Exception(response.Content);
                        }
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        //return false;
                    }
                }
            }
        }

        public void DeleteOBDeleteLineItemNewRecent(Dictionary<Guid, string> lstEntity, long companyId, List<OpeningBalanceDetailLineItem> listOfDetailLineItems)
        {
            foreach (var item in listOfDetailLineItems)
            {
                var oldLineItem = listOfDetailLineItems.Where(a => a.Id == item.Id).FirstOrDefault();
                if (oldLineItem != null)
                {
                    DeleteFileModel deleteViewModel = new DeleteFileModel();
                    deleteViewModel.CompanyId = companyId;
                    deleteViewModel.FileShareName = companyId.ToString("000");
                    deleteViewModel.Path = "Entities" + "/" + lstEntity.Where(s => s.Key == (oldLineItem.EntityId)).Select(a => a.Value) + "/" + "Bills" + oldLineItem.DocumentReference;
                    deleteViewModel.CursorName = CommonConstant.BeanCursor;
                    var json = RestSharpHelper.ConvertObjectToJason(deleteViewModel);
                    try
                    {
                        object obj = deleteViewModel;
                        string url = ConfigurationManager.AppSettings["AzureUrl"];
                        var response = RestSharpHelper.ZPost(url, "api/storage/deleteccfolder", json);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var data = JsonConvert.DeserializeObject<DeleteViewModel>(response.Content);
                        }
                        else
                        {
                            throw new Exception(response.Content);
                        }
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                        //return false;
                    }
                }
            }
        }
        public bool SaveScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldEntityId)
        {
            ScreenRecordsSave screenRecords = new ScreenRecordsSave();
            screenRecords.ReferenceId = refrenceId;
            screenRecords.FeatureId = featureId;
            screenRecords.RecordId = recordId;
            screenRecords.recordName = recordName;
            screenRecords.UserName = userName;
            screenRecords.Date = date.Value;
            screenRecords.isAdd = isAdd;
            screenRecords.CursorName = "Bean Cursor";
            screenRecords.ScreenName = screenName;
            screenRecords.CompanyId = comapnyid;
            screenRecords.OldFeatureId = oldEntityId;
            screenRecords.CreatedDate = date.Value;
            screenRecords.UserCreated = userName;
            var json = RestSharpHelper.ConvertObjectToJason(screenRecords);
            try
            {
                object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.ZPost(url, "api/document/savescreenfolders", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<ScreenRecordsSave>(response.Content);
                    screenRecords = data;
                }
                else
                {
                    throw new Exception(response.Content);
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public void DeleteOBDeleteLineItem(Dictionary<Guid, string> lstObLineitem, long companyId)
        {
            foreach (var item in lstObLineitem)
            {
                DeleteViewModel deleteViewModel = new DeleteViewModel();
                deleteViewModel.id = item.Key;
                deleteViewModel.Name = item.Value;
                deleteViewModel.CompanyId = companyId;

                var json = RestSharpHelper.ConvertObjectToJason(deleteViewModel);
                try
                {
                    object obj = deleteViewModel;
                    string url = ConfigurationManager.AppSettings["AdminUrl"];
                    var response = RestSharpHelper.ZPost(url, "api/document/deletescreenfolders", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<DeleteViewModel>(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                    //return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    //return false;
                }
            }
        }
        public void DeleteObLineItem(Guid id, string name, long companyId)
        {
            DeleteViewModel deleteViewModel = new DeleteViewModel();
            deleteViewModel.id = id;
            deleteViewModel.Name = name;
            deleteViewModel.CompanyId = companyId;

            var json = RestSharpHelper.ConvertObjectToJason(deleteViewModel);
            try
            {
                object obj = deleteViewModel;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.ZPost(url, "api/document/deletescreenfolders", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<DeleteViewModel>(response.Content);
                }
                else
                {
                    throw new Exception(response.Content);
                }
                //return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                //return false;
            }

        }
    }
}
