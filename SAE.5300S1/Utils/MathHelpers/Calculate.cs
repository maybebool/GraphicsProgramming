namespace SAE._5300S1.Utils.MathHelpers {
    public static class Calculate {
        
        public static float DegreesToRadians(float degrees) {
            return MathF.PI / 180f * degrees;
        }

        public static float DegreesToRadiansOnVariable(this float degrees) {
            return MathF.PI / 180f * degrees;
        }

        public static float Rotation360(this float degrees, float speed) {
            degrees += speed * Time.DeltaTime;
            if (degrees > 360)
                return 0;
            return degrees;
        }
    }
}