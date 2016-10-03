using Assets.Scrips.Components;
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
            var engineModule = EngineSystem.Instance.EngineModule;
            Rpm.text = engineModule != null ? engineModule.GetState<EngineComponent>().CurrentRpm.ToString() : "0";
        }
    }
}