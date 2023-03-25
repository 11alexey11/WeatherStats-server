using System.Collections.Generic;
using WeatherStats.Data.Models;

namespace WeatherStats.Data
{
    public class TyphoonDataProvider : ITyphoonDataProvider
    {
        private readonly string _infoFilePath;
        private readonly string _dataFilePath;

        public TyphoonDataProvider(string dataFilePath, string infoFilePath)
        {
            if (!File.Exists(dataFilePath))
                throw new FileNotFoundException("File not found", dataFilePath);

            if (!File.Exists(infoFilePath))
                throw new FileNotFoundException("File not found", infoFilePath);

            _dataFilePath = dataFilePath;
            _infoFilePath = infoFilePath;
        }

        public List<TyphoonDataItem> GetTyphoonData()
        {
            return CsvFileReader.ReadFile<TyphoonDataItem>(_dataFilePath, startRowIndex: 1);
        }

        public List<TyphoonInfoItem> GetTyphoonInfo()
        {
            return CsvFileReader.ReadFile<TyphoonInfoItem>(_infoFilePath, startRowIndex: 1);
        }
    }

    public interface ITyphoonDataProvider
    {
        List<TyphoonDataItem> GetTyphoonData();
        List<TyphoonInfoItem> GetTyphoonInfo();
    }
}
