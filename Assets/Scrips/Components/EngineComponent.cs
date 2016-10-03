using System;

namespace Assets.Scrips.Components
{
    [Serializable]
    class EngineComponent : IComponent
    {
        public int CurrentRpm;

        public EngineComponent(int currentRpm)
        {
            CurrentRpm = currentRpm;
        }
    }
}
