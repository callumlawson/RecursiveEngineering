using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
