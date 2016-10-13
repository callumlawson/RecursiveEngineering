using System;
using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;

namespace Assets.Scrips.States
{
    [Serializable]
    public class SubstanceNetworkState : IComparable<SubstanceNetworkState>, IState
    {
        private readonly Dictionary<SubstanceType, float> substances;

        public SubstanceNetworkState(Dictionary<SubstanceType, float> substances = null)
        {
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

        public int CompareTo(SubstanceNetworkState other)
        {
            throw new NotImplementedException();
        }
    }
}
