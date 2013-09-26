using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Bas
{
    public class Bas_Dept : ISingle
    {
        const string MTABLE = "Bas_Dept";
        const string MKEY = "DeptID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Bas_Dept.DeptID", "=", "DeptID", KeyValue));
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
                            Bas_Dept.*,d.DeptID As PDeptID,d.DeptName As PDeptName 
                    From Bas_Dept                         
                        Left Join Bas_Dept d On d.RID=Bas_Dept.PID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Bas_Dept 
                        Left Join Bas_Dept d On d.RID=Bas_Dept.PID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Bas_Dept.*,d.DeptID As PDeptID,d.DeptName As PDeptName 
                    From Bas_Dept                         
                        Left Join Bas_Dept d On d.RID=Bas_Dept.PID
                    Where 1=1 {0} And Bas_Dept.DeptID Not In (
                    Select Top {3} Bas_Dept.DeptID 
                    From Bas_Dept    
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

        public object[] ApplyMaster(DataTable dt,RDFNew.Module.DALEntity.Sys_Log la)
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
                            dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"),
                            "00000000", MTABLE, MKEY);
                        dr["RSeq"] = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RSeq", "PID", dr["PID"].ToString());
                        dr["RID"] = dr["PID"].ToString() + dr["RSeq"].ToString();
                        if (dr["PID"].ToString() != "")
                            dr["TreeLevel"] = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From {0} Where RID='{1}' ", MTABLE, dr["PID"]), null);
                        else
                            dr["TreeLevel"] = 0;
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                        ApplyMaster_AfterInsert(dr, tran);

                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        if (!dr["PID", DataRowVersion.Current].Equals(dr["PID", DataRowVersion.Original]))
                        {
                            dr["RSeq"] = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RSeq", "PID", dr["PID"].ToString());
                            dr["RID"] = dr["PID"].ToString() + dr["RSeq"].ToString();
                            if (dr["PID"].ToString() != "")
                                dr["TreeLevel"] = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From Bas_Dept Where RID='{0}' ", dr["PID"]), null);
                            else
                                dr["TreeLevel"] = 0;                            
                        }
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);

                        //递归更改下级相关字段
                        if (!dr["PID", DataRowVersion.Current].Equals(dr["PID", DataRowVersion.Original]))
                            ApplyMaster_AfterMove(dr["RID", DataRowVersion.Original].ToString(), dr["RID"].ToString(), tran);

                        ApplyMaster_AfterModify(dr, tran);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr,tran);
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

        void ApplyMaster_AfterMove(string OrgPID, string NewPID, SqlTransaction tran)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Bas_Dept Where PID='{0}' Order By RSeq 
            ", OrgPID), null);
            String NewRSeq;
            String NewRID;
            object NewTreeLevel;
            foreach (DataRow dr in dt.Rows)
            {
                NewRSeq = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RSeq", "PID", NewPID);
                NewRID = NewPID + NewRSeq;
                NewTreeLevel = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From Bas_Dept Where RID='{0}' ", NewPID), null);
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Update Bas_Dept Set PID='{0}',RSeq='{1}',RID='{2}',TreeLevel={3} Where  DeptID='{4}'
                    ", NewPID, NewRSeq, NewRID, NewTreeLevel,dr["DeptID"]), null);
                ApplyMaster_AfterMove(dr["RID"].ToString(), NewRID,tran);
            }
        }

        void ApplyMaster_AfterInsert(DataRow d, SqlTransaction tran)
        {
            
        }

        void ApplyMaster_AfterModify(DataRow d, SqlTransaction tran)
        {
    
        }

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            if (DALHelper.IsExist(tran, @"
                    Select Top 1 DeptID From Bas_Dept Where IsNull(PID,'')=@PID                    
                    ",
                new SqlParameter[] { 
                        new SqlParameter("@PID",d["RID",DataRowVersion.Original])
                    }))
            {
                throw new Exception(String.Format(@"部门 [{0}] 存在子级部门,不可以删除.",
                    d["DeptName", DataRowVersion.Original]));
            }

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
