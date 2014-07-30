using System;
namespace Alexis.WindowsPhone.Social.Douban
{

    public class DoubanAccessToken
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string douban_user_id { get; set; }

        public DateTime ExpiresTime { get; set; }
    }
}
