using UnityEngine;

namespace Assets.Scrips.Util
{
    public struct GridCoordinate
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
            return new Vector3(grid.X * LayoutConstants.TileSizeInMeters, grid.Y * LayoutConstants.TileSizeInMeters);
        }
    }
}
