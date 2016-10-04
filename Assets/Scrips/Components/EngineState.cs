using System;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class EngineState : State
    {
        public int CurrentRpm;

        public EngineState(int currentRpm)
        {
            CurrentRpm = currentRpm;
        }
    }
}
