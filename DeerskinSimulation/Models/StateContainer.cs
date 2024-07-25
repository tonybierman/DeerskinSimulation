namespace DeerskinSimulation.Models
{
    public class StateContainer
    {
        private bool? m_debug;

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

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
