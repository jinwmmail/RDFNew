using System;
using System.Data ;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDFNew.Module.Admin
{
    public interface ISingle
    {
        object[] GetMaster(DALEntity.QuerySet qrys);

        object[] GetMaster(String KeyVal);

        object[] ApplyMaster(DataTable dt, RDFNew.Module.DALEntity.Sys_Log la);
        
    }
}
