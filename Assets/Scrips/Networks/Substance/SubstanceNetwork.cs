using System;
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
            return GetNodeForComponent(component) != null ? GetNodeForComponent(component).GetSubstance(SubstanceTypes.WATER) : 0.0f;
        }

//        public List<SubstanceNetworkNode> GetNodesForComonent(EngiComponent module)
//        {
//            var result = new List<SubstanceNetworkNode>();
//            foreach (var substanceNode in Network.Vertices)
//            {
//                if (substanceNode.Module == module)
//                {
//                    result.Add(substanceNode);
//                }
//            }
//            return result;
//        }

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

                foreach (var neigbour in addedModule.ParentModule.ModuleGrid.GetNeigbouringComponents(grid))
                {
                    var addedModuleGrid = addedModule.GetGridPosition();
                    var neigbourGrid = neigbour.GetGridPosition();
                    var direction = AdjacentDirection(addedModuleGrid, neigbourGrid);

                    if (neigbour.GetComponent<SubstanceConnector>() != null && HaveFacingConnections(direction,  addedModule.GetComponent<SubstanceConnector>(), neigbour.GetComponent<SubstanceConnector>()))
                    {
                        AddBidirectionalConnection(GetNodeForComponent(addedModule), GetNodeForComponent(neigbour));
                    }
                }
            }
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

        private static bool HaveFacingConnections(Direction direction, SubstanceConnector source, SubstanceConnector target)
        {
            switch (direction)
            {
                case Direction.Left:
                    return source.Diretions.Contains(Direction.Right) && target.Diretions.Contains(Direction.Left);
                case Direction.Right:
                    return source.Diretions.Contains(Direction.Left) && target.Diretions.Contains(Direction.Right);
                case Direction.Up:
                    return source.Diretions.Contains(Direction.Up) && target.Diretions.Contains(Direction.Down);
                case Direction.Down:
                    return source.Diretions.Contains(Direction.Down) && target.Diretions.Contains(Direction.Up);
                case Direction.None:
                    return false;
                default:
                    return false;
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