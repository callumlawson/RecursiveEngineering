using System;
using System.Collections.Generic;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class HierarchyState : State
    {
        public int? ParentEntity;
        public List<int> ChildEntities;
        public GridCoordinate Coordinate;
        public int Width;
        public int Height;

        public HierarchyState(int parentEntity, List<int> childEntities)
        {
            ParentEntity = parentEntity;
            ChildEntities = childEntities;
        }

        public static GridCoordinate GetGridForContainedEntity(int entityId)
        {
            var states = EntityManager.GetEntity(entityId);
            var hierarchy = Entity.GetState<HierarchyState>(states);
        }
    }
}
