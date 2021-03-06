﻿using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Assets.Scrips.Util;

namespace Assets.Framework.Systems
{
    public class EntityStateSystem
    {
        private readonly Dictionary<IFilteredSystem, List<Entity>> activeEntitiesPerSystem = new Dictionary<IFilteredSystem, List<Entity>>();
        private readonly List<ITickEntitySystem> tickEntitySystems = new List<ITickEntitySystem>();
        private readonly List<ITickSystem> tickSystems = new List<ITickSystem>();
        private readonly List<IUpdateEntitySystem> updateEntitySystems = new List<IUpdateEntitySystem>();
        private readonly List<IUpdateSystem> updateSytems = new List<IUpdateSystem>();
        private readonly List<IInitSystem> initSystems = new List<IInitSystem>();
        private readonly List<Entity> entitiesToRemove = new List<Entity>();
        private readonly EntityManager entityManager;

        public EntityStateSystem()
        {
            entityManager = new EntityManager();
        }

        public void AddSystem(ISystem system)
        {
            var tickEntitySystem = system as ITickEntitySystem;
            var updateEntitySystem = system as IUpdateEntitySystem;
            var updateSystem = system as IUpdateSystem;
            var tickSystem = system as ITickSystem;
            var fiteredSystem = system as IFilteredSystem;
            var entityManagerSystem = system as IEntityManager;
            var initSystem = system as IInitSystem;

            if (entityManagerSystem != null)
            {
                entityManagerSystem.SetEntitySystem(this);
            }

            if (tickEntitySystem != null)
            {
              tickEntitySystems.Add(tickEntitySystem);  
            }

            if (updateEntitySystem != null)
            {
                updateEntitySystems.Add(updateEntitySystem);
            }

            if (updateSystem != null)
            {
                updateSytems.Add(updateSystem);
            }

            if (tickSystem != null)
            {
                tickSystems.Add(tickSystem);
            }

            if (fiteredSystem != null)
            {
                activeEntitiesPerSystem.Add(fiteredSystem, new List<Entity>());
            }

            if (initSystem != null)
            {
                initSystems.Add(initSystem);
            }
        }

        public void Init()
        {
            foreach (var system in initSystems)
            {
                system.OnInit();
            }
        }

        public void Update()
        {
            foreach (var system in updateEntitySystems)
            {
                system.Update(activeEntitiesPerSystem[system]);
            }
            foreach (var system in updateSytems)
            {
                system.Update();
            }
            //DeleteMarkedEntities();
        }

        public void Tick()
        {
            foreach (var system in tickEntitySystems)
            {
                system.Tick(activeEntitiesPerSystem[system]);
            }
            foreach (var system in tickSystems)
            {
                system.Tick();
            }
            DeleteMarkedEntities();
        }

        public Entity CreateEntity(List<IState> states, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var newStates = states.DeepClone();
            var entity = BuildEntity(newStates);
            InitEnvironmentEntities(entity);

            if (entityToAddItTo != null && PhysicalState.AddEntityToEntity(entity, locationToAddIt, entityToAddItTo))
            {
                EntityAdded(entity);
                return entity;
            }
            if (entityToAddItTo == null)
            {
                EntityAdded(entity);
                return entity;
            }
            return null;
        }

        public Entity BuildEntity(List<IState> states)
        {
            return entityManager.BuildEntity(states);
        }

        public void RemoveEntity(Entity entityToRemove)
        {
            if (entityToRemove != null &&
                entityToRemove.HasState<PhysicalState>() &&
                entityToRemove.GetState<PhysicalState>().IsTangible &&
                !entityToRemove.GetState<PhysicalState>().IsRoot())
            {
                entitiesToRemove.Add(entityToRemove);
            }
        }

        public string DebugEntity(int entityId)
        {
            var entity = entityManager.GetEntity(entityId);
            return entity.ToString();
        }

        private void DeleteMarkedEntities()
        {
            entitiesToRemove.ForEach(entityToRemove =>
            {
                if (entityToRemove.HasState<PhysicalState>() && entityToRemove.GetState<PhysicalState>().ParentEntity != null)
                {
                    var parentEntity = entityToRemove.GetState<PhysicalState>().ParentEntity;
                    parentEntity.GetState<PhysicalState>().RemoveEntityFromEntity(entityToRemove);
                }

                if (entityToRemove.HasState<PhysicalState>())
                {
                    var children = entityToRemove.GetState<PhysicalState>().ChildEntities;
                    foreach (var child in children)
                    {
                        EntityRemoved(child);
                        entityManager.DeleteEntity(child);
                    }
                }

                EntityRemoved(entityToRemove);
                entityManager.DeleteEntity(entityToRemove);
                entitiesToRemove.Remove(entityToRemove);
            });
        }

        private void InitEnvironmentEntities(Entity entity)
        {
            entity.GetState<PhysicalState>().ForEachGrid(grid => CreateEntity(InitialBuildableEntities.Environment, entity, grid));
        }

        private void EntityAdded(Entity entity)
        {
            foreach (var system in activeEntitiesPerSystem.Keys)
            {
                //TODO: Cache required states to avoid unneeded alocation.
                var entityHasAllStates = system.RequiredStates().TrueForAll(entity.HasState);
                if (entityHasAllStates)
                {
                    activeEntitiesPerSystem[system].Add(entity);
                    if (system is IReactiveEntitySystem)
                    {
                        var reactiveSystem = system as IReactiveEntitySystem;
                        reactiveSystem.OnEntityAdded(entity);
                    }
                }
            }
        }

        private void EntityRemoved(Entity entity)
        {
            foreach (var system in activeEntitiesPerSystem.Keys)
            {
                //TODO: Cache required states to avoid unneeded alocation.
                var entityHasAllStates = system.RequiredStates().TrueForAll(entity.HasState);
                if (entityHasAllStates)
                {
                    if (system is IReactiveEntitySystem)
                    {
                        var reactiveSystem = system as IReactiveEntitySystem;
                        reactiveSystem.OnEntityRemoved(entity);
                    }
                    activeEntitiesPerSystem[system].Remove(entity);
                }
            }
        }
    }
}
