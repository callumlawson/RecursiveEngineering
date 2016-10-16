using System.Collections;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Framework.Util;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Systems
{
    public class PlayerEntityModificationSystem : IEntityManager, IInitSystem
    {
        private EntityStateSystem entitySystem;

        public void SetEntitySystem(EntityStateSystem manager)
        {
            entitySystem = manager;
            StaticGameObject.Instance.StartCoroutine(InputListener());
        }

        public void OnInit()
        {
            StaticStates.Get<ActiveEntityState>().ActiveEntity =
                AddEntity(
                    new List<IState>
                    {
                        new NameState("Sub Pen"),
                        new PhysicalState(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, 28, 13)
                    },
                    null,
                    new GridCoordinate(0, 0)
                );
        }

        private void SingleClick(int button, GridCoordinate currentlySelectedGrid)
        {
            if (button == 0)
            {
                AddEntity(StaticStates.Get<EntityLibraryState>().GetSelectedEntity(),
                    StaticStates.Get<ActiveEntityState>().ActiveEntity, currentlySelectedGrid);
            }

            if (button == 1)
            {
                RemoveEntity(StaticStates.Get<SelectedState>().Entity);
            }
        }

        private void DoubleClick(int button)
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

        private Entity AddEntity(List<IState> states, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var newStates = states.DeepClone();
            var entity = entitySystem.BuildEntity(newStates);
            InitEnvironmentEntities(entity);
            PhysicalState.AddEntityToEntity(entity, entityToAddItTo, locationToAddIt);
            entitySystem.EntityAdded(entity);
            return entity;
        }

        private void InitEnvironmentEntities(Entity entity)
        {
            entity.GetState<PhysicalState>()
                .ForEachGrid(grid => AddEntity(InitialBuildableEntities.Environment, entity, grid));
        }

        private void RemoveEntity(Entity entityToRemove)
        {
            if (entityToRemove != null &&
                entityToRemove.HasState<PhysicalState>()
                && !entityToRemove.GetState<PhysicalState>().IsRoot())
            {
                entitySystem.EntityRemoved(entityToRemove);
                var parentEntity = entityToRemove.GetState<PhysicalState>().ParentEntity;
                parentEntity.GetState<PhysicalState>().RemoveEntityFromEntity(entityToRemove);
                entityToRemove.Delete();
            }
        }

        private IEnumerator InputListener()
        {
            while (true)
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
                    DoubleClick(button);
                    yield break;
                }
                count += Time.deltaTime;
                yield return null;
            }
            SingleClick(button, selectedGrid);
        }
    }
}