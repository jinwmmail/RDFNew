using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Sys
{
    public class Sys_User : ISingle
    {
        const string MTABLE = "Sys_User";
        const string MKEY = "UserID";

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Sys_User.UserID", "=", "UserID", KeyValue));
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
                            qi.Union,qi.GroupBegin, qi.FieldName, qi.Oper, qi.ParamName,qi.GroupEnd));
                        parms.Add(new SqlParameter("@" + qi.ParamName, qi.ParamValue));
                    }
                    OrderBy = qrys.OrderBy != "" ? " Order By " + qrys.OrderBy : "";
                }
                if (qrys == null || qrys.PageInfo == null)
                {
                    Sql = @"
                    Select 
                            Sys_User.* 
                    From Sys_User 
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Sys_User 
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Sys_User.* 
                    From Sys_User 
                    Where 1=1 {0} And Sys_User.UserID Not In (
                        Select Top {3} Sys_User.UserID 
                        From Sys_User 
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
            return ApplyMaster(dt, null, null, la);
        }

        public object[] ApplyMaster(DataTable dt, DataTable dtSys_Role, DataTable Sys_ModuleF,
            RDFNew.Module.DALEntity.Sys_Log la)
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
                            dr[MKEY] = DALHelper.GetMasterNo(tran,"","000000", MTABLE, MKEY);
                        ApplyMaster_BeforeCheck(dr, dtSys_Role, Sys_ModuleF, tran);
                        rtns[1] = DALHelper.InsertTable(MTABLE, MKEY, dr, tran, la);
                        ApplyMaster_AfterInsert(dr, dtSys_Role, Sys_ModuleF, tran);
                    }
                    else if (dr.RowState == DataRowState.Modified)
                    {
                        //修改
                        ApplyMaster_BeforeCheck(dr, dtSys_Role, Sys_ModuleF, tran);
                        rtns[1] = DALHelper.UpdateTable(MTABLE, MKEY, dr, tran, la);
                        ApplyMaster_AfterModify(dr, dtSys_Role,Sys_ModuleF,tran);
                    }
                    else if (dr.RowState == DataRowState.Deleted)
                    {
                        //删除
                        ApplyMaster_BeforeDelete(dr, dtSys_Role,Sys_ModuleF, tran);
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

        void ApplyMaster_BeforeCheck(DataRow d, DataTable dtSys_Role, DataTable Sys_ModuleF, SqlTransaction tran)
        {
            string UserCode = "", Email = "", UserCodeOrg = "", EmailOrg = "";
            UserCode = d["UserCode"].ToString();
            Email = d["Email"].ToString();
            if (d.RowState == DataRowState.Modified)
            {
                UserCodeOrg = d["UserCode", DataRowVersion.Original].ToString();
                EmailOrg = d["Email", DataRowVersion.Original].ToString();
            }

            if (UserCode!="" && DALHelper.IsExist(tran, String.Format(@"
                Select Top 1 UserID From Sys_User Where UserCode='{0}' And UserCode!='{1}'
                ", UserCode, UserCodeOrg), null))
            {
                throw new Exception(String.Format("用户帐号 {0} 已存在.", d["UserCode"]));
            }
            if (Email != "" && DALHelper.IsExist(tran, String.Format(@"
                Select Top 1 UserID From Sys_User Where Email='{0}'  And Email!='{1}'
                ", Email, EmailOrg), null))
            {
                throw new Exception(String.Format("邮件地址 {0} 已存在.", d["Email"]));
            }
        }

        void ApplyMaster_AfterInsert(DataRow d, DataTable dtSys_Role, DataTable Sys_ModuleF, SqlTransaction tran)
        {            
            string UserRID = "";
            //处理用户属于哪些角色
            if (dtSys_Role != null)
            {
                foreach (DataRow dr in dtSys_Role.Rows)
                {
                    UserRID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_UserR", "UserRID");
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_UserR (UserRID,UserID,RoleID )
                        Select '{0}','{1}','{2}'
                    ",
                        UserRID, d["UserID"], dr["RoleID"]), null);
                }
            }
            //处理用户含有哪些权限
            if (Sys_ModuleF != null)
            {
                foreach (DataRow dr in Sys_ModuleF.Rows)
                {
                    UserRID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_UserMF", "UserMFID");
                    SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_UserMF (UserMFID,UserID,ModuleFID )
                        Select '{0}','{1}','{2}'
                    ",
                        UserRID, d["UserID"], dr["ModuleFID"]), null);
                }
            }
        }

        void ApplyMaster_AfterModify(DataRow d, DataTable dtSys_Role, DataTable Sys_ModuleF,
            SqlTransaction tran)
        {
            string UserRID = "";
            DataTable dtOrg;
            //处理用户属于哪些角色
            if (dtSys_Role != null)
            {
                dtOrg = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Sys_UserR Where UserID='{0}'
                ", d["UserID"]), null);
                foreach (DataRow dr in dtOrg.Rows)
                {
                    if (dtSys_Role.Select("RoleID='" + dr["RoleID"].ToString() + "'").Length == 0)
                    {
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserR Where UserRID='{0}'
                    ",
                            dr["UserRID"]), null);
                    }
                }                
                foreach (DataRow dr in dtSys_Role.Rows)
                {
                    if (dtOrg.Select("RoleID='" + dr["RoleID"].ToString() + "'").Length == 0)
                    {
                        UserRID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_UserR", "UserRID");
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_UserR (UserRID,UserID,RoleID )
                        Select '{0}','{1}','{2}'
                    ",
                            UserRID, d["UserID"], dr["RoleID"]), null);
                    }
                }
            }
            //处理用户含有哪些权限
            if (Sys_ModuleF != null)
            {
                dtOrg = SqlHelper.ExecuteDataTable(tran, String.Format(@"
                Select * From Sys_UserMF Where UserID='{0}'
                ", d["UserID"]), null);
                foreach (DataRow dr in dtOrg.Rows)
                {
                    if (Sys_ModuleF.Select("ModuleFID='" + dr["ModuleFID"].ToString() + "'").Length == 0)
                    {
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserMF Where UserMFID='{0}'
                    ",
                            dr["UserMFID"]), null);
                    }
                }
                foreach (DataRow dr in Sys_ModuleF.Rows)
                {
                    if (dtOrg.Select("ModuleFID='" + dr["ModuleFID"].ToString() + "'").Length == 0)
                    {
                        UserRID = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", "Sys_UserMF", "UserMFID");
                        SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Insert Into Sys_UserMF (UserMFID,UserID,ModuleFID )
                        Select '{0}','{1}','{2}'
                    ",
                            UserRID, d["UserID"], dr["ModuleFID"]), null);
                    }
                }
            }            
        }

        void ApplyMaster_BeforeDelete(DataRow d, DataTable dtSys_Role, DataTable Sys_ModuleF,
            SqlTransaction tran)
        {
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserMF Where UserID='{0}'
                    ",
                d["UserID", DataRowVersion.Original]), null);

            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, String.Format(@"
                        Delete Sys_UserR Where UserID='{0}'
                    ",
                d["UserID", DataRowVersion.Original]), null);
        }
    }
}
