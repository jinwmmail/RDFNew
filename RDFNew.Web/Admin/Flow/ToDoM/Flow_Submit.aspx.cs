using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FineUI;
using System.IO;
using Newtonsoft.Json.Linq;

namespace RDFNew.Web.Admin.Flow.ToDoM
{
    public partial class Flow_Submit : App_Com.PageMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_ToDoM";
            B_ModuleName = "流程列表";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_ToDoM();
            
            B_Window1 = this.Window1;
            
            B_DetailSessionKey = "Flow_ToDoDAdd";            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack && Request.Form["__EVENTARGUMENT"] == "ChangedDetail")
            {                                
                LoadDetail();
            }
        }

        protected override void FillControlData()
        {
            App_Com.Helper.SetSession(B_DetailSessionKey, null);

            if (B_Action.ToLower() == "add" || B_Action.ToLower() == "edit")
            {
                              
            }
        }
       
        protected override void SetControlState()
        {            
            this.txtDescriptionSub.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {                
                
            }            
        }

        protected override void GetData(DataRow dr)
        {           
            LoadDetail();
        }

        protected override void LoadDetail()
        {            
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt == null)
            {
                RDFNew.Module.Admin.Flow.Flow_ToDoD da = new RDFNew.Module.Admin.Flow.Flow_ToDoD();
                dt = da.GetDataByParent(B_Keyword)[1] as DataTable;             
                App_Com.Helper.SetSession(B_DetailSessionKey, dt);
            }            
            this.Grid1.DataSource = dt;
            this.Grid1.DataBind();
        }
        
        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_ToDoM obj = new RDFNew.Module.Admin.Flow.Flow_ToDoM();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];

                    Boolean success = false;
                    DS.XBPM.API.ProcessInstance ins = App_Com.FlowHelper.Engine.GetExecutionService().FindProcessInstance(dr["InstanceID"].ToString());                    
                    if (ins != null)
                    {
                        RDFNew.Module.Admin.Flow.Flow_DeployD dal = new RDFNew.Module.Admin.Flow.Flow_DeployD();
                        string[] curActNames = ins.FindActiveExecutionNames();
                        for (int j = 0; j < curActNames.Length; j++)
                        {
                            string act = curActNames[j];
                            string owner = dal.GetActivityOwner(ins.DeploymentId, ins.ProcessName, act);
                            if ((owner.ToUpper() == "APPLICANT" && dr["CrtBy"].ToString() == App_Com.Sys_User.GetUserInfo("UserID"))||
                                owner == App_Com.Sys_User.GetUserInfo("UserID"))
                            {
                                DS.XBPM.API.ProcessAction action = new DS.XBPM.API.ProcessAction();
                                action.SetVariable("GoBack", 0);
                                success = ins.SignalExecution(act, action);
                                //FineUI.Alert.ShowInParent(success ? "流程提交成功" : "流程提交失败");
                                break;
                            }
                        }
                    }
                    if (!success)
                        throw new Exception("流程提交败.");
                                                      
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),GetDetail(), null,
                                            App_Com.Helper.BuildLog("Flow_ToDoM", "edit"));
                    if (data[0].ToString() != "0") //正常                
                        throw data[1] as Exception;
                    else
                        return data[1].ToString();
                }
                else
                {
                    throw new Exception("需要修改的记录已不存在,请刷新后再试.");
                }
            }
            else
            {
                throw data[1] as Exception;
            }
        }

        DataTable GetDetail()
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt != null)
            {
                DataRow dr;
                dr = dt.NewRow();
                dr["Description"] = App_Com.Helper.InputText(this.txtDescriptionSub.Text, 500);

                dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
