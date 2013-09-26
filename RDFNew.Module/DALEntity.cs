using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDFNew.Module.DALEntity
{
    public class QuerySet
    {
        public PageInfo PageInfo = null;
        public List<QueryInfo> QueryInfos = new List<QueryInfo>();
        string mOrderBy = "";

        public QuerySet() { }

        public QuerySet(List<QueryInfo> _QueryInfos)
        {
            QueryInfos = _QueryInfos;
        }

        public QuerySet(List<QueryInfo> _QueryInfos, PageInfo _PageInfo)
        {
            QueryInfos = _QueryInfos;
            PageInfo = _PageInfo;
        }

        public QuerySet(List<QueryInfo> _QueryInfos, string _OrderBy)
        {
            QueryInfos = _QueryInfos;
            mOrderBy = _OrderBy;
        }

        public QuerySet(List<QueryInfo> _QueryInfos, PageInfo _PageInfo, string _OrderBy)
        {
            QueryInfos = _QueryInfos;
            PageInfo = _PageInfo;
            mOrderBy = _OrderBy;
        }

        public string OrderBy
        {
            get
            {
                if (String.IsNullOrEmpty(mOrderBy))
                    return "";
                return mOrderBy.Trim();
            }
            set
            {
                mOrderBy = value;
            }
        }
    }

    public class PageInfo
    {
        public int PageSize = 5;
        public int PageIndex = 0;

        public PageInfo() { }

        public PageInfo(int _PageSize, int _PageIndex)
        {
            PageSize = _PageSize;
            PageIndex = _PageIndex;
        }
    }

    public class QueryInfo
    {
        public string Union = "And";
        public string GroupBegin = "";
        public string GroupEnd = "";
        public string FieldName = "";
        public string Oper = "";
        public string ParamName = "";
        public string ParamValue = "";

        public QueryInfo() { }

        public QueryInfo(string _Union, string _GroupBegin, string _GroupEnd, string _FieldName, string _Oper, string _ParamName, string _ParamValue)
        {
            Union = _Union;
            GroupBegin = _GroupBegin;
            GroupEnd = _GroupEnd;
            FieldName = _FieldName;
            Oper = _Oper;
            ParamName = _ParamName;
            ParamValue = _ParamValue;
        }

        public QueryInfo(string _Union, string _FieldName, string _Oper, string _ParamName, string _ParamValue)
        {
            Union = _Union;
            FieldName = _FieldName;
            Oper = _Oper;
            ParamName = _ParamName;
            ParamValue = _ParamValue;
        }

        public QueryInfo(string _FieldName, string _Oper, string _ParamName, string _ParamValue)
        {
            FieldName = _FieldName;
            Oper = _Oper;
            ParamName = _ParamName;
            ParamValue = _ParamValue;
        }

        public QueryInfo(string _FieldName, string _Oper, string _ParamValue)
        {
            FieldName = _FieldName;
            Oper = _Oper;
            ParamName = _FieldName;
            ParamValue = _ParamValue;
        }
    }

    public class Sys_Log
    {
        public string LogID;
        public string Module;
        public string Page;
        public string Action;
        public string Table;
        public string Key;
        public string Value;
        public string User;
        public string WlanIP;
        public string LanIP;
        public string MacAddr;
        public string PCName;
        public string OS;
        public string Browser;
        public string DateTime;
        public string Backup;

        public Sys_Log() { }

        public Sys_Log(
        string _LogID,
        string _Module,
        string _Page,
        string _Action,
        string _Table,
        string _Key,
        string _Value,
        string _User,
        string _WlanIP,
        string _LanIP,
        string _MacAddr,
        string _PCName,
        string _OS,
        string _Browser,
        string _DateTime,
        string _Backup
            )
        {
            LogID = _LogID;
            Module = _Module;
            Page = _Page;
            Action = _Action;
            Table = _Table;
            Key = _Key;
            Value = _Value;
            User = _User;
            WlanIP = _WlanIP;
            LanIP = _LanIP;
            MacAddr = _MacAddr;
            PCName = _PCName;
            OS = _OS;
            Browser = _Browser;
            DateTime = _DateTime;
            Backup = _Backup;
        }
    }
}
