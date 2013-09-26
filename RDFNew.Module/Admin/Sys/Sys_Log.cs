using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Sys
{
    public class Sys_Log : ISingle
    {
        const string MTABLE = "Sys_Log";
        const string MKEY = "LogID";

        public object[] GetMaster(string KeyValue)
        {
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Log.LogID", "=", "LogID", KeyValue));
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
                            Sys_Log.*
                    From Sys_Log                         
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Sys_Log 
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select  Top {2} 
                            Sys_Log.*
                    From Sys_Log                             
                    Where 1=1 {0} And Sys_Log.LogID Not In (
                        Select Top {3} Sys_Log.LogID 
                        From Sys_Log 
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
            return new object[] { 0, "", null };
        }

        public object[] ApplyMaster(DataTable dt, SqlTransaction tranx)
        {
            object[] rtns = new object[] { 0, "", null };
            SqlConnection conn = null;
            SqlTransaction tran = null;
            if (tranx != null)
            {
                tran = tranx;
            }
            else
            {
                conn = new SqlConnection(SqlHelper.ConnectionString);
                conn.Open();
                tran = conn.BeginTransaction();
            }
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        //新增
                        dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyyMMdd"),
                            "0000000000", MTABLE, MKEY);
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, null);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, null);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除                        
                        rtns[1] = DALHelper.DeleteTable(MTABLE, MKEY, dr, tran, null);
                    }
                }
                if (tranx == null)
                    tran.Commit();
            }
            catch (Exception ex)
            {
                if (tranx == null)
                    tran.Rollback();
                rtns[0] = 1;
                rtns[1] = ex;
            }
            finally
            {
                if (tranx == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return rtns;
        }
    }
}
