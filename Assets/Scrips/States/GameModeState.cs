using Assets.Framework.States;
using System;

namespace Assets.Scrips.States
{
    public enum GameMode
    {
        Design,
        Play
    }

    class GameModeState : IState
    {
        public Action GameModeChanged;
        public GameMode GameMode { get; private set; }

        public GameModeState(GameMode gameMode)
        {
            GameMode = gameMode;
        }

        public void SetGameMode(GameMode gameMode)
        {
            GameMode = gameMode;
            if (GameModeChanged != null)
            {
                GameModeChanged.Invoke();
            }
        }
    }
}
