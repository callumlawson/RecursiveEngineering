using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    internal class SeaSystem : ITickSystem, IInitSystem
    {
        private readonly List<SubstanceNetworkState> worldEdgeEnvironments = new List<SubstanceNetworkState>();
        private readonly List<SubstanceNetworkState> worldEnvironments = new List<SubstanceNetworkState>();

        public SeaSystem()
        {
            StaticStates.Get<GameModeState>().GameModeChanged += OnGameModeChanged;
        }

        public void OnInit()
        {
            var worldEntity = StaticStates.Get<WorldEntityState>().World;
            var worldPhysicalState = worldEntity.GetState<PhysicalState>();

            worldPhysicalState.ForEachGrid(grid =>
            {
                var entity = worldPhysicalState.GetEntityAtGrid(grid);
                if (!entity.HasState<SubstanceNetworkState>())
                {
                    return;
                }
                var entitySubstanceState = entity.GetState<SubstanceNetworkState>();
                if (GridIsOnEdge(grid, worldPhysicalState.InternalWidth, worldPhysicalState.InternalHeight))
                {
                    worldEdgeEnvironments.Add(entitySubstanceState);
                } 
                worldEnvironments.Add(entitySubstanceState);
            });
        }

        public void Tick()
        {
            if (StaticStates.Get<GameModeState>().GameMode == GameMode.Play)
            {
                Flood();
            }
        }

        private void OnGameModeChanged(GameMode mode)
        {
            if (mode == GameMode.Design)
            {
                Drain();
            }
        }

        private void Flood()
        {
            worldEdgeEnvironments.ForEach(environment =>
            {
                var spareCapacity = 100 - environment.GetSubstance(SubstanceType.SeaWater);
                if (spareCapacity > 0)
                {
                    environment.AddSubstance(SubstanceType.SeaWater, spareCapacity);
                }
            });
        }

        private void Drain()
        {
            worldEnvironments.ForEach(environment => environment.ClearSubstance(SubstanceType.SeaWater));
        }

        private static bool GridIsOnEdge(GridCoordinate grid, int width, int height)
        {
            return grid.X == 0 || grid.Y == 0 || grid.X == width - 1 || grid.Y == height - 1;
        }
    }
}