namespace WebApp.Services
{
    public class DropdownService
    {
        public event Action StateChanged = delegate { };

        private string? _openId = null;
        public string? OpenId
        {
            get => _openId;
            set
            {
                if (_openId != value)
                {
                    _openId = value;
                    StateChanged.Invoke();
                }
            }
        }

        public void Toggle(string id)
        {
            OpenId = (OpenId == id) ? null : id;
        }

        public bool IsOpen(string id)
        {
            return OpenId == id;
        }
    }
}
