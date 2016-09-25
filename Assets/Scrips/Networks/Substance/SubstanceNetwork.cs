using System.Linq;
using Assets.Scrips.Components;
using Assets.Scrips.Networks.Graph;
using Assets.Scrips.Networks.Substance;

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

        public float GetWater(EngiComponent component)
        {
            var result = 0.0f;
            foreach (var substanceNode in Network.Vertices)
            {
                if (substanceNode.Component == component)
                {
                    result += substanceNode.GetSubstance(SubstanceTypes.WATER);
                }
            }
            return result;
        }

        public void Tick()
        {
            Flow();
        }

        public SubstanceNetworkNode GetNodeForComponent(EngiComponent component)
        {
            foreach (var vertex in Network.Vertices)
            {
                if (vertex.Component == component)
                {
                    return vertex;
                }
            }
            return null;
        }

        public bool AddConnection(SubstanceNetworkNode source, SubstanceNetworkNode destination)
        {
            return Network.AddEdge(source, destination);
        }

        public void AddBidirectionalConnection(SubstanceNetworkNode source, SubstanceNetworkNode destination)
        {
            Network.AddEdge(destination, source);
            Network.AddEdge(source, destination);
        }

        public void AddNode(SubstanceNetworkNode node)
        {
            Network.AddVertex(node);
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
                var averageValue = neighbours.Sum(vertex => vertex.GetSubstance(SubstanceTypes.WATER)) / neighbours.Count;
                foreach (var neighbour in neighbours)
                {
                    neighbour.UpdateSubstance(SubstanceTypes.WATER, averageValue);
                }
            }
        }
    }
}
