using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;

namespace RDFNew.Module.Admin.Pur
{
    public class Pur_PoD : ISingle
    {
        const string MTABLE = "Pur_PoD";
        const string MKEY = "PoDID";

        public object[] GetDataByParent(string ParentKey)
        {
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Pur_PoD.PoMID", "=", "PoMID", ParentKey));
            qs.OrderBy = " Pur_PoD.Seq ";
            return GetMaster(qs);
        }

        public object[] GetMaster(string KeyValue)
        {            
            RDFNew.Module.DALEntity.QuerySet qs = new RDFNew.Module.DALEntity.QuerySet();
            qs.QueryInfos.Add(new RDFNew.Module.DALEntity.QueryInfo("Pur_PoD.PoDID", "=", "PoDID", KeyValue));
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
                            Pur_PoD.*,b.MaterialName
                    From Pur_PoD 
                        Left Join Bas_Material b On b.MaterialID=Pur_PoD.MaterialID
                    Where 1=1 {0} {1}
                    ";
                    rtn[1] = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.Text,
                    String.Format(Sql, sb.ToString(), OrderBy), parms.ToArray());
                }
                else
                {
                    Sql = @"
                    Select Count(1) 
                    From Pur_PoD 
                        Left Join Bas_Material b On b.MaterialID=Pur_PoD.MaterialID
                    Where 1=1 {0}
                    ";
                    rtn[2] = SqlHelper.ExecuteScalar(String.Format(Sql,
                        sb.ToString()), parms.ToArray());
                    Sql = @"
                    Select Top {2} 
                            Pur_PoD.*,b.MaterialName
                    From Pur_PoD 
                        Left Join Bas_Material b On b.MaterialID=Pur_PoD.MaterialID
                    Where 1=1 {0} And Pur_PoD.PoDID Not In (
                        Select Top {3} Pur_PoD.PoDID 
                        From Pur_PoD 
                            Left Join Bas_Material b On b.MaterialID=Pur_PoD.MaterialID
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
            return ApplyMaster(null,"",dt,null, la);
        }

        public object[] ApplyMaster(DataRow drM, string MID, DataTable dtD, SqlTransaction tran, RDFNew.Module.DALEntity.Sys_Log la)
        {
            object[] rtns = new object[] { 0, "", null };
            if (drM == null || String.IsNullOrEmpty(MID) || dtD == null)
                return rtns;
            foreach (DataRow dr in dtD.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    //新增
                    dr[MKEY] = DALHelper.GetMasterNo(tran, System.DateTime.Now.ToString("yyyy"), "00000000", MTABLE, MKEY);                        
                    dr["PoMID"] = MID;
                    if (dr["Seq"].ToString() == "")
                        dr["Seq"] = DALHelper.GetDetailSeq(tran, "", "0000", 1, "Pur_POD", "Seq", "PoMID", MID);
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
            }
            return rtns;
        }

        void ApplyMaster_BeforeDelete(DataRow d, SqlTransaction tran)
        {
            
        }
    }
}
