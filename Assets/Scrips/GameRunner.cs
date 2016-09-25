using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.MonoBehaviours.Presentation;
using Assets.Scrips.Networks;
using Assets.Scrips.Networks.Substance;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    //THINGS I NOW UNDERSTAND!
    //THERE SHOULD ONLY BE ONE OF EACH NETWORK!.
    //TODO: Extract input handler from game runner.
    public class GameRunner : MonoBehaviour
    {
        [UsedImplicitly] public ComponentRenderer ComponentRenderer;
        [UsedImplicitly] public CameraController CameraController;

        private EngiComponent ActiveComponent { get; set; }
        private EngiComponent WorldComponent { get; set; }

        private const float DoubleClickTimeLimit = 0.20f;

        //Networks
        public SubstanceNetwork GlobalSubstanceNetwork;

        [UsedImplicitly]
        public void Start()
        {
            GlobalSubstanceNetwork = new SubstanceNetwork();
            WorldComponent = new EngiComponent("Sub Pen", null, 32, 18, false, GlobalSubstanceNetwork);
            SetActiveComponent(WorldComponent);

            StartCoroutine(InputListener());
            StartCoroutine(Ticker());
        }

        [UsedImplicitly]
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var possibleNode = GlobalSubstanceNetwork.GetNodeForComponent(CurrentlySelectedComponent());
                if (possibleNode != null)
                {
                    UnityEngine.Debug.Log("adding water!");
                    possibleNode.UpdateSubstance(SubstanceTypes.WATER, possibleNode.GetSubstance(SubstanceTypes.WATER) + 10);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                //TODO: Rotate selected component.     
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }

        public List<EngiComponent> CurrentHeirarchy()
        {
            var heirarchy = new List<EngiComponent>();
            var current = ActiveComponent;
            do
            {
                heirarchy.Add(current);
                current = current.ParentComponent;
            } while (current != null);
            return heirarchy;
        }

        public EngiComponent CurrentlySelectedComponent()
        {
            var selectedGrid = ComponentRenderer.CurrentlySelectedGrid();
            if (!ActiveComponent.GridIsEmpty(selectedGrid))
            {
                return ActiveComponent.GetComponent(selectedGrid);
            }
            return null;
        }

        private IEnumerator Ticker()
        {
            while (true)
            {
                GlobalSubstanceNetwork.Tick();
                yield return new WaitForSeconds(1.0f);
            }
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                //var componentToAdd = new EngiComponent("Box", ActiveComponent, 10, 10, false, globalSubstanceNetwork);
                var aDifferentComponentToAdd = new EngiComponent("Tank", ActiveComponent, 6, 7, true, GlobalSubstanceNetwork); 
                AddComponentToActiveComponent(aDifferentComponentToAdd, currentlySelectedGrid);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 1 && ActiveComponent.ParentComponent != null)
            {
                SetActiveComponent(ActiveComponent.ParentComponent);
            }

            if (CurrentlySelectedComponent() != null && button == 0)
            {
                SetActiveComponent(CurrentlySelectedComponent());
            }
        }

        private void AddComponentToActiveComponent(EngiComponent componentToAdd, GridCoordinate placeToAdd)
        {
            if (ActiveComponent.AddComponent(componentToAdd, placeToAdd))
            {
                ComponentRenderer.RenderComponent(ActiveComponent);
            }
        }

        private void SetActiveComponent(EngiComponent component)
        {
            //TODO: Encapsulate active component.
            ActiveComponent = component;
            ComponentRenderer.RenderComponent(component);
            CameraController.SetPosition(GridCoordinate.GridToPosition(ActiveComponent.GetCenterGrid()));
        }

        //TODO: Factor into input listener class.
        private IEnumerator InputListener()
        {
            while (enabled)
            { 
                if (Input.GetMouseButtonDown(0))
                    yield return ClickEvent(0, ComponentRenderer.CurrentlySelectedGrid());

                if (Input.GetMouseButtonDown(1))
                    yield return ClickEvent(1, ComponentRenderer.CurrentlySelectedGrid());

                yield return null;
            }
        }

        private IEnumerator ClickEvent(int button, GridCoordinate selectedGrid)
        {
            yield return new WaitForEndOfFrame();

            var count = 0f;
            while (count < DoubleClickTimeLimit)
            {
                if (Input.GetMouseButtonDown(button))
                {
                    DoubleClick(button, selectedGrid);
                    yield break;
                }
                count += Time.deltaTime;
                yield return null;
            }
            SingleClick(button, selectedGrid);
        }
    }
}
