using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Alexis.WindowsPhone.Social;
using YueFM.Utils;

namespace YueFM.Pages
{
    public partial class SocialLoginPage : PhoneApplicationPage
    {
        public SocialLoginPage()
        {
            InitializeComponent();

            AppUtils.FlurryLog("Social");
            
            LoadLoginControl();
        }

        private void LoadLoginControl()
        {
            AuthControl control = new AuthControl();
            var type = SocialUtils.CurrentSocialType;
            control.SetData(type, SocialUtils.GetClient(type));
            control.action += (p) =>
            {
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    if (NavigationService.CanGoBack)
                    {
                        NavigationService.GoBack();
                    }
                });

            };
            this.LayoutRoot.Children.Add(control);
        }
    }
} 