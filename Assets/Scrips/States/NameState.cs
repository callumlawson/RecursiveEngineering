using System;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    [Serializable]
    public class NameState : IState
    {
        public string Name { get; set; }
      
        public NameState(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}", Name);
        }
    }
}
