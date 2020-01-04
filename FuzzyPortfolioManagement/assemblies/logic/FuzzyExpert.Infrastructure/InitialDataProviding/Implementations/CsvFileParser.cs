using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using Microsoft.VisualBasic.FileIO;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Implementations
{
    public class CsvFileParser : IFileParser<List<string[]>>
    {
        public List<string[]> ParseFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(nameof(filePath));

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