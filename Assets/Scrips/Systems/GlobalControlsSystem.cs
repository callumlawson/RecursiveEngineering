using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.States;
using UnityEngine;

namespace Assets.Scrips.Systems
{
    public class GlobalControlsSystem : IUpdateSystem
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                StaticStates.Get<GameModeState>().SetGameMode(GameMode.Play);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                StaticStates.Get<GameModeState>().SetGameMode(GameMode.Design);
            }
        }
    }
}
