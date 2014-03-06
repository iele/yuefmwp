using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using System.Threading;
using YueFM.Managers;

namespace YueFM.Utils
{
    public static class AppUtils
    {
        public static string ConvertExtendedASCII(string HTML)
        {
            StringBuilder str = new StringBuilder();
            char c;
            for (int i = 0; i < HTML.Length; i++)
            {
                c = HTML[i];
                if (Convert.ToInt32(c) > 127)
                {
                    str.Append("&#" + Convert.ToInt32(c) + ";");
                }
                else
                {
                    str.Append(c);
                }
            }
            return str.ToString();
        }

        public static void ToastPromptShow(String title, String msg, int time = 1000)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Width = 480;
            toast.Title = title;
            toast.Message = msg;
            toast.MillisecondsUntilHidden = time;
            toast.Show();
        }

        public static void FlurryLog(String e)
        {
            if (SettingManager.GetInstance().crash_report)
            {
                FlurryWP8SDK.Api.LogEvent(e);
            }
        }

        public static void FlurryError(String e, Exception ex)
        {
            if (SettingManager.GetInstance().crash_report)
            {
                FlurryWP8SDK.Api.LogError(e, ex);
            }
        }
    }
}
