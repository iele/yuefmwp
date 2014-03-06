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
using Alexis.WindowsPhone.Social;

namespace YueFM.Utils
{
    public class SocialUtils
    {
        public static SocialType CurrentSocialType { get; set; }

        public static bool IsLoginGoBack { get; set; }

        public static string Statues { get; set; }

        public static ClientInfo GetClient(SocialType type)
        {
            ClientInfo client = new ClientInfo();
            switch (type)
            {
                case SocialType.Weibo:
                    client.ClientId = "3078098760";
                    client.ClientSecret = "c922e2b287b0119bb7efeb855f768caf";
                    client.RedirectUri = "http://yue.fm";
                    break;
                case SocialType.Tencent:
                    client.ClientId = "801238433";
                    client.ClientSecret = "e8106e640888edfe5c625c5604dd27ef";
                    break;
                case SocialType.Renren:
                    client.ClientId = "f48fa7cc377c4bafaf66f6d84afc48e8";
                    client.ClientSecret = "f1a2c691e0964a54965a1ec24795deb1";
                    break;
                case SocialType.Douban:
                    client.ClientId = "06adb4bf2a683e6a14091e3d7f38bb3e";
                    client.ClientSecret = "f4143a2feb70e01e";
                    client.RedirectUri = "http://yue.fm";
                    break;
                default:
                    break;
            }
            return client;
        }
    }
}
