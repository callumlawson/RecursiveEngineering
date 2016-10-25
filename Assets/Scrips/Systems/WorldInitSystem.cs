using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Framework.States;
using Assets.Framework.Systems;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;

namespace Assets.Scrips.Systems
{
    class WorldInitSystem : IEntityManager, IInitSystem
    {
        private EntityStateSystem entitySystem;

        public void SetEntitySystem(EntityStateSystem ess)
        {
            entitySystem = ess;
        }

        public void OnInit()
        {
            var sub = entitySystem.CreateEntity(
                    new List<IState>
                    {
                        new EntityTypeState("The World"),
                        new PhysicalState(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, 28, 13, true, true)
                    },
                    null,
                    new GridCoordinate(0, 0)
                );

            StaticStates.Get<ActiveEntityState>().ActiveEntity = sub;
            StaticStates.Get<WorldEntityState>().World = sub;
        }
    }
}
