using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;

namespace Assets.Scrips.States
{
    [Serializable]
    public class SubstanceNetworkState : IComparable<SubstanceNetworkState>, IState
    {
        private readonly Dictionary<SubstanceType, float> substances;

        public SubstanceNetworkState()
        {
            substances = new Dictionary<SubstanceType, float>();
            foreach (SubstanceType substance in Enum.GetValues(typeof(SubstanceType)))
            {
                substances.Add(substance, 0.0f);
            }
        }

        public void AddSubstance(SubstanceType substance, float amount)
        {
            substances[substance] += amount;
        }

        public void UpdateSubstance(SubstanceType substance, float amount)
        {
            substances[substance] = amount;
        }

        public void ClearSubstance(SubstanceType substance)
        {
            substances[substance] = 0;
        }

        public float GetSubstance(SubstanceType substance)
        {
            return substances[substance];
        }

        public int CompareTo(SubstanceNetworkState other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            if (substances.Values.Any())
            {
                var lines = substances.Select(kvp => kvp.Key + ": " + kvp.Value.ToString(CultureInfo.InvariantCulture)).ToArray();
                return string.Join(Environment.NewLine, lines);
            }
            return "No Substances Detected";
        }
    }
}
