using System;
namespace Alexis.WindowsPhone.Social.Fanfou{

    public class FanfouAccessToken
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string user_id { get; set; }

        public DateTime ExpiresTime { get; set; }
    }
}
