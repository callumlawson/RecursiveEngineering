using System.Collections;
using Assets.Scrips.Components;
using Assets.Scrips.MonoBehaviours.Presentation;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    public class GameRunner : MonoBehaviour
    {
        [UsedImplicitly] public ComponentRenderer ComponentRenderer;
        [UsedImplicitly] public CameraController CameraController;

        private EngiComponent activeComponent;
        private EngiComponent worldComponent;

        private const float DoubleClickTimeLimit = 0.20f;

        [UsedImplicitly]
        public void Start()
        {
            StartCoroutine(InputListener());

            worldComponent = new EngiComponent("GlobalComponent", null, 32, 18);

            SetActiveComponent(worldComponent);
        }

        [UsedImplicitly]
        public void Update()
        {
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                AddComponentToActiveComponent(new EngiComponent("Box", activeComponent, 10, 10), currentlySelectedGrid);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 1 && activeComponent.ParentComponent != null)
            {
                SetActiveComponent(activeComponent.ParentComponent);
            }

            if (CurrentlySelectedComponent() != null && button == 0)
            {
                SetActiveComponent(CurrentlySelectedComponent());
            }
        }

        private void AddComponentToActiveComponent(EngiComponent componentToAdd, GridCoordinate placeToAdd)
        {
            if (activeComponent.GridIsInComponent(placeToAdd) && activeComponent.GridIsEmpty(placeToAdd))
            {
                activeComponent.AddComponent(componentToAdd, placeToAdd);
                ComponentRenderer.RenderComponent(activeComponent);
            }
        }

        private void SetActiveComponent(EngiComponent component)
        {
            //TODO: Encapsulate active component.
            activeComponent = component;
            ComponentRenderer.RenderComponent(component);
            CameraController.SetPosition(GridCoordinate.GridToPosition(activeComponent.GetCenterGrid()));
        }

        private EngiComponent CurrentlySelectedComponent()
        {
            var selectedGrid = ComponentRenderer.CurrentlySelectedGrid();
            if (!activeComponent.GridIsEmpty(selectedGrid))
            {
                return activeComponent.GetComponent(selectedGrid);
            }
            return null;
        }

        //TODO: Factor into input listener class.
        // Update is called once per frame
        private IEnumerator InputListener()
        {
            while (enabled)
            { //Run as long as this is active

                if (Input.GetMouseButtonDown(0))
                    yield return ClickEvent(0, ComponentRenderer.CurrentlySelectedGrid());

                if (Input.GetMouseButtonDown(1))
                    yield return ClickEvent(1, ComponentRenderer.CurrentlySelectedGrid());

                yield return null;
            }
        }

        private IEnumerator ClickEvent(int button, GridCoordinate selectedGrid)
        {
            //pause a frame so you don't pick up the same mouse down event.
            yield return new WaitForEndOfFrame();

            float count = 0f;
            while (count < DoubleClickTimeLimit)
            {
                if (Input.GetMouseButtonDown(button))
                {
                    DoubleClick(button, selectedGrid);
                    yield break;
                }
                count += Time.deltaTime;// increment counter by change in time between frames
                yield return null; // wait for the next frame
            }
            SingleClick(button, selectedGrid);
        }

      
    }
}
