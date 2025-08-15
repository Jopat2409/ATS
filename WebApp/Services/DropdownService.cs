namespace WebApp.Services
{
    public class DropdownService(ILogger<DropdownService> logger)
    {

        private readonly ILogger<DropdownService> _logger = logger;

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
            _logger.LogInformation("Opening dropdown {id}{}", id, OpenId != null ? $" closing dropdown {OpenId}" : "");
            OpenId = (OpenId == id) ? null : id;
        }

        public bool IsOpen(string id)
        {
            return OpenId == id;
        }
    }
}
