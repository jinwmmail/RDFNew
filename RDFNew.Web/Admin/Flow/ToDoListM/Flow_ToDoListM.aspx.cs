﻿using System;
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

namespace RDFNew.Web.Admin.Flow.ToDoListM
{
    public partial class Flow_ToDoListM : App_Com.PageMulti
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_ToDoListM";
            B_ModuleName = "流程列表";
            B_ToolBar1 = this.Toolbar1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_ToDoListM();

            B_Grid1 = this.Grid1;
            B_Window1 = this.Window1;
            
            B_DetailSessionKey = "Flow_ToDoListDAdd";            
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
            this.txtToDoListMID.Readonly = B_Action.ToLower() != "add";                        
            this.txtDescription.Readonly = B_Action.ToLower() == "view";
            if (B_Action.ToLower() != "view")
            {
                this.txtToDoListMID.CssStyle = this.txtToDoListMID.Readonly?"background:#c0c0c0;":"";                
            }            
        }

        protected override void GetData(DataRow dr)
        {
            this.txtToDoListMID.Text = String.Format("{0}", dr["ToDoListMID"]);
            this.txtDescription.Text = String.Format("{0}", dr["Description"]);
            
            LoadDetail();
        }

        protected override void LoadDetail()
        {            
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt == null)
            {
                RDFNew.Module.Admin.Flow.Flow_ToDoListD da = new RDFNew.Module.Admin.Flow.Flow_ToDoListD();
                dt = da.GetDataByParent(B_Keyword)[1] as DataTable;             
                App_Com.Helper.SetSession(B_DetailSessionKey, dt);
            }            
            this.Grid1.DataSource = dt;
            this.Grid1.DataBind();
        }
        
        protected override string AddData()
        {
            RDFNew.Module.Admin.Flow.Flow_ToDoListM obj = new RDFNew.Module.Admin.Flow.Flow_ToDoListM();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Flow_ToDoListM");
            DataRow dr;
            dr = dt.NewRow();
            dr["ToDoListMID"] = App_Com.Helper.InputText(this.txtToDoListMID.Text, 500);            
            dr["Description"] = App_Com.Helper.InputText(this.txtDescription.Text, 500);

            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);

            BuildDetail();
            DataTable dtDetail = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),dtDetail,
                                        App_Com.Helper.BuildLog("Flow_ToDoListM", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_ToDoListM obj = new RDFNew.Module.Admin.Flow.Flow_ToDoListM();            
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];

                    dr["Description"] = App_Com.Helper.InputText(this.txtDescription.Text, 500);                  

                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    BuildDetail();
                    DataTable dtDetail = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),dtDetail,
                                            App_Com.Helper.BuildLog("Flow_ToDoListM", "edit"));
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

        protected override void DeleteDataDetail()
        {
            DataTable dt;
            dt = App_Com.Helper.GetSession(B_DetailSessionKey, false) as DataTable;
            if (dt != null && this.Grid1.SelectedRowIndexArray.Length > 0)
            {
                object[] o = this.Grid1.DataKeys[this.Grid1.SelectedRowIndex];
                string ID = o[0].ToString();
                DataRow[] drs = dt.Select("ToDoListDID='" + ID + "'");
                if (drs.Length > 0)
                {
                    drs[0].Delete();
                    LoadDetail();
                    PageContext.RegisterStartupScript("PageList.grid_SelectRowFocus();");       
                }
            }
        }
        
        protected void Grid1_RowCommand(object sender, FineUI.GridCommandEventArgs e)
        {
            //if (e.CommandName == "View")
            //    ViweDataDetail();
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {            
            if (B_Action.ToUpper() == "VIEW")
            {
              
            }
        }   
    }
}
