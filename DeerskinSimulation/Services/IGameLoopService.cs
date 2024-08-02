using DeerskinSimulation.Models;

namespace DeerskinSimulation.Services
{
    public interface IGameLoopService
    {
        TimelapseActivityMeta? CurrentActivityMeta { get; set; }
        int DaysPassed { get; }
        double FPS { get; }

        event Action OnDayPassed;
        event Action OnGameTick;

        void Dispose();
        void StartActivity(TimelapseActivityMeta activityMeta);
    }
}