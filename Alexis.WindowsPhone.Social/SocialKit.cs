// ===============================================================================
// SocialKit.cs
// Description:Socail Kit for Getting Url
// Author:  Alexis
// Date:    2012.05.27
// Blog: http://alexis.cnblogs.com
// ===============================================================================
// Copyright (c) Alexis. 
// All rights reserved.
// ===============================================================================

using System;
using System.Net;
using System.Windows;
using System.Text;
using System.Runtime.Serialization.Json;
using Alexis.WindowsPhone.Social.Helper;
using System.Threading;
using System.IO;

namespace Alexis.WindowsPhone.Social
{
    public class ClientInfo
    {
        /// <summary>
        /// client id or key id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// client secret or secret key
        /// </summary>
        public string ClientSecret { get; set; }

        public string Tag { get; set; }

        public string RedirectUri { get; set; }

    }

    public class SocialKit
    {
        /// <summary>
        /// get social authorize url
        /// </summary>
        /// <param name="type">social type</param>
        /// <param name="client">client info</param>
        /// <returns></returns>
        internal static string GetAuthorizeUrl(SocialType type, ClientInfo client)
        {
            string url = "";
            switch (type)
            {
                case SocialType.Weibo:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "https://api.weibo.com/oauth2/default.html";
                    }
                    url = "https://api.weibo.com/oauth2/authorize?client_id=" + client.ClientId + "&response_type=code&redirect_uri=" + client.RedirectUri + "&display=mobile";
                    break;
                case SocialType.Tencent:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://t.qq.com";
                    }
                    url = "https://open.t.qq.com/cgi-bin/oauth2/authorize?client_id=" + client.ClientId + "&response_type=code&redirect_uri=" + client.RedirectUri + "&wap=false";
                    break;
                case SocialType.Renren:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://graph.renren.com/oauth/login_success.html";
                    }
                    url = "https://graph.renren.com/oauth/authorize?response_type=code&client_id=" + client.ClientId + "&redirect_uri=" + client.RedirectUri + "&display=mobile&scope=photo_upload+publish_feed";
                    break;
                case SocialType.Douban:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "https://douban.com";
                    }
                    url = "https://www.douban.com/service/auth2/auth?client_id=" + client.ClientId + "&response_type=code&redirect_uri=" + client.RedirectUri + "&response_type=code" + "&display=mobile&scope=shuo_basic_r,shuo_basic_w";
                    break;
                case SocialType.Fanfou:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://fanfou.com";
                    }
                    url = " http://fanfou.com/oauth/request_token?oauth_consumer_key=" + client.ClientId + "&oauth_signature_method=code&redirect_uri=" + client.RedirectUri + "&response_type=code";
                    break;
                default:
                    break;
            }
            return url;
        }

        /// <summary>
        /// get token url
        /// </summary>
        /// <param name="type">social type</param>
        /// <param name="client">client info</param>
        /// <param name="code">code get from authorized url</param>
        /// <returns></returns>
        internal static string GetTokenUrl(SocialType type, ClientInfo client, string code)
        {
            string url = "";
            switch (type)
            {
                case SocialType.Weibo:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "https://api.weibo.com/oauth2/default.html";
                    }
                    url = "https://api.weibo.com/oauth2/access_token?client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&grant_type=authorization_code&redirect_uri=" + client.RedirectUri + "&" + code;
                    break;
                case SocialType.Tencent:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://t.qq.com";
                    }
                    url = "https://open.t.qq.com/cgi-bin/oauth2/access_token?client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&redirect_uri=" + client.RedirectUri + "&grant_type=authorization_code&" + code;
                    break;
                case SocialType.Renren:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://graph.renren.com/oauth/login_success.html";
                    }
                    url = "https://graph.renren.com/oauth/token?grant_type=authorization_code&client_id=" + client.ClientId + "&redirect_uri=" + client.RedirectUri + "&client_secret=" + client.ClientSecret + "&" + code;
                    break;
                case SocialType.Douban:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://douban.com";
                    }
                    url = "https://www.douban.com/service/auth2/token";
                    break;
                case SocialType.Fanfou:
                    if (string.IsNullOrEmpty(client.RedirectUri))
                    {
                        client.RedirectUri = "http://fanfou.com";
                    }
                    url = "http://fanfou.com/oauth/access_token?access_token?client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&redirect_uri=" + client.RedirectUri + "&grant_type=authorization_code&" + code;
                    break;
                default:
                    break;
            }
            return url;
        }

        /// <summary>
        /// get access token
        /// </summary>
        /// <param name="type">social type</param>
        /// <param name="client">client info</param>
        /// <param name="code"></param>
        /// <param name="action"></param>

        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private static String post_param;
        private static void ReadCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream postStream = request.EndGetRequestStream(asynchronousResult);
            string postData = post_param;

            byte[] byteArray = (new AsciiEncoding()).GetBytes(postData);
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();
            allDone.Set();
        }

        internal static void GetToken(SocialType type, ClientInfo client, string code, Action<AccessToken> action)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(GetTokenUrl(type, client, code));
            httpWebRequest.Method = "POST";

            if (type == SocialType.Douban)
            {
                post_param = "client_id=" + client.ClientId + "&client_secret=" + client.ClientSecret + "&redirect_uri=" + HttpUtility.UrlEncode(client.RedirectUri) + "&grant_type=authorization_code&" + code;

                httpWebRequest.BeginGetRequestStream(new AsyncCallback(ReadCallback), httpWebRequest);
                allDone.WaitOne();
            }

            httpWebRequest.BeginGetResponse((p) =>
            {
                HttpWebRequest request = (HttpWebRequest)p.AsyncState;
                HttpWebResponse httpWebResponse;
                try
                {
                    httpWebResponse = (HttpWebResponse)request.EndGetResponse(p);
                }
                catch (WebException ex)
                {
                    return;
                }
                if (httpWebResponse != null)
                {
                    using (var stream = httpWebResponse.GetResponseStream())
                    {
                        AccessToken token = new AccessToken();
                        if (type == SocialType.Tencent)
                        {
                            using (var reader = new System.IO.StreamReader(stream))
                            {
                                string text = reader.ReadToEnd();
                                if (!string.IsNullOrEmpty(text))
                                {
                                    //access_token=ec70e646177f025591e4282946c19b67&expires_in=604800&name=xshf12345
                                    var acc = text.Split('&');
                                    foreach (var item in acc)
                                    {
                                        var single = item.Split('=');
                                        if (single[0] == "access_token")
                                        {
                                            token.Token = single[1];
                                        }
                                        else if (single[0] == "expires_in")
                                        {
                                            token.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToInt32(single[1]));
                                        }
                                        else if (single[0] == "name")
                                        {
                                            token.UserInfo = single[1];
                                        }
                                    }
                                    token.OpenId = client.Tag;
                                }
                            }
                        }
                        else if (type == SocialType.Weibo)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Weibo.WeiboAccessToken));
                            var item = ser.ReadObject(stream) as Weibo.WeiboAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.uid;
                        }
                        else if (type == SocialType.Renren)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Renren.RenrenAccessToken));
                            var item = ser.ReadObject(stream) as Renren.RenrenAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.user.name;
                        }
                        else if (type == SocialType.Douban)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Douban.DoubanAccessToken));
                            var item = ser.ReadObject(stream) as Douban.DoubanAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.douban_user_id;
                        }
                        else if (type == SocialType.Fanfou)
                        {
                            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Fanfou.FanfouAccessToken));
                            var item = ser.ReadObject(stream) as Fanfou.FanfouAccessToken;
                            item.ExpiresTime = DateTime.Now.AddSeconds(Convert.ToDouble(item.expires_in));
                            token.Token = item.access_token;
                            token.ExpiresTime = item.ExpiresTime;
                            token.UserInfo = item.user_id;
                        }
                        action(token);
                        string filePath = string.Format(SocialAPI.ACCESS_TOKEN_PREFIX, type.ToString());
                        JsonHelper.SerializeData<AccessToken>(filePath, token);
                    }
                }
            }, httpWebRequest);
        }

    }
}
