using System;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Systems
{
    public class EngineSystem : ISystem
    {
        public static EngineSystem Instance;

        public Module EngineModule;

        public EngineSystem()
        {
            Instance = this;
        }

        public void Tick()
        {
            if (EngineModule == null)
            {
                return;
            }

            var engineComponent = EngineModule.GetState<EngineComponent>();
            engineComponent.CurrentRpm += 1;

            UnityEngine.Debug.Log("Engine running");
        }

        public void OnModuleAdded(Module module)
        {
            if (module.GetState<EngineComponent>() != null)
            {
                if (EngineModule == null)
                {
                    EngineModule = module;
                }
                else
                {
                    throw new Exception("More than one engine added! Only one engine is supported.");
                }
            }
        }

        public void OnModuleRemoved(Module module)
        {
            if (EngineModule == module)
            {
                EngineModule = null;
            }
        }
    }
}
