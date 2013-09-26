using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using RDFNew.Module;
using FineUI;

namespace RDFNew.Web.App_Com
{
    public class Helper
    {
        static string m_CompanyName = "";

        public static string GetCompanyName()
        {
            if (m_CompanyName == "")
                m_CompanyName = System.Configuration.ConfigurationManager.AppSettings["CompanyName"];
            return m_CompanyName;
        }

        public static string GetRootURI()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            HttpRequest Req;
            if (HttpCurrent != null)
            {
                Req = HttpCurrent.Request;

                string UrlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);
                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                    //直接安装在   Web   站点   
                    AppPath = UrlAuthority;
                else
                    //安装在虚拟子目录下   
                    AppPath = UrlAuthority + Req.ApplicationPath;
            }
            return AppPath;
        }

        public static string InputText(string text, int maxLength)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
        }

        public static string CleanNonWord(string text)
        {
            return Regex.Replace(text, "\\W", "");
        }

        public static void SetSession(string key, object obj)
        {
            HttpContext.Current.Session.Add(key, obj);
        }

        public static object GetSession(string key, bool gologin)
        {
            object o = HttpContext.Current.Session[key];
            if (o == null && gologin)
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx", true);
            }
            return o;
        }

        public static void SetCache(string key, object obj)
        {
            if (obj == null)
                HttpContext.Current.Cache.Remove(key);
            else
            {
                HttpContext.Current.Cache.Insert(key, obj, null, System.DateTime.Now.AddMinutes(30), TimeSpan.Zero);
            }
        }

        public static object GetCache(string key)
        {
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// 取当前月第一天
        /// </summary>
        /// <returns></returns>
        public static string GetFirstDay()
        {
            return System.DateTime.Now.ToString("yyyy-MM") + "-01";
        }

        /// <summary>
        /// 取当前月最后天
        /// </summary>
        /// <returns></returns>
        public static string GetLastDay()
        {
            return Convert.ToDateTime(System.DateTime.Now.AddMonths(1).ToString("yyyy-MM") + "-01").AddDays(-1).ToString("yyyy-MM-dd");
        }

        public static string GetPYString(string str, int len)
        {
            string tempStr = "";
            int count = str.Length > len ? len : str.Length;
            char c;
            for (int i = 0; i < count; i++)
            {
                if (i == str.Length)
                    break;
                c = str[i];
                int x = (int)c;
                if (x >= 32 && x <= 126)
                {
                    tempStr += c.ToString().ToUpper();
                }
                else if (x == 65289 || x == 65288)
                {
                    //全角括号
                    count++;
                    continue;
                }
                else
                {//累加拼音声母   
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }
        /// <summary>
        /// 取单个字符的拼音声母
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        static string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i == 58557) return "H";//浣字的首字母
            if (i == 63182) return "X";//鑫字首字母
            if (i == 60105) return "S";//晟字首字母
            if (i == 59627) return "H";//桦字首字母
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "A";
            if (i < 0xB2C1) return "B";
            if (i < 0xB4EE) return "C";
            if (i < 0xB6EA) return "D";
            if (i < 0xB7A2) return "E";
            if (i < 0xB8C1) return "F";
            if (i < 0xB9FE) return "G";
            if (i < 0xBBF7) return "H";
            if (i < 0xBFA6) return "J";
            if (i < 0xC0AC) return "K";
            if (i < 0xC2E8) return "L";
            if (i < 0xC4C3) return "M";
            if (i < 0xC5B6) return "N";
            if (i < 0xC5BE) return "O";
            if (i < 0xC6DA) return "P";
            if (i < 0xC8BB) return "Q";
            if (i < 0xC8F6) return "R";
            if (i < 0xCBFA) return "S";
            if (i < 0xCDDA) return "T";
            if (i < 0xCEF4) return "W";
            if (i < 0xD1B9) return "X";
            if (i < 0xD4D1) return "Y";
            if (i < 0xD7FA) return "Z";
            return "";
        }

        /**/
        /// <summary> 
        /// 生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath,
            int width, int height, string mode, string water)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(width, height);

            //新建一个画板 
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(System.Drawing.Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, (height - toheight) / 2, towidth, toheight),
             new System.Drawing.Rectangle(x, y, ow, oh),
             System.Drawing.GraphicsUnit.Pixel);

            if ((towidth >= 100 || toheight >= 100) && !String.IsNullOrEmpty(water)) //太小图片不加水印
            {
                float fontsize = width / 36f;
                StringFormat sf = new StringFormat();
                sf.Alignment = System.Drawing.StringAlignment.Center;
                sf.LineAlignment = System.Drawing.StringAlignment.Center;

                g.DrawString(water, new Font("Arial", fontsize, System.Drawing.FontStyle.Bold),
                 System.Drawing.Brushes.White, width / 2, height / 2, sf);
            }

            try
            {
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        public static RDFNew.Module.DALEntity.Sys_Log BuildLog(string mcode, string fcode)
        {
            RDFNew.Module.DALEntity.Sys_Log la = new RDFNew.Module.DALEntity.Sys_Log();
            la.LogID = "";
            la.Module = mcode;
            la.Page = HttpContext.Current.Request.RawUrl;
            la.Action = fcode;
            la.User = App_Com.Sys_User.GetUserInfo("UserID");
            la.WlanIP = GetPortalanIP();
            la.LanIP = HttpContext.Current.Request.UserHostAddress;
            la.MacAddr = ""; 
            la.PCName = System.Web.HttpContext.Current.Request.UserHostName;
            la.OS = System.Web.HttpContext.Current.Request.Browser.Platform.ToString();
            la.Browser = System.Web.HttpContext.Current.Request.Browser.Browser.ToString() +
                System.Web.HttpContext.Current.Request.Browser.Version.ToString();
            la.DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return la;
        }

        public static string GetPortalanIP()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 使用SHA1加密字符串。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>加密后的字符串。（40个字符）</returns>
        public static string StringToSHA1Hash(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return "";
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] encryptedBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString();
        }

        private static byte[] key = ASCIIEncoding.ASCII.GetBytes("www.goldtech.com");
        private static byte[] iv = ASCIIEncoding.ASCII.GetBytes("12345678");

        /// <summary>
        /// DES加密。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>加密后的字符串。</returns>
        public static string DESEncrypt(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return "";
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamWriter sw = null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                sw = new StreamWriter(cs);
                sw.Write(inputString);
                sw.Flush();
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            finally
            {
                if (sw != null) sw.Close();
                if (cs != null) cs.Close();
                if (ms != null) ms.Close();
            }
        }

        /// <summary>
        /// DES解密。
        /// </summary>
        /// <param name="inputString">输入字符串。</param>
        /// <returns>解密后的字符串。</returns>
        public static string DESDecrypt(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return "";
            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamReader sr = null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            try
            {
                ms = new MemoryStream(Convert.FromBase64String(inputString));
                cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
            finally
            {
                if (sr != null) sr.Close();
                if (cs != null) cs.Close();
                if (ms != null) ms.Close();
            }
        }

        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /*
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.AddHeader("content-disposition", 
                    String.Format("attachment; filename={0}.xls",System.DateTime.Now.ToString("MMddHHmmss")));
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(App_Com.Helper.GetGridTableHtml(Grid1));
                Response.End();
                  
         */
        public static string GetGridTableHtml(Grid grid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=UTF-8\"/>");
            sb.Append("<table cellspacing=\"0\" rules=\"all\" border=\"1\" style=\"border-collapse:collapse;\">");
            sb.Append("<tr>");
            foreach (GridColumn column in grid.Columns)
            {
                if (!column.Hidden)
                    sb.AppendFormat("<td>{0}</td>", column.HeaderText);
            }
            sb.Append("</tr>");
            foreach (GridRow row in grid.Rows)
            {
                sb.Append("<tr>");
                foreach (GridColumn column in grid.Columns)
                {
                    if (!column.Hidden)
                    {
                        string html = row.Values[column.ColumnIndex].ToString();
                        // 处理CheckBox
                        if (html.Contains("box-grid-static-checkbox"))
                        {
                            if (html.Contains("box-grid-static-checkbox-uncheck"))
                            {
                                html = "×";
                            }
                            else
                            {
                                html = "√";
                            }
                        }

                        // 处理图片
                        if (html.ToLower().Contains("<img"))
                        {
                            string prefix = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, "");
                            if (html.ToLower().Contains("../"))
                            {
                                html = html.Replace("../", "");
                                prefix = prefix + "/";
                            }
                            html = html.Replace("src=\"", "src=\"" + prefix);
                        }
                        if (html.ToLower().StartsWith("<a") && html.ToLower().EndsWith("</a>"))
                        {
                            sb.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", html.Substring(html.IndexOf("\">") + 2).Replace("</a>", ""));
                            continue;
                        }
                        if (html.ToLower().Contains("class=_csiscolor") && html.ToUpper().Contains("BACKGROUND-COLOR:"))
                        {
                            sb.AppendFormat("<td style=\"BACKGROUND-COLOR:{0};\">&nbsp;</td>", html.Substring(html.ToUpper().LastIndexOf("BACKGROUND-COLOR:") + 17, 8));
                            continue;
                        }                        
                        if (column.TextAlign == FineUI.TextAlign.Right)
                            sb.AppendFormat("<td>{0}</td>", html);
                        else
                            sb.AppendFormat("<td style='vnd.ms-excel.numberformat:@'>{0}</td>", html);
                    }
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        /// <summary>
        /*
         * BLL.Sys.Dept bll = new BLL.Sys.Dept();
            DataTable dt = bll.GetDept();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" +
                this.GetType().Name + "_" + System.DateTime.Now.ToString("yyMMdd_fff") + ".xls");
            Response.Charset = "GB2312";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.Write(App_Code.Util.GetExcelString(dt));
            Response.End();
         * */
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetExcelString(DataTable dt)
        {
            string Template = "" +
                "<?xml version=\"1.0\" encoding=\"GB2312\"?> \r\n" +
                "<?mso-application progid=\"Excel.Sheet\"?> \r\n" +
                "<Workbook \r\n" +
                "   xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n" +
                "   xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n" +
                "   xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n" +
                "   xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n" +
                "   xmlns:html=\"http://www.w3.org/TR/REC-html40\"> \r\n" +
                "   <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"> \r\n" +
                "       <Title>MS Excel</Title> \r\n" +
                "       <LastAuthor>Nothing</LastAuthor> \r\n" +
                "   </DocumentProperties> \r\n" +
                "   <Styles> \r\n" +
                "       <Style ss:ID=\"Default\" ss:Name=\"Normal\"> \r\n" +
                "           <Alignment ss:Vertical=\"Center\"/> \r\n" +
                "           <Font ss:FontName=\"宋体\" x:CharSet=\"134\" ss:Size=\"12\"/> \r\n" +
                "       </Style> \r\n" +
                "   </Styles> \r\n" +
                "{0}" +
                "</Workbook> \r\n" +
                "";
            return String.Format(Template, GetExcelWorksheet(1, dt));
        }

        public static string GetExcelString(DataSet ds)
        {
            string Template = "" +
                "<?xml version=\"1.0\" encoding=\"GB2312\"?> \r\n" +
                "<?mso-application progid=\"Excel.Sheet\"?> \r\n" +
                "<Workbook \r\n" +
                "   xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n" +
                "   xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n" +
                "   xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n" +
                "   xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" \r\n" +
                "   xmlns:html=\"http://www.w3.org/TR/REC-html40\"> \r\n" +
                "   <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"> \r\n" +
                "       <Title>MS Excel</Title> \r\n" +
                "       <LastAuthor>Nothing</LastAuthor> \r\n" +
                "   </DocumentProperties> \r\n" +
                "   <Styles> \r\n" +
                "       <Style ss:ID=\"Default\" ss:Name=\"Normal\"> \r\n" +
                "           <Alignment ss:Vertical=\"Center\"/> \r\n" +
                "           <Font ss:FontName=\"宋体\" x:CharSet=\"134\" ss:Size=\"12\"/> \r\n" +
                "       </Style> \r\n" +
                "   </Styles> \r\n" +
                "{0}" +
                "</Workbook> \r\n" +
                "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                sb.Append(GetExcelWorksheet(i + 1, ds.Tables[i]));
            }
            return String.Format(Template, sb.ToString());
        }

        public static string GetExcelWorksheet(int idx, DataTable dt)
        {
            string str = "" +
                "   <Worksheet ss:Name=\"Sheet{0}\"> \r\n" +
                "       <Table> \r\n" +
                "           {1}" +
                "       </Table> \r\n" +
                "   </Worksheet> \r\n";
            StringBuilder sb = new StringBuilder();
            //写标题    
            DataColumn dc;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                dc = dt.Columns[i];
                if (i == 0)
                {
                    sb.Append("<Row>\r\n");
                }
                sb.Append(String.Format("<Cell><Data ss:Type=\"String\">{0}</Data></Cell> \r\n", dc.ColumnName));
            }
            sb.Append("</Row> \r\n");

            //写内容 
            string ValType = "String";
            DataRow dr;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                dr = dt.Rows[j];
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    dc = dt.Columns[k];
                    if (k == 0)
                    {
                        sb.Append("<Row> \r\n");
                    }
                    if (dc.DataType == typeof(Int32) ||
                        dc.DataType == typeof(Decimal) ||
                        dc.DataType == typeof(float) ||
                        dc.DataType == typeof(Single) ||
                        dc.DataType == typeof(Double))
                    {
                        ValType = "Number";
                    }
                    else
                    {
                        ValType = "String";
                    }
                    sb.Append(String.Format("<Cell><Data ss:Type=\"{0}\">{1}</Data></Cell> \r\n", ValType, dr[k]));
                }
                sb.Append("</Row> \r\n");
            }
            return String.Format(str, idx, sb.ToString());
        }

        public static string GetColumnInfoSql(string tableName)
        {
            return String.Format(@"
                Select x.CName,x.CDesc,x.CLen,x.CType
                From (
                SELECT 
                (case when a.colorder=1 then d.name else '' end) As TName,
                a.colorder CSeq,
                a.name CName,
                (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '1'else '0' end) IsIdent,
                (case when (SELECT count(*) 
                FROM sysobjects 
                WHERE (name in (SELECT name
                FROM sysindexes
                WHERE (id = a.id) AND (indid in (SELECT indid
                FROM sysindexkeys
                WHERE (id = a.id) AND (colid in (SELECT colid
                FROM syscolumns
                WHERE (id = a.id) AND (name = a.name)
                )
                )
                )
                )
                )
                ) AND (xtype = 'PK') 
                ) > 0 then '1' else '0' end) IsKey,
                b.name CType,
                a.length CBytes,
                COLUMNPROPERTY(a.id,a.name,'PRECISION') as CLen,
                isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as CDeciNum,
                (case when a.isnullable=1 then '1'else '0' end) CanEmpty,
                isnull(e.text,'') CDefault,
                isnull(g.[value],'') AS CDesc,a.id,a.colorder 
                FROM  syscolumns a 
                left join systypes b on a.xtype=b.xusertype
                inner join sysobjects d on a.id=d.id  and  d.xtype='U' and d.name<>'dtproperties'
                left join syscomments e on a.cdefault=e.id
                left join sysproperties g on a.id=g.id AND a.colid = g.smallid  
                Where d.Name='{0}' 
                ) x
                order by x.id,x.colorder
            ", tableName);
        }

        public static void CheckDiffMinutes(String FrD, String FrT, String ToD, String ToT)
        {
            TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(FrD + " " + FrT).Ticks);
            TimeSpan ts2 = new TimeSpan(Convert.ToDateTime(ToD + " " + ToT).Ticks);
            if (ts1 >= ts2)
                throw new Exception("开始日期不可大于结束日期");
        }

        public static int GetDiffMinutes(String FrD, String FrT, String ToD, String ToT)
        {
            //标准上下班时间 
            String SStarTime = "08:30", SEndTime = "17:00";
            //标准午休时间
            String XStarTime = "12:30", XEndTime = "13:00";
            //相差分钟数
            int Minutes = 0;

            TimeSpan ts1 = new TimeSpan(Convert.ToDateTime(FrD + " " + FrT).Ticks);
            TimeSpan ts2 = new TimeSpan(Convert.ToDateTime(ToD + " " + ToT).Ticks);
            if (ts1 >= ts2)
                return 0;
            else
            {
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                int DiffDays = ts.Days;
                if (DiffDays == 0)
                {
                    DateTime Tmp = Convert.ToDateTime(FrD);
                    if (Tmp.DayOfWeek != DayOfWeek.Saturday && Tmp.DayOfWeek != DayOfWeek.Sunday)
                    {
                        //当天                                        
                        int iSStarTime = Convert.ToInt32("1" + SStarTime.Replace(":", ""));
                        int iSEndTime = Convert.ToInt32("1" + SEndTime.Replace(":", ""));
                        int iXStarTime = Convert.ToInt32("1" + XStarTime.Replace(":", ""));
                        int iXEndTime = Convert.ToInt32("1" + XEndTime.Replace(":", ""));

                        int iFrT = Convert.ToInt32("1" + FrT.Replace(":", ""));
                        int iToT = Convert.ToInt32("1" + ToT.Replace(":", ""));
                        if (iFrT < iSStarTime)
                        {
                            iFrT = iSStarTime;
                            FrT = SStarTime;
                        }
                        if (iToT > iSEndTime)
                        {
                            iToT = iSEndTime;
                            ToT = SEndTime;
                        }
                        if (iFrT > iXStarTime && iFrT < iXEndTime)
                        {
                            iFrT = iXEndTime;
                            FrT = XEndTime;
                        }
                        if (iToT > iXStarTime && iToT < iXEndTime)
                        {
                            iToT = iXStarTime;
                            ToT = XStarTime;
                        }

                        if ((iFrT >= iSStarTime && iFrT <= iXStarTime && iToT >= iSStarTime && iToT <= iXStarTime) || //都在上半 或下半
                            (iFrT >= iXEndTime && iFrT <= iSEndTime && iToT >= iXEndTime && iToT <= iSEndTime))
                        {
                            TimeSpan t1 = new TimeSpan(Convert.ToDateTime(FrD + " " + FrT).Ticks);
                            TimeSpan t2 = new TimeSpan(Convert.ToDateTime(ToD + " " + ToT).Ticks);
                            TimeSpan t3 = t1.Subtract(t2).Duration();
                            Minutes = Convert.ToInt32(t3.TotalMinutes);
                        }
                        else if (iFrT >= iSStarTime && iFrT <= iXStarTime && iToT >= iXEndTime && iToT <= iSEndTime) //上下
                        {
                            ts1 = new TimeSpan(Convert.ToDateTime(FrD + " " + FrT).Ticks);
                            ts2 = new TimeSpan(Convert.ToDateTime(ToD + " " + ToT).Ticks);
                            ts = ts1.Subtract(ts2).Duration();

                            TimeSpan t1 = new TimeSpan(Convert.ToDateTime(FrD + " " + XStarTime).Ticks);
                            TimeSpan t2 = new TimeSpan(Convert.ToDateTime(ToD + " " + XEndTime).Ticks);
                            TimeSpan t3 = t1.Subtract(t2).Duration();
                            Minutes = Convert.ToInt32(ts.TotalMinutes - t3.TotalMinutes);
                        }
                    }
                }
                else if (DiffDays == 1)
                {
                    //隔天
                    Minutes = GetDiffMinutes(FrD, FrT, FrD, SEndTime) + GetDiffMinutes(ToD, SStarTime, ToD, ToT);
                }
                else
                {
                    //多天
                    Minutes = GetDiffMinutes(FrD, FrT, FrD, SEndTime) + GetDiffMinutes(ToD, SStarTime, ToD, ToT);
                    DateTime Tmp;
                    Int32 i = DiffDays - 1;
                    while (i > 0)
                    {
                        Tmp = Convert.ToDateTime(FrD).AddDays(i);
                        if (Tmp.DayOfWeek != DayOfWeek.Saturday && Tmp.DayOfWeek != DayOfWeek.Sunday)
                            Minutes += GetDiffMinutes(Tmp.ToString("yyyy-MM-dd"), SStarTime, Tmp.ToString("yyyy-MM-dd"), SEndTime);
                        i--;
                    }
                }
            }
            return Minutes;
        }
    }
}
