using System;
using System.Collections.Generic;
using System.Text;

namespace ReportTool
{
    class Menu
    {
        public Menu() { }

        public int chooseAgent(List<string> agents)
        {
            int count = 1;
            Console.WriteLine("Which agent is working?");
            foreach(string agent in agents)
            {
                Console.WriteLine($"{count}.  {agent}");
                count++;
            }
            Console.WriteLine("\nEnter Value of Agent: ");
            string input = Console.ReadLine();

            if(int.TryParse(input, out int num))
            {
                return num;
            }
            return 0;
        }

        public void incorrecInput()
        {
            Console.WriteLine("Not an acceptable value, try again");
        }
    }
}
