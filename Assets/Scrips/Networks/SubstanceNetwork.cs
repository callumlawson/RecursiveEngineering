using System.Collections.Generic;
using Assets.Scrips.Networks.Graph;

namespace Assets.Scrips.Networks
{
    //A comoponent can have serveral networks. 
    //Networks have a type. 
    //Networks of the same type can be combined.
    //Networks are basically graphs with rules on their connections. 
    public class SubstanceNetwork
    {
        private EngiDirectedSparseGraph<SubstanceNetworkNode> fluidNetwork;
        private List<NetworkNode> OurNetworkVerts;
    }
}
