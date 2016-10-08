﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Datatypes;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;
using Assets.Scrips.MonoBehaviours.Controls;
using Assets.Scrips.Networks;
using Assets.Scrips.Systems;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    //TODO: Extract input handler from game runner.
    public class GameRunner : MonoBehaviour
    {
        public static GameRunner Instance;

        public Entity ActiveEntity { get; private set; }

        private bool acceptingInput = true;
        private const float DoubleClickTimeLimit = 0.20f;
        private const float SimulationTickPeriodInSeconds = 0.1f;

        private EntityManager entityManager;

        [UsedImplicitly]
        public void Start()
        {
            entityManager = new EntityManager();
            ActiveEntity = entityManager.BuildEntity(new List<IState>
                {
                    new NameState("Sub Pen"),
                    new PhysicalState(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, 28, 13)
                });

            Instance = this;

            //EntityLibrary.Instance.UpdateModulesFromDisk();

            SystemManager.AddSystem(new EngineSystem());

            //LoadModule("Start");

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
//                if (CurrentlySelectedEntity() != null)
//                {
//                    CurrentlySelectedEntity().AddWater();
//                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                EntityLibrary.Instance.DecrementSelectedComponent();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                EntityLibrary.Instance.IncrementSelectedComponent();
            }

//            if (Input.GetKeyDown(KeyCode.O))
//            {
//                acceptingInput = false;
//                UserTextQuery.Instance.GetTextResponse("Module to load...", LoadModule);
//            }
//
//            if (Input.GetKeyDown(KeyCode.P))
//            {
//                acceptingInput = false;
//                UserTextQuery.Instance.GetTextResponse("Module to save...", SaveModule);
//            }
        }

//        //TODO: Actual error handling.
//        private void LoadModule(string moduleName)
//        {
//            acceptingInput = true;
//            var module = Module.FromJson(DiskOperations.ReadText(moduleName));
//            ActiveEntity = entityManager.BuildEntity(module.Components);
//            ActiveModule = module;
//        }
//
//        private void SaveModule(string moduleName)
//        {
//            acceptingInput = true;
//            DiskOperations.SaveText(moduleName, Module.ToJson(ActiveModule));
//            EntityLibrary.Instance.UpdateModulesFromDisk();
//        }

        public IEnumerable<Entity> CurrentHeirarchy()
        {
            var heirarchy = new List<Entity>();
            var current = ActiveEntity;
            do
            {
                heirarchy.Add(current);
                current = current.GetState<PhysicalState>().ParentEntity;
            } while (current != null);
            return heirarchy;
        }

        public Entity CurrentlySelectedEntity()
        {
            var selectedGrid = GridSelector.CurrentlySelectedGrid(ActiveEntity);
            var physicalState = ActiveEntity.GetState<PhysicalState>();
            return !physicalState.GridIsEmpty(selectedGrid) ? physicalState.GetEntityAtGrid(selectedGrid) : null;
        }

        private static IEnumerator Ticker()
        {
            while (true)
            {
                SubstanceNetwork.Instance.Simulate();
                SystemManager.Tick();
                yield return new WaitForSeconds(SimulationTickPeriodInSeconds);
            }
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                AddEntity(EntityLibrary.Instance.GetSelectedModule(), ActiveEntity, currentlySelectedGrid);
            }

            if (button == 1)
            {
                RemoveEntity(ActiveEntity);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 1 && ActiveEntity.GetState<PhysicalState>().ParentEntity != null)
            {
                ActiveEntity = ActiveEntity.GetState<PhysicalState>().ParentEntity;
            }

            if (CurrentlySelectedEntity() != null && button == 0 && !CurrentlySelectedEntity().GetState<PhysicalState>().IsRoot())
            {
                ActiveEntity = CurrentlySelectedEntity();
            }
        }

        private void AddEntity(List<IState> states, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var newStates = states.DeepClone();
            var entity = entityManager.BuildEntity(newStates);
            PhysicalState.AddEntityToEntity(entity, entityToAddItTo, locationToAddIt);
        }

        private void RemoveEntity(Entity entityToRemove)
        {
            ActiveEntity.GetState<PhysicalState>().ChildEntities.Remove(entityToRemove);
            entityManager.DeleteEntity(entityToRemove);
        }

        #region Input to be factored out.

        //TODO: Factor into input listener class.
        private IEnumerator InputListener()
        {
            while (enabled)
            {
                if (Input.GetMouseButtonDown(0) && acceptingInput)
                    yield return ClickEvent(0, GridSelector.CurrentlySelectedGrid(ActiveEntity));

                if (Input.GetMouseButtonDown(1) && acceptingInput)
                    yield return ClickEvent(1, GridSelector.CurrentlySelectedGrid(ActiveEntity));

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

        #endregion
    }
}