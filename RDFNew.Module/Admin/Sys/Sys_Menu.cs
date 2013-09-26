using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Sys
{
    public class Sys_Menu : ISingle
    {
        const string MTABLE = "Sys_Menu";
        const string MKEY = "MenuID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();            
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_Menu.MenuID", "=", "MenuID", KeyValue));
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
                            Sys_Menu.*,b.Url,
                            c.MenuID As PMenuID,c.MenuName As PMenuName
                    From Sys_Menu 
                        Left Join Sys_Module b On b.ModuleID=Sys_Menu.ModuleID
                        Left Join Sys_Menu c On c.RID=Sys_Menu.PID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Sys_Menu 
                        Left Join Sys_Module b On b.ModuleID=Sys_Menu.ModuleID
                        Left Join Sys_Menu c On c.RID=Sys_Menu.PID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select  Top {2} 
                            Sys_Menu.*,b.Url,
                            c.MenuID As PMenuID,c.MenuName As PMenuName
                    From Sys_Menu 
                        Left Join Sys_Module b On b.ModuleID=Sys_Menu.ModuleID
                        Left Join Sys_Menu c On c.RID=Sys_Menu.PID
                    Where 1=1 {0} And Sys_Menu.MenuID Not In (
                            Select Top {3} Sys_Menu.MenuID 
                            From Sys_Menu 
                                Left Join Sys_Module b On b.ModuleID=Sys_Menu.ModuleID
                                Left Join Sys_Menu c On c.RID=Sys_Menu.PID
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
                            dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"),
                            "00000000", MTABLE, MKEY);
                        dr["RLine"] = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RLine", "PID", dr["PID"].ToString());
                        dr["RID"] = dr["PID"].ToString() + dr["RLine"].ToString();
                        if (dr["PID"].ToString() != "")
                            dr["TreeLevel"] = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From {0} Where RID='{1}' ",MTABLE, dr["PID"]), null);
                        else
                            dr["TreeLevel"] = 0;
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        if (!dr["PID", DataRowVersion.Current].Equals(dr["PID", DataRowVersion.Original]))
                        {
                            dr["RLine"] = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RLine", "PID", dr["PID"].ToString());
                            dr["RID"] = dr["PID"].ToString() + dr["RLine"].ToString();
                            if (dr["PID"].ToString() != "")
                                dr["TreeLevel"] = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From {0} Where RID='{1}' ", MTABLE, dr["PID"]), null);
                            else
                                dr["TreeLevel"] = 0;
                        }
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);

                        //递归更改下级相关字段
                        if (!dr["PID", DataRowVersion.Current].Equals(dr["PID", DataRowVersion.Original]))
                            ApplyMaster_AfterMove(dr["RID", DataRowVersion.Original].ToString(), dr["RID"].ToString(), tran);
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

        void ApplyMaster_AfterMove(string OrgPID, string NewPID, SqlTransaction tran)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Sys_Menu Where PID='{0}' Order By RLine 
            ", OrgPID), null);
            String NewRLine;
            String NewRID;
            object NewTreeLevel;
            foreach (DataRow dr in dt.Rows)
            {
                NewRLine = DALHelper.GetDetailSeq(tran, "", "00000000", 50, MTABLE, "RLine", "PID", NewPID);
                NewRID = NewPID + NewRLine;
                NewTreeLevel = DALHelper.GetFristValue(tran, String.Format(@"
                                Select TreeLevel+1 From Sys_Menu Where RID='{0}' ", NewPID), null);
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Update Sys_Menu Set PID='{0}',RLine='{1}',RID='{2}',TreeLevel={3} Where  MenuID='{4}'
                    ", NewPID, NewRLine, NewRID, NewTreeLevel, dr["MenuID"]), null);
                ApplyMaster_AfterMove(dr["RID"].ToString(), NewRID, tran);
            }
        }

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            if (DALHelper.IsExist(tran, @"
                    Select Top 1 MenuID From Sys_Menu Where IsNull(PID,'')=@PID                    
                    ",
                new SqlParameter[] { 
                        new SqlParameter("@PID",d["RID",DataRowVersion.Original])
                    }))
            {
                throw new Exception(String.Format(@"菜单 [{0}] 存在子级菜单,不可以删除.",
                    d["MenuName", DataRowVersion.Original]));
            }
        }
    }
}
