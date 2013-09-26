using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using FineUI;
using System.IO;

namespace RDFNew.Web.App_Com
{
    public class ImageHelper
    {
        public static void DeleteImage(DataRow dr,bool IsDel)
        {
            if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Deleted)
            {
                string _OrgFile = "";
                if (dr.Table.Columns.Contains("ImageS"))
                {
                    if (dr["ImageS", DataRowVersion.Original] != System.DBNull.Value && dr["ImageS", DataRowVersion.Original].ToString() != "")
                    {
                        _OrgFile = HttpContext.Current.Server.MapPath(dr["ImageS", DataRowVersion.Original].ToString());
                        if (System.IO.File.Exists(_OrgFile)) { System.IO.File.Delete(_OrgFile); }
                        if (dr.RowState == DataRowState.Modified && IsDel) dr["ImageS"] = "";
                    }
                }
                if (dr.Table.Columns.Contains("ImageM"))
                {
                    if (dr["ImageM", DataRowVersion.Original] != System.DBNull.Value && dr["ImageM", DataRowVersion.Original].ToString() != "")
                    {
                        _OrgFile = HttpContext.Current.Server.MapPath(dr["ImageM", DataRowVersion.Original].ToString());
                        if (System.IO.File.Exists(_OrgFile)) { System.IO.File.Delete(_OrgFile); }
                        if (dr.RowState == DataRowState.Modified && IsDel) dr["ImageM"] = "";
                    }
                }
                if (dr.Table.Columns.Contains("ImageL"))
                {
                    if (dr["ImageL", DataRowVersion.Original] != System.DBNull.Value && dr["ImageL", DataRowVersion.Original].ToString() != "")
                    {
                        _OrgFile = HttpContext.Current.Server.MapPath(dr["ImageL", DataRowVersion.Original].ToString());
                        if (System.IO.File.Exists(_OrgFile)) { System.IO.File.Delete(_OrgFile); }
                        if (dr.RowState == DataRowState.Modified && IsDel) dr["ImageL"] = "";
                    }
                }
            }
        }

        public static object[] UploadImageS(System.Web.UI.WebControls.FileUpload upFile, string _ServerPath)
        {
            object[] rtn = new object[] { 1, "", "", "" }; //0成功1失败
            string fileName = upFile.FileName;
            string ext = fileName.Substring(fileName.LastIndexOf("."));
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string S = fileName + "_S" + ext;
            fileName = fileName + "_" + ext;
            string ServerPath = HttpContext.Current.Server.MapPath(_ServerPath);
            if (!System.IO.Directory.Exists(ServerPath))
                System.IO.Directory.CreateDirectory(ServerPath);
            if (upFile.HasFile)
                upFile.SaveAs(ServerPath + fileName);
            try
            {
                App_Com.ImageHelper.MakeThumbnail(ServerPath + fileName, ServerPath + S, 200, 150, "W", App_Com.Helper.GetCompanyName(), "RB", 10);
                rtn[0] = 0;
                rtn[1] = _ServerPath + S;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
            finally
            {
                if (System.IO.File.Exists(ServerPath + fileName)) { System.IO.File.Delete(ServerPath + fileName); }
            }
            return rtn;
        }

