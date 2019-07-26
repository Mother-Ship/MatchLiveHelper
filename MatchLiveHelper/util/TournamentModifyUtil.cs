using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MatchLiveHelper.util
{
    public class TournamentModifyUtil
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); //窗体置顶
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2); //取消窗体置顶
        public const uint SWP_NOMOVE = 0x0002; //不调整窗体位置
        public const uint SWP_NOSIZE = 0x0001; //不调整窗体大小

        public static void CancelOsuTopMost()
        {
            for (int i = 0; i < 8; i++)
            {
                IntPtr littleOsuHandle = FindWindow("WindowsForms10.Window.2b.app.0.34f5582_r12_ad1", null);
                if (littleOsuHandle == null || littleOsuHandle == IntPtr.Zero)
                    continue;
                SetWindowPos(littleOsuHandle, HWND_NOTOPMOST, 1, 1, 1, 1, SWP_NOMOVE | SWP_NOSIZE);

                IntPtr OsuManagerHandle = FindWindow("WindowsForms10.Window.2b.app.0.bb8560_r12_ad1", null);
                SetParent(littleOsuHandle, OsuManagerHandle);
            }
        }
        
    }
}
