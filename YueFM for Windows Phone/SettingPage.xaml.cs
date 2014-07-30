using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using YueFM.Managers;
using YueFM.Utils;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using Microsoft.Live;
using Microsoft.Live.Controls;

namespace YueFM.Pages
{
    public partial class SettingPage : PhoneApplicationPage
    {
        private static readonly String[] fonts = { "Microsoft YaHei",
                                                   "Microsoft MHei",
                                                   "DengXian" };

        private SettingManager settingManager = SettingManager.GetInstance();
        private APIManager apiManager = APIManager.GetInstance();

        private LiveConnectClient client;

        public SettingPage()
        {
            InitializeComponent();
      
            this.accountControl.BindAction = ((p) =>
            {
                SocialUtils.IsLoginGoBack = true;
                SocialUtils.CurrentSocialType = p;
                NavigationService.Navigate(new Uri("/SocialLoginPage.xaml", UriKind.Relative));
            });

            apiManager.PostSessionHandler += new APIManager.PostSessionEvent(apiManager_PostSessionHandler);

            this.FontListPicker.DataContext = fonts;
            this.FontListPicker.SelectedIndex = Array.IndexOf(fonts, settingManager.article_font);

            this.SizeTextBlock.Text = settingManager.article_size;
            this.SliderFontSize.Value = (Double.Parse(settingManager.article_size));

            Color f = Color.FromArgb(0xff,
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11));
            this.NightTextBlock.Text = settingManager.night_light;
            this.NightTextBlock.Foreground = new SolidColorBrush(f);
            this.SliderNight.Value = (Double.Parse(settingManager.night_light));

            this.NightStackPanel.Visibility = settingManager.night_mode ? Visibility.Visible : Visibility.Collapsed;
            this.NightStackPanel.RenderTransform = this.NightStackPanelTransformGroup;


            this.ImageToggleSwitch.IsChecked = settingManager.article_is_image;
            this.NightToggleSwitch.IsChecked = settingManager.night_mode;
            this.crashToggleSwitch.IsChecked = settingManager.crash_report;
            this.QuitToggleSwitch.IsChecked = settingManager.quit_confirm;

            this.crashToggleSwitch.Checked += this.crashToggleSwitch_Checked;
            this.crashToggleSwitch.Unchecked += this.crashToggleSwitch_Unchecked;

            this.FontListPicker.SelectionChanged += FontListPicker_SelectionChanged;
            this.SliderFontSize.ValueChanged += SliderFontSize_ValueChanged;
            this.SliderNight.ValueChanged += SliderNight_ValueChanged;

            this.LogoutButton.Click += LogoutButton_Click;

            this.ButtonSignin.SignInText = "登录";
            this.ButtonSignin.SignOutText = "注销";

            if (apiManager.isLogin)
            {
                this.AccountTextBlock.Text = apiManager.username;
                this.LogoutButton.IsEnabled = true;
                this.LogoutButton.Content = "注销";
            }
            else
            {
                this.AccountTextBlock.Text = "尚未登录";
                this.LogoutButton.Content = "登录";
            }

           AppUtils.FlurryLog("Setting");
        }

