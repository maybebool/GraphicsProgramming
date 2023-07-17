namespace SAE._5300S1
{
    public static class Calculate
    {
        public static float DegreesToRadians(float degrees)
        {
            return MathF.PI / 180f * degrees;
        }
        
        public static float DegreesToRadiansOnVariable(this float degrees)
        {
            return MathF.PI / 180f * degrees;
        }
    }
}