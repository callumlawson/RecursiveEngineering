using System;
using System.Collections.Generic;
using Assets.Scrips.Util;

namespace Assets.Scrips.Components
{
    public class ComponentGrid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private readonly EngiComponent[,] InnerComponents;

        public ComponentGrid(int width, int height)
        {
            Width = width;
            Height = height;
            InnerComponents = new EngiComponent[width, height];
        }

        public bool AddComponent(EngiComponent component, GridCoordinate coord)
        {
            if (GridIsEmpty(coord))
            {
                InnerComponents[coord.X, coord.Y] = component;
                return true;
            }
            return false;
        }

        public EngiComponent GetComponent(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return InnerComponents[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return InnerComponents[grid.X, grid.Y] == null;
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
                   coord.X < InnerComponents.GetLength(0) &&
                   coord.Y < InnerComponents.GetLength(1);
        }
    }
}