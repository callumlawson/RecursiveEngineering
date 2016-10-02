using Assets.Scrips.Util;

namespace Assets.Scrips.Modules
{
    public static class ModuleUtils
    {
        public const int TileSizeInPx = 64;
        public const int PixelsPerMeter = 100;
        public const float TileSizeInMeters = (float)TileSizeInPx/PixelsPerMeter;
        public const int MaxWidth = MediumToLargeRatio * 32;
        public const int MaxHeight = MediumToLargeRatio * 18;
        public const int MediumToLargeRatio = 7;

        public static GridCoordinate GetGridOffset(Module module)
        {
            var moduleGridPosition = module.GetGridPosition();
            return new GridCoordinate(
                moduleGridPosition.X * ModuleUtils.MediumToLargeRatio,
                moduleGridPosition.Y * ModuleUtils.MediumToLargeRatio
            );
        }

    }
}
