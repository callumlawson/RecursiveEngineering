using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;

namespace Assets.Scrips.States
{
    class SelectedState : IState
    {
        public GridCoordinate Grid;
        public Entity Entity;
    }
}
