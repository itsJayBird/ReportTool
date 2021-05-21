using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ReportTool
{
    class SPE
    {
        public List<Agent> agents { get; set; }
        public SPE() 
        {
            agents = new List<Agent>();
            createList();

        }

        private void createList()
        {
            string[] lines = File.ReadAllLines("agents.txt");
            int count = 0;
            foreach (string line in lines)
            {
                if (line.Contains("#"))
                {
                    Agent agent = new Agent();
                    agent.addName(line.Substring(1, line.Length-1));
                    agents.Add(agent);
                    count++;
                }
                var cur = agents[count - 1];
                cur.add(line);
            }
        }
    }
}
