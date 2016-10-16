using System;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.Systems;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    public class CrewMovementSystem : ITickEntitySystem
    {
        public List<Type> RequiredStates()
        {
            return new List<Type> {typeof(CrewState), typeof(PhysicalState)};
        }

        public void Tick(List<Entity> matchingEntities)
        {
            foreach (var entity in matchingEntities)
            {
                var physicalState = entity.GetState<PhysicalState>();
                var neighbours = physicalState.GetNeighbouringEntities();

            }
        }
    }
}
