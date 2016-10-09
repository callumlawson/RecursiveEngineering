using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scrips.Entities;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    class EntityInitSystem : ISystem
    {
        public EntityInitSystem()
        {
            
        }

        public void Tick()
        {
        }

        public void OnEntityAdded(Entity entity)
        {
            if (entity.HasState<PhysicalState>() && !entity.GetState<PhysicalState>().IsTerminal())
            {
                
            }
        }

        public void OnEntityRemoved(Entity entity)
        {
        }
    }
}
