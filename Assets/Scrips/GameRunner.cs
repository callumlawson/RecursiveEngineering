using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;
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
        [UsedImplicitly] public SubstanceRenderer SubstanceRenderer;
        [UsedImplicitly] public CameraController CameraController;

        public ModuleLibrary ModuleLibrary { get; private set; }
        private Module ActiveComponent { get; set; }
        private Module WorldComponent { get; set; }

        private const float DoubleClickTimeLimit = 0.20f;
        private const float SimulationTickPeriodInSeconds = 0.1f;

        //Networks
        public SubstanceNetwork GlobalSubstanceNetwork;

        [UsedImplicitly]
        public void Start()
        {
            GlobalSubstanceNetwork = new SubstanceNetwork();
            ModuleLibrary = new ModuleLibrary();
            WorldComponent = new Module( 
                null,
                new List<IComponent>{new CoreComponent("Sub Pen", 32 ,18 , ModuleType.Container)} 
            );

            SetActiveComponent(WorldComponent);
            StartCoroutine(InputListener());
            StartCoroutine(Ticker());
        }

        [UsedImplicitly]
        public void Update()
        {
            ComponentRenderer.Render(ActiveComponent);
            SubstanceRenderer.Render(ActiveComponent, GlobalSubstanceNetwork);

            if (Input.GetKeyDown(KeyCode.T))
            {
                var possibleNode = GlobalSubstanceNetwork.GetNodeForComponent(CurrentlySelectedComponent());
                if (possibleNode != null)
                {
                    possibleNode.UpdateSubstance(SubstanceTypes.WATER, possibleNode.GetSubstance(SubstanceTypes.WATER) + 10);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ModuleLibrary.DecrementSelectedComponent();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ModuleLibrary.IncrementSelectedComponent();
            }
        }

        public List<Module> CurrentHeirarchy()
        {
            var heirarchy = new List<Module>();
            var current = ActiveComponent;
            do
            {
                heirarchy.Add(current);
                current = current.ParentModule;
            } while (current != null);
            return heirarchy;
        }

        public Module CurrentlySelectedComponent()
        {
            var selectedGrid = ComponentRenderer.CurrentlySelectedGrid();
            if (!ActiveComponent.GridIsEmpty(selectedGrid))
            {
                return ActiveComponent.GetModule(selectedGrid);
            }
            return null;
        }

        private IEnumerator Ticker()
        {
            while (true)
            {
                GlobalSubstanceNetwork.Simulate();
                yield return new WaitForSeconds(SimulationTickPeriodInSeconds);
            }
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                var componentToAdd = new Module(ActiveComponent, ModuleLibrary.GetSelectedComponent());
                AddComponentToActiveComponent(componentToAdd, currentlySelectedGrid);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 1 && ActiveComponent.ParentModule != null)
            {
                SetActiveComponent(ActiveComponent.ParentModule);
            }

            if (CurrentlySelectedComponent() != null && button == 0 && !CurrentlySelectedComponent().IsTerminalModule)
            {
                SetActiveComponent(CurrentlySelectedComponent());
            }
        }

        private void AddComponentToActiveComponent(Module componentToAdd, GridCoordinate placeToAdd)
        {
            if (ActiveComponent.AddModule(componentToAdd, placeToAdd))
            {
                GlobalSubstanceNetwork.AddComponent(componentToAdd, placeToAdd);
                ComponentRenderer.Render(ActiveComponent);
            }
        }

        private void SetActiveComponent(Module component)
        {
            //TODO: Encapsulate active component.
            ActiveComponent = component;
            CameraController.SetPosition(GridCoordinate.GridToPosition(ActiveComponent.GetCenterGridHAXDONTUSE()));
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
