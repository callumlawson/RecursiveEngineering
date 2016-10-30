using System;
using System.Text;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.MonoBehaviours.Presentation
{
    public class EntityTooltipRenderer : MonoBehaviour
    {
        [UsedImplicitly] public GameObject TooltipRoot;
        [UsedImplicitly] public Text TooltipTextField;
        [UsedImplicitly] public GameObject TooltipWindow;

        private const float TooltipTime = 0.3f;
        private float hoverTime;
        private GridCoordinate lastSelectedGrid;

        [UsedImplicitly]
        public void Start()
        {
            TooltipWindow.SetActive(false);
        }

        [UsedImplicitly]
        public void Update()
        {
            var activeEntity = StaticStates.Get<ActiveEntityState>().ActiveEntity;
            var selectedGrid = StaticStates.Get<SelectedState>().Grid;
            var hoveredEntities = activeEntity.GetState<PhysicalState>().GetEntitiesAtGrid(selectedGrid);

            UpdateHoverTime(selectedGrid);
            CleanPreviousTooltips();

            if (hoverTime > TooltipTime)
            {
                TooltipRoot.GetComponent<RectTransform>().transform.position = Input.mousePosition;

                foreach (var entity in hoveredEntities)
                {
                    var tooltip = Instantiate(TooltipWindow);
                    tooltip.GetComponent<RectTransform>().SetParent(TooltipRoot.transform);
                    var textComponent = tooltip.GetComponentInChildren<Text>();
                    textComponent.text = TooltipMessage(entity);
                }

                MatchWidths();

                foreach (Transform child in TooltipRoot.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        private static string TooltipMessage(Entity entity)
        {
            var message = new StringBuilder();
            message.Append(string.Format("> {0} ID: {1}", entity.GetState<EntityTypeState>().EntityType, entity.EntityId));
            foreach (var state in entity.DebugStates)
            {
                message.Append(Environment.NewLine);
                message.Append(state);
            }
            return message.ToString();
        }

        private void UpdateHoverTime(GridCoordinate grid)
        {
            if (grid == lastSelectedGrid)
            {
                hoverTime += Time.deltaTime;
            }
            else
            {
                hoverTime = 0;
            }
            lastSelectedGrid = grid;
        }

        private void CleanPreviousTooltips()
        {
            foreach (Transform child in TooltipRoot.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void MatchWidths()
        {
            var largestWidth = 0.0f;
            foreach (Transform child in TooltipRoot.transform)
            {
                var width = child.GetComponent<RectTransform>().rect.width;
                if (width > largestWidth)
                {
                    largestWidth = width;
                }
            }

            foreach (Transform child in TooltipRoot.transform)
            {
                child.GetComponent<LayoutElement>().minWidth = largestWidth;
            }
        }
    }
}