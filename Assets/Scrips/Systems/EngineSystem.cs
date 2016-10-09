using System.Collections.Generic;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Systems.Substance;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ISystem
    {
        private readonly List<Entity> activeEntities;

        private const float FuelPerSecond = 3 * GlobalConstants.TickPeriodInSeconds;
        private const float RmpDeltaPerSecond = 10 * GlobalConstants.TickPeriodInSeconds;
        private const float MaxRpm = 1200;

        public EngineSystem()
        {
            activeEntities = new List<Entity>();
        }

        public void OnEntityAdded(Entity entity)
        {
            if (entity.HasState<EngineState>())
            {
                activeEntities.Add(entity);
            }
        }

        public void Tick()
        {
            foreach (var entity in activeEntities)
            {
                var substanceState = entity.GetState<SubstanceNetworkState>();
                var diesel = substanceState.GetSubstance(SubstanceType.Diesel);
                var engineState = entity.GetState<EngineState>();
                if (diesel >= FuelPerSecond)
                {
                    substanceState.UpdateSubstance(SubstanceType.Diesel, diesel - FuelPerSecond);
                    if (engineState.CurrentRpm < MaxRpm)
                    {
                        engineState.CurrentRpm += RmpDeltaPerSecond;
                    }
                }
                else if(engineState.CurrentRpm > 0)
                {
                    engineState.CurrentRpm -= RmpDeltaPerSecond;
                }
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
