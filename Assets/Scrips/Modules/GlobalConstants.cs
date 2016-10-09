using Assets.Scrips.Datastructures;
namespace Assets.Scrips.Modules
{
    public static class GlobalConstants
    {
        public const int TileSizeInPx = 64;
        public const int PixelsPerMeter = 100;
        public const float TileSizeInMeters = (float)TileSizeInPx/PixelsPerMeter;
        public const int MaxWidth = MediumToLargeRatio * 32;
        public const int MaxHeight = MediumToLargeRatio * 18;
        public const int MediumToLargeRatio = 7;
        public const float TickPeriodInSeconds = 0.2f;
        public const float DoubleClickTimeLimit = 0.20f;
    }
}
