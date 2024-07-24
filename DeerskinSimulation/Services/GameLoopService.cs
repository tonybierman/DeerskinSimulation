namespace DeerskinSimulation.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;

    public class GameLoopService : IDisposable
    {
        private Timer timer;
        private DateTime lastUpdateTime;
        private int frameCount;
        private DateTime startTime;
        private double fps;
        private int tickCount;
        private int dayCount;
        private bool isActivityRunning;
        private CancellationTokenSource cancellationTokenSource;
        public event Action OnGameTick;
        public event Action OnDayPassed;

        public double FPS => fps;

        public GameLoopService()
        {
            cancellationTokenSource = new CancellationTokenSource();
            lastUpdateTime = DateTime.Now;
            startTime = DateTime.Now;
            frameCount = 0;
            tickCount = 0;
            dayCount = 0;
            isActivityRunning = false;
            timer = new Timer(GameLoop, null, 0, 16); // Roughly 60 FPS
        }

        public void StartActivity()
        {
            isActivityRunning = true;
            tickCount = 0;
            dayCount = 0;
        }

        private void GameLoop(object state)
        {
            var currentTime = DateTime.Now;
            var deltaTime = (currentTime - lastUpdateTime).TotalMilliseconds;
            lastUpdateTime = currentTime;

            frameCount++;
            var elapsedTime = (currentTime - startTime).TotalSeconds;
            if (elapsedTime >= 1.0)
            {
                fps = frameCount / elapsedTime;
                frameCount = 0;
                startTime = currentTime;
            }

            if (isActivityRunning)
            {
                tickCount++;
                if (tickCount >= 10) // One day has passed
                {
                    tickCount = 0;
                    dayCount++;
                    OnDayPassed?.Invoke();

                    if (dayCount >= 10) // Activity finishes after 10 days
                    {
                        isActivityRunning = false;
                    }
                }
            }

            // Invoke the game tick event
            OnGameTick?.Invoke();
        }

        public void Dispose()
        {
            cancellationTokenSource.Cancel();
            timer?.Dispose();
        }
    }


}
