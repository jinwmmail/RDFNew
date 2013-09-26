using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Hr
{
    public class Hr_Leave : ISingle
    {
        const string MTABLE = "Hr_Leave";
        const string MKEY = "LeaveID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Hr_Leave.LeaveID", "=", "LeaveID", KeyValue));
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
                            Hr_Leave.*,b.LeaveTypeName,c.EmployeeName,d.ToDoMID
                    From Hr_Leave 
						Left Join Hr_LeaveType b On b.LeaveTypeID=Hr_Leave.LeaveTypeID
                        Left Join Bas_Employee c On c.EmployeeID=Hr_Leave.EmployeeID
                        Left Join Flow_ToDoM d On d.InstanceID=Hr_Leave.InstanceID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Hr_Leave 
						Left Join Hr_LeaveType b On b.LeaveTypeID=Hr_Leave.LeaveTypeID
                        Left Join Bas_Employee c On c.EmployeeID=Hr_Leave.EmployeeID
                        Left Join Flow_ToDoM d On d.InstanceID=Hr_Leave.InstanceID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Hr_Leave.*,b.LeaveTypeName,c.EmployeeName,d.ToDoMID
                    From Hr_Leave 
						Left Join Hr_LeaveType b On b.LeaveTypeID=Hr_Leave.LeaveTypeID
                        Left Join Bas_Employee c On c.EmployeeID=Hr_Leave.EmployeeID
                        Left Join Flow_ToDoM d On d.InstanceID=Hr_Leave.InstanceID
                    Where 1=1 {0} And Hr_Leave.LeaveID Not In (
                        Select Top {3} Hr_Leave.LeaveID 
                        From Hr_Leave 
						    Left Join Hr_LeaveType b On b.LeaveTypeID=Hr_Leave.LeaveTypeID
                            Left Join Bas_Employee c On c.EmployeeID=Hr_Leave.EmployeeID
                            Left Join Flow_ToDoM d On d.InstanceID=Hr_Leave.InstanceID
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
            return ApplyMaster(dt, la, null);
        }

        public object[] ApplyMaster(DataTable dt, RDFNew.Module.DALEntity.Sys_Log la, DataTable dtFlow)
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
                            dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyyMM"), "0000", MTABLE, MKEY);
                        if (dtFlow != null)
                        {
                            //记录实例ID,用于后续联动
                            dr["InstanceID"] = dtFlow.Rows[0]["InstanceID"];
                        }
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr, tran);
                        rtns[1] = DALHelper.DeleteTable(MTABLE, MKEY, dr, tran, la);
                    }

                    //处理流程主表
                    if (dtFlow != null)
                    {
                        dtFlow.Rows[0]["BillID"] = dr[MKEY];
                        dtFlow.Rows[0]["OwnerID"] = dr["EmployeeID"];
                        new RDFNew.Module.Admin.Flow.Flow_ToDoM().ApplyMaster(dtFlow, null, tran, la);
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

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            //删除相关联的待办事项
             DataTable dt = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select Top 1 * From Flow_ToDoM Where InstanceID='{0}'
            ", d["InstanceID", DataRowVersion.Original]));
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0].Delete();
                new RDFNew.Module.Admin.Flow.Flow_ToDoM().ApplyMaster(dt, null);
            }
        }
    }
}
