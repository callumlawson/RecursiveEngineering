using System;
using System.Text;
using Assets.Scrips.Entities;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class EntityTooltipRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject TooltipWindow;
        [UsedImplicitly] public Text TooltipTextField;

        private const float TooltipTime = 0.3f;
        private float hoverTime;
        private Entity lastEntitySelected;

        [UsedImplicitly]
        public void Start()
        {
            TooltipWindow.SetActive(false);
        }

        [UsedImplicitly]
        public void Update()
        {
            var currentlySelectedEntity = GameRunner.Instance.CurrentlySelectedEntity();

            if (currentlySelectedEntity != null && currentlySelectedEntity == lastEntitySelected)
            {
                hoverTime += Time.deltaTime;
            }
            else
            {
                hoverTime = 0;
            }
            lastEntitySelected = currentlySelectedEntity;

            if (hoverTime > TooltipTime)
            {
                TooltipTextField.text = TooltipMessage(currentlySelectedEntity);
                TooltipWindow.GetComponent<RectTransform>().transform.position = Input.mousePosition;
                TooltipWindow.SetActive(true);
            }
            else
            {
                TooltipWindow.SetActive(false);
            }
        }

        //TODO: Replace with automatic builder based on state metadata. Use Annotations.
        private static string TooltipMessage(Entity entity)
        {
            var message = new StringBuilder();
            message.Append(string.Format("Name: {0}", entity.GetState<NameState>().Name));
            if (entity.HasState<EngineState>())
            {
                message.Append(Environment.NewLine);
                message.Append(string.Format("RPM: {0}", entity.GetState<EngineState>().CurrentRpm));
            }
            return message.ToString();
        }
    }
}
