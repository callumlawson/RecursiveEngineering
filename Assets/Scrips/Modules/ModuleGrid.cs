using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Components;
using Assets.Scrips.Util;

namespace Assets.Scrips.Modules
{
    public class ModuleGrid : IEnumerable<Module>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly Module[,] innerComponents;

        public ModuleGrid(int width, int height)
        {
            Width = width;
            Height = height;
            innerComponents = new Module[width, height];
        }

        public bool AddComponent(Module component, GridCoordinate coord)
        {
            if (GridIsInComponent(coord) && GridIsEmpty(coord))
            {
                innerComponents[coord.X, coord.Y] = component;
                return true;
            }
            return false;
        }

        public Module GetComponent(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return innerComponents[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public GridCoordinate GetGridForComponent(Module component)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var gridCoordinate = new GridCoordinate(x, y);
                    if (GetComponent(gridCoordinate) == component)
                    {
                        return gridCoordinate;
                    }
                }
            }
            return null;
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return innerComponents[grid.X, grid.Y] == null;
            }
            return true;
        }

        public IEnumerable<Module> GetNeigbouringComponents(GridCoordinate grid)
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

        public IEnumerator<Module> GetEnumerator()
        {
            foreach (var component in innerComponents)
            {
                if (component != null)
                {
                    yield return component;
                }
            }
        }

        private Module GetNeigbouringModule(GridCoordinate grid, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    var up = GetGridInDirection(grid, Direction.Up);
                    if (GridIsInComponent(up) && GridIsFull(up))
                    {
                        return GetComponent(up);
                    }
                    return null;
                case Direction.Down:
                    var down = GetGridInDirection(grid, Direction.Down);
                    if (GridIsInComponent(down) && GridIsFull(down))
                    {
                        return GetComponent(down);
                    }
                    return null;
                case Direction.Left:
                    var left = GetGridInDirection(grid, Direction.Left);
                    if (GridIsInComponent(left) && GridIsFull(left))
                    {
                        return GetComponent(left);
                    }
                    return null;
                case Direction.Right:
                    var right = GetGridInDirection(grid, Direction.Right);
                    if (GridIsInComponent(right) && GridIsFull(right))
                    {
                        return GetComponent(right);
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

        private bool GridIsInComponent(GridCoordinate coord)
        {
            return coord.X >= 0 && coord.Y >= 0 && coord.X < innerComponents.GetLength(0) && coord.Y < innerComponents.GetLength(1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}