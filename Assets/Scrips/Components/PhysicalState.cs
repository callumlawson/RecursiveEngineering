using System;
using System.Collections.Generic;
using Assets.Scrips.Datatypes;
using Assets.Scrips.Entities;
using Assets.Scrips.Modules;

namespace Assets.Scrips.Components
{
    [Serializable]
    public class PhysicalState : IState
    {
        public Entity ParentEntity;
        public List<Entity> ChildEntities;
        public GridCoordinate BottomLeftCoordinate;
        public int ExternalWidth;
        public int ExternalHeight;
        public int InternalWidth;
        public int InternalHeight;

        public PhysicalState()
        {
            ParentEntity = null;
            ChildEntities = new List<Entity>();
            BottomLeftCoordinate = new GridCoordinate(0, 0);
            ExternalWidth = 1;
            ExternalHeight = 1;
            InternalWidth = EntityUtils.MediumToLargeRatio;
            InternalHeight = EntityUtils.MediumToLargeRatio;
        }

        public PhysicalState(Entity parentEntity, List<Entity> childEntities, GridCoordinate bottomLeftCoordinate, int externalWidth, int externalHeight, int internalWidth, int internalHeight)
        {
            ParentEntity = parentEntity;
            ChildEntities = childEntities;
            BottomLeftCoordinate = bottomLeftCoordinate;
            ExternalWidth = externalWidth;
            ExternalHeight = externalHeight;
            InternalWidth = internalWidth;
            InternalHeight = internalHeight;
        }

        public bool IsTerminal()
        {
            return InternalWidth == 0 || InternalHeight == 0;
        }

        public bool IsRoot()
        {
            return ParentEntity == null;
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            foreach (var entity in ChildEntities)
            {
                if (entity.GetState<PhysicalState>().BottomLeftCoordinate == grid)
                {
                    return false;
                }
            }
            return true;
        }

        public Entity GetEntityAtGrid(GridCoordinate grid)
        {
            foreach (var entity in ChildEntities)
            {
                if (entity.GetState<PhysicalState>().BottomLeftCoordinate == grid)
                {
                    return entity;
                }
            }
            return null;
        }

        public static void AddEntityToEntity(Entity entityToAdd, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var physicalState = entityToAdd.GetState<PhysicalState>();
            physicalState.ParentEntity = entityToAddItTo;
            physicalState.BottomLeftCoordinate = locationToAddIt;
            entityToAddItTo.GetState<PhysicalState>().ChildEntities.Add(entityToAdd);
        }
    }
}
