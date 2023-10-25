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
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap(classMap);
            var records = csv.GetRecords<T>();

            return records.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine("Unable to read file. " + e);
            throw;
        }
    }
}