using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Datastructures.Graph;
using Assets.Scrips.Modules;
using Assets.Scrips.States;
using UnityEngine;

namespace Assets.Scrips.Systems
{
    public class SubstanceNetworkSystem : IReactiveEntitySystem, ITickEntitySystem, IUpdateSystem
    {
        private readonly DirectedSparseGraph<Entity> network;

        public SubstanceNetworkSystem()
        {
            network = new DirectedSparseGraph<Entity>();
        }

        public List<Type> RequiredStates()
        {
            return new List<Type> {typeof(SubstanceConnectorState), typeof(SubstanceNetworkState), typeof(PhysicalState)};
        }

        public void OnEntityAdded(Entity entity)
        {
            network.AddVertex(entity);
            ConnectToAdjacentModulesWithinModule(entity);
        }

        public void Tick(List<Entity> matchingEntities)
        {
            //TODO: Build into system predicates.
            if (StaticStates.Get<GameModeState>().GameMode == GameMode.Design)
            {
                return;
            }

            Profiler.BeginSample("SubstanceSystem");
            foreach (var substanceEntity in matchingEntities)
            {
                if (IsUnobstructed(substanceEntity))
                {
                    foreach (SubstanceType substance in Enum.GetValues(typeof(SubstanceType)))
                    {
                        Profiler.BeginSample("SubstanceSystem-GetNeigbours");
                        var neighbours = network.NeighboursInclusive(substanceEntity);
                        var validNeighbours = neighbours.Where(IsUnobstructed).ToList();
                        Profiler.EndSample();
                        Profiler.BeginSample("SubstanceSystem-CalcNewValuesAndApply");
                        var averageValue = validNeighbours.Sum(entity => entity.GetState<SubstanceNetworkState>().GetSubstance(substance)) / validNeighbours.Count;
                        foreach (var neighbour in validNeighbours)
                        {
                            neighbour.GetState<SubstanceNetworkState>().UpdateSubstance(substance, averageValue);
                        }
                        Profiler.EndSample();
                    }
                }
            }
            Profiler.EndSample();
        }

        private static bool IsUnobstructed(Entity entity)
        {
            var entityPhysicalState = entity.GetState<PhysicalState>();
            var entityGrid = entityPhysicalState.BottomLeftCoordinate;

            var entityParent = entityPhysicalState.ParentEntity;
            var entityParentPhysicalState = entityParent.GetState<PhysicalState>();
            if (entityParentPhysicalState == null)
            {
                UnityEngine.Debug.Log("Entity: " + entity);
                UnityEngine.Debug.Log("Parent: " + entityParent);
            }

           
            var allEntitiesAtGrid = entityParentPhysicalState.GetEntitiesAtGridWithState<PhysicalState>(entityGrid);
            return allEntitiesAtGrid.All(entityOnSameGrid => entityOnSameGrid.GetState<PhysicalState>().IsPermeable);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var selectedEntity = StaticStates.Get<SelectedState>().Entity;
                if (selectedEntity != null && selectedEntity.HasState<SubstanceNetworkState>())
                {
                    var substanceState = selectedEntity.GetState<SubstanceNetworkState>();
                    substanceState.UpdateSubstance(SubstanceType.Diesel,
                        substanceState.GetSubstance(SubstanceType.Diesel) + 10);
                }
            }
        }

        public void OnEntityRemoved(Entity entity)
        {
            network.RemoveVertex(entity);
        }

        public float GetDiesel(Entity entity)
        {
            return network.HasVertex(entity) ? 
                entity.GetState<SubstanceNetworkState>().GetSubstance(SubstanceType.Diesel) : 
                0.0f;
        }

        public string Readable()
        {
            return network.ToReadable();
        }

        //Generalise to arbitary numbers of levels. Make Neigbouring Components include those at higher levels?
        private void ConnectToAdjacentModulesWithinModule(Entity addedEntity)
        {
            var addedEntityGrid = addedEntity.GetState<PhysicalState>().BottomLeftCoordinate;
            foreach (var neigbour in addedEntity.GetState<PhysicalState>().GetNeighbouringEntities())
            {
                var neigbourGrid = neigbour.GetState<PhysicalState>().BottomLeftCoordinate;
                var direction = AdjacentDirection(addedEntityGrid, neigbourGrid);

                if (neigbour.HasState<SubstanceConnectorState>() && 
                    HaveFacingConnections(direction, addedEntity.GetState<SubstanceConnectorState>().Diretions, neigbour.GetState<SubstanceConnectorState>().Diretions))
                {
                    AddBidirectionalConnection(addedEntity, neigbour);
                }
            }
        }

        private Direction EdgeConnection(Module module, GridCoordinate grid, SubstanceConnectorState connectorState)
        {
            if (grid.X == 0 && connectorState.Diretions.Contains(Direction.Left))
            {
                return Direction.Left;
            }
            if (grid.X == module.GetState<PhysicalState>().InternalWidth &&
                connectorState.Diretions.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            if (grid.Y == 0 && connectorState.Diretions.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            if (grid.Y == module.GetState<PhysicalState>().InternalHeight &&
               connectorState.Diretions.Contains(Direction.Up))
            {
                return Direction.Up;
            }
            return Direction.None;
        }

        private Direction AdjacentDirection(GridCoordinate sourceGrid, GridCoordinate targetGrid)
        {
            if (sourceGrid.X == targetGrid.X)
            {
                if (sourceGrid.Y - targetGrid.Y > 0)
                {
                    return Direction.Down;
                }
                if (sourceGrid.Y - targetGrid.Y < 0)
                {
                    return Direction.Up;
                }
            }
            if (sourceGrid.Y == targetGrid.Y)
            {
                if (sourceGrid.X - targetGrid.X > 0)
                {
                    return Direction.Right;
                }
                if (sourceGrid.X - targetGrid.X < 0)
                {
                    return Direction.Left;
                }
            }
            return Direction.None;
        }

        private static bool HaveFacingConnections(Direction direction, List<Direction> sourceDirections, List<Direction> targetDirections)
        {
            switch (direction)
            {
                case Direction.Left:
                    return sourceDirections.Contains(Direction.Right) && targetDirections.Contains(Direction.Left);
                case Direction.Right:
                    return sourceDirections.Contains(Direction.Left) && targetDirections.Contains(Direction.Right);
                case Direction.Up:
                    return sourceDirections.Contains(Direction.Up) && targetDirections.Contains(Direction.Down);
                case Direction.Down:
                    return sourceDirections.Contains(Direction.Down) && targetDirections.Contains(Direction.Up);
                case Direction.None:
                    return false;
                default:
                    return false;
            }
        }

        private void AddBidirectionalConnection(Entity source, Entity destination)
        {
            network.AddEdge(destination, source);
            network.AddEdge(source, destination);
        }
    }
}