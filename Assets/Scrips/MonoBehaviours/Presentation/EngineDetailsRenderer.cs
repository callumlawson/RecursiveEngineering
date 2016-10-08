using Assets.Scrips.Entities;
using Assets.Scrips.States;
using Assets.Scrips.Systems;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class EngineDetailsRenderer : MonoBehaviour
    {
        [UsedImplicitly] public Text Rpm;

        [UsedImplicitly]
        public void Update()
        {
            EngineState engineState = null;
//            if (EngineSystem.Instance.EngineEntityId.HasValue)
//            {
//                engineState = Entity.GetState<EngineState>(EngineSystem.Instance.EngineEntityId.Value);
//            }

            Rpm.text = engineState != null ? engineState.CurrentRpm.ToString() : "0";
        }
    }
}