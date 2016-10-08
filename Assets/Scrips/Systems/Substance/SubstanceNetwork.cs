using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Datastructures.Graph;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems.Substance
{
    //A comoponent can have serveral networks. 
    //Networks have a type. 
    //Networks of the same type can be combined.
    //Networks are basically graphs with rules on their connections. 
    //TODO: Consider using injection.
    public class SubstanceNetwork : ISystem
    {
        private static SubstanceNetwork instance;

        public static SubstanceNetwork Instance
        {
            get { return instance ?? (instance = new SubstanceNetwork()); }
        }

        private DirectedSparseGraph<SubstanceNetworkNode> Network;

        public SubstanceNetwork()
        {
            Network = new DirectedSparseGraph<SubstanceNetworkNode>();
        }

        public float GetWater(Entity entity)
        {
            var nodeValue = GetNodeForComponent(entity) != null
                ? GetNodeForComponent(entity).GetSubstance(SubstanceTypes.WATER)
                : 0.0f;

            foreach (var childModule in entity.GetState<PhysicalState>().ChildEntities)
            {
                nodeValue += GetWater(childModule);
            }

            return nodeValue;
        }

        public SubstanceNetworkNode GetNodeForComponent(Entity entity)
        {
            foreach (var vertex in Network.Vertices)
            {
                if (vertex.Entity == entity)
                {
                    return vertex;
                }
            }
            return null;
        }

        public void AddWaterToEntity(Entity entity)
        {
            var possibleNode = GetNodeForComponent(entity);
            if (possibleNode != null)
            {
                possibleNode.UpdateSubstance(SubstanceTypes.WATER, possibleNode.GetSubstance(SubstanceTypes.WATER) + 10);
            }
        }

        public void AddEntityToNetwork(Entity entity)
        {
            if (entity.GetState<SubstanceConnectorState>() != null)
            {
                AddNode(new SubstanceNetworkNode(entity));
            }
        }

        public void OnEntityAdded(Entity entity)
        {
            if (entity.HasState<SubstanceConnectorState>())
            {
                AddNode(new SubstanceNetworkNode(entity));
            }
        }

        public void Tick()
        {
            Flow();
        }

        public void OnEntityRemoved(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveModuleFromNetwork(Entity entity)
        {
            if (entity.GetState<SubstanceConnectorState>() != null)
            {
                RemoveNode(GetNodeForComponent(entity));
            }
        }

        public void ConnectModule(Entity entity)
        {
            if (entity.GetState<SubstanceConnectorState>() != null)
            {
                ConnectToAdjacentModulesWithinModule(entity);
            }
        }

        public void SetupModule(Entity addedEntity)
        {
            AddEntityToNetwork(addedEntity);
            ConnectModule(addedEntity);
        }

        //Generalise to arbitary numbers of levels. Make Neigbouring Components include those at higher levels?
        private void ConnectToAdjacentModulesWithinModule(Entity addedEntity)
        {
            var grid = addedEntity.GetState<PhysicalState>().BottomLeftCoordinate;
            foreach (var neigbour in addedEntity.GetState<PhysicalState>().GetNeighbouringEntities())
            {
                var neigbourGrid = neigbour.GetState<PhysicalState>().BottomLeftCoordinate;
                var direction = AdjacentDirection(grid, neigbourGrid);

                if (neigbour.GetState<SubstanceConnectorState>() != null &&
                    HaveFacingConnections(direction, addedEntity.GetState<SubstanceConnectorState>().Diretions,
                        neigbour.GetState<SubstanceConnectorState>().Diretions))
                {
                    AddBidirectionalConnection(GetNodeForComponent(addedEntity), GetNodeForComponent(neigbour));
                }
            }
        }

        public string Readable()
        {
            return Network.ToReadable();
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

        private void Flow()
        {
            foreach (var graphVertex in Network.Vertices)
            {
                var neighbours = Network.NeighboursInclusive(graphVertex);
                var averageValue = neighbours.Sum(vertex => vertex.GetSubstance(SubstanceTypes.WATER))/neighbours.Count;
                foreach (var neighbour in neighbours)
                {
                    neighbour.UpdateSubstance(SubstanceTypes.WATER, averageValue);
                }
            }
        }

        private void AddBidirectionalConnection(SubstanceNetworkNode source, SubstanceNetworkNode destination)
        {
            Network.AddEdge(destination, source);
            Network.AddEdge(source, destination);
        }

        private void RemoveNode(SubstanceNetworkNode node)
        {
            Network.RemoveVertex(node);
        }

        private void AddNode(SubstanceNetworkNode node)
        {
            Network.AddVertex(node);
        }

    }
}