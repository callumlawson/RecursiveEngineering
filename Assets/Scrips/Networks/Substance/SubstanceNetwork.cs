using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scrips.Components;
using Assets.Scrips.Datatypes;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;
using Assets.Scrips.Networks.Graph;
using Assets.Scrips.Networks.Substance;
using Assets.Scrips.Util;

namespace Assets.Scrips.Networks
{
    //A comoponent can have serveral networks. 
    //Networks have a type. 
    //Networks of the same type can be combined.
    //Networks are basically graphs with rules on their connections. 
    //TODO: Consider using injection.
    public class SubstanceNetwork
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
            return 0.0f;
//            var nodeValue = GetNodeForComponent(entity) != null
//                ? GetNodeForComponent(entity).GetSubstance(SubstanceTypes.WATER)
//                : 0.0f;
//
//            foreach (var childModule in entity.GetState<PhysicalState>().ChildEntities)
//            {
//                nodeValue += GetWater(childModule);
//            }
//
//            return nodeValue;
        }

        public SubstanceNetworkNode GetNodeForComponent(Module module)
        {
            foreach (var vertex in Network.Vertices)
            {
                if (vertex.Module == module)
                {
                    return vertex;
                }
            }
            return null;
        }

        public void AddWaterToModule(Module module)
        {
            var possibleNode = GetNodeForComponent(module);
            if (possibleNode != null)
            {
                possibleNode.UpdateSubstance(SubstanceTypes.WATER, possibleNode.GetSubstance(SubstanceTypes.WATER) + 10);
            }
        }

        public void Simulate()
        {
            Flow();
        }

        public void AddModuleToNetwork(Module module)
        {
            if (module.GetState<SubstanceConnector>() != null)
            {
                AddNode(new SubstanceNetworkNode(module));
            }
        }

        public void RemoveModuleFromNetwork(Module module)
        {
            if (module.GetState<SubstanceConnector>() != null)
            {
                RemoveNode(GetNodeForComponent(module));
            }
        }

        public void ConnectModule(Module module)
        {
            if (module.GetState<SubstanceConnector>() != null)
            {
                ConnectToAdjacentModulesWithinModule(module);
            }
        }

        public void SetupModule(Module addedModule)
        {
            AddModuleToNetwork(addedModule);
            ConnectModule(addedModule);
        }

        //Generalise to arbitary numbers of levels. Make Neigbouring Components include those at higher levels?
        private void ConnectToAdjacentModulesWithinModule(Module addedModule)
        {
//            var grid = addedModule.GetGridPosition();
//            foreach (var neigbour in addedModule.ParentModule.GetNeighbouringModules(grid))
//            {
//                var addedModuleGrid = addedModule.GetGridPosition();
//                var neigbourGrid = neigbour.GetGridPosition();
//                var direction = AdjacentDirection(addedModuleGrid, neigbourGrid);
//
//                if (neigbour.GetState<SubstanceConnector>() != null &&
//                    HaveFacingConnections(direction, addedModule.GetState<SubstanceConnector>().Diretions,
//                        neigbour.GetState<SubstanceConnector>().Diretions))
//                {
//                    AddBidirectionalConnection(GetNodeForComponent(addedModule), GetNodeForComponent(neigbour));
//                }
//            }
        }

        public string Readable()
        {
            return Network.ToReadable();
        }

        private Direction EdgeConnection(Module module, GridCoordinate grid, SubstanceConnector connector)
        {
            if (grid.X == 0 && connector.Diretions.Contains(Direction.Left))
            {
                return Direction.Left;
            }
            if (grid.X == module.GetState<PhysicalState>().InternalWidth &&
                connector.Diretions.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            if (grid.Y == 0 && connector.Diretions.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            if (grid.Y == module.GetState<PhysicalState>().InternalHeight &&
               connector.Diretions.Contains(Direction.Up))
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