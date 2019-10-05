using System;
using System.Collections.Generic;
using System.Text;

namespace MMQuickFit
{
    public class Process
    {
        public string Name { get; set; }
        public int RegB { get; set; }
        public int RegL { get; set; }

        public Process(string n, int rb, int rl) {
            Name = n;
            RegB = rb;
            RegL = rl;
        }
    }
}
