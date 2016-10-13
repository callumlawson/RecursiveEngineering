using System.Collections.Generic;
using Assets.Framework.Entities;

namespace Assets.Framework.Systems
{
    public static class SystemManager
    {
        private static readonly Dictionary<IFilteredSystem, List<Entity>> ActiveEntitiesPerSystem = new Dictionary<IFilteredSystem, List<Entity>>();
        private static readonly List<ITickEntitySystem> TickSystems = new List<ITickEntitySystem>();
        private static readonly List<IUpdateEntitySystem> UpdateEntitySystems = new List<IUpdateEntitySystem>();
        private static readonly List<IUpdateSystem> UpdateSytems = new List<IUpdateSystem>();

        public static void AddSystem(ISystem system)
        {
            var tickSystem = system as ITickEntitySystem;
            var updateEntitySystem = system as IUpdateEntitySystem;
            var updateSystem = system as IUpdateSystem;
            var fiteredSystem = system as IFilteredSystem;

            if (tickSystem != null)
            {
              TickSystems.Add(tickSystem);  
            }

            if (updateEntitySystem != null)
            {
                UpdateEntitySystems.Add(updateEntitySystem);
            }

            if (updateSystem != null)
            {
                UpdateSytems.Add(updateSystem);
            }

            if (fiteredSystem != null)
            {
                ActiveEntitiesPerSystem.Add(fiteredSystem, new List<Entity>());
            }
        }

        public static void Update()
        {
            foreach (var system in UpdateEntitySystems)
            {
                system.Update(ActiveEntitiesPerSystem[system]);
            }

            foreach (var system in UpdateSytems)
            {
                system.Update();
            }
        }

        public static void Tick()
        {
            foreach (var system in TickSystems)
            {
                system.Tick(ActiveEntitiesPerSystem[system]);
            }
        }

        public static void EntityAdded(Entity entity)
        {
            foreach (var system in ActiveEntitiesPerSystem.Keys)
            {
                //TODO: Cache required states to avoid unneeded alocation.
                var entityHasAllStates = system.RequiredStates().TrueForAll(entity.HasState);
                if (entityHasAllStates)
                {
                    ActiveEntitiesPerSystem[system].Add(entity);
                    if (system is IReactiveEntitySystem)
                    {
                        var reactiveSystem = system as IReactiveEntitySystem;
                        reactiveSystem.OnEntityAdded(entity);
                    }
                }
            }
        }

        public static void EntityRemoved(Entity entity)
        {
            foreach (var system in ActiveEntitiesPerSystem.Keys)
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
                    ActiveEntitiesPerSystem[system].Remove(entity);
                }
            }
        }
    }
}
