namespace Assets.Scrips.Util
{
    public static class GlobalConstants
    {
        public const int TileSizeInPx = 64;
        public const int PixelsPerMeter = 100;
        public const float TileSizeInMeters = (float)TileSizeInPx/PixelsPerMeter;
        public const int MaxWidth = 28;
        public const int MaxHeight = 13;
        public const int MediumToLargeRatio = 7;
        public const float TickPeriodInSeconds = 0.2f;
        public const float DoubleClickTimeLimit = 0.20f;
    }
}
