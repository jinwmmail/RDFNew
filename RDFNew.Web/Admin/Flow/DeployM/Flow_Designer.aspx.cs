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
using System.Text.RegularExpressions;
using DS.XBPM.Design.Web;

namespace RDFNew.Web.Admin.Flow.Designer
{
    public partial class Flow_Designer : App_Com.PageSingle
    {
        String P_FileName = "";
        String P_DeployName = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            B_ModuleID = "Flow_DeployM";
            B_ModuleName = "流程设计";
            B_IDAL = new RDFNew.Module.Admin.Flow.Flow_DeployM();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void GetData(DataRow dr)
        {
            this.WorkflowDesigner1.CurrentFileName = dr["FileName"].ToString();
        }

        protected void WorkflowDesigner1_SavingFile(object sender, WorkflowEventArgs e)
        {
            string pdl = e.Content;
            Regex r = new Regex("<owner>(.*?)</owner>");
            pdl = r.Replace(pdl, "<owner>OA</owner>");
            P_FileName = e.FileName;
            e.Content = pdl;  

            //取第一个Name节点
            r = new Regex("<name>(.*?)</name>");
            MatchCollection mc = r.Matches(pdl);
            if (mc.Count > 0)
                P_DeployName = GetValue(mc[0].Value, "<name>", "</name>");         
            SaveData();            
        }

        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        void SaveData()
        {            
            RDFNew.Module.Admin.Flow.Flow_DeployM dal = new RDFNew.Module.Admin.Flow.Flow_DeployM();
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_DeployM.DeployMID", "=", "DeployMID", B_Keyword));
            DataTable dt = dal.GetMaster(qs)[1] as DataTable;
            if (dt.Rows.Count == 0)
            {
                //新增
                AddData();
            }
            else
            {
                //更新
                UpdateData();
            }
        }

        protected override string AddData()
        {

            RDFNew.Module.Admin.Flow.Flow_DeployM obj = new RDFNew.Module.Admin.Flow.Flow_DeployM();
            DataTable dt = RDFNew.Module.DALHelper.GetMasterEmpty(null, "Flow_DeployM");
            DataRow dr;
            dr = dt.NewRow();
            dr["FileName"] = P_FileName;
            dr["DeployName"] = P_DeployName;
           
            dr["CrtBy"] = App_Com.Sys_User.GetUserInfo("UserID");
            dr["CrtOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            dt.Rows.Add(dr);
            object[] data = obj.ApplyMaster(dt.GetChanges(DataRowState.Added),
                                        App_Com.Helper.BuildLog("Flow_DeployM", "add"));
            if (data[0].ToString() != "0") //正常                
                throw data[1] as Exception;
            else
                return data[1].ToString();
        }

        protected override string UpdateData()
        {
            RDFNew.Module.Admin.Flow.Flow_DeployM obj = new RDFNew.Module.Admin.Flow.Flow_DeployM();
            object[] data = obj.GetMaster(B_Keyword);
            if (data[0].ToString() == "0") //正常
            {
                DataTable dt = data[1] as DataTable;
                if (dt.Rows.Count > 0)
                {
                    DataRow dr;
                    dr = dt.Rows[0];
                    dr["DeployKey"] = "";      //每次修改后，需要重新部署
                    dr["FileName"] = P_FileName;
                   
                    dr["ModBy"] = App_Com.Sys_User.GetUserInfo("UserID");
                    dr["ModOn"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    data = obj.ApplyMaster(dt.GetChanges(DataRowState.Modified),
                                            App_Com.Helper.BuildLog("Flow_File", "edit"));
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
    }
}
