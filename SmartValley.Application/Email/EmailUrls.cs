namespace SmartValley.Application.Email
{
    public class EmailUrls
    {
        private readonly string _root;

        public EmailUrls(string root)
        {
            _root = root;
        }

        public string GetConfirmEmail(string address, string token) =>
            $"{_root}/auth/confirm/{address}/{token}";
    }
}