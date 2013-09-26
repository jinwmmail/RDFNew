using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Sys
{
    public class Sys_Module : ISingle
    {
        const string MTABLE = "Sys_Module";
        const string MKEY = "ModuleID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Module.ModuleID", "=", "ModuleID", KeyValue));
            return GetMaster(qs);            
        }

        public object[] GetMaster(DALEntity.QuerySet qrys)
        {
            object[] rtn = new object[] { 0, null, 0 }; //0成功或1失败，DataTable或Exception，0总行数
            string Sql = "";
            string OrderBy = "";
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            try
            {
                if (qrys != null)
                {
                    foreach (DALEntity.QueryInfo qi in qrys.QueryInfos)
                    {
                        sb.Append(string.Format(" {0} {1} {2} {3} @{4} {5} ",
                            qi.Union, qi.GroupBegin, qi.FieldName, qi.Oper, qi.ParamName, qi.GroupEnd));
                        parms.Add(new SqlParameter("@" + qi.ParamName, qi.ParamValue));
                    }
                    OrderBy = qrys.OrderBy != "" ? " Order By " + qrys.OrderBy : "";
                }
                if (qrys == null || qrys.PageInfo == null)
                {
                    Sql = @"
                    Select 
                            Sys_Module.* 
                    From Sys_Module 
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Sys_Module 
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Sys_Module.* 
                    From Sys_Module 
                    Where 1=1 {0} And Sys_Module.ModuleID Not In (
                        Select Top {3} Sys_Module.ModuleID 
                        From Sys_Module 
                        Where 1=1 {0}      
                        {1}                      
                    ) 
                    {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(String.Format(Sql,
                        sb.ToString(), OrderBy, qrys.PageInfo.PageSize, qrys.PageInfo.PageIndex * qrys.PageInfo.PageSize), parms.ToArray());
                }
            }
            catch (Exception ex)
            {
                rtn[0] = 1; //失败
                rtn[1] = ex;
            }
            return rtn;
        }

        public object[] ApplyMaster(DataTable dt, RDFNew.Module.DALEntity.Sys_Log la)
        {
            return ApplyMaster(dt, null, la);
        }

        public object[] ApplyMaster(DataTable dt,DataTable dtSys_Function, RDFNew.Module.DALEntity.Sys_Log la)
        {
            object[] rtns = new object[] { 0, "", null };
            SqlConnection conn = new SqlConnection(SqlHelper.ConnectionString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        //新增
                        if (dr[MKEY].ToString() == "")
                            dr[MKEY] = DALHelper.GetMasterNo(tran, "M" + System.DateTime.Now.ToString("yyyy"), "0000", MTABLE, MKEY);
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                        ApplyMaster_AfterInsert(dr,dtSys_Function, tran);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);
                        ApplyMaster_AfterModify(dr,dtSys_Function, tran);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr,dtSys_Function, tran);
                        rtns[1] = DALHelper.DeleteTable(MTABLE, MKEY, dr, tran, la);
                    }
                }
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                rtns[0] = 1;
                rtns[1] = ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return rtns;
        }

        void ApplyMaster_AfterInsert(DataRow d, DataTable dtSys_Function, SqlTransaction tran)
        {
            if (dtSys_Function == null)
                return;
            string ModuleFID = "";
            foreach (DataRow dr in dtSys_Function.Rows)
            {
                ModuleFID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_ModuleF", "ModuleFID");
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_ModuleF (ModuleFID,ModuleID,FunctionID )
                        Select '{0}','{1}','{2}'
                    ",
                    ModuleFID, d["ModuleID"], dr["FunctionID"]), null);
            }
        }

        void ApplyMaster_AfterModify(DataRow d, DataTable dtSys_Function, SqlTransaction tran)
        {
            if (dtSys_Function == null)
                return;
            DataTable dtOrg = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Sys_ModuleF Where ModuleID='{0}'
                ", d["ModuleID"]), null);
            foreach (DataRow dr in dtOrg.Rows)
            {
                if (dtSys_Function.Select("FunctionID='" + dr["FunctionID"].ToString() + "'").Length == 0)
                {
                    //同时删除 角色 和 用户 已引用的权限
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_RoleMF Where ModuleFID='{0}'
                    ",
                        dr["ModuleFID"]), null);

                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserMF Where ModuleFID='{0}'
                    ",
                        dr["ModuleFID"]), null);

                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_ModuleF Where ModuleFID='{0}'
                    ",
                        dr["ModuleFID"]), null);
                }
            }
            string ModuleFID = "";
            foreach (DataRow dr in dtSys_Function.Rows)
            {
                if (dtOrg.Select("FunctionID='" + dr["FunctionID"].ToString() + "'").Length == 0)
                {
                    ModuleFID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_ModuleF", "ModuleFID");
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_ModuleF (ModuleFID,ModuleID,FunctionID )
                        Select '{0}','{1}','{2}'
                    ",
                        ModuleFID, d["ModuleID"], dr["FunctionID"]), null);
                }
            }
        }

        void ApplyMaster_BeforeDelete(DataRow d,DataTable dtSys_Function, SqlTransaction tran)
        {
            //同时删除 角色 和 用户 已引用的权限
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_RoleMF Where ModuleFID In (Select ModuleFID From Sys_ModuleF Where ModuleID='{0}')
                    ",
                d["ModuleID", DataRowVersion.Original]), null);

            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserMF Where ModuleFID In (Select ModuleFID From Sys_ModuleF Where ModuleID='{0}')
                    ",
                d["ModuleID", DataRowVersion.Original]), null);

            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_ModuleF Where ModuleID='{0}'
                    ",
                d["ModuleID", DataRowVersion.Original]), null);

            //已被引用不可删除
            DataTable dt = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select 
                        'Select Top 1 {1} From '+Table_Name+' Where {1}=''{2}'''
                        ,Table_Name
                FROM INFORMATION_SCHEMA.COLUMNS     
                WHERE Column_Name='{1}' And Table_Name!='{0}'
            ", MTABLE, MKEY, d[MKEY, DataRowVersion.Original]), null);
            foreach (DataRow dr in dt.Rows)
            {
                if (SqlHelper.ExecuteDataTable(tran, dr[0].ToString(), null).Rows.Count > 0)
                {
                    throw new Exception(String.Format(@"项 {0} 已被表 {1} 使用.",
                    d[MKEY, DataRowVersion.Original], dr[1]));
                }
            }
        }
    }
}
