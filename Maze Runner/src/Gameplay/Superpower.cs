using System.Diagnostics;

using MazeRunner.Core;

namespace MazeRunner.Gameplay
{
    sealed class Superpower
    {
        public int Charges { get; private set; }
        public bool IsActive { get; private set; }

        private readonly TickTimer _chargeTimer = new(Settings.TURNS_NEEDED_FOR_SUPERPOWER_CHARGE);

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
        public void Toggle() => IsActive = !IsActive;

        public void Update()
        {
            if (IsActive)
            {
                Debug.Assert(Charges > 0);

                --Charges;
                if (Charges <= 0)
                    Deactivate();
            }
            else if (Charges < Settings.MAX_SUPERPOWER_CHARGES)
            {
                if (_chargeTimer.HasFinished)
                {
                    _chargeTimer.Reset(Settings.TURNS_NEEDED_FOR_SUPERPOWER_CHARGE);
                    ++Charges;
                }
                else _chargeTimer.Update();
            }
        }
    }
}
