using System.Collections.Generic;

namespace Assets.Scrips.Systems
{
    public static class SystemManager
    {
        public static readonly List<ISystem> Systems = new List<ISystem>();

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
    }
}
