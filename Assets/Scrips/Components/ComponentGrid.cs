using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scrips.Util;

namespace Assets.Scrips.Components
{
    public class ComponentGrid : IEnumerable<EngiComponent>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly EngiComponent[,] innerComponents;

        public ComponentGrid(int width, int height)
        {
            Width = width;
            Height = height;
            innerComponents = new EngiComponent[width, height];
        }

        public bool AddComponent(EngiComponent component, GridCoordinate coord)
        {
            if (GridIsInComponent(coord) && GridIsEmpty(coord))
            {
                innerComponents[coord.X, coord.Y] = component;
                return true;
            }
            return false;
        }

        public EngiComponent GetComponent(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return innerComponents[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public GridCoordinate GetGridForComponent(EngiComponent component)
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

        public bool GridIsFull(GridCoordinate grid)
        {
            return !GridIsEmpty(grid);
        }

        public List<EngiComponent> GetNeigbouringComponents(GridCoordinate grid)
        {
            var list = new List<EngiComponent>();
            var neighbourGrids = new List<GridCoordinate>
            {
                new GridCoordinate(grid.X - 1, grid.Y),
                new GridCoordinate(grid.X + 1, grid.Y),
                new GridCoordinate(grid.X, grid.Y - 1),
                new GridCoordinate(grid.X, grid.Y + 1)
            };
            foreach (var neighbourGrid in neighbourGrids)
            {
                if (GridIsFull(neighbourGrid))
                {
                    list.Add(GetComponent(neighbourGrid));
                }
            }
            return list;
        }

        private bool GridIsInComponent(GridCoordinate coord)
        {
            return coord.X >= 0 &&
                   coord.Y >= 0 &&
                   coord.X < innerComponents.GetLength(0) &&
                   coord.Y < innerComponents.GetLength(1);
        }

        public IEnumerator<EngiComponent> GetEnumerator()
        {
            foreach (var component in innerComponents)
            {
                if (component != null)
                {
                    yield return component;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}