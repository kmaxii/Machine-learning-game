namespace DefaultNamespace
{
    public static class FloatExtensions
    {
        
        //This method was taken from: https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
        public static float Map(this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
        {
            return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
        }
    }

}