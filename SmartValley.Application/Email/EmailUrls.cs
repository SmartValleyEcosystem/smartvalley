namespace SmartValley.Application.Email
{
    public class EmailUrls
    {
        private readonly string _root;

        public EmailUrls(string root)
        {
            _root = root;
        }

        public string GetConfirmEmail(string token) =>
            $"{_root}/auth/confirm/{token}";
    }
}