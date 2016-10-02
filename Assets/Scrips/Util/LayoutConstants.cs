namespace Assets.Scrips.Util
{
    public static class LayoutConstants
    {
        public const int TileSizeInPx = 64;
        public const int PixelsPerMeter = 100;
        public const float TileSizeInMeters = (float)TileSizeInPx/PixelsPerMeter;
        public const int MaxWidth = MediumToLargeRatio * 32;
        public const int MaxHeight = MediumToLargeRatio * 18;
        public const int MediumToLargeRatio = 7;
    }
}
