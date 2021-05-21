using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using GemBox.Spreadsheet;

namespace ReportTool
{
    class csvHandler
    {
        /*
         * this will be a list of csv objects to pull from
         */
        public List<csvObject> data { get; set; }
        public List<string> sites { get; set; }
        public List<string> output { get; set; }
        public csvHandler(List<string> sites) 
        {
            data = new List<csvObject>();
            output = new List<string>();
            this.sites = sites;
            
            setUpList();
        }

        private void setUpList()
        {
            // this opens the report csv
            using var streamReader = File.OpenText("report.csv");
            using var reader = new CsvReader(streamReader, CultureInfo.CurrentCulture);
            while (reader.Read())
            {
                // this loop pulls all relevant data we want and adds it into a sector object
                // the obbects get added to a list
                foreach(string site in sites)
                {
                    if(reader.GetField(1).ToLower().Contains(site.ToLower()))
                    {
                        csvObject sector = new csvObject(reader.GetField(1), int.Parse(reader.GetField(2)),
                                                        double.Parse(reader.GetField(4)), double.Parse(reader.GetField(5)),
                                                        int.Parse(reader.GetField(3)), int.Parse(reader.GetField(11)));
                        data.Add(sector);
                    }
                }
            }
            sortList();
        }

        public void setSites(List<string> sites)
        {
            this.sites = sites;
        }

        private void sortList()
        {
            // first we grab all the latency and put it into a dictionary
            Dictionary<int, int> sectors = new Dictionary<int, int>();
            int count = 0;
            foreach (var sector in data)
            {
                sectors.Add(count, sector.latency);
                count++;
            }

            // that gets converted to a list to sort
            List<KeyValuePair<int, int>> list = sectors.ToList();
            //lamba of the commented out code:
            list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            
            /*
            list.Sort(delegate (KeyValuePair<int, int> pair1, KeyValuePair<int, int> pair2)
            {
                return pair1.Value.CompareTo(pair2.Value);
            });
            */
            
            // make a temporary list and reorganize the original in reverse order
            List<csvObject> tmp = new List<csvObject>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                KeyValuePair<int, int> s = list[i];
                tmp.Add(data[s.Key]);
            }
            data = tmp;
        }

        public void displayList()
        {
            string header = $"Sector\t\t|  Latency\t|\tDL\t|\tUL\t|  Active\t|  Total\t|  Avg TPUT per Customer ";
            Console.WriteLine(header);
            foreach(var site in data)
            {
                string sector = site.sector + "\t\b";
                if(sector.Length > 20)
                {
                    sector = site.sector.Substring(0, 20) + "\t";
                }
                
                string record = String.Format("{0,10}|{1,10}\t|{2,10}\t|{3,10}\t|{4,10}\t|{5,10}\t|{6,10}",
                                              sector,site.latency,site.downstream,site.upstream,site.activeSubs,site.totalSubs,site.avgThroughput.ToString("#.##"));
                Console.WriteLine(record);
            }
        }

        public void saveList()
        {
            output.Add("Sector,Latency,DL,UL,Active,Total,Avg TPUT per Customer");
            foreach(var site in data)
            {
                output.Add($"{site.sector},{site.latency},{site.downstream},{site.upstream},{site.activeSubs},{site.totalSubs},{site.avgThroughput.ToString("#.##")}");
            }

            using StreamWriter file = new StreamWriter("finalReport.csv");

            foreach(string line in output)
            {
                file.WriteLine(line);
            }
            formatSheet();
        }

        private void formatSheet()
        {
            var workbook = ExcelFile.Load("finalReport.csv");
            var worksheet = workbook.Worksheets[0];
            for (int i = 0; i < 7; i++)
            {
                var cell = worksheet.Cells[0, i];
                cell.Style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(0, 0, 0));
                cell.Style.Font.Color = SpreadsheetColor.FromArgb(255,255,255);
            }
            for (int i = 1; i < worksheet.Rows.Count; i++)
            {
                var cell = worksheet.Cells[i,1];
                double res = 0;
                try
                {
                    res = (double)cell.Value;
                }
                catch (Exception)
                {

                }
                if (res > 125)
                {
                    cell.Style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 155, 155));
                    cell.Style.Font.Color = SpreadsheetColor.FromArgb(140, 20, 20);
                }
                if (res > 75 && res <= 125)
                {
                    cell.Style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 235, 120));
                    cell.Style.Font.Color = SpreadsheetColor.FromArgb(150, 130, 20);
                }
                if (res <= 75)
                {
                    cell.Style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(115, 255, 110));
                    cell.Style.Font.Color = SpreadsheetColor.FromArgb(30, 140, 25);
                }
            }
            workbook.Save("formatted.xlsx");
        }
    }
}
