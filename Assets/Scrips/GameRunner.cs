using System.Collections;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Systems;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scrips
{
    //TODO: Extract input handler from game runner.
    public class GameRunner : MonoBehaviour
    {
        private EntityManager entityManager;

        [UsedImplicitly]
        public void Start()
        {
            entityManager = new EntityManager();

            var activeEntity = entityManager.BuildEntity(
                new List<IState>
                {
                    new NameState("Sub Pen"),
                    new PhysicalState(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, 28, 13)
                }
            );

            StaticStates.Add(new GameModeState(GameMode.Design));
            StaticStates.Add(new EntityLibraryState(InitialBuildableEntities.BuildableEntityLibrary));
            StaticStates.Add(new ActiveEntityState(activeEntity));
            StaticStates.Add(new SelectedState());

            SystemManager.AddSystem(new EngineSystem());
            SystemManager.AddSystem(new SubstanceNetworkSystem());
            SystemManager.AddSystem(new EntityLibrarySystem());
            SystemManager.AddSystem(new SaveLoadSystem());

            StartCoroutine(InputListener());
            StartCoroutine(Ticker());
        }

        [UsedImplicitly]
        public void Update()
        {
            SystemManager.Update();

            if (Input.GetKeyDown(KeyCode.T))
            {
                var selectedEntity = StaticStates.Get<SelectedState>().Entity;
                if (selectedEntity != null && selectedEntity.HasState<SubstanceNetworkState>())
                {
                    var substanceState = selectedEntity.GetState<SubstanceNetworkState>();
                    substanceState.UpdateSubstance(SubstanceType.Diesel,
                        substanceState.GetSubstance(SubstanceType.Diesel) + 10);
                }
            }
        }

        private static IEnumerator Ticker()
        {
            while (true)
            {
                SystemManager.Tick();
                yield return new WaitForSeconds(GlobalConstants.TickPeriodInSeconds);
            }
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                AddEntity(StaticStates.Get<EntityLibraryState>().GetSelectedEntity(), StaticStates.Get<ActiveEntityState>().ActiveEntity, currentlySelectedGrid);
            }

            if (button == 1)
            {
                RemoveEntity(StaticStates.Get<SelectedState>().Entity);
            }
        }

        private void DoubleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            var activeEntity = StaticStates.Get<ActiveEntityState>().ActiveEntity;
            if (button == 1 && !activeEntity.GetState<PhysicalState>().IsRoot())
            {
                StaticStates.Get<ActiveEntityState>().ActiveEntity = activeEntity.GetState<PhysicalState>().ParentEntity;
            }

            var selectedEntity = StaticStates.Get<SelectedState>().Entity;
            if (selectedEntity != null && button == 0 && !selectedEntity.GetState<PhysicalState>().IsRoot())
            {
                StaticStates.Get<ActiveEntityState>().ActiveEntity = selectedEntity;
            }
        }

        private void AddEntity(List<IState> states, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var newStates = states.DeepClone();
            var entity = entityManager.BuildEntity(newStates);
            PhysicalState.AddEntityToEntity(entity, entityToAddItTo, locationToAddIt);
            SystemManager.EntityAdded(entity);
        }

        private void RemoveEntity(Entity entityToRemove)
        {
            if (entityToRemove != null &&
                entityToRemove.HasState<PhysicalState>()
                && !entityToRemove.GetState<PhysicalState>().IsRoot())
            {
                SystemManager.EntityRemoved(entityToRemove);
                var parentEntity = entityToRemove.GetState<PhysicalState>().ParentEntity;
                parentEntity.GetState<PhysicalState>().RemoveEntityFromEntity(entityToRemove);
                entityToRemove.Delete();
            }
        }

        #region Input to be factored out.
        //TODO: Factor into input listener class.
        private IEnumerator InputListener()
        {
            while (enabled)
            {
                var selectedGrid = StaticStates.Get<SelectedState>().Grid;

                if (Input.GetMouseButtonDown(0))
                    yield return ClickEvent(0, selectedGrid);

                if (Input.GetMouseButtonDown(1))
                    yield return ClickEvent(1, selectedGrid);

                yield return null;
            }
        }

        private IEnumerator ClickEvent(int button, GridCoordinate selectedGrid)
        {
            yield return new WaitForEndOfFrame();

            var count = 0f;
            while (count < GlobalConstants.DoubleClickTimeLimit)
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