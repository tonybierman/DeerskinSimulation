namespace DeerskinSimulation.Models
{
    public class UserInitiatedActivitySequence
    {
        public TimelapseActivityMeta? Meta { get; set; }
        public Func<Task>? Start { get; set; }
        public Func<Task>? Finish { get; set; }
        public Func<Task>? InProcess { get; set; }
    }
}
