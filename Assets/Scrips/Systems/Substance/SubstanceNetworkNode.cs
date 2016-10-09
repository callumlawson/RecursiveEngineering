using System;
using System.Collections.Generic;
using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems.Substance
{
    public class SubstanceNetworkNode : IComparable<SubstanceNetworkNode>
    {
        public Entity Entity { get; private set; }

        private readonly Dictionary<SubstanceType, float> substances;

        public SubstanceNetworkNode(Entity entity, Dictionary<SubstanceType, float> substances = null)
        {
            Entity = entity;
            this.substances = substances ?? new Dictionary<SubstanceType, float>();
        }

        public void UpdateSubstance(SubstanceType substance, float amount)
        {
            substances[substance] = amount;
        }

        public float GetSubstance(SubstanceType substance)
        {
            return substances.ContainsKey(substance) ? substances[substance] : 0.0f;
        }

        public int CompareTo(SubstanceNetworkNode other)
        {
            throw new NotImplementedException();
        }
    }
}
