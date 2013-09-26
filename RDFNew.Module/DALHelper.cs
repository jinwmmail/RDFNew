using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RDFNew.Module
{
    public abstract class DALHelper
    {
        public static object InsertTable(string tbName, string pkField, DataRow dr,
             SqlTransaction tran, RDFNew.Module.DALEntity.Sys_Log la)
        {
            DataTable dtEmpty = GetMasterEmpty(tran, tbName);
            string SqlInsert = "";
            System.Text.StringBuilder sbSql = new StringBuilder();
            System.Text.StringBuilder sbPm = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            sbSql.Append("Insert Into " + tbName + " ( ");
            sbPm.Append("Select ");
            foreach (DataColumn dc in dtEmpty.Columns)
            {
                if (dr[dc.ColumnName] != System.DBNull.Value)
                {
                    sbSql.Append("[" + dc.ColumnName + "],");
                    sbPm.Append("@" + dc.ColumnName + ",");
                    parms.Add(new SqlParameter("@" + dc.ColumnName, dr[dc.ColumnName]));
                }
            }
            SqlInsert = sbSql.ToString().TrimEnd(',') + ") " + sbPm.ToString().TrimEnd(',');
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, SqlInsert, parms.ToArray());

            if (la != null)
            {
                la.Table = tbName; //新增在操作后记日志，修改删除在操作前记日志
                la.Key= pkField;
                la.Value= dr[pkField].ToString();
                ApplyLogAction(tran, la);
            }

            return dr[pkField];
        }

        public static object UpdateTable(string tbName, string pkField, DataRow dr,
            SqlTransaction tran, RDFNew.Module.DALEntity.Sys_Log la)
        {
            string FID = dr[pkField].ToString();

            if (la != null)
            {
                la.Table = tbName; //新增在操作后记日志，修改删除在操作前记日志
                la.Key = pkField;
                la.Value = FID;
                ApplyLogAction(tran, la);
            }
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo(pkField, "=", FID));
            DataTable dtOrg = GetMaster(tran, tbName, qs);
            string SqlInsert = "";
            System.Text.StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            sbSql.Append("Update  " + tbName + " Set ");
            int chg = 0;
            foreach (DataColumn dc in dtOrg.Columns)
            {
                if (!dr[dc.ColumnName].Equals(dtOrg.Rows[0][dc.ColumnName]))
                {
                    sbSql.Append("[" + dc.ColumnName + "]=@" + dc.ColumnName + ",");
                    parms.Add(new SqlParameter("@" + dc.ColumnName, dr[dc.ColumnName]));
                    chg++;
                }
            }
            if (chg > 0)
            {
                SqlInsert = sbSql.ToString().TrimEnd(',') + " Where " + pkField + "=@" + pkField + " ";
                parms.Add(new SqlParameter("@" + pkField, FID));
                SqlHelper.ExecuteNonQuery(tran, CommandType.Text, SqlInsert, parms.ToArray());
            }

            return FID;
        }

        public static object DeleteTable(string tbName, string pkField, DataRow dr,
            SqlTransaction tran, RDFNew.Module.DALEntity.Sys_Log la)
        {
            string FID = dr[pkField, DataRowVersion.Original].ToString();

            if (la != null)
            {
                la.Table = tbName; //新增在操作后记日志，修改删除在操作前记日志
                la.Key = pkField;
                la.Value = FID;
                ApplyLogAction(tran, la);
            }

            string SqlInsert = "";
            System.Text.StringBuilder sbSql = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            sbSql.Append("Delete  " + tbName + " Where [" + pkField + "]=@" + pkField + " ");
            SqlInsert = sbSql.ToString();
            parms.Add(new SqlParameter("@" + pkField, FID));
            SqlHelper.ExecuteNonQuery(tran, CommandType.Text, SqlInsert, parms.ToArray());
            return FID;
        }

        public static object GetFristValue(SqlTransaction tran, string Sql, SqlParameter[] parms)
        {
            if (tran != null)
                return SqlHelper.ExecuteScalar(tran, Sql, parms);
            else
                return SqlHelper.ExecuteScalar(Sql, parms);
        }

        public static bool IsExist(SqlTransaction tran, string Sql, SqlParameter[] parms)
        {
            bool b = false;
            DataTable dt = null;
            if (tran != null)
                dt = SqlHelper.ExecuteDataTable(tran, Sql, parms);
            else
                dt = SqlHelper.ExecuteDataTable(Sql, parms);
            if (dt != null && dt.Rows.Count > 0)
                b = true;
            return b;
        }

        public static string GetMasterNo(SqlTransaction tran, string prefix, string suffix,string last, int step, string tableName, string filedName)
        {
            string Sql = "Select IsNull(( " +
                "Select Top 1 {0} " +
                "From {1}  " +
                "Where IsNull({0},'') Like '{2}%{4}' And Len({0})=Len('{2}'+'{3}'+'{4}') " +
                "Order By {0} Desc " +
                "),'{2}'+'{3}'+'{4}')";
            string OldRowId = "";
            OldRowId = GetFristValue(tran, String.Format(Sql, filedName, tableName, prefix, suffix, last), null).ToString();
            string NewRowId = prefix + (Convert.ToInt32(OldRowId.Substring(prefix.Length, suffix.Length)) + step).ToString().PadLeft(suffix.Length, '0')+last;
            return NewRowId;
        }

        public static string GetMasterNo(SqlTransaction tran, string prefix, string suffix, int step, string tableName, string filedName)
        {
            string Sql = "Select IsNull(( " +
                "Select Top 1 {0} " +
                "From {1}  " +
                "Where IsNull({0},'') Like '{2}%' And Len({0})=Len('{2}'+'{3}') " +
                "Order By {0} Desc " +
                "),'{2}'+'{3}')";
            string OldRowId = "";
            OldRowId = GetFristValue(tran, String.Format(Sql, filedName, tableName, prefix, suffix), null).ToString();
            string NewRowId = prefix + (Convert.ToInt32(OldRowId.Substring(prefix.Length)) + step).ToString().PadLeft(suffix.Length, '0');
            return NewRowId;
        }

        public static string GetMasterNo(SqlTransaction tran, string prefix, string suffix, string tableName, string filedName)
        {
            return GetMasterNo(tran, prefix, suffix, 1, tableName, filedName);
        }

        public static string GetDetailSeq(SqlTransaction tran, string prefix, string suffix, int step, string tableName,
            string filedName, string filedKey, string filedValue)
        {
            string Sql = "Select IsNull(( " +
                "Select Top 1 {0} " +
                "From {1}  " +
                "Where IsNull({0},'') Like '{2}%' And IsNull({4},'')='{5}' And Len({0})=Len('{2}'+'{3}') " +
                "Order By {0} Desc " +
                "),'{2}'+'{3}')";
            string OldRowId = GetFristValue(
                tran,
                String.Format(Sql,
                filedName, tableName, prefix, suffix, filedKey, filedValue), null).ToString();
            string NewRowId = prefix + (Convert.ToInt32(OldRowId.Substring(prefix.Length)) + step).ToString().PadLeft(suffix.Length, '0');
            return NewRowId;
        }

        public static DataTable GetMasterEmpty(SqlTransaction tran, string tbName)
        {
            string sqlSelect = @"
                Select * From {0} Where 1=2
            ";
            DataTable dtEmpty = null;
            if (tran != null)
                dtEmpty = SqlHelper.ExecuteDataTable(tran, String.Format(sqlSelect, tbName), null);
            else
                dtEmpty = SqlHelper.ExecuteDataTable(String.Format(sqlSelect, tbName), null);
            return dtEmpty;
        }

        public static DataTable GetMaster(SqlTransaction tran, string tbName, DALEntity.QuerySet qrys)
        {
            DataTable dt;
            string Sql = @"
                Select * From {0}  Where 1=1 {1} {2}
                ";
            string OrderBy = "";
            StringBuilder sb = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            if (qrys != null)
            {
                foreach (DALEntity.QueryInfo qi in qrys.QueryInfos)
                {
                    sb.Append(string.Format(" And [{0}] {1} @{2} ", qi.FieldName, qi.Oper, qi.ParamName));
                    parms.Add(new SqlParameter("@" + qi.ParamName, qi.ParamValue));
                }
                OrderBy = qrys.OrderBy != "" ? " Order By " + qrys.OrderBy : "";
            }
            if (tran != null)
                dt = SqlHelper.ExecuteDataTable(tran, String.Format(Sql, tbName, sb.ToString(), OrderBy), parms.ToArray());
            else
                dt = SqlHelper.ExecuteDataTable(String.Format(Sql, tbName, sb.ToString(), OrderBy), parms.ToArray());
            return dt;
        }

        public static void ApplyLogAction(SqlTransaction tran, DALEntity.Sys_Log la)
        {
            if (la == null) return;
            DataTable dt = DALHelper.GetMasterEmpty(tran, "Sys_Log");
            DataRow dr;
            dr = dt.NewRow();
            dr["LogID"] = la.LogID;
            dr["Module"] = la.Module;
            dr["Page"] = la.Page;
            dr["Action"] = la.Action;
            dr["Table"] = la.Table;
            dr["Key"] = la.Key;
            dr["Value"] = la.Value;
            dr["User"] = la.User;
            dr["WlanIP"] = la.WlanIP;
            dr["LanIP"] = la.LanIP;
            dr["MacAddr"] = la.MacAddr;
            dr["PCName"] = la.PCName;
            dr["OS"] = la.OS;
            dr["Browser"] = la.Browser;
            dr["DateTime"] = la.DateTime;
            dr["Backup"] = GetBackupString(tran, la);
            dt.Rows.Add(dr);
            new RDFNew.Module.Admin.Sys.Sys_Log().ApplyMaster(dt, tran);
        }

        static string GetBackupString(SqlTransaction tran, DALEntity.Sys_Log la)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            if (la != null)
            {
                if (la.Action.ToLower() == "edit" || la.Action.ToLower() == "delete")
                {
                    DataTable dt = null;
                        dt = GetMaster(tran, la.Table,
                            new RDFNew.Module.DALEntity.QuerySet(new List<RDFNew.Module.DALEntity.QueryInfo>(){
                                new RDFNew.Module.DALEntity.QueryInfo(la.Key,"=",la.Value)}));                                 
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dt.Rows[0][dc.ColumnName].ToString().Length <= 6000)
                        {
                            sb.Append(String.Format("[{0}]:{1}|", dc.ColumnName, dt.Rows[0][dc.ColumnName]));
                        }
                    }
                }
            }
            return sb.ToString().TrimEnd('|');
        }
    }
}
