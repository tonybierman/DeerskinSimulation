namespace DeerskinSimulation.Models
{
    public class UserInitiatedActivitySequence
    {
        // Constructor for easy initialization
        public UserInitiatedActivitySequence(
            TimelapseActivityMeta? meta = null,
            Func<Task>? start = null,
            Func<Task>? finish = null,
            Func<Task>? inProcess = null)
        {
            Meta = meta;
            Start = start;
            Finish = finish;
            InProcess = inProcess;
        }

        // Properties are virtual to allow for mocking
        public virtual TimelapseActivityMeta? Meta { get; set; }
        public virtual Func<Task>? Start { get; set; }
        public virtual Func<Task>? Finish { get; set; }
        public virtual Func<Task>? InProcess { get; set; }
    }
}
