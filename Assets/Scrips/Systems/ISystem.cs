using Assets.Scrips.Modules;

namespace Assets.Scrips.Systems
{
    public interface ISystem
    {
        void Tick();
        void OnModuleAdded(Module module);
        void OnModuleRemoved(Module module);
    }
}
