using System;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Networks.Graph;

namespace Assets.Scrips.Networks
{
    public class SubstanceNetworkNode : NetworkNode, IComparable<SubstanceNetworkNode>
    {
        public EngiComponent Component { get; private set; }

        private readonly Dictionary<string, float> substances;

        public SubstanceNetworkNode(EngiComponent component, Dictionary<string, float> substances = null)
        {
            Component = component;
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
