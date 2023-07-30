namespace SAE._5300S1
{
    public static class Calculate
    {
        
        public static float DeltaTime { get; set; }
        public static float DegreesToRadians(float degrees)
        {
            return MathF.PI / 180f * degrees;
        }
        
        public static float DegreesToRadiansOnVariable(this float degrees)
        {
            return MathF.PI / 180f * degrees;
        }
        
        public static float Rotation360(this float degrees, float speed) {
            degrees += speed * DeltaTime;
            if (degrees > 360)
                return 0;
            return degrees;
        }
        
        public static void UpdateDeltaTime(double deltaTime) {
            DeltaTime = (float)deltaTime;
        }
        
        
    }
}