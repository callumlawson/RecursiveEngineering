using System;
using System.Collections.Generic;
using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems.Substance
{
    public class SubstanceNetworkNode : IComparable<SubstanceNetworkNode>
    {
        public Entity Entity { get; private set; }

        private readonly Dictionary<string, float> substances;

        public SubstanceNetworkNode(Entity entity, Dictionary<string, float> substances = null)
        {
            Entity = entity;
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
