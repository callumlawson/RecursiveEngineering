using System;
using Assets.Scrips.Networks;
using Assets.Scrips.Networks.Graph;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Components
{
    public class EngiComponent
    {
        public string Name;
        public int InternalWidth;
        public int InteralHeight;

        public EngiComponent ParentComponent;
        public EngiComponent[,] InnerComponents;

        private EngiDirectedSparseGraph<SubstanceNetworkNode> SubstanceNetwork;

        public EngiComponent() { }

        public EngiComponent(string name, EngiComponent parentComponent, int internalWidth, int interalHeight)
        {
            Name = name;
            ParentComponent = parentComponent;
            InternalWidth = internalWidth;
            InteralHeight = interalHeight;
            InnerComponents = new EngiComponent[InternalWidth, InteralHeight];
            SubstanceNetwork = new EngiDirectedSparseGraph<SubstanceNetworkNode>();
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
            return coord.X >= 0 && coord.Y >= 0 && coord.X < InternalWidth && coord.Y < InteralHeight;
        }

        public GridCoordinate GetCenterGrid()
        {
            return new GridCoordinate(Mathf.RoundToInt(InternalWidth/2.0f), Mathf.RoundToInt(InteralHeight /2.0f));
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
