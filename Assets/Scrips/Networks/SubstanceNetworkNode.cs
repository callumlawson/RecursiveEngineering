using System;
using System.Collections.Generic;
using Assets.Scrips.Networks.Graph;

namespace Assets.Scrips.Networks
{
    public class SubstanceNetworkNode : NetworkNode, IComparable<SubstanceNetworkNode>
    {
        private readonly Dictionary<string, float> substances;

        public SubstanceNetworkNode(Dictionary<string, float> substances = null)
        {
            this.substances = substances ?? new Dictionary<string, float>();
        }

        public void UpdateSubstance(string substance, float amount)
        {
            substances[substance] = amount;
        }

        public int CompareTo(SubstanceNetworkNode other)
        {
            throw new NotImplementedException();
        }
    }
}
