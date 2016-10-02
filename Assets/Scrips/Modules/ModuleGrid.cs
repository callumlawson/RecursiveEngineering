using System;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Util;
using Newtonsoft.Json;

namespace Assets.Scrips.Modules
{
    [Serializable]
    public class ModuleGrid
    {
        [JsonProperty] public int Width { get; private set; }
        [JsonProperty] public int Height { get; private set; }
        [JsonProperty] private readonly Module[,] innerModules;

        public ModuleGrid(int width, int height)
        {
            Width = width;
            Height = height;
            innerModules = new Module[width, height];
        }

        public bool AddModule(Module module, GridCoordinate coord)
        {
            if (GridIsInModule(coord) && GridIsEmpty(coord))
            {
                innerModules[coord.X, coord.Y] = module;
                return true;
            }
            return false;
        }

        public Module RemoveModule(GridCoordinate grid)
        {
            Module moduleRemoved = null;
            if (GridIsInModule(grid))
            {
                moduleRemoved = innerModules[grid.X, grid.Y];
                innerModules[grid.X, grid.Y] = null;
            }
            return moduleRemoved;
        }

        public Module GetModule(GridCoordinate grid)
        {
            if (GridIsInModule(grid))
            {
                return innerModules[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public GridCoordinate GetGridForModule(Module module)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var gridCoordinate = new GridCoordinate(x, y);
                    if (GetModule(gridCoordinate) == module)
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
                return innerModules[grid.X, grid.Y] == null;
            }
            return true;
        }

        public IEnumerable<Module> GetNeigbouringModules(GridCoordinate grid)
        {
            var list = new List<Module>();

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var module = GetNeigbouringModule(grid, direction);
                if (module != null)
                {
                    list.Add(module);
                }
            }
            return list;
        }

        public List<Module> GetContainedModules()
        {
            var results = new List<Module>();
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

        private Module GetNeigbouringModule(GridCoordinate grid, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    var up = GetGridInDirection(grid, Direction.Up);
                    if (GridIsInModule(up) && GridIsFull(up))
                    {
                        return GetModule(up);
                    }
                    else
                    {
                        
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

        private GridCoordinate GetGridInDirection(GridCoordinate grid, Direction direction)
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
            return coord.X >= 0 && coord.Y >= 0 && coord.X < innerModules.GetLength(0) && coord.Y < innerModules.GetLength(1);
        }

    }
}