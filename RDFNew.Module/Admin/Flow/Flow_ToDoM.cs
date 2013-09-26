﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Flow
{
    public class Flow_ToDoM : ISingle
    {
        const string MTABLE = "Flow_ToDoM";
        const string MKEY = "ToDoMID";

        public object[] GetMaster(string KeyValue)
        {
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Flow_ToDoM.ToDoMID", "=", "ToDoMID", KeyValue));
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
                            Flow_ToDoM.*,b.EmployeeName As OwnerName 
                    From Flow_ToDoM     
                        Left Join Bas_Employee b On b.EmployeeID=Flow_ToDoM.OwnerID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Flow_ToDoM                         
                        Left Join Bas_Employee b On b.EmployeeID=Flow_ToDoM.OwnerID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Flow_ToDoM.*,b.EmployeeName As OwnerName 
                    From Flow_ToDoM     
                        Left Join Bas_Employee b On b.EmployeeID=Flow_ToDoM.OwnerID              
                    Where 1=1 {0} And Flow_ToDoM.ToDoMID Not In (
                        Select Top {3} Flow_ToDoM.ToDoMID 
                        From Flow_ToDoM                             
                            Left Join Bas_Employee b On b.EmployeeID=Flow_ToDoM.OwnerID
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

        public object[] ApplyMaster(DataTable dtM, RDFNew.Module.DALEntity.Sys_Log la)
        {
            DataTable dtDetail = null;
            if (dtM.Rows[0].RowState ==DataRowState.Deleted || (la != null && la.Action.ToUpper() == "DELETE"))
            {
                dtDetail = new RDFNew.Module.Admin.Flow.Flow_ToDoD().GetDataByParent(
                    dtM.Rows[0]["ToDoMID", DataRowVersion.Original].ToString())[1] as DataTable;
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                    dtDetail.Rows[i].Delete();
            }
            return ApplyMaster(dtM, dtDetail,null, la);
        }

        public object[] ApplyMaster(DataTable dtM, DataTable dtD, SqlTransaction _tran, RDFNew.Module.DALEntity.Sys_Log la)
        {
            object[] rtns = new object[] { 0, "", null };
            if (dtM == null)
                return rtns;

            SqlConnection conn = null;
            SqlTransaction tran = null;
            if (_tran != null)
            {
                tran = _tran;
            }
            else
            {
                conn = new SqlConnection(SqlHelper.ConnectionString);
                conn.Open();
                tran = conn.BeginTransaction();
            }
            try
            {
                foreach (DataRow dr in dtM.Rows)
                {
                    if (dr.RowState == DataRowState.Added)
                    {
                        //新增
                        if (dr[MKEY].ToString() == "")
                            dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyyMM"), "0000", MTABLE, MKEY);
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

                    //处理子表
                    new Flow_ToDoD().ApplyMaster(dr, rtns[1].ToString(), dtD, tran, la);
                }
                if (_tran == null)
                    tran.Commit();
            }
            catch (Exception ex)
            {
                if (_tran == null)
                    tran.Rollback();
                rtns[0] = 1;
                rtns[1] = ex;
            }
            finally
            {
                if (_tran == null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return rtns;
        }

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            //if (DALHelper.IsExist(tran, @"
            //        Select Top 1 UnitTypeCode From Bas_Unit Where IsNull(UnitTypeCode,'')=@UnitTypeCode                    
            //        ",
            //    new SqlParameter[] { 
            //            new SqlParameter("@UnitTypeCode",d["UnitTypeCode",DataRowVersion.Original])
            //        }))
            //{
            //    throw new Exception(String.Format(@"单位类别 {0} 已被使用.",
            //        d["UnitTypeCode", DataRowVersion.Original]));
            //}
        }
    }
}
