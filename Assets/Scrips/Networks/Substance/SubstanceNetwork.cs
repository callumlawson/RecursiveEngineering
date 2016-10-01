using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scrips.Components;
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
        private EngiDirectedSparseGraph<SubstanceNetworkNode> Network;

        public SubstanceNetwork()
        {
            Network = new EngiDirectedSparseGraph<SubstanceNetworkNode>();
        }

        public float GetWater(Module component)
        {
            var nodeValue = GetNodeForComponent(component) != null
                ? GetNodeForComponent(component).GetSubstance(SubstanceTypes.WATER)
                : 0.0f;

            foreach (var childModule in component.GetContainedModules())
            {
                nodeValue += GetWater(childModule);
            }

            return nodeValue;
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

        public void Simulate()
        {
            Flow();
        }

        public void AddComponent(Module addedModule, GridCoordinate grid)
        {
            if (addedModule.GetComponent<SubstanceConnector>() != null)
            {
                AddNode(new SubstanceNetworkNode(addedModule));
                ConnectToAdjacentModulesWithinMoudle(addedModule, grid);
                ConnectToModulesAlongEdge(addedModule, grid);
            }
        }

        private void ConnectToModulesAlongEdge(Module addedModule, GridCoordinate grid)
        {
            foreach (var module in addedModule.ParentModule.ModuleGrid.GetNeigbouringModules(grid))
            {

            }
        }

        //Generalise to arbitary numbers of levels. Make Neigbouring Components include those at higher levels?
        private void ConnectToAdjacentModulesWithinMoudle(Module addedModule, GridCoordinate grid)
        {
            foreach (var neigbour in addedModule.ParentModule.ModuleGrid.GetNeigbouringModules(grid))
            {
                var addedModuleGrid = addedModule.GetGridPosition();
                var neigbourGrid = neigbour.GetGridPosition();
                var direction = AdjacentDirection(addedModuleGrid, neigbourGrid);

                if (neigbour.GetComponent<SubstanceConnector>() != null &&
                    HaveFacingConnections(direction, addedModule.GetComponent<SubstanceConnector>().Diretions,
                        neigbour.GetComponent<SubstanceConnector>().Diretions))
                {
                    AddBidirectionalConnection(GetNodeForComponent(addedModule), GetNodeForComponent(neigbour));
                }
            }
        }

        public bool AddConnection(SubstanceNetworkNode source, SubstanceNetworkNode destination)
        {
            return Network.AddEdge(source, destination);
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
            if (grid.X == module.GetComponent<CoreComponent>().InternalWidth &&
                connector.Diretions.Contains(Direction.Right))
            {
                return Direction.Right;
            }
            if (grid.Y == 0 && connector.Diretions.Contains(Direction.Down))
            {
                return Direction.Down;
            }
            if (grid.Y == module.GetComponent<CoreComponent>().InteralHeight &&
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

        private void AddNode(SubstanceNetworkNode node)
        {
            Network.AddVertex(node);
        }
    }
}