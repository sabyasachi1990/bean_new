using AppsWorld.Bean.WebAPI.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;
using Ziraff.FrameWork.Logging;

namespace AppsWorld.Bean.WebAPI.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RolePermissionAttribute : BaseActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var roleStart = DateTime.UtcNow;
            try
            {
                var controller = actionContext.ControllerContext.Controller as BaseController;
                AuthInformation authInformationModel = controller.AuthInformation;
                var companyid = authInformationModel.companyId == null ? 0 : authInformationModel.companyId.Value;

                if (System.Configuration.ConfigurationManager.AppSettings[Constant.ScreenPermissionEnable].ToString().ToLower() == ConstantVariables.True && companyid != 0)
                {
                    var result = GetUserPermission(authInformationModel.companyId ?? 0, authInformationModel.userName, authInformationModel.moduleDetailId ?? 0, authInformationModel.Values.FirstOrDefault(t => t.Key == "PermissionName").Value);

                    //var milliSecInrole = (DateTime.UtcNow - roleStart).TotalMilliseconds;
                    LoggingHelper.LogMessage("RolePermission", $"ElapsedMilliseconds: {(DateTime.UtcNow - roleStart).TotalMilliseconds}, RolePermission execution time.");

                    if (!result)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(
                 HttpStatusCode.Unauthorized, new { Message = Constant.RoleUnAuthorizedExceptionMessage },
                 actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("RolePermission", ex, $"{ex.Message}");
                actionContext.Response = new HttpResponseMessage();
                actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Unauthorized, new { Message = Constant.RoleUnAuthorizedExceptionMessage },
                actionContext.ControllerContext.Configuration.Formatters.JsonFormatter);
            }
        }
        public bool GetUserPermission(long companyId, string username, long moduledetailid, string permisionName)
        {
            try
            {
                SqlConnection cn = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                string strcmd = $"select Top(1) Permissions from Auth.UserPermissionNew where ModuleDetailId={moduledetailid} and CompanyUserId=(select Id from Common.CompanyUser where CompanyId = {companyId} and Username = '{username}')";

                SqlCommand cmd = new SqlCommand(strcmd, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                var permission = rdr[0];
                cn.Close();
                if (permission != null)
                {
                    ModuleDetailModel moduleDetailModel = JsonConvert.DeserializeObject<ModuleDetailModel>(permission.ToString());
                    return moduleDetailModel.ModuleDetailPermissions.Where(c => c.Name == permisionName && c.IsChecked == true).Any();
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }



    public class ScreenPermissionModel
    {
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public long CompanyId { get; set; }
        public string PermissionName { get; set; }
        public string ModuleMasterName { get; set; }
        public string ModuleDetailId { get; set; }
        public string GroupName { get; set; }
        public string ParentScreenName { get; set; }
        public string PermissionType { get; set; }

    }



    public class ModuleDetailModel
    {
        public ModuleDetailModel()
        {
            ModuleDetailPermissions = new List<ModuleDetailPermissionModel>();
        }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public Nullable<long> ParentId { get; set; }
        public Nullable<long> ModuleDetailId { get; set; }
        public bool? HasTabs { get; set; }
        public bool IsLinkFirstTab { get; set; }
        public bool IsPermissionInherited { get; set; }
        public bool IsHideTab { get; set; }

        public int? Recorder { get; set; }
        public bool IsDisable { get; set; }


        public List<ModuleDetailPermissionModel> ModuleDetailPermissions { get; set; }
        public List<ModuleDetailModel> Tabs { get; set; } = new List<ModuleDetailModel>();
    }

    public class ModuleDetailPermissionModel
    {
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsChecked { get; set; }
        public long ModuleDetailPermissionId { get; set; }
        public bool IsMainActions { get; set; }
        public bool? IsReference { get; set; }
    }
}
