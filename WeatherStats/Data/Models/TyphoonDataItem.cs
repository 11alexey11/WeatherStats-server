namespace WeatherStats.Data.Models
{
    public class TyphoonDataItem
    {
        [CsvCollumn(Number = 1)]
        public int InternationalNumberID { get; set; }

        [CsvCollumn(Number = 2)]
        public int Year { get; set; }

        [CsvCollumn(Number = 3)]
        public int Month { get; set; }

        [CsvCollumn(Number = 4)]
        public int Day { get; set; }

        [CsvCollumn(Number = 5)]
        public int Hour { get; set; }

        [CsvCollumn(Number = 6)]
        public string? Grade { get; set; }

        [CsvCollumn(Number = 7)]
        public string LatitudeOfTheCenter { get; set; }

        [CsvCollumn(Number = 8)]
        public string LongitudeOfTheCenter { get; set; }

        [CsvCollumn(Number = 9)]
        public string CentralPressure { get; set; }

        [CsvCollumn(Number = 10)]
        public double? MaximumSustainedWindSpeed { get; set; }

        [CsvCollumn(Number = 11)]
        public string? DirectionOfTheLongestRadiusOf50ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 12)]
        public string? TheLongeastRadiusOf50ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 13)]
        public string? TheShortestRadiusOf50ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 14)]
        public string? DirectionOfTheLongestRadiusOf30ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 15)]
        public string? TheLongeastRadiusOf30ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 16)]
        public string? TheShortestRadiusOf30ktWindsOrGreater { get; set; }

        [CsvCollumn(Number = 17)]
        public string? IndicatorOfLandfallOrPassage { get; set; }
    }
}
