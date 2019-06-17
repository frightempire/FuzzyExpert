using System.Collections.Generic;
using CommonLogic;
using DataProvider.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace DataProvider.Implementations
{
    public class CsvFileParser : IFileParser<List<string[]>>
    {
        public List<string[]> ParseFile(string filePath)
        {
            ExceptionAssert.FileExists(filePath);

            List<string[]> collection = new List<string[]>();
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(":");
                while (!parser.EndOfData) collection.Add(parser.ReadFields());
            }
            return collection;
        }
    }
}