using System;
using System.Collections.Generic;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.Util;
using Entity = Assets.Framework.Entities.Entity;

namespace Assets.Scrips.States
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

        public PhysicalState(int width, int height) : this(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, width, height) { }

        public PhysicalState() : this (null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, GlobalConstants.MediumToLargeRatio, GlobalConstants.MediumToLargeRatio) { }

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

        public bool GridIsFull(GridCoordinate grid)
        {
            return !GridIsEmpty(grid);
        }

        public void ForEachGrid(Action<GridCoordinate> actionToApply)
        {
            for (int x = 0; x < InternalWidth; x++)
            {
                for (int y = 0; y < InternalHeight; y++)
                {
                    actionToApply.Invoke(new GridCoordinate(x, y));
                }
            }
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

        public List<Entity> GetNeighbouringEntities()
        {
            if (IsRoot())
            {
                return new List<Entity>();
            }

            var parentGrid = ParentEntity.GetState<PhysicalState>();
            var results = new List<Entity>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var neighbourGrid = GridOperations.GetGridInDirection(BottomLeftCoordinate, direction);
                var neighbour = parentGrid.GetEntityAtGrid(neighbourGrid);
                if (neighbour != null)
                {
                    results.Add(neighbour);
                }    
            }

            return results;
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

        public List<Entity> GetEntitiesAtGrid(GridCoordinate grid)
        {
            var results = new List<Entity>();
            foreach (var entity in ChildEntities)
            {
                if (entity.GetState<PhysicalState>().BottomLeftCoordinate == grid)
                {
                    results.Add(entity);
                }
            }
            return results;
        }

        public static void AddEntityToEntity(Entity entityToAdd, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var physicalState = entityToAdd.GetState<PhysicalState>();
            physicalState.ParentEntity = entityToAddItTo;
            physicalState.BottomLeftCoordinate = locationToAddIt;
            if (entityToAddItTo != null)
            {
                entityToAddItTo.GetState<PhysicalState>().ChildEntities.Add(entityToAdd);
            }
        }

        public void RemoveEntityFromEntity(Entity entityToRemove)
        {
            ChildEntities.ForEach(entity => {
                if (entity == entityToRemove)
                {
                    ChildEntities.Remove(entityToRemove);
                }
            });
        }
    }
}
