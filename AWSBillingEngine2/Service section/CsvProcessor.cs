using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace AWSBillingEngine2;

public class CsvProcessor
{
    public string FilePath { get; set; }
    public ClassMap ClassMap { get; set; }

    public static List<T> ProcessCsvFile<T>(string filePath, ClassMap classMap)
    {
        try
        {
            using var reader = new StreamReader(path: filePath);
            using var csv = new CsvReader(reader: reader, culture: CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap(map: classMap);
            var records = csv.GetRecords<T>();

            return records.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(value: "Unable to read file. " + e);
            throw;
        }
    }
}