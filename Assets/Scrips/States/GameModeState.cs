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
        public Action<GameMode> GameModeChanged;
        public GameMode GameMode { get; private set; }

        public GameModeState(GameMode gameMode)
        {
            SetGameMode(gameMode);
        }

        public void SetGameMode(GameMode gameMode)
        {
            if (gameMode == GameMode)
            {
                return;
            }
            GameMode = gameMode;
            if (GameModeChanged != null)
            {
                GameModeChanged.Invoke(GameMode);
            }
        }
    }
}
