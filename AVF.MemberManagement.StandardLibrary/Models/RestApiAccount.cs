namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class RestApiAccount
    {
        private string _apiUrl;

        public string ApiUrl
        {
            get => _apiUrl;
            set => SetProperty(ref _apiUrl, value);
        }

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private void SetProperty(ref string reference, string value)
        {
            if (reference != value)
            {
                HasChanged = true;
            }

            reference = value;
        }

        public bool HasChanged;
    }
}
