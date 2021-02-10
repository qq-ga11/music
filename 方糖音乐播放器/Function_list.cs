﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace 方糖音乐播放器
{
    public static class Function_list
    {
        //秒转分钟函数--秒
        public static string sec_to_hms(double duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = String.Format("{0:00}", ts.Hours) + ":" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00:" + String.Format("{0:00}", ts.Minutes) + ":" + String.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:00:" + String.Format("{0:00}", ts.Seconds);
            }
            return str;
        }
        //立即回收内存
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        public static void FlushMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        //截取字符串--目标--从这里--到这里
        public static string Substring(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            try
            {
                startindex = sourse.IndexOf(startstr);
                if (startindex == -1)
                {
                    return result;
                }
                string tmpstr = sourse.Substring(startindex + startstr.Length);
                endindex = tmpstr.IndexOf(endstr);
                if (endindex == -1)
                {
                    return result;
                }
                result = tmpstr.Remove(endindex);
            }
            catch { }
            return result;
        }
        //图片转bitmapImage对象，防止图片被占用
        public  static BitmapImage Album_pictures(string file)
        {
            if (file != null)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                try
                {
                    bitmapImage.UriSource = new Uri(file);//图片的全路径
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
                catch { return null; }
            }
            else { return null; }
        }
        //读取专辑文件
        public static BitmapImage GetCover(string path)
        {
            BitmapImage bmp = new BitmapImage();
            TagLib.File f = TagLib.File.Create(path);
            try
            {
                if (f.Tag.Pictures != null && f.Tag.Pictures.Length != 0)
                {
                    //字节流转BitmapImage对象
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                    bmp.EndInit();
                    return bmp;
                }
            }
            catch //(Exception ex)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(System.IO.Path.GetTempPath() + "临时文件.png", FileMode.Create, FileAccess.Write);
                    fs.Write(f.Tag.Pictures[0].Data.Data, 0, f.Tag.Pictures[0].Data.Data.Length);
                }
                catch { }
                finally
                {
                    fs.Close();//释放文件句柄
                    fs.Dispose();
                }
                System.Drawing.Image Dummy = System.Drawing.Image.FromFile(System.IO.Path.GetTempPath() + "临时文件.png");
                Dummy.Save(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png", System.Drawing.Imaging.ImageFormat.Bmp);
                bmp = Function_list.Album_pictures(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png");
                Dummy.Dispose();
                System.IO.File.Delete(System.IO.Path.GetDirectoryName(path) + @"\" + System.IO.Path.GetFileNameWithoutExtension(path) + ".png");
                System.IO.File.Delete(System.IO.Path.GetTempPath() + "临时文件.png");
                return bmp;
            }
            return null;
        }
        //填充菜单
        public static void 填充菜单(System.Windows.Controls.ListBox box, string name, string tooltip, int width, int height, int fontsize, bool Centered)
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = name;//设置显示名称
            item.Width = width;//设置宽度
            item.Height = height;//设置高度
            item.FontSize = fontsize;//设置字号
            item.ToolTip = tooltip;//设置提示
            if (Centered == true) { item.HorizontalContentAlignment = HorizontalAlignment.Center; }//设置文本上下左右居中
            box.Items.Add(item);//将控件添加到集合里
        }
    }
}
