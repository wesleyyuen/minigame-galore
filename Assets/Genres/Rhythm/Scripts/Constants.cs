namespace Rhythm
{
    public static class Constants
    {
        public const float COLUMN_GAP = 0.2f;
        public const float BEAT_HIT_HEIGHT = 0.15f;
        public const float MAX_SEC_BEHIND_BEAT = 0.2f;
        public const int SAMPLE_SIZE = 1024;
    }

    public enum Column
    {
        Left,
        Center,
        Right
    }
}