namespace SmartValley.Application.Email
{
    public class EmailUrls
    {
        private readonly string _root;

        public EmailUrls(string root)
        {
            _root = root;
        }

        public string GetConfirmEmailUrl(string token) =>
            $"{_root}/auth/confirm/{token}";

        public string GetAccountUrl()
            => $"{_root}/account";

        public string GetScoringsListUrl()
            => $"{_root}/scorings";

        public string GetRootUrl() => _root;
    }
}