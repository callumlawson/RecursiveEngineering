using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Entities
{
    public static class Entity
    {
        public static T GetState<T>(List<State> states) where T : State
        {
            foreach (var state in states)
            {
                if (state.GetType() == typeof(T))
                {
                    return state as T;
                }
            }
            return null;
        }

        public static T GetState<T>(int entityId) where T : State
        {
            var states = EntityManager.GetEntity(entityId);
            return GetState<T>(states);
        }

        public GridCoordinate GetGridPosition(int entityId)
        {
            var states = EntityManager.GetEntity(entityId);
            var hierarchy = GetState<HierarchyState>(entityId);
            return hierarchy.ParentEntity.HasValue ? ParentModule.GetGridForContainedModule(this) : new GridCoordinate(0, 0);
        }

        public GetGridForContainedModule(int entityId)
        {
            
        }
    }
}
