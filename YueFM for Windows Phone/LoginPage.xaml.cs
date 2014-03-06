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
using System.IO.IsolatedStorage;
using YueFM.Managers;
using YueFM.Utils;

namespace YueFM.Pages
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private APIManager apiManager = APIManager.GetInstance();
        
        public LoginPage()
        {
            InitializeComponent();

            AppUtils.FlurryLog("Login");
            
            apiManager.PostUsersHandler += PostUsersHandler;
            apiManager.PostSessionHandler += PostSessionHandler;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            apiManager.username = this.UsernameTextBox.Text;
            apiManager.password = this.PasswordTextBox.Password;

            if (this.checkBox.IsChecked.Value)
            {
                apiManager.PostUsers(apiManager.username, apiManager.password);
            }
            else
            {
                apiManager.PostSession();
            }

            this.buttonLogin.IsEnabled = false;
                
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void PostSessionHandler(Boolean success)
        {
            if (success)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    AppUtils.ToastPromptShow("阅FM", "登陆成功~");
                });
            }
            else
            {
                if (apiManager.errorMessage == "InfomationError")
                {
                    Dispatcher.BeginInvoke(() =>
                   {
                       AppUtils.ToastPromptShow("阅FM", "登录失败，用户名及密码可能不对吧？");
                   });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        AppUtils.ToastPromptShow("阅FM", "登录失败，是否网络状况不佳？");
                    });
                }

                Dispatcher.BeginInvoke(()=> this.buttonLogin.IsEnabled = true);
            }
        }

        private void PostUsersHandler(Boolean success)
        {
            if (success)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    AppUtils.ToastPromptShow("阅FM", "注册成功，您可以使用该账户推荐文章了。");
                });

                apiManager.PostSession();
            }
            else
            {
                if (apiManager.errorMessage == "InfomationError")
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        AppUtils.ToastPromptShow("阅FM", "注册失败，建议用户名和密码再复杂点？");
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        AppUtils.ToastPromptShow("阅FM", "注册出现错误，是否网络状况不佳？");
                    });
                }
            }

            Dispatcher.BeginInvoke(() => this.buttonLogin.IsEnabled = true);
        }
            
        

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.checkBox.IsChecked.Value)
                this.buttonLogin.Content = "注册并登陆";
            else
                this.buttonLogin.Content = "登陆";
        }
    }
}