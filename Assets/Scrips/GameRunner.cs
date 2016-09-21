using Assets.Scrips.Components;
using Assets.Scrips.MonoBehaviours.Presentation;
using JetBrains.Annotations;
using UnityEngine;

//TODO: Define grid.
//TODO: Basic cell definitions.

namespace Assets.Scrips
{
    public class GameRunner : MonoBehaviour
    {
        [UsedImplicitly]
        public ComponentRenderer ComponentRenderer;

        private EngiComponent worldComponent;

        [UsedImplicitly]
        public void Start()
        {
            //Create the top level component
            worldComponent = new EngiComponent("GlobalComponent", 80, 70);
            ComponentRenderer.RenderComponent(worldComponent);
        }

        [UsedImplicitly]
        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                var selectedGrid = ComponentRenderer.CurrentlySelectedGrid();
                if (worldComponent.GridIsInComponent(selectedGrid) && worldComponent.GridIsEmpty(selectedGrid))
                {
                    var tank = new EngiComponent("Tank", 1, 1);
                    worldComponent.AddComponent(tank, ComponentRenderer.CurrentlySelectedGrid());
                    ComponentRenderer.RenderComponent(worldComponent);
                }
            }
        }
    }
}
