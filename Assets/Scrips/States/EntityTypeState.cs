using System;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    [Serializable]
    public class EntityTypeState : IState
    {
        public string EntityType { get; set; }
      
        public EntityTypeState(string entityType)
        {
            EntityType = entityType;
        }

        public override string ToString()
        {
            return string.Format("Entity Type: {0}", EntityType);
        }
    }
}
