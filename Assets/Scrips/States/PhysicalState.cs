using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Framework.States;
using Assets.Scrips.Datastructures;
using Assets.Scrips.Modules;
using Assets.Scrips.Util;
using JetBrains.Annotations;
using Entity = Assets.Framework.Entities.Entity;

namespace Assets.Scrips.States
{
    [Serializable]
    public class PhysicalState : IState
    {
        public Entity ParentEntity;
        public readonly List<Entity> ChildEntities;
        public GridCoordinate BottomLeftCoordinate;
        public int ExternalWidth;
        public int ExternalHeight;
        public int InternalWidth;
        public int InternalHeight;
        public bool IsTangible;
        public bool IsPermeable;

        //Used as a performance optimisation on entity lookup.
        private readonly Dictionary<GridCoordinate, List<Entity>> childEntityLookup = new Dictionary<GridCoordinate, List<Entity>>();

        public PhysicalState(int width, int height, bool isTangible, bool isPermeable) : this(null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, width, height, isTangible, isPermeable) { }

        public PhysicalState() : this (null, new List<Entity>(), new GridCoordinate(0, 0), 1, 1, GlobalConstants.MediumToLargeRatio, GlobalConstants.MediumToLargeRatio, true, true) { }

        public PhysicalState(Entity parentEntity, List<Entity> childEntities, GridCoordinate bottomLeftCoordinate, int externalWidth, int externalHeight, int internalWidth, int internalHeight, bool isTangible, bool isPermeable)
        {
            ParentEntity = parentEntity;
            ChildEntities = childEntities;
            BottomLeftCoordinate = bottomLeftCoordinate;
            ExternalWidth = externalWidth;
            ExternalHeight = externalHeight;
            InternalWidth = internalWidth;
            InternalHeight = internalHeight;
            IsTangible = isTangible;
            IsPermeable = isPermeable;
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

        public IEnumerable<Entity> GetNeighbouringEntities()
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
            if (childEntityLookup.ContainsKey(grid) && childEntityLookup[grid].Count > 0)
            {
                return childEntityLookup[grid].ElementAt(0);
            }
            return null;
        }

        public IEnumerable<Entity> GetEntitiesAtGrid(GridCoordinate grid)
        {
            if (childEntityLookup.ContainsKey(grid))
            {
                return childEntityLookup[grid];
            }
            return Enumerable.Empty<Entity>();
        }

        public IEnumerable<Entity> GetEntitiesAtGridWithState<T>(GridCoordinate grid) where T : IState
        {
            if (childEntityLookup.ContainsKey(grid))
            {
                var childEntities = childEntityLookup[grid];
                return childEntities.Where(entity => entity.HasState<T>());
            }
            return Enumerable.Empty<Entity>();
        }


        public static bool AddEntityToEntity([NotNull] Entity entityToAdd, [NotNull] GridCoordinate locationToAddIt, [NotNull] Entity entityToAddItTo)
        {
            var physicalState = entityToAdd.GetState<PhysicalState>();
            physicalState.ParentEntity = entityToAddItTo;
            physicalState.BottomLeftCoordinate = locationToAddIt;
            var targetEntityPhysicalState = entityToAddItTo.GetState<PhysicalState>();
            var entitiesAtTargetLocation = targetEntityPhysicalState.GetEntitiesAtGrid(locationToAddIt).ToList();

            if (!entitiesAtTargetLocation.Any() || entitiesAtTargetLocation.All(entity => !entity.GetState<PhysicalState>().IsTangible))
            {
                targetEntityPhysicalState.ChildEntities.Add(entityToAdd);
                UpdateLocationLookup(entityToAdd, locationToAddIt, targetEntityPhysicalState);
                return true;
            }
            return false;
        }

        public void RemoveEntityFromEntity(Entity entityToRemove)
        {
            ChildEntities.ForEach(entity => {
                if (entity == entityToRemove)
                {
                    childEntityLookup[entityToRemove.GetState<PhysicalState>().BottomLeftCoordinate].Remove(entityToRemove);
                    ChildEntities.Remove(entityToRemove);
                }
            });
        }

        private static void UpdateLocationLookup(Entity entityToAdd, GridCoordinate locationToAddIt, PhysicalState targetEntityPhysicalState)
        {
            if (!targetEntityPhysicalState.childEntityLookup.ContainsKey(locationToAddIt))
            {
                targetEntityPhysicalState.childEntityLookup.Add(locationToAddIt, new List<Entity>());
            }
            targetEntityPhysicalState.childEntityLookup[locationToAddIt].Add(entityToAdd);
        }

        public override string ToString()
        {
            return string.Format("Physical State - Parent: {0}", ParentEntity.EntityId);
        }
    }
}
