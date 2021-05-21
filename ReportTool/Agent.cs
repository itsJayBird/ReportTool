using System;
using System.Collections.Generic;
using System.Text;

namespace ReportTool
{
    class Agent
    {
        public string name { get; set; }
        public List<string> sites { get; set; }
        public Agent() 
        {
            sites = new List<string>();
        }
        public void add(string site)
        {
            sites.Add(site);
        }
        public void addName(string name)
        {
            this.name = name;
        }
    }
}
