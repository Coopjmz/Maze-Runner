namespace MazeRunner.Core
{
    sealed class TickTimer(int ticks, bool start = true)
    {
        public int TicksLeft { get; private set; } = ticks;
        public bool IsRunning { get; private set; } = start;

        public bool HasFinished => TicksLeft <= 0;
        public bool IsNotFinished => !HasFinished;

        public void Start() => IsRunning = true;
        public void Stop() => IsRunning = false;

        public void Reset(int ticks) => TicksLeft = ticks;

        public void Update()
        {
            if (IsRunning && TicksLeft > 0)
                --TicksLeft;
        }
    }
}
