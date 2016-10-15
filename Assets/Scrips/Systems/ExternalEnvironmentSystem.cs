using System;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    class ExternalEnvironmentSystem : ISystem
    {
        public ExternalEnvironmentSystem()
        {
            StaticStates.Get<GameModeState>().GameModeChanged += OnGameModeChanged;
        }

        private void OnGameModeChanged(GameMode mode)
        {
        }
    }
}
