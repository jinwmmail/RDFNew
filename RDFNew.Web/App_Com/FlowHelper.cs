using System;
using System.Data;
using System.Configuration;
using DS.XBPM.API;
using System.IO;
using System.Web.UI;
using System.Web;
namespace RDFNew.Web.App_Com
{
    public class FlowHelper
    {
        internal static string PDL_Folder
        {
            get
            {
                Page page = HttpContext.Current.Handler as Page;
                return page.MapPath("~/_PDLFolder");
            }
        }

        internal static int PDL_Port
        {
            get
            {                
                return 7070;
            }
        }

        internal static DS.XBPM.API.ProcessEngine Engine
        {
            get
            {
                DS.XBPM.API.Connection conn = new DS.XBPM.API.Connection("127.0.0.1", PDL_Port);
                return conn.BuildProcessEngine();
            }
        }

        internal static string ReplaceConst(string str)
        {
            str = str.Replace("FlowNode", "流程节点");
            str = str.Replace("Activity", "活动");

            str = str.Replace("Created", "创建");
            str = str.Replace("Completed", "完成");
            str = str.Replace("Suspended", "挂起");
            str = str.Replace("Resumed", "恢复");
            str = str.Replace("Terminated", "终止");

            str = str.Replace("Init", "流入");
            str = str.Replace("Fina", "流出");

            str = str.Replace("Close", "已执行");
            str = str.Replace("Cancel", "已取消");
            str = str.Replace("Error", "发生错误");

            return str;
        }

        internal static string ReplaceConst(EndStateType endState)
        {
            string str = "未知";

            switch (endState)
            {
                case EndStateType.Completed:
                    str = "完成";
                    break;
                case EndStateType.Terminated:
                    str = "终止";
                    break;
            }

            return str;
        }

        internal static string ReplaceConst(CurrentStateType currentState)
        {
            string str = "未知";

            switch (currentState)
            {
                case CurrentStateType.Running:
                    str = "正在运行";
                    break;
                case CurrentStateType.Suspended:
                    str = "已挂起";
                    break;
            }

            return str;
        }
    }
}
