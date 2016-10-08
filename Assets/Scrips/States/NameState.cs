using System;

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
    }
}
