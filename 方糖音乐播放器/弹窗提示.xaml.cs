﻿using System;
///using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
//using System.Linq;
//using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using System.Windows.Threading;

namespace 方糖音乐播放器.Properties
{
    /// <summary>
    /// 弹窗提示.xaml 的交互逻辑
    /// </summary>
    public partial class 弹窗提示 : Window
    {
        //播放动画函数
        Storyboard story;
        private void 动画播放(string a)
        {
            story = (Storyboard)FindResource(a);
            BeginStoryboard(story);
        }
        public event Func<int, int> fcc1;

        [DllImport("winmm.dll")]
        public static extern bool PlaySound(String Filename, int Mod, int Flags);
        [Obsolete]
        public 弹窗提示(int parameter, Color color,int Other_parameters,string file)
        {
            InitializeComponent();
            模板一.Visibility = Visibility.Collapsed;
            模板二.Visibility = Visibility.Collapsed;
            模板三.Visibility = Visibility.Collapsed;
            模板四.Visibility = Visibility.Collapsed;
            模板五.Visibility = Visibility.Collapsed;
            单文件.Background = new SolidColorBrush(color);
            if (parameter == 0)
            {
                模板一.Visibility = Visibility.Visible;

            }
            else if (parameter == 1)
            {
                模板二.Visibility = Visibility.Visible;
                if (Other_parameters == 0)
                {
                    提示.Text = "共扫描到了" + Other_parameters + "首歌曲，似乎神马也木有扫描到(＃°Д°)";
                }
                else
                {
                    提示.Text = "共扫描到了" + Other_parameters + "首歌曲";
                }
            }
            else if(parameter == 2)
            {

                TagLib.File Read_information = TagLib.File.Create(file);
                文件名称.Text = System.IO.Path.GetFileName(file);
                歌曲名称.Text = Read_information.Tag.Title;
                for (int i = 0; i < Read_information.Tag.Artists.Length; i++)
                {
                    if (i > 0) { 歌手信息.Text += ";" + Read_information.Tag.Artists[i]; }
                    else { 歌手信息.Text = Read_information.Tag.Artists[i]; }
                }
                专辑信息.Text = Read_information.Tag.Album;
                音频比特.Text = Read_information.Properties.AudioBitrate + " kbps";
                音频频道.Text = Convert.ToString(Read_information.Properties.AudioChannels) + " 声道";
                音频采样.Text = Convert.ToString(Read_information.Properties.AudioSampleRate) + " Hz";
                播放时长.Text = Convert.ToString(Read_information.Properties.Duration);
                文件描述.Text = Read_information.Properties.Description;
                文件路径.Text = file;
                if (Read_information.Tag.Lyrics != null) { 嵌入歌词.Text = "有内嵌歌词"; }
                else { 嵌入歌词.Text = "没有嵌入歌词"; }
                if (Read_information.Tag.Pictures != null && Read_information.Tag.Pictures.Length != 0) { 专辑图片.Text = "有专辑图片"; }
                else { 专辑图片.Text = "没有专辑图片"; }
                System.IO.FileInfo f = new FileInfo(file);
                文件大小.Text = GetFileSize(f.Length);
                文件类型.Text = System.IO.Path.GetExtension(file);
                创建日期.Text = f.CreationTimeUtc.ToString();
                修改日期.Text = f.LastWriteTimeUtc.ToString();
                模板三.Visibility = Visibility.Visible;
            }
            else if(parameter == 3)
            {
                PlaySound(@"C:\Windows\media\Windows Proximity Notification.wav", 0, 1);
                模板四.Visibility = Visibility.Visible;
                提示2.Text = file;
            }
            else if (parameter == 4)
            {
                PlaySound(@"C:\Windows\media\Windows Proximity Notification.wav", 0, 1);
                模板五.Visibility = Visibility.Visible;
                问题.Text  = file;
            }
            动画播放("打开");
        }

        //字节转单位
        public static string GetFileSize(long size)
        {
            var num = 1024.00; //byte
            if (size < num) { return size + "B"; }    
            if (size < Math.Pow(num, 2)) { return (size / num).ToString("f2") + "KB"; }
            if (size < Math.Pow(num, 3)) { return (size / Math.Pow(num, 2)).ToString("f2") + "MB"; }
            if (size < Math.Pow(num, 4)) { return (size / Math.Pow(num, 3)).ToString("f2") + "GB"; }
            return (size / Math.Pow(num, 4)).ToString("f2") + "TB"; //T
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void 单文件_Click(object sender, RoutedEventArgs e)
        {
            fcc1(1);
            exit();
        }

        private void 全文件_Click(object sender, RoutedEventArgs e)
        {
            fcc1(2);
            exit();
        }

        private void 取消_Click(object sender, RoutedEventArgs e)
        {
            fcc1(3);
            exit();
        }

        private void 确定_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void exit()
        {
            动画播放("关闭");
            System.Timers.Timer t1 = new System.Timers.Timer(200);//实例化Timer类
            t1.Elapsed += new System.Timers.ElapsedEventHandler(theout1);//到达时间的时候执行事件
            t1.AutoReset = false;//设置是执行一次（false）还是一直执行(true)
            t1.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }
        public void theout1(object source, System.Timers.ElapsedEventArgs e)
        {
            new Thread(() =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        Close();
                    }));
            }).Start();
        }
        //int a = 0;
        private void Window_Activated(object sender, EventArgs e)
        {
            //if (a == 0)
            //{
            //    动画播放("打开");
            //}
            //a = 1;
        }

        private void 关闭程序_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void 知道了_Click(object sender, RoutedEventArgs e)
        {
            exit();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(问题.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            exit();
        }
    }
}
