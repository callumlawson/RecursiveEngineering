using System;
using System.Collections.Generic;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class PhysicalState : IState
    {
        public int? ParentEntity;
        public List<int> ChildEntities;
        public GridCoordinate BottomLeftCoordinate;
        public int ExternalWidth;
        public int ExternalHeight;

        public PhysicalState(int? parentEntity, List<int> childEntities, GridCoordinate bottomLeftCoordinate, int externalWidth, int externalHeight)
        {
            ParentEntity = parentEntity;
            ChildEntities = childEntities;
            BottomLeftCoordinate = bottomLeftCoordinate;
            ExternalWidth = externalWidth;
            ExternalHeight = externalHeight;
        }
    }
}
