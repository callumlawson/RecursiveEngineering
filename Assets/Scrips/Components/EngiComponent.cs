using System;
using Assets.Scrips.Networks;
using Assets.Scrips.Util;

namespace Assets.Scrips.Components
{
    public class EngiComponent
    {
        public string Name;
        public int Width;
        public int Height;
        public EngiComponent[,] InnerComponents;

        private SubstanceNetwork SubstanceNetwork;

        public EngiComponent() { }

        public EngiComponent(string name, int width, int height)
        {
            Width = width;
            Height = height;
            Name = name;
            InnerComponents = new EngiComponent[Width, Height];
            SubstanceNetwork = new SubstanceNetwork();
        }

        public void AddComponent(EngiComponent component, GridCoordinate coord)
        {
            if (InnerComponents[coord.X, coord.Y] == null)
            {
                InnerComponents[coord.X, coord.Y] = component;
            }
        }

        public EngiComponent GetComponent(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return InnerComponents[grid.X, grid.Y];
            }
            throw new ArgumentOutOfRangeException("grid");
        }

        public bool GridIsInComponent(GridCoordinate coord)
        {
            return coord.X >= 0 && coord.Y >= 0 && coord.X < Width && coord.Y < Height;
        }

        public bool GridIsEmpty(GridCoordinate grid)
        {
            if (GridIsInComponent(grid))
            {
                return InnerComponents[grid.X, grid.Y] == null;
            }
            throw new ArgumentOutOfRangeException("gridCoord");
        }
    }
}
