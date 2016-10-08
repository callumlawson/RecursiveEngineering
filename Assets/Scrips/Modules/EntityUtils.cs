using Assets.Scrips.Datatypes;
using Assets.Scrips.Entities;

namespace Assets.Scrips.Modules
{
    public static class EntityUtils
    {
        public const int TileSizeInPx = 64;
        public const int PixelsPerMeter = 100;
        public const float TileSizeInMeters = (float)TileSizeInPx/PixelsPerMeter;
        public const int MaxWidth = MediumToLargeRatio * 32;
        public const int MaxHeight = MediumToLargeRatio * 18;
        public const int MediumToLargeRatio = 7;

        public static GridCoordinate GetGridOffset(Entity entity)
        {
            return new GridCoordinate(0, 0);
//            var moduleGridPosition = entity.GetGridPosition();
//            return new GridCoordinate(
//                moduleGridPosition.X * MediumToLargeRatio,
//                moduleGridPosition.Y * MediumToLargeRatio
//            );
        }

    }
}
