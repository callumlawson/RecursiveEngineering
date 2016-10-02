﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Modules;
using Assets.Scrips.MonoBehaviours.Controls;
using Assets.Scrips.Networks;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    //TODO: Extract input handler from game runner.
    public class GameRunner : MonoBehaviour
    {
        public static GameRunner Instance;

        public Module ActiveModule { get; private set; }

        private const float DoubleClickTimeLimit = 0.20f;
        private const float SimulationTickPeriodInSeconds = 0.1f;
        private bool acceptingInput = true;

        [UsedImplicitly]
        public void Start()
        {
            Instance = this;
            ModuleLibrary.Instance.UpdateModulesFromDisk();

        //  WorldComponent = new Module( 
        //       null,
        //       new List<IComponent>{new CoreComponent("Sub Pen", 32 ,18 , ModuleType.Container)} 
        //  );

            LoadModule("Start");
            //SetActiveModule(WorldComponent);
            StartCoroutine(InputListener());
            StartCoroutine(Ticker());
        }

        [UsedImplicitly]
        public void Update()
        {
            if (!acceptingInput)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (CurrentlySelectedModule() != null)
                {
                    CurrentlySelectedModule().AddWater();
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ModuleLibrary.Instance.DecrementSelectedComponent();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ModuleLibrary.Instance.IncrementSelectedComponent();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                acceptingInput = false;
                UserTextQuery.Instance.GetTextResponse("Module to load...", LoadModule);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                acceptingInput = false;
                UserTextQuery.Instance.GetTextResponse("Module to save...", SaveModule);
            }
        }

        //TODO: Actual error handling.
        private void LoadModule(string moduleName)
        {
            acceptingInput = true;
            var module = Module.FromJson(DiskOperations.ReadText(moduleName));
            ActiveModule = module;
        }

        private void SaveModule(string moduleName)
        {
            acceptingInput = true;
            DiskOperations.SaveText(moduleName, Module.ToJson(ActiveModule));
            ModuleLibrary.Instance.UpdateModulesFromDisk();
        }

        public List<Module> CurrentHeirarchy()
        {
            var heirarchy = new List<Module>();
            var current = ActiveModule;
            do
            {
                heirarchy.Add(current);
                current = current.ParentModule;
            } while (current != null);
            return heirarchy;
        }

        public Module CurrentlySelectedModule()
        {
            var selectedGrid = GridSelector.CurrentlySelectedGrid(ActiveModule);
            if (!ActiveModule.GridIsEmpty(selectedGrid))
            {
                return ActiveModule.GetModule(selectedGrid);
            }
            return null;
        }

        private IEnumerator Ticker()
        {
            while (true)
            {
                SubstanceNetwork.Instance.Simulate();
                yield return new WaitForSeconds(SimulationTickPeriodInSeconds);
            }
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                var moduleToAdd = ModuleLibrary.Instance.GetSelectedModule().DeepClone();
                moduleToAdd.ParentModule = ActiveModule;
                ActiveModule.AddModule(moduleToAdd, currentlySelectedGrid);
            }

            if (button == 1)
            {
                ActiveModule.RemoveModule(currentlySelectedGrid);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 1 && ActiveModule.ParentModule != null)
            {
                ActiveModule = ActiveModule.ParentModule;
            }

            if (CurrentlySelectedModule() != null && button == 0 && !CurrentlySelectedModule().IsTerminalModule)
            {
                ActiveModule = CurrentlySelectedModule();
            }
        }

        //TODO: Factor into input listener class.
        private IEnumerator InputListener()
        {
            while (enabled)
            {
                if (Input.GetMouseButtonDown(0) && acceptingInput)
                    yield return ClickEvent(0, GridSelector.CurrentlySelectedGrid(ActiveModule));

                if (Input.GetMouseButtonDown(1) && acceptingInput)
                    yield return ClickEvent(1, GridSelector.CurrentlySelectedGrid(ActiveModule));

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
