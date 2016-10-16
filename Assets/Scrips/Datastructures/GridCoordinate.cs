using System;
using Assets.Scrips.Modules;
using Assets.Scrips.Util;
using UnityEngine;

namespace Assets.Scrips.Datastructures
{
    [Serializable]
    public class GridCoordinate
    {
        public readonly int X;
        public readonly int Y;

        public GridCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector3 GridToPosition(GridCoordinate grid)
        {
            return new Vector3(grid.X*GlobalConstants.TileSizeInMeters, grid.Y*GlobalConstants.TileSizeInMeters);
        }

        public static GridCoordinate operator +(GridCoordinate first, GridCoordinate second)
        {
            return new GridCoordinate(first.X + second.X, first.Y + second.Y);
        }

        public static bool operator ==(GridCoordinate a, GridCoordinate b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return Equals(a, b);
        }

        public static bool operator !=(GridCoordinate a, GridCoordinate b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var otherGrid = obj as GridCoordinate;
            return otherGrid != null && Equals(this, otherGrid);
        }

        private static bool Equals(GridCoordinate firstGrid, GridCoordinate otherGrid)
        {
            if (otherGrid == null)
            {
                return false;
            }
            return otherGrid.X == firstGrid.X && otherGrid.Y == firstGrid.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }
    }
}