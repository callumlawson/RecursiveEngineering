using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Systems;

namespace Assets.Scrips.Entities
{
    public static class EntityManager
    {
        private static readonly Dictionary<int, List<State>> EntityList = new Dictionary<int, List<State>>();

        private static int entityIdCount;

        public static int CreateEntity(List<State> states)
        {
            var entityId = entityIdCount;
            EntityList.Add(entityId, states);
            
            foreach (var system in SystemManager.Systems)
            {
                system.OnEntityAdded(entityId);
            }

            entityIdCount++;
            return entityId;
        }

        public static List<State> GetEntity(int entityId)
        {
            List<State> states;
            EntityList.TryGetValue(entityId, out states);
            return states;
        }

        public static void RemoveEntity(int entityId)
        {
            foreach (var system in SystemManager.Systems)
            {
                system.OnEntityRemoved(entityId);
            }

            EntityList.Remove(entityId);
        }
    }
}