        void apiManager_PostSessionHandler(bool success)
        {
            if (success)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    this.AccountTextBlock.Text = apiManager.username;
                    this.LogoutButton.IsEnabled = true;
                    this.LogoutButton.Content = "注销";
                });
            }
        }

        private void FontListPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settingManager.article_font = (sender as ListPicker).SelectedItem as String;
        }

        private void ImageToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            this.ImageToggleSwitch.Content = "是";
            settingManager.article_is_image = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void NightToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            this.NightToggleSwitch.Content = "是";
            this.NightStackPanel.Visibility = Visibility.Visible;
            this.NightStackPanelFadeInStoryboard.Begin();
            settingManager.night_mode = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void NightToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            this.NightToggleSwitch.Content = "否";
            this.NightStackPanelFadeOutStoryboard.Begin();
            settingManager.night_mode = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void NightStackPanelFadeInStoryboard_Completed(object sender, EventArgs e)
        {
            ThemeManager.ToDarkTheme();
        }

        private void NightStackPanelFadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            this.NightStackPanel.Visibility = Visibility.Collapsed;
            ThemeManager.ToLightTheme();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (apiManager.isLogin == true)
            {
                apiManager.isLogin = false;

                apiManager.RemoveLoginState();
                apiManager.likesArticle = null;

                this.AccountTextBlock.Text = "尚未登录";
                this.LogoutButton.Content = "登录";
                return;
            }
            else
            {
                Dispatcher.BeginInvoke(() =>
                {
                    NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                });
            }
        }

        private void marketButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        private void mailButton_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "关于阅FM for Windows Phone的意见";
            emailComposeTask.To = "melephas@gmail.com";
            emailComposeTask.Show();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.accountControl.SetContent();

            base.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            settingManager.SaveSettings();

            base.OnNavigatedFrom(e);
        }

        private void ImageToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            this.ImageToggleSwitch.Content = "否";
            settingManager.article_is_image = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void crashToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            this.crashToggleSwitch.Content = "是";
        }

        private void crashToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            this.crashToggleSwitch.Content = "否";
            settingManager.crash_report = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void crashToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (this.crashToggleSwitch.IsChecked == true)
            {
                MessageBoxResult mbr = MessageBox.Show(
    @"感谢您将使用情况发送给作者，统计的内容将仅包含您使用行为的日志，如加载文章，推荐文章等活动。作者不会也无法收集您的隐私信息，包括其行为的具体内容和个人信息，如有疑问可以通过邮件详细咨询作者，也可以选择关闭。",
              "说明", MessageBoxButton.OK);

                this.crashToggleSwitch.Content = "是";
                settingManager.crash_report = (sender as ToggleSwitch).IsChecked.Value;
            }
            else {
                FlurryWP8SDK.Api.LogEvent("StopTracking");
            }
        }

        private void QuitToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            this.QuitToggleSwitch.Content = "否";
            settingManager.quit_confirm = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void QuitToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            this.QuitToggleSwitch.Content = "是";
            settingManager.quit_confirm = (sender as ToggleSwitch).IsChecked.Value;
        }

        private void SliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SizeTextBlock.Text = ((int)(sender as Slider).Value).ToString();
            settingManager.article_size = ((int)(sender as Slider).Value).ToString();
        }

        private void SliderNight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.NightTextBlock.Text = ((int)(sender as Slider).Value).ToString();
            settingManager.night_light = ((int)(sender as Slider).Value).ToString();
            Color f = Color.FromArgb(0xff,
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                           (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11));
            this.NightTextBlock.Foreground = new SolidColorBrush(f);
        }

        private void SliderFontSize_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            this.Pivot.IsHitTestVisible = true;
        }

        private void SliderFontSize_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            this.Pivot.IsHitTestVisible = false;
        }

        private void SliderNight_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            this.Pivot.IsHitTestVisible = true;
        }

        private void SliderNight_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            this.Pivot.IsHitTestVisible = false;
        }

        private async void ButtonSignin_SessionChanged(object sender, LiveConnectSessionChangedEventArgs ent)
        {
            if (ent.Status == LiveConnectSessionStatus.Connected)
            {
                client = new LiveConnectClient(ent.Session);
                this.SkyDriveTextBlock.Text = "已经登录";
                var e = await client.GetAsync("me");
                 if (e.Result.ContainsKey("first_name") &&
                    e.Result.ContainsKey("last_name"))
                {
                    if (e.Result["first_name"] != null &&
                        e.Result["last_name"] != null)
                    {
                        string name = e.Result["first_name"] + " " + e.Result["last_name"];
                        if (name.Length >= 14)
                        {
                            name = name.Substring(0, 12) + "...";
                        }
                        this.SkyDriveTextBlock.Text = name;
                    }
                }
                else
                {
                    this.SkyDriveTextBlock.Text = "已经登录";
                }

                if (settingManager.skydrive_login == false)
                    Dispatcher.BeginInvoke(() => AppUtils.ToastPromptShow("阅FM", "绑定成功,若此次不能使用上传请重启本应用"));
                settingManager.skydrive_login = true;
            }
            else
            {
                this.SkyDriveTextBlock.Text = "尚未登录";

                settingManager.skydrive_login = false;
            }
        }
    }
}