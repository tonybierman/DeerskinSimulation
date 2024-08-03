namespace DeerskinSimulation.Models
{
    public class TimelapseActivityMeta
    {
        // Constructor for easy initialization
        public TimelapseActivityMeta(string name = "", int duration = 0, int elapsed = 0, EventResultStatus status = EventResultStatus.None)
        {
            Name = name;
            Duration = duration;
            Elapsed = elapsed;
            Status = status;
        }

        // Properties are virtual to allow for mocking
        public virtual string Name { get; set; }
        public virtual int Duration { get; set; }
        public virtual int Elapsed { get; set; }
        public virtual EventResultStatus Status { get; set; }
    }
}
