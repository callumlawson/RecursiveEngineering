using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ISystem
    {
        public static EngineSystem Instance;

        public Entity EngineEntityId;

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

        public void OnEntityAdded(Entity entityId)
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

        public void OnEntityRemoved(Entity entityId)
        {
            if (EngineEntityId == entityId)
            {
                EngineEntityId = null;
            }
        }
    }
}
