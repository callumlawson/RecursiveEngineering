using UnityEngine;

namespace Assets.Scrips.Modules
{
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
            return new Vector3(grid.X * ModuleUtils.TileSizeInMeters, grid.Y * ModuleUtils.TileSizeInMeters);
        }

        public static GridCoordinate operator +(GridCoordinate first, GridCoordinate second)
        {
            return new GridCoordinate(first.X + second.X, first.Y + second.Y);
        }
      
        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }
    }
}
