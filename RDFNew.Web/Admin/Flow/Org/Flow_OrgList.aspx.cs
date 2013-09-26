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

namespace RDFNew.Web.Admin.Flow.Org
{
    public partial class Flow_OrgList : App_Com.PageListSingle
    {                
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_Org";
            B_ModuleName = "组织";
            B_PageDetail = "Flow_Org.aspx";
            B_ToolBar1 = this.Toolbar1;
            B_Window1 = this.Window1;
            B_Grid1 = this.Grid1;
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_Org();
            B_TableKey = "Flow_Org.OrgID";
            B_OrderBy = " Flow_Org.RID ";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override List<RDFNew.Module.DALEntity.QueryInfo> GetQueryInfo()
        {
            //树形结构无查询条件

            List<RDFNew.Module.DALEntity.QueryInfo> qi = new List<RDFNew.Module.DALEntity.QueryInfo>();
            //string Str = "";
            //Str = this.txtOrgID.Text.Trim();
            //if (Str != "")
            //    qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_Org.OrgID", "Like", "OrgID", "%" + Str + "%"));
            //Str = this.txtOrgName.Text.Trim();
            //if (Str != "")
            //    qi.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_Org.OrgName", "Like", "OrgName", "%" + Str + "%"));
            return qi;
        }

        protected override void ClearControls()
        {
            this.txtOrgID.Text = "";
            this.txtOrgName.Text = "";
        }

        protected override void AddData()
        {
            if (B_Grid1.SelectedRowIndexArray.Length > 0)
            {
                int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
                GridRow row = B_Grid1.Rows[selectedIndex];
                if (row.Values[9] == "1" || row.Values[9] == "2")
                {
                    return;
                }
            }

            String ParentID = "";
            String ParentName = "";
            String ParentRID = "";
            if (this.Grid1.SelectedRowIndexArray.Length > 0)
            {
                ParentID = this.Grid1.DataKeys[this.Grid1.SelectedRowIndex][0].ToString();
                ParentName = this.Grid1.Rows[this.Grid1.SelectedRowIndex].Values[1].Split('-')[1];
                ParentRID = this.Grid1.Rows[this.Grid1.SelectedRowIndex].Values[2];
            }
            B_Window1.Title = String.Format("{0}-[{1}]", B_TitleAdd, B_ModuleName);
            B_Window1.IFrameUrl = String.Format("{0}?action=add&parentID={1}&parentName={2}&parentrid={3}",
                B_PageDetail, ParentID, Server.UrlEncode(ParentName), ParentRID);
            B_Window1.Hidden = false;
        }

