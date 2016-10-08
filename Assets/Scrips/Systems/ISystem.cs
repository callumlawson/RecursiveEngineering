using Assets.Scrips.Entities;

namespace Assets.Scrips.Systems
{
    public interface ISystem
    {
        void Tick();
        void OnEntityAdded(Entity entity);
        void OnEntityRemoved(Entity entity);
    }
}
