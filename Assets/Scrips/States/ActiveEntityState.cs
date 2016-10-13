using Assets.Framework.Entities;
using Assets.Framework.States;

namespace Assets.Scrips.States
{
    public class ActiveEntityState : IState
    {
        public Entity ActiveEntity;

        public ActiveEntityState(Entity activeEntity)
        {
            ActiveEntity = activeEntity;
        }
    }
}
