using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MatchLiveHelper.util
{
    class IniUtil
    {

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);//与ini交互必须统一编码格式

        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);

        }

        public static string ReadIniData(string section, string key)
        {
            string encodingName = "utf-8";
            int size = 1024;
            byte[] buffer = new byte[size];
            string fileName = Environment.CurrentDirectory + "\\data\\config.ini";
            int count = GetPrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), null, buffer, size, fileName);
            return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();
        }

    }
}
