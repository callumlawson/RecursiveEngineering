using System;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    [Serializable]
    public class EngineState : IState
    {
        public float CurrentRpm;

        public EngineState()
        {
            CurrentRpm = 0;
        }

        public EngineState(float currentRpm)
        {
            CurrentRpm = currentRpm;
        }

        public override string ToString()
        {
            return string.Format("RPM: {0}", CurrentRpm);
        }
    }
}
