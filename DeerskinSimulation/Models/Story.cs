namespace DeerskinSimulation.Models
{
    public class Story
    {
        private readonly EventRecord? _record;

        public Story(EventRecord? record) 
        {
            _record = record;
        }

        public EventRecord? Record => _record;
    }
}
