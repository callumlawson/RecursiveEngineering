namespace Assets.Scrips.Systems
{
    public interface ISystem
    {
        void Tick();
        void OnEntityAdded(int entityId);
        void OnEntityRemoved(int entity);
    }
}
