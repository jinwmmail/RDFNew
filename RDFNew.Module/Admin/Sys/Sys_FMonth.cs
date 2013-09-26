using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Sys
{
    public class Sys_FMonth : ISingle
    {
        const string MTABLE = "Sys_FMonth";
        const string MKEY = "FMonthID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_FMonth.FMonthID", "=", "FMonthID", KeyValue));
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
                            Sys_FMonth.*
                    From Sys_FMonth                         
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Sys_FMonth 
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Sys_FMonth.*
                    From Sys_FMonth                             
                    Where 1=1 {0} And Sys_FMonth.FMonthID Not In (
                        Select Top {3} Sys_FMonth.FMonthID 
                        From Sys_FMonth 
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
                        ApplyMaster_BeforeInsert(dr, tran);
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        ApplyMaster_BeforeModify(dr, tran);
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr, tran);
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

        void ApplyMaster_BeforeInsert(DataRow d, SqlTransaction tran)
        {
            if (Convert.ToBoolean(d["Closed"]))
                throw new Exception(String.Format(@"财务月份 {0} 新增时不可为已关帐状态.",
                d["FMonthID"]));
        }

        void ApplyMaster_BeforeModify(DataRow d, SqlTransaction tran)
        {
            Boolean Closed = Convert.ToBoolean(d["Closed"]);
            if (Closed) //关帐
            {
                //上一个财务月份是否已关帐，如没有，则不可
                if (DALHelper.IsExist(tran, @"
                        Select Top 1 FMonthID 
                        From Sys_FMonth 
                        Where FMonthID<@FMonthID And Closed=0
                        Order By FMonthID Desc
                        ",
                    new SqlParameter[] { 
                            new SqlParameter("@FMonthID",d["FMonthID",DataRowVersion.Original])
                        }))
                {
                    throw new Exception(String.Format(@"财务月份 {0} 的上一个财务月份还没有关帐，不可以执行此操作.",
                        d["FMonthID", DataRowVersion.Original]));
                }
            }
            else //反关帐            
            {
                //下一个财务月份是否已反关帐，如没有，则不可
                if (DALHelper.IsExist(tran, @"
                        Select Top 1 FMonthID 
                        From Sys_FMonth 
                        Where FMonthID>@FMonthID And Closed=1
                        Order By FMonthID
                        ",
                    new SqlParameter[] { 
                            new SqlParameter("@FMonthID",d["FMonthID",DataRowVersion.Original])
                        }))
                {
                    throw new Exception(String.Format(@"财务月份 {0} 的下一个财务月份还没有反关帐，不可以执行此操作.",
                        d["FMonthID", DataRowVersion.Original]));
                }
            }

            //一个月结帐 
            string[] TableNames1 = new string[] { };  //表名字
            foreach (string s in TableNames1)
            {
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                Update {4}  Set Closed={0},ClosedBy='{1}',ClosedOn='{2}' Where IsNull(CrtOn,'')!='' And SubString(CrtOn,1,4)+SubString(CrtOn,6,2)='{3}' 
            ",
                 Closed ? 1 : 0,
                 Closed ? d["ModBy", DataRowVersion.Original] : "",
                 Closed ? d["ModOn", DataRowVersion.Original] : "",
                 d["FMonthID", DataRowVersion.Original],
                 s),
                 null);
            }
            //三个月结帐
            String F = d["FMonthID", DataRowVersion.Original].ToString();
            String F3 =Convert.ToDateTime(F.Substring(0, 4) + "-" + F.Substring(4, 2) + "-01").AddMonths(-2).ToString("yyyyMM");
            string[] TableNames3 = new string[] { };    //表名字
            foreach (string s in TableNames3)
            {
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                Update {4}  Set Closed={0},ClosedBy='{1}',ClosedOn='{2}' Where IsNull(CrtOn,'')!='' And SubString(CrtOn,1,4)+SubString(CrtOn,6,2)='{3}' 
            ",
                 Closed ? 1 : 0,
                 Closed ? d["ModBy", DataRowVersion.Original] : "",
                 Closed ? d["ModOn", DataRowVersion.Original] : "",
                 F3,
                 s),
                 null);
            }
        }
        
        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            if (Convert.ToBoolean(d["Closed", DataRowVersion.Original]))
            {
                throw new Exception(String.Format(@"财务月份 [{0}] 已关帐,不可以删除.",
                    d["FMonthID", DataRowVersion.Original]));
            }
        }
    }
}
