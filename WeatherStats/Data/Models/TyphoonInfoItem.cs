namespace WeatherStats.Data.Models
{
    public class TyphoonInfoItem
    {
        [CsvCollumn(Number = 1)]
        public int InternationalNumberID { get; set; }

        [CsvCollumn(Number = 2)]
        public double? TropicalCycloneNumberID { get; set; }

        [CsvCollumn(Number = 3)]
        public string FlagOfTheLastDataLine { get; set; }

        [CsvCollumn(Number = 4)]
        public int DifferenceBetweenTheTimeOfTheLastDataAndTheTimeOfTheFinalAnalysis { get; set; }

        [CsvCollumn(Number = 5)]
        public string Name { get; set; }

        [CsvCollumn(Number = 6)]
        public DateTime LatestRevision { get; set; }
    }
}
