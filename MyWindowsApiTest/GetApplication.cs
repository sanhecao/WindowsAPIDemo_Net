using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWindowsApiTest
{
    public class GetApplication
    {
        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("user32.dll")]
        private extern static int GetWindowLongA(int hWnd, int Indx);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);
        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();

        //获取窗口类名 
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(int hWnd);

        //用来遍历所有窗口 
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll", EntryPoint = "WIndowFromPoint")]//指定坐标处窗体句柄

        private static extern int WindowFromPoint(int xPoint, int yPoint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        //获取窗口Text 
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);//得到窗口风格
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;
        private const int GW_CHILD= 0;

        public static List<string> GetRunApplication(Form form)
        {
            List<string> appString = new List<string>();

            try
            {
                  int handle = (int)form.Handle;
             //   int handle = (int)GetDesktopWindow();
                int hwCurr;
                IntPtr hwChild;
                 hwCurr = GetWindow(handle, GW_HWNDFIRST);
               // hwCurr = GetWindow(handle, 5);
                while (hwCurr > 0)
                {
                    //| WS_BORDER
                    int isTask = (WS_VISIBLE );
                    int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                    bool taskWindow = ((lngStyle & isTask) == isTask);
                    if (taskWindow)
                    {
                        int length = GetWindowTextLength(new IntPtr(hwCurr));
                        StringBuilder sb = new StringBuilder(2 * length + 1);
                        GetWindowText(hwCurr, sb, sb.Capacity);
                        string strTitle = sb.ToString();
                        if (!string.IsNullOrEmpty(strTitle))
                        {
                            appString.Add(strTitle);
                            /*  //子窗口
                                   hwChild =(IntPtr)  GetWindow(hwCurr, GW_CHILD);
                              GetClassNameW(hwChild, sb, sb.Capacity);
                              string strTitlechild = sb.ToString();
                              if (!string.IsNullOrEmpty(strTitlechild))
                              {
                                  appString.Add(strTitle + "==" + strTitlechild);

                              }*/

                        }
                    }
                    hwCurr = GetWindow(hwCurr, GW_HWNDNEXT);
                }
                return appString;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

     
       
        //自定义一个类，用来保存句柄信息，在遍历的时候，随便也用空上句柄来获取些信息，呵呵 
        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }
        //用来保存窗口对象 列表
        static List<WindowInfo> wndList = new List<WindowInfo>();
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;                             //最左坐标
            public int Top;                             //最上坐标
            public int Right;                           //最右坐标
            public int Bottom;                        //最下坐标
        }
        public static List<WindowInfo> GetAllDesktopWindows()
        {
            wndList.Clear();
            WNDENUMPROC ewp = new WNDENUMPROC(MyENUMPROC);
            //enum all desktop windows 
            EnumWindows(ewp, 0);

            return wndList;
        }
        static bool MyENUMPROC(IntPtr hWnd, int lParam)
        {
            WindowInfo wnd = new WindowInfo();
            StringBuilder sb = new StringBuilder(256);
           
            //get hwnd 
            wnd.hWnd = hWnd;
            //if (IsWindowVisible((int)hWnd))
            {
                //get window name  
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();

                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);              
                


                RECT rc = new RECT();
                GetWindowRect(hWnd, ref rc);
                int width = rc.Right - rc.Left;                        //窗口的宽度
                int height = rc.Bottom - rc.Top;                   //窗口的高度
                int x = rc.Left;
                int y = rc.Top;

                wnd.szClassName = sb.ToString() + " 位置:" + width.ToString() + " " + height.ToString()
                    + " " + x.ToString() + " " + y.ToString();
                //add it into list 
                wndList.Add(wnd);
            }
            return true;
        }
}
}

