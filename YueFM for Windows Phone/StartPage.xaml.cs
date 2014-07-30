using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using YueFM.Managers;
using YueFM.Contents;
using Microsoft.Live;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YueFM.Pages
{
    public partial class StartPage : PhoneApplicationPage
    {
        public APIManager apiManager = APIManager.GetInstance();
        private SettingManager settingManager = SettingManager.GetInstance();

        public StartPage()
        {
            InitializeComponent();

            if (YueFM.Utils.ResolutionUtils.CurrentResolution == Utils.Resolutions.HD720p) 
            {
                image.Source = new BitmapImage(new Uri("/SplashScreenImage.Screen-720p.jpg", UriKind.Relative));
                image.Height = 853;
            }

            this.Loaded += StartPage_Loaded;
         
        }

        void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            APIManager.GetInstance().RestoreLoginState(new Action<Boolean>((b) => Dispatcher.BeginInvoke(() => { })));
            settingManager.RestoreSettings();

            this.content.RenderTransform = TranslateTransform;

            apiManager.RandomArticleHandler += apiManager_RandomArticleHandler;
            apiManager.GetRandomArticle();
        }

        void apiManager_RandomArticleHandler(ArticleContent ac)
        {
            Dispatcher.BeginInvoke(() =>
            {
                apiManager.RandomArticleHandler -= apiManager_RandomArticleHandler;

                ImageStoryboard.Begin();
            });
        }

        void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationService.RemoveBackEntry();
            NavigationService.Navigated -= NavigationService_Navigated;
        }

        private void ImageStoryboard_Completed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            NavigationService.Navigated += NavigationService_Navigated;
        }
    }
}