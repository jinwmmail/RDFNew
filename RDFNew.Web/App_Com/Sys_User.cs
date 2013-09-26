using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using RDFNew.Module;

namespace RDFNew.Web.App_Com
{
    public class Sys_User
    {
        public const string SYS_USER = "Sys_User";
        public const string SYS_USERMF = "Sys_UserMF";
        public const string SYS_MENU = "Sys_Menu";
        public const string SYS_FUNCTION = "Sys_Function";

        public static object[] UserLogin(string UserID,string Pwd)
        {
            object[] rtn = new object[] {0,"登录成功." };
            RDFNew.Module.Admin.Sys.Sys_User u = new RDFNew.Module.Admin.Sys.Sys_User();
            object[] o=u.GetMaster(new RDFNew.Module.DALEntity.QuerySet(new List<RDFNew.Module.DALEntity.QueryInfo>(){
                new RDFNew.Module.DALEntity.QueryInfo("And","(","","UserID","=","UserID",UserID),
                new RDFNew.Module.DALEntity.QueryInfo("Or","","","UserCode","=","UserCode",UserID),
                new RDFNew.Module.DALEntity.QueryInfo("Or","",")","Email","=","Email",UserID),
            }));
            if (o[0].ToString() == "0")
            {
                DataTable dt = o[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    if (dt.Select(" Enabled=1 ").Length > 0)
                    {
                        if (dt.Select(" IsNull(Pwd,'')='" + App_Com.Helper.StringToSHA1Hash(Pwd) + "'").Length > 0)
                        {
                            if (dt.Rows[0]["LoginTimes"] == System.DBNull.Value)
                                dt.Rows[0]["LoginTimes"] = 0;
                            dt.Rows[0]["LoginTimes"] = Convert.ToInt32(dt.Rows[0]["LoginTimes"]) + 1;
                            dt.Rows[0]["LoginLast"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            RDFNew.Web.App_Com.Helper.SetSession(SYS_USER, dt);
                            RDFNew.Web.App_Com.Helper.SetSession(SYS_USERMF, GetSys_UserMF(dt.Rows[0]["UserID"].ToString()));                            
                            u.ApplyMaster(dt.GetChanges(DataRowState.Modified), null, null,App_Com.Helper.BuildLog("Sys_User", "login"));                          
                        }
                        else
                        {
                            rtn[0] = 1;
                            rtn[1] = new Exception("登录密码不正确.");
                        }
                    }
                    else
                    {
                        rtn[0] = 1;
                        rtn[1] = new Exception("帐号未启用.");
                    }
                }
                else
                {
                    rtn[0] = 1;
                    rtn[1] = new Exception("帐号不存在.");
                }
            }
            else
            {
                rtn[0] = 1;
                rtn[1] = o[1];
            }

            return rtn;
        }

        static DataTable GetSys_UserMF(string UserID)
        {
            DataTable dt;
            dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select Distinct a.ModuleID,a.FunctionID 
                From (
	                Select 'R' As Type,a.RoleMFID,c.ModuleID,c.FunctionID 
	                From Sys_RoleMF a
		                Left Join Sys_UserR b On b.RoleID=a.RoleID
		                Left Join Sys_ModuleF c On c.ModuleFID=a.ModuleFID
	                Where b.UserID='{0}'
	                Union
	                Select 'U' As Type,a.UserMFID,c.ModuleID,c.FunctionID 
	                From Sys_UserMF a
		                Left Join Sys_ModuleF c On c.ModuleFID=a.ModuleFID
	                Where a.UserID='{0}'
	                ) a
            ", UserID), null);
            return dt;
        }

        static DataTable GetSys_UserFlow(string UserID)
        {
            DataTable dt;
            dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select a.* 
                From Sys_UserFlow a
                Where a.UserID='{0}'
            ", UserID), null);
            return dt;
        }

        public static void ClearSys_Menu()
        {
            RDFNew.Web.App_Com.Helper.SetCache(SYS_MENU, null);
        }

        public static void ClearSys_Function()
        {
            RDFNew.Web.App_Com.Helper.SetCache(SYS_FUNCTION, null);
        }

        public static DataTable GetSys_Menu()
        {
            DataTable dt;
            dt = App_Com.Helper.GetCache(SYS_MENU) as DataTable;
            if (dt == null)
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                    Select a.*,b.Url,IsNull(b.Enabled,1) As ModuleEnabled
                    From Sys_Menu a
                        Left Join Sys_Module b On b.ModuleID=a.ModuleID  And b.Enabled=1
                    Where  a.Visibled=1  
                    Order By RID
                    ", ""), null);
                RDFNew.Web.App_Com.Helper.SetCache(SYS_MENU, dt);
            }
            return dt;
        }

        public static DataTable GetSys_Function()
        {
            DataTable dt;
            dt = App_Com.Helper.GetCache(SYS_FUNCTION) as DataTable;
            if (dt == null)
            {
                dt = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                    Select  a.*,c.Seq,c.FunctionName,c.Icon
                    From Sys_ModuleF a
	                    Left Join Sys_Module b On b.ModuleID=a.ModuleID
	                    Left Join Sys_Function c On c.FunctionID=a.FunctionID
                    Where b.Enabled=1 And c.Enabled=1
                    Order By a.ModuleID,c.Seq 
                    ", ""), null);
                RDFNew.Web.App_Com.Helper.SetCache(SYS_FUNCTION, dt);
            }
            return dt;
        }

        public static bool CheckAuthorize(string ModuleID,string FunctionID)
        {
            bool rtn = false;
            if (Convert.ToBoolean(GetUserInfo("IsAdmin")))
                return true;
            DataTable dt;
            dt = App_Com.Helper.GetSession(SYS_USERMF, true) as DataTable;
            if (dt!=null)
                rtn=dt.Select(String.Format("ModuleID='{0}' And FunctionID='{1}'",ModuleID,FunctionID)).Length>0;
            return rtn;
        }

        public static string GetUserInfo(string FieldName)
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(SYS_USER, true) as DataTable;
            if (dt != null)
                return dt.Rows[0][FieldName].ToString();
            return "";
        }
    }
}
