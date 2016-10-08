using System;

namespace Assets.Scrips.States
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
