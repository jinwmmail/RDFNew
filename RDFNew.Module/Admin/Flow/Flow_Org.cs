using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Flow
{
    public class Flow_Org : ISingle
    {
        const string MTABLE = "Flow_Org";
        const string MKEY = "OrgID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_Org.OrgID", "=", "OrgID", KeyValue));
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
                            Flow_Org.*,b.JobName,c.RoleName,d.OrgID As POrgID,d.OrgName As POrgName 
                    From Flow_Org 
                        Left Join Flow_Job b On b.JobID=Flow_Org.JobID 
                        Left Join Sys_Role c On c.RoleID=Flow_Org.RoleID 
                        Left Join Flow_Org d On d.RID=Flow_Org.PID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Flow_Org 
                        Left Join Flow_Job b On b.JobID=Flow_Org.JobID 
                        Left Join Sys_Role c On c.RoleID=Flow_Org.RoleID 
                        Left Join Flow_Org d On d.RID=Flow_Org.PID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Flow_Org.*,b.JobName,c.RoleName,d.OrgID As POrgID,d.OrgName As POrgName
                    From Flow_Org 
                        Left Join Flow_Job b On b.JobID=Flow_Org.JobID 
                        Left Join Sys_Role c On c.RoleID=Flow_Org.RoleID 
                        Left Join Flow_Org d On d.RID=Flow_Org.PID
                    Where 1=1 {0} And Flow_Org.OrgID Not In (
                        Select Top {3} Flow_Org.OrgID 
                        From Flow_Org 
                            Left Join Flow_Job b On b.JobID=Flow_Org.JobID 
                            Left Join Sys_Role c On c.RoleID=Flow_Org.RoleID 
                            Left Join Flow_Org d On d.RID=Flow_Org.PID
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

        public object[] ApplyMaster(DataTable dt, DataTable dtSys_Role, RDFNew.Module.DALEntity.Sys_Log la)
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
                        ApplyMaster_AfterInsert(dr, dtSys_Role, tran);

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
                                Select TreeLevel+1 From Flow_Org Where RID='{0}' ", dr["PID"]), null);
                            else
                                dr["TreeLevel"] = 0;                            
                        }
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);

                        //递归更改下级相关字段
                        if (!dr["PID", DataRowVersion.Current].Equals(dr["PID", DataRowVersion.Original]))
                            ApplyMaster_AfterMove(dr["RID", DataRowVersion.Original].ToString(), dr["RID"].ToString(), tran);

                        ApplyMaster_AfterModify(dr, dtSys_Role, tran);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr,dtSys_Role, tran);
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
                Select * From Flow_Org Where PID='{0}' Order By RSeq 
            ", OrgPID), null);
            String NewRSeq;
            String NewRID;
            object NewTreeLevel;
            foreach (DataRow dr in dt.Rows)
            {
                NewRSeq = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RSeq", "PID", NewPID);
                NewRID = NewPID + NewRSeq;
                NewTreeLevel = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From Flow_Org Where RID='{0}' ", NewPID), null);
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Update Flow_Org Set PID='{0}',RSeq='{1}',RID='{2}',TreeLevel={3} Where  OrgID='{4}'
                    ", NewPID, NewRSeq, NewRID, NewTreeLevel,dr["OrgID"]), null);
                ApplyMaster_AfterMove(dr["RID"].ToString(), NewRID,tran);
            }
        }

        void ApplyMaster_AfterInsert(DataRow d, DataTable dtSys_Role, SqlTransaction tran)
        {
            if (dtSys_Role == null)
                return;            
            foreach (DataRow dr in dtSys_Role.Rows)
            {                
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Flow_OrgR (OrgID,RoleID )
                        Select '{0}','{1}'
                    ",
                    d["OrgID"], dr["RoleID"]), null);               
            }
        }

        void ApplyMaster_AfterModify(DataRow d, DataTable dtSys_Role, SqlTransaction tran)
        {
            if (dtSys_Role == null)
                return;
            DataTable dtOrg = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Flow_OrgR Where OrgID='{0}'
                ", d["OrgID"]), null);
            foreach (DataRow dr in dtOrg.Rows)
            {
                if (dtSys_Role.Select("RoleID='" + dr["RoleID"].ToString() + "'").Length == 0)
                {
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Flow_OrgR Where OrgID='{0}' And RoleID='{1}'
                    ",
                        dr["OrgID"], dr["RoleID"]), null);
                }
            }
            foreach (DataRow dr in dtSys_Role.Rows)
            {
                if (dtOrg.Select("RoleID='" + dr["RoleID"].ToString() + "'").Length == 0)
                {                    
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Flow_OrgR (OrgID,RoleID )
                        Select '{0}','{1}'
                    ",
                        d["OrgID"], dr["RoleID"]), null);
                }
            }
        }

        void ApplyMaster_BeforeDelete(DataRow d, DataTable dtSys_Role, SqlTransaction tran)
        {
            if (DALHelper.IsExist(tran, @"
                    Select Top 1 OrgID From Flow_Org Where IsNull(PID,'')=@PID                    
                    ",
                new SqlParameter[] { 
                        new SqlParameter("@PID",d["RID",DataRowVersion.Original])
                    }))
            {
                throw new Exception(String.Format(@"部门 [{0}] 存在子级部门,不可以删除.",
                    d["OrgName", DataRowVersion.Original]));
            }

            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Flow_OrgR Where OrgID='{0}'
                    ",
                d["OrgID", DataRowVersion.Original]), null);

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
