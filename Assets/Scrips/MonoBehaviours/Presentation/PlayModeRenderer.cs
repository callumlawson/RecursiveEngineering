using Assets.Framework.States;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class PlayModeRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject PlayModeSymbol;

        [UsedImplicitly]
        public void OnEnable()
        {
            StaticStates.Get<GameModeState>().GameModeChanged += OnGameModeChanged;
            OnGameModeChanged(StaticStates.Get<GameModeState>().GameMode);
        }

        private void OnGameModeChanged(GameMode mode)
        {
            PlayModeSymbol.SetActive(mode == GameMode.Play);
        }
    }
}
