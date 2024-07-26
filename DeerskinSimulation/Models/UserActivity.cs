namespace DeerskinSimulation.Models
{
    public class UserActivity
    {
        public TimedActivityMeta? Meta { get; set; }
        public Func<Task>? Start { get; set; }
        public Func<Task>? Finish { get; set; }
        public Func<Task>? InProcess { get; set; }
        public bool Completed { get; set; }
    }
}
