using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Infra;
using AppsWorld.BillModule.Infra.Resources;
using AppsWorld.BillModule.Models;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ziraff.FrameWork.Logging;

namespace AppsWorld.BillModule.Service
{
    public class HangFireService
    {
        //for DocumentScreenSave
        public void SaveBillRelatedFile(BillModel Tobject, string oldDocNo, Guid oldEntityId, bool isAdd, Bill _bill, string name)
        {
            try
            {
                if (Tobject.DocSubType != DocTypeConstants.Payroll)
                {
                    if (isAdd == true)
                    {
                        //if (Tobject.DocSubType == DocTypeConstants.Claim && Tobject.IsExternal == true)
                        //{
                        if (!isFileExist(Tobject.CompanyId, Tobject.EntityId.ToString(), Tobject.EntityId.ToString()))
                        {

                            saveScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), name, Tobject.UserCreated, DateTime.UtcNow, true, Tobject.CompanyId, "Entities", Tobject.EntityId.ToString());
                            //string name=
                            //saveScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, isFirst, beDTO.CompanyId, MasterModuleValidations.Entities, entityId.ToString());
                        }
                        //}
                        //else
                        saveScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, isAdd, Tobject.CompanyId, "Bills", Tobject.EntityId.ToString());
                    }
                    else
                    {
                        if (oldEntityId != Tobject.EntityId)
                        {
                            if (!isFileExist(Tobject.CompanyId, Tobject.EntityId.ToString(), Tobject.EntityId.ToString()))
                            {
                                saveScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), name, Tobject.UserCreated, DateTime.UtcNow, true, Tobject.CompanyId, "Entities", Tobject.EntityId.ToString());
                            }
                            //else
                            saveScreenRecords(Tobject.Id.ToString(), oldEntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
                        }
                        if (oldDocNo != Tobject.DocNo)
                            saveScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
                    }
                    if (Tobject.DocSubType != DocTypeConstants.Claim)
                    {
                        if (Tobject.TileAttachments != null && Tobject.TileAttachments.Any())
                        {
                            foreach (var fileAttachMent in Tobject.TileAttachments)
                            {
                                //SaveBillScreenFiles(fileAttachMent.FileId, fileAttachMent.Name, fileAttachMent.FileSize, Tobject.EntityId.ToString(), _bill.Id.ToString(), _bill.Id.ToString(), Tobject.CompanyId, fileAttachMent.RecordStatus, fileAttachMent.Description, Tobject.UserCreated, Tobject.ModifiedBy, fileAttachMent.IsSystem);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Issues_in_Bill_Folder_creation);
            }
        }


        private bool SaveBillScreenFiles(string FileId, string fileName, string fileSize, string featureId, string recordId, string referenceId, long companyId, string recordStatus, string desc, string createdBy, string modifiedBy, bool isSystem)
        {
            List<DocTilesFilesVM> lstTilesVm = new List<DocTilesFilesVM>();
            DocTilesFilesVM tilesFileVM = new DocTilesFilesVM();
            tilesFileVM.FileId = FileId;
            tilesFileVM.Name = fileName;
            tilesFileVM.FileSize = fileSize;
            tilesFileVM.FeatureId = featureId;
            tilesFileVM.RecordId = recordId;
            tilesFileVM.ReferenceId = referenceId;
            tilesFileVM.CompanyId = companyId;
            tilesFileVM.IsSystem = isSystem;
            tilesFileVM.ModuleName = "Bean Cursor";
            tilesFileVM.TabName = "Bills";
            tilesFileVM.Status = RecordStatusEnum.Active;
            tilesFileVM.RecordStatus = recordStatus;
            tilesFileVM.CreatedBy = createdBy;
            tilesFileVM.ModifiedBy = modifiedBy;
            if (recordStatus == "Added")
                tilesFileVM.CreatedDate = DateTime.UtcNow;
            else
                tilesFileVM.ModifiedDate = DateTime.UtcNow;
            lstTilesVm.Add(tilesFileVM);
            var json = RestSharpHelper.ConvertObjectToJason(lstTilesVm);
            try
            {
                object obj = lstTilesVm;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.ZPost(url, "api/document/savesscreenfiles", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<DocTilesFilesVM>(response.Content);
                    //lstTilesVm = data;
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


        public bool saveScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldEntityId)
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
            var json = RestHelper.ConvertObjectToJason(screenRecords);
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

        public bool isFileExist(long companyId, string recordId, string featureId)
        {
            bool isExist = false;
            try
            {
                List<List<string, string>> lstParams = new List<List<string, string>>();
                lstParams.Add(new List<string, string>() { Name = "companyId", Value = companyId.ToString() });
                lstParams.Add(new List<string, string>() { Name = "featureId", Value = featureId.ToString() });
                //object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestHelper.RestGet(url, "api/document/isfolderexists", lstParams);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<bool>(response.Content);
                    //screenRecords = data;
                    if (data != null)
                    {
                        if (data)
                            isExist = true;
                        else
                            isExist = false;
                    }
                }
                else
                {
                    throw new Exception(response.Content);
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return isExist;
        }
        public bool saveEntityScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldFeatureId)
        {
            ScreenRecordsSave screenRecords = new ScreenRecordsSave();
            screenRecords.ReferenceId = refrenceId;
            screenRecords.FeatureId = featureId;
            screenRecords.RecordId = recordId;
            screenRecords.recordName = recordName;
            screenRecords.UserName = userName;
            screenRecords.CreatedDate = date.Value;
            screenRecords.isAdd = isAdd;
            screenRecords.CursorName = "Bean Cursor";
            screenRecords.ScreenName = screenName;
            screenRecords.CompanyId = comapnyid;
            screenRecords.OldFeatureId = oldFeatureId;
            var json = RestHelper.ConvertObjectToJason(screenRecords);
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

    }
}
