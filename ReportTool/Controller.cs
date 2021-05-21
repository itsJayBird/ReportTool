using System;
using System.Collections.Generic;
using System.Text;

namespace ReportTool
{
    class Controller
    {
        private SPE spe;
        private Menu menu;
        private csvHandler handler;
        public Controller() 
        {
            spe = new SPE();
            menu = new Menu();
        }

        public void startProgram()
        {
            List<string> list = new List<string>();
            foreach(Agent agent in spe.agents)
            {
                list.Add(agent.name);
            }
            int val = menu.chooseAgent(list);

            // if correct value we create the csvHandler
            if(val > 0)
            {
                var a = spe.agents[val-1];
                handler = new csvHandler(a.sites);
                // move onto displaying the values
                handler.displayList();
                handler.saveList();
            }
            // if incorrect value we try again
            if(val == 0)
            {
                menu.incorrecInput();
                startProgram();
            }
        }


    }
}