        protected override void BeforeDataBind(DataTable dt)
        {
            dt.Constraints.Clear();
            DataColumn dc;
            dc = new DataColumn("Manager", typeof(String)); dt.Columns.Add(dc);
            dc = new DataColumn("_Type", typeof(int)); dc.AllowDBNull = false; dc.DefaultValue = 0; dt.Columns.Add(dc);

            DataTable dtSys_UserR = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select a.*,b.UserName 
                From Sys_UserR a
	                Left Join Sys_User b on b.UserID=a.UserID                
                ",""));
            DataTable dtFlow_OrgR = RDFNew.Module.SqlHelper.ExecuteDataTable(String.Format(@"
                Select a.*,b.Seq,b.RoleName 
                From Flow_OrgR a
	                Left Join Sys_Role b On b.RoleID=a.RoleID               
                ", ""));
            DataRow[] drs;
            StringBuilder sb = new StringBuilder();
            DataTable dtTmp = dt.Clone();
            DataRow drNew;
            foreach (DataRow dr in dt.Rows)
            {
                sb.Remove(0, sb.Length);
                //取主管角色的所有用户
                drs = dtSys_UserR.Select("RoleID='"+dr["RoleID"].ToString()+"'", "UserID");
                foreach (DataRow r in drs)
                    sb.AppendFormat("{0},", r["UserName"]);
                dr["Manager"] = sb.ToString().TrimEnd(',');

                //取组织所含角色

                drs = dtFlow_OrgR.Select("OrgID='" + dr["OrgID"].ToString() + "'", "RoleID");                
                foreach (DataRow r in drs)
                {
                    drNew = dtTmp.NewRow();
                    foreach (DataColumn c in dt.Columns)
                        drNew[c.ColumnName] = dr[c.ColumnName];
                    drNew["OrgID"] = r["RoleID"];
                    drNew["OrgName"] = String.Format("<img src='../../../Res/FineUI/icon/folder.png'/>-{0}", r["RoleName"]);
                    drNew["RoleName"] = "";
                    drNew["JobName"] = "";
                    drNew["Manager"] = "";
                    drNew["_Type"] = 1;
                    drNew["TreeLevel"] = Convert.ToInt32(dr["TreeLevel"]) + 1;
                    dtTmp.Rows.Add(drNew);

                    //取角色所含用户
                    drs = dtSys_UserR.Select("RoleID='" + r["RoleID"].ToString() + "'", "UserID");
                    foreach (DataRow d in drs)
                    {
                        drNew = dtTmp.NewRow();
                        foreach (DataColumn c in dt.Columns)
                            drNew[c.ColumnName] = dr[c.ColumnName];
                        drNew["OrgID"] = d["UserID"];
                        drNew["OrgName"] = String.Format("<img src='../../../Res/FineUI/icon/user.png'/>-{0}", d["UserName"]);
                        drNew["RoleName"] = "";
                        drNew["JobName"] = "";
                        drNew["Manager"] = "";
                        drNew["_Type"] = 2;
                        drNew["TreeLevel"] = Convert.ToInt32(dr["TreeLevel"]) + 2;
                        dtTmp.Rows.Add(drNew);                        
                    }
                }
                dr["OrgName"] = String.Format("<img src='../../../Res/FineUI/icon/world.png'/>-{0}", dr["OrgName"]);
            }
            dt.Merge(dtTmp);
            dtTmp.Clear();
            drs = dt.Select("1=1", "RID");
            foreach (DataRow dr in drs)
                dtTmp.ImportRow(dr);
            dt.Clear();
            dt.Merge(dtTmp);
        }

        protected override void ViweData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            if (row.Values[9] == "0")
            {
                base.ViweData();
            }
            if (row.Values[9] == "1")
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleView, "角色", KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=view&keyword={1}", "../../Sys/Role/Sys_Role.aspx", KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());                
            }
            if (row.Values[9] == "2")
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleView, "用户", KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=view&keyword={1}", "../../Sys/User/Sys_User.aspx", KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference()); 
            }
        }

        protected override void EditData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            if (row.Values[9] == "0")
            {
                base.EditData();
            }
            if (row.Values[9] == "1")
            {
                string KeyVal = "";
                KeyVal = B_Grid1.DataKeys[B_Grid1.SelectedRowIndex][0].ToString();
                B_Window1.Title = String.Format("{0}-[{1}-{2}]", B_TitleView, "角色", KeyVal);
                B_Window1.IFrameUrl = String.Format("{0}?action=edit&keyword={1}", "../../Sys/Role/Sys_Role.aspx", KeyVal);
                B_Window1.Hidden = false;
                if (B_WindowMaxSize)
                    PageContext.RegisterStartupScript(B_Window1.GetMaximizeReference());
            }
            if (row.Values[9] == "2")
            {
                FineUI.Alert.ShowInParent("该行不可以执行此操作");
            }
        }

        protected override void DeleteData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            if (row.Values[9] == "0")
            {
                base.DeleteData();
            }
            else
            {
                FineUI.Alert.ShowInParent("该行不可以执行此操作");
            }
        }

        protected override string[] BackSelectData()
        {
            int selectedIndex = B_Grid1.SelectedRowIndexArray[0];
            GridRow row = B_Grid1.Rows[selectedIndex];
            return new string[] { row.DataKeys[0].ToString(), row.Values[1].Split('-')[1], row.Values[2] };
        }

        protected override string[] BackEmptyData()
        {
            return new string[] { "", "","" };
        }
    }
}
