using System;
using System.Collections.Generic;
using System.Text;

namespace MMQuickFit
{
    public class Process
    {
        public string Name { get; set; }
        public long RegB { get; set; }
        public long RegL { get; set; }
        public double TimeToFindIndex { get; set; }

        public Process(string n, long rb, long rl, long time = 0) {
            Name = n;
            RegB = rb;
            RegL = rl;
            TimeToFindIndex = time;
        }
    }
}
