namespace DeerskinSimulation.Models
{
    public class StateContainer
    {
        private bool? m_debug;
        private string? m_avatarUrl;

        public bool? Debug
        {
            get => m_debug ?? false;
            set
            {
                if (m_debug != value)
                {
                    m_debug = value;
                    NotifyStateChanged();
                }
            }
        }

        public string? AvatarUrl
        {
            get => m_avatarUrl ?? "images/avatar_wm_256.jpg";
            set
            {
                if (m_avatarUrl != value)
                {
                    m_avatarUrl = value;
                    NotifyStateChanged();
                }
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
