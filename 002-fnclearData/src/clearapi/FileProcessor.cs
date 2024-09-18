using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

public class FileProcessor
{
    public List<Pessoa> ProcessarCSV(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Pessoa>();
        return new List<Pessoa>(records);
    }
}
