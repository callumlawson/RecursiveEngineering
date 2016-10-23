using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Framework.Entities;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Assets.Scrips.Util;

namespace Assets.Scrips.Systems
{
    public class CrewHealthSystem : ITickEntitySystem
    {
        private const float DrowningThreshold = 80.0f;
        private const float DrowningDamagePerTick = 10.0f * GlobalConstants.TickPeriodInSeconds;

        public List<Type> RequiredStates()
        {
            return new List<Type> { typeof(CrewState), typeof(HealthState), typeof(PhysicalState) };
        }

        public void Tick(List<Entity> matchingEntities)
        {
            foreach (var entity in matchingEntities)
            {
                var healthState = entity.GetState<HealthState>();
                var physicalState = entity.GetState<PhysicalState>();
                Drown(healthState, physicalState);
            }
        }

        private static void Drown(HealthState healthState, PhysicalState physicalState)
        {
            var entityGrid = physicalState.BottomLeftCoordinate;
            var entityParent = physicalState.ParentEntity;
            var otherEntitiesInGrid = entityParent.GetState<PhysicalState>().GetEntitiesAtGrid(entityGrid);
            var enoughLiquid = otherEntitiesInGrid.Any(entityInGrid => entityInGrid.HasState<SubstanceNetworkState>() &&
                                                      (entityInGrid.GetState<SubstanceNetworkState>().GetSubstance(SubstanceType.SeaWater) > DrowningThreshold ||
                                                       entityInGrid.GetState<SubstanceNetworkState>().GetSubstance(SubstanceType.Diesel) > DrowningThreshold));
            if (enoughLiquid)
            {
                healthState.DoDamage(DrowningDamagePerTick); 
            }
        }
    }
}
