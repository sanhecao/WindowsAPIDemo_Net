﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyWindowsApiTest.GetApplication;

namespace MyWindowsApiTest
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();
        /// <summary>
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；
        /// </summary>
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        /// <summary>
        /// 窗口与要获得句柄的窗口之间的关系。
        /// </summary>
        enum GetWindowCmd : uint
        {
            /// <summary>
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /*GetWindowCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上：
              GW_CHILD
              寻找源窗口的第一个子窗口
              GW_HWNDFIRST
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口
              GW_HWNDLAST
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口
              GW_HWNDNEXT
              为源窗口寻找下一个兄弟窗口
              GW_HWNDPREV
              为源窗口寻找前一个兄弟窗口
              GW_OWNER
              寻找窗口的所有者
         */

        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        //================================================================
        //定义一个符合WINAPI返回值和参数的委托
        public delegate bool CallBack(IntPtr hwnd, int lParam);
        //声明符合上述委托的函数（定义一个函数指针）
        private static CallBack myCallBack;
        //===========================================================

        public Form1()
        {
         
            myCallBack = new CallBack(Report);
            InitializeComponent();
            GetHandle("C#");
            this.textBox1.Multiline = true;
            this.textBox1.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1、获取桌面窗口的句柄
            IntPtr desktopPtr = GetDesktopWindow();
            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）
            IntPtr winPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);

            //3、循环取得桌面下的所有子窗口
            while (winPtr != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(256);
                GetWindowText(winPtr, sb, sb.Capacity);
                string strTitle = sb.ToString();
                if (!string.IsNullOrEmpty(strTitle))
                    listBox1.Items.Add(strTitle);
                //4、继续获取下一个子窗口
                winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> list = GetApplication.GetRunApplication(this);
            list.ForEach(process =>
            {
                listBox1.Items.Add(process);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
            foreach (Process p in Process.GetProcesses(System.Environment.MachineName))
            {
                if (p.MainWindowHandle != IntPtr.Zero)
                {
                    // Display the user name of the program
                    
                    listBox1.Items.Add(p.ProcessName + "--" + p.MainWindowTitle);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<WindowInfo> list = GetApplication.GetAllDesktopWindows();
            list.ForEach(process =>
            {
              //  if (process.szWindowName == "微信")
                {
                    listBox1.Items.Add(process.hWnd + "=" + process.cls + "=" + process.szWindowName);
                }
            });
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            List<WindowInfo> list = GetApplication.GetRunApplicationInfo(this);
            list.ForEach(process =>
            {
                //  if (process.szWindowName == "微信")
                {
                    listBox1.Items.Add(process.hWnd + "=====" + process.cls + "=====" + process.szWindowName + "=====" + process.app);
                }
            });
        }
        //=====================================================================
        private void GetHandle(string windcaption)
        {
            IntPtr mainHandle = FindWindow(null, windcaption);
            if (IntPtr.Zero != mainHandle)
            {
                AppendText(string.Format("{0}句柄：{1}", windcaption, Convert.ToString((int)mainHandle, 10)));

                //EnumChildWindows((int)mainHandle, myCallBack, 0);
                //修改窗口标题
               // SetWindowText((int)mainHandle, "C#");
                StringBuilder s = new StringBuilder(512);
                //获取控件标题
                int i = GetWindowText(mainHandle, s, s.Capacity);
                AppendText(string.Format("..句柄{0}的caption：{1}", Convert.ToString((int)mainHandle, 10), s.ToString()));
                //枚举所有子窗体，并将子窗体句柄传给myCallBack
                EnumChildWindows((int)mainHandle, myCallBack, 0);
            }
        }

        private void AppendText(string msg)
        {
            this.textBox1.AppendText(msg);
            this.textBox1.AppendText("\r\n");
        }
        //根据窗体句柄，输出窗体caption
        public bool Report(IntPtr hWnd, int lParam)
        {
            StringBuilder s = new StringBuilder(512);
            int i = GetWindowText((IntPtr)hWnd, s, s.Capacity);
            AppendText(string.Format("CallBack句柄{0}的caption：{1}", Convert.ToString((int)hWnd, 16), s.ToString()));
            return true;
        }

        /// <summary>
        /// 获取窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns>返回句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern int EnumWindows(CallBack x, int y);

        [DllImport("user32.dll")]
        private static extern IntPtr EnumChildWindows(int hWndParent, CallBack lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(int handle, string title);

       // [DllImport("user32.dll", EntryPoint = "GetWindowText")]
       // public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);



        private void button6_Click(object sender, EventArgs e)
        {
            StringBuilder s = new StringBuilder(512);
            //获取控件标题,对TRichEdit无效
            //int i = GetWindowText((IntPtr)int.Parse(txtHandle.Text), s, s.Capacity);
            //获取控件标题,对TRichEdit有效
            int i = SendMessage((IntPtr)int.Parse(txtHandle.Text), 0x000D, 1000, s);
            AppendText(string.Format("句柄{0}的caption：{1}", txtHandle.Text, s.ToString()));

        }
    }
}
