using System.Collections.Generic;
using Assets.Scrips.Entities;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ISystem
    {
        private List<Entity> activeEntities;

        public EngineSystem()
        {
            activeEntities = new List<Entity>();
        }

        public void Tick()
        {
            foreach (var entity in activeEntities)
            {
                entity.GetState<EngineState>().CurrentRpm += 1;
            }
        }

        public void OnEntityAdded(Entity entity)
        {
            if (entity.HasState<EngineState>())
            {
                activeEntities.Add(entity);
            }
        }

        public void OnEntityRemoved(Entity entity)
        {
            if (entity.HasState<EngineState>())
            {
                activeEntities.Remove(entity);
            }
        }
    }
}
