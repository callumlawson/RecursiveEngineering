using System;
using System.Collections.Generic;
using Assets.Scrips.Modules;
using Assets.Scrips.Networks.Graph;

namespace Assets.Scrips.Networks
{
    public class SubstanceNetworkNode : NetworkNode, IComparable<SubstanceNetworkNode>
    {
        public Module Module { get; private set; }

        private readonly Dictionary<string, float> substances;

        public SubstanceNetworkNode(Module module, Dictionary<string, float> substances = null)
        {
            Module = module;
            this.substances = substances ?? new Dictionary<string, float>();
        }

        public void UpdateSubstance(string substance, float amount)
        {
            substances[substance] = amount;
        }

        public float GetSubstance(string substance)
        {
            return substances.ContainsKey(substance) ? substances[substance] : 0.0f;
        }

        public int CompareTo(SubstanceNetworkNode other)
        {
            throw new NotImplementedException();
        }
    }
}
