using System;
using System.Collections.Generic;
using System.Text;

namespace ReportTool
{
    class csvObject
    {
        public string sector { get; set; }
        public int latency { get; set; }
        public double downstream { get; set; }
        public double upstream { get; set; }
        public int activeSubs { get; set; }
        public int totalSubs { get; set; }
        public double avgThroughput { get; set; }

        public csvObject(string sector, int latency, double downstream, 
                        double upstream, int activeSubs, int totalSubs) 
        {
            this.sector = sector;
            this.latency = latency;
            this.downstream = downstream;
            this.upstream = upstream;
            this.activeSubs = activeSubs;
            this.totalSubs = totalSubs;

            getAvgThroughput();
        }

        private void getAvgThroughput()
        {
            avgThroughput = (downstream + upstream) / activeSubs;
        }


    }
}
