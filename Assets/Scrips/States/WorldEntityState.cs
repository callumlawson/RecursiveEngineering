using Assets.Framework.Entities;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    public class WorldEntityState : IState
    {
        public Entity World;

        public WorldEntityState(Entity world)
        {
            World = world;
        }
    }
}
