using System.Collections.Generic;
using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems
{
    public static class SystemManager
    {
        private static readonly List<ISystem> Systems = new List<ISystem>();

        public static void Tick()
        {
            foreach (var system in Systems)
            {
                system.Tick();
            }
        }

        public static void AddSystem(ISystem system)
        {
            Systems.Add(system);
        }

        public static void EntityAdded(Entity entity)
        {
            foreach (var system in Systems)
            {
                system.OnEntityAdded(entity);
            }
        }

        public static void EntityRemoved(Entity entity)
        {
            foreach (var system in Systems)
            {
                system.OnEntityRemoved(entity);
            }
        }
    }
}
