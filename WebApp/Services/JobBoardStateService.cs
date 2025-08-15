using WebApp.Models;

namespace WebApp.Services
{
    public class JobBoardStateService
    {
        private Job? _CurrentJob = null;
        public Job? CurrentJob
        {
            get => _CurrentJob;
            set
            {
                _CurrentJob = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
