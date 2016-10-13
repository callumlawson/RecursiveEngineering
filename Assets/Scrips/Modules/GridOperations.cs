using System;
using System.Collections.Generic;
using Assets.Framework.Entities;
using Assets.Scrips.Datastructures;
using Assets.Scrips.States;
using Newtonsoft.Json;

namespace Assets.Scrips.Modules
{
    [Serializable]
    public class GridOperations
    {
        [JsonProperty] public int Width { get; private set; }
        [JsonProperty] public int Height { get; private set; }
        [JsonProperty] private readonly Entity[,] innerEntities;

        public GridOperations(int width, int height)
        {
            Width = width;
            Height = height;
            innerEntities = new Entity[width, height];
        }

//        public bool AddModule(Entity Entity, GridCoordinate coord)
//        {
//            if (GridIsInModule(coord) && GridIsEmpty(coord))
//            {
//                innerEntities[coord.X, coord.Y] = Entity;
//                return true;
//            }
//            return false;
//        }
//
//        public Entity RemoveModule(Entity moduleToRemove)
//        {
//            for (var x = 0; x < Width; x++)
//            {
//                for (var y = 0; y < Height; y++)
//                {
//                    var grid = new GridCoordinate(x, y);
//                    var moduleAtGrid = GetModule(grid);
//                    if (moduleAtGrid == moduleToRemove)
//                    {
//                        innerEntities[grid.X, grid.Y] = null;
//                        return moduleAtGrid;
//                    }
//                }
//            }
//            return null;
//        }

        public Entity RemoveModule(GridCoordinate grid)
        {
            Entity entityRemoved = null;
            if (GridIsInModule(grid))
            {
                entityRemoved = innerEntities[grid.X, grid.Y];
                innerEntities[grid.X, grid.Y] = null;
            }
            return entityRemoved;
        }

        public Entity GetModule(GridCoordinate grid)
        {
            if (GridIsInModule(grid))
            {
                return innerEntities[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public GridCoordinate GetGridForModule(Entity entity)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var gridCoordinate = new GridCoordinate(x, y);
                    if (GetModule(gridCoordinate) == entity)
                    {
                        return gridCoordinate;
                    }
                }
            }
            return null;
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            if (GridIsInModule(grid))
            {
                return innerEntities[grid.X, grid.Y] == null;
            }
            return true;
        }

        public IEnumerable<Entity> GetNeigbouringEntities(GridCoordinate grid)
        {
            var list = new List<Entity>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var entity = GetNeigbouringEntity(grid, direction);
                if (entity != null)
                {
                    list.Add(entity);
                }
            }
            return list;
        }

        public List<Entity> GetContainedModules()
        {
            var results = new List<Entity>();
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var gridCoordinate = new GridCoordinate(x, y);
                    if (GridIsFull(gridCoordinate))
                    {
                        results.Add(GetModule(gridCoordinate));
                    }
                }
            }
            return results;
        }

        private Entity GetNeigbouringEntity(GridCoordinate grid, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    var up = GetGridInDirection(grid, Direction.Up);
                    if (GridIsInModule(up) && GridIsFull(up))
                    {
                        return GetModule(up);
                    }
                    return null;
                case Direction.Down:
                    var down = GetGridInDirection(grid, Direction.Down);
                    if (GridIsInModule(down) && GridIsFull(down))
                    {
                        return GetModule(down);
                    }
                    return null;
                case Direction.Left:
                    var left = GetGridInDirection(grid, Direction.Left);
                    if (GridIsInModule(left) && GridIsFull(left))
                    {
                        return GetModule(left);
                    }
                    return null;
                case Direction.Right:
                    var right = GetGridInDirection(grid, Direction.Right);
                    if (GridIsInModule(right) && GridIsFull(right))
                    {
                        return GetModule(right);
                    }
                    return null;
                case Direction.None:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        public static GridCoordinate GetGridInDirection(GridCoordinate grid, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return new GridCoordinate(grid.X, grid.Y + 1);
                case Direction.Down:
                    return new GridCoordinate(grid.X, grid.Y - 1);
                case Direction.Left:
                    return new GridCoordinate(grid.X - 1, grid.Y);
                case Direction.Right:
                    return new GridCoordinate(grid.X + 1, grid.Y); ;
                case Direction.None:
                    return grid;
                default:
                    throw new ArgumentOutOfRangeException("direction", direction, null);
            }
        }

        private bool GridIsFull(GridCoordinate grid)
        {
            return !GridIsEmpty(grid);
        }

        private bool GridIsInModule(GridCoordinate coord)
        {
            return coord.X >= 0 && coord.Y >= 0 && coord.X < innerEntities.GetLength(0) && coord.Y < innerEntities.GetLength(1);
        }

    }
}