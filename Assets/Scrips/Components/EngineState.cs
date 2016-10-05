using System;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class EngineState : IState
    {
        public int CurrentRpm;

        public EngineState(int currentRpm)
        {
            CurrentRpm = currentRpm;
        }
    }
}