        public static object[] UploadImage(System.Web.UI.WebControls.FileUpload upFile, string _ServerPath,string SML)
        {
            object[] rtn = new object[] { 1, "", "", "" }; //0成功1失败
            string fileName = upFile.FileName;
            string ext = fileName.Substring(fileName.LastIndexOf("."));
            fileName = fileName.Replace(":", "_").Replace(" ", "_").Replace("\\", "_").Replace("/", "_");
            fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string L = fileName + "_L" + ext;
            string M = fileName + "_M" + ext;
            string S = fileName + "_S" + ext;
            fileName = fileName + "_" + ext;
            string ServerPath = HttpContext.Current.Server.MapPath(_ServerPath);
            if (!System.IO.Directory.Exists(ServerPath))
                System.IO.Directory.CreateDirectory(ServerPath);
            if (upFile.HasFile)
                upFile.SaveAs(ServerPath + fileName);
            try
            {
                if (SML.ToUpper().IndexOf('L') >= 0)
                    App_Com.ImageHelper.MakeThumbnail(ServerPath + fileName, ServerPath + L, 800, 600, "W", App_Com.Helper.GetCompanyName(), "RB", 16);
                if (SML.ToUpper().IndexOf('M') >= 0)
                    App_Com.ImageHelper.MakeThumbnail(ServerPath + fileName, ServerPath + M, 400, 300, "W", App_Com.Helper.GetCompanyName(), "RB", 14);
                if (SML.ToUpper().IndexOf('S') >= 0)
                    App_Com.ImageHelper.MakeThumbnail(ServerPath + fileName, ServerPath + S, 200, 150, "W", App_Com.Helper.GetCompanyName(), "RB", 10);                
                rtn[0] = 0;
                rtn[1] = _ServerPath + L;
                rtn[2] = _ServerPath + M;
                rtn[3] = _ServerPath + S;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }
            finally
            {
                if (System.IO.File.Exists(ServerPath + fileName)) { System.IO.File.Delete(ServerPath + fileName); }
            }
            return rtn;
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, 
            int width, int height, string mode,string water,string Loc,int FontSize)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            string type = "";
            if (originalImagePath.ToUpper().EndsWith(".JPG") || originalImagePath.ToUpper().EndsWith(".JPEG"))
                type = "JPG";
            else if (originalImagePath.ToUpper().EndsWith(".BMP"))
                type = "BMP";
            else if (originalImagePath.ToUpper().EndsWith(".GIF"))
                type = "GIF";
            else if (originalImagePath.ToUpper().EndsWith(".PNG"))
                type = "PNG";
            else
                return;

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
                case "Cut"://指定高宽裁减（不变形） 
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
                case "DB"://等比缩放（不变形，如果高大按高，宽大按宽缩放） 
                    if ((double)originalImage.Width / (double)towidth < (double)originalImage.Height / (double)toheight)
                    {
                        toheight = height;
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    else
                    {
                        towidth = width;
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
            new System.Drawing.Rectangle(x, y, ow, oh),
            System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //保存水印
                System.Drawing.Font f = new System.Drawing.Font("Verdana",FontSize);
                System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                System.Drawing.PointF p = new System.Drawing.PointF();
                switch (Loc)
                {
                    case "LT": //左上                        
                        break;
                    case "LM": //左中
                        p.Y = toheight / 2 - g.MeasureString(water, f).Height / 2;
                        break;
                    case "LB": //左下
                        p.Y = toheight - g.MeasureString(water, f).Height;
                        break;
                    case "TC": //上中
                        p.X = towidth / 2 - g.MeasureString(water, f).Width / 2;
                        break;
                    case "C": //正中
                        p.Y = toheight / 2 - g.MeasureString(water, f).Height / 2;
                        p.X = towidth / 2 - g.MeasureString(water, f).Width / 2;
                        break;
                    case "BC": //下中
                        p.Y = toheight - g.MeasureString(water, f).Height;
                        p.X = towidth / 2 - g.MeasureString(water, f).Width / 2;
                        break;
                    case "RT": //右上
                        p.X = towidth - g.MeasureString(water, f).Width;
                        break;
                    case "RM": //右中
                        p.Y = toheight / 2 - g.MeasureString(water, f).Height / 2;
                        p.X = towidth - g.MeasureString(water, f).Width;
                        break;
                    case "RB": //右下
                        p.Y = toheight - g.MeasureString(water, f).Height;
                        p.X = towidth - g.MeasureString(water, f).Width;
                        break;
                }
                if (p.X < 0) p.X = 0;
                if (p.Y < 0) p.Y = 0;
                g.DrawString(water, f, b,p);

                //保存缩略图
                if (type == "JPG")
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                if (type == "BMP")
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                if (type == "GIF")
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif);
                }
                if (type == "PNG")
                {
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png);
                }
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

        /**/
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        public static void AddShuiYinWord(string water,string Path, string Path_sy)
        {            
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);            
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 16);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

            g.DrawString(water, f, b, 15, 15);
            g.Dispose();

            image.Save(Path_sy);
            
            image.Dispose();
        }

        /**/
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddShuiYinPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
    }
}
