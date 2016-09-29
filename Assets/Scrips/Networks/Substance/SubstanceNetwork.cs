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
            if (addedModule.GetComponent<CoreComponent>().Type == ModuleType.WaterTank)
            {
                AddNode(new SubstanceNetworkNode(addedModule));

                foreach (var neigbour in addedModule.ParentModule.ModuleGrid.GetNeigbouringComponents(grid))
                {
                    if (neigbour.GetComponent<CoreComponent>().Type == ModuleType.WaterTank)
                    {
                        AddBidirectionalConnection(GetNodeForComponent(addedModule), GetNodeForComponent(neigbour));
                    }
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