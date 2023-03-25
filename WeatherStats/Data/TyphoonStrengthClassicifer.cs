namespace WeatherStats.Data
{
    public static class TyphoonStrengthClassicifer
    {
        public static TyphoonStrenthScale GetBySpeed(double speed)
        {
            if (speed >= 105)
            {
                return TyphoonStrenthScale.MostStrongest;
            } 
            else if (speed >= 85)
            {
                return TyphoonStrenthScale.VeryStrong;
            }
            else if (speed >= 64)
            {
                return TyphoonStrenthScale.Strong;
            }
            else
            {
                return TyphoonStrenthScale.None;
            }
        }
    }

    public enum TyphoonStrenthScale
    {
        None,
        Strong,
        VeryStrong,
        MostStrongest
    }
}
