using System;

namespace Assets.Scrips.States
{
    [Serializable]
    public class EngineState : IState
    {
        public float CurrentRpm;

        public EngineState(float currentRpm)
        {
            CurrentRpm = currentRpm;
        }
    }
}
