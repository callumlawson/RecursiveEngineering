using System;
using Assets.Scrips.Components;
using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ISystem
    {
        public static EngineSystem Instance;

        public int? EngineEntityId;

        public EngineSystem()
        {
            Instance = this;
        }

        public void Tick()
        {
//            if (EngineEntityId == null)
//            {
//                return;
//            }
//
//            var engineComponent = Entity.GetState<EngineState>(EngineEntityId.Value);
//            engineComponent.CurrentRpm += 1;
//
//            UnityEngine.Debug.Log("Engine running");
        }

        public void OnEntityAdded(int entityId)
        {
//            if (Entity.GetState<EngineState>(entityId) != null)
//            {
//                if (EngineEntityId == null)
//                {
//                    EngineEntityId = entityId;
//                }
//                else
//                {
//                    throw new Exception("More than one engine added! Only one engine is supported.");
//                }
//            }
        }

        public void OnEntityRemoved(int entityId)
        {
            if (EngineEntityId == entityId)
            {
                EngineEntityId = null;
            }
        }
    }
}
