using System;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ITickEntitySystem
    {
        private const float FuelRequiredPerSecond = 3 * GlobalConstants.TickPeriodInSeconds;
        private const float RmpDeltaPerSecond = 10 * GlobalConstants.TickPeriodInSeconds;
        private const float MaxRpm = 1200;

        public List<Type> RequiredStates()
        {
            return new List<Type> {typeof(EngineState)};
        }

        public void Tick(List<Entity> matchingEntities)
        {
            Profiler.BeginSample("EngineSystem");
            foreach (var entity in matchingEntities)
            {
                var substanceState = entity.GetState<SubstanceNetworkState>();
                var engineState = entity.GetState<EngineState>();
                var currentDieselAmount = substanceState.GetSubstance(SubstanceType.Diesel);

                if (currentDieselAmount >= FuelRequiredPerSecond)
                {
                    substanceState.UpdateSubstance(SubstanceType.Diesel, currentDieselAmount - FuelRequiredPerSecond);
                    if (engineState.CurrentRpm < MaxRpm)
                    {
                        engineState.CurrentRpm += RmpDeltaPerSecond;
                    }
                }
                else if (engineState.CurrentRpm > 0)
                {
                    engineState.CurrentRpm -= RmpDeltaPerSecond;
                }
            }
            Profiler.EndSample();
        }
    }
}
