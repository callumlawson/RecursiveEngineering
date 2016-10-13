using Assets.Scrips.States;
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
//            if (EngineSystem.Instance.EngineEntity.HasValue)
//            {
//                engineState = Entity.Get<EngineState>(EngineSystem.Instance.EngineEntity.Value);
//            }

            Rpm.text = engineState != null ? engineState.CurrentRpm.ToString() : "0";
        }
    }
}