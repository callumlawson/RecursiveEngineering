using System;
using System.Collections.Generic;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Entity = Assets.Scrips.Entities.Entity;

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

        public static void AddEntityToEntity(Entity entityToAdd, Entity entityToAddItTo, GridCoordinate locationToAddIt)
        {
            var physicalState = entityToAdd.GetState<PhysicalState>();
            physicalState.ParentEntity = entityToAddItTo;
            physicalState.BottomLeftCoordinate = locationToAddIt;
            entityToAddItTo.GetState<PhysicalState>().ChildEntities.Add(entityToAdd);
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
