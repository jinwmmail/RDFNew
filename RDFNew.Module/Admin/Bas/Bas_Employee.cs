using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Bas
{
    public class Bas_Employee : ISingle
    {
        const string MTABLE = "Bas_Employee";
        const string MKEY = "EmployeeID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Employee.EmployeeID", "=", "EmployeeID", KeyValue));
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
                    Select Bas_Employee.* ,b.DeptName                       
                    From Bas_Employee    
                        Left Join Bas_Dept b On b.DeptID= Bas_Employee.DeptID                    
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1)                      
                    From Bas_Employee    
                        Left Join Bas_Dept b On b.DeptID= Bas_Employee.DeptID   
                    Where 1=1 {0} 
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} Bas_Employee.* ,b.DeptName                       
                    From Bas_Employee    
                        Left Join Bas_Dept b On b.DeptID= Bas_Employee.DeptID                          
                    Where 1=1 {0} And Bas_Employee.EmployeeID Not In (
                        Select Top {3} Bas_Employee.EmployeeID 
                        From Bas_Employee 
                            Left Join Bas_Dept b On b.DeptID= Bas_Employee.DeptID 
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
                        if (dr[MKEY].ToString() == "")
                            dr[MKEY] = DALHelper.GetMasterNo(tran, "", "000000", MTABLE, MKEY);
                        else
                            dr[MKEY] = dr[MKEY].ToString().ToUpper();
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                        if (Convert.ToBoolean(dr["IsUser"]))
                            SyncEmployeeAndUser(dr, tran);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);
                        if (Convert.ToBoolean(dr["IsUser"]))
                            SyncEmployeeAndUser(dr, tran);
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

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select 'Select Top 1 {1} From '+Table_Name+' Where {1}=''{2}'''
                FROM INFORMATION_SCHEMA.COLUMNS     
                WHERE Column_Name='{1}' And Table_Name!='{0}'
            ", MTABLE, MKEY, d[MKEY, DataRowVersion.Original]), null);
            foreach (DataRow dr in dt.Rows)
            {
                if (SqlHelper.ExecuteDataTable(tran, dr[0].ToString(), null).Rows.Count > 0)
                {
                    throw new Exception(String.Format(@"雇员 {0} 已被使用.",
                    d["EmployeeName", DataRowVersion.Original]));
                }
            }
        }

        void SyncEmployeeAndUser(DataRow d, SqlTransaction tran)
        {
            RDFNew.Module.SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                if Not Exists(Select UserID From Sys_User Where UserID='{0}')
                Begin
                    Insert Into Sys_User (
                        UserID,UserName,NameE,Enabled,EmployeeID
                    )
                    Select '{0}','{1}','{2}',1,'{0}'
                End
                Else
                Begin                    
                    if Not Exists(Select UserID From Sys_User Where UserID!='{0}' And Email='{3}')
                    Begin
                        Update Sys_User Set Email='{3}' Where UserID='{0}' And '{3}'!=''
                    End
                End
            ", d["EmployeeID"], d["EmployeeName"], d["NameE"], d["Email"]), null);
        }
    }
}
