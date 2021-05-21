using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using GemBox.Spreadsheet;

namespace ReportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            Controller c = new Controller();
            c.startProgram();
        }
    }
}
