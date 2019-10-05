using MMQuickFit.src;
using System;
using System.Collections.Generic;

namespace MMQuickFit
{
    class Program
    {
        public const long MemorySize = (4 * (2 ^ 10) * 8); //32768 bits
        public const long FrameSize = (1 * (2 ^ 10) * 8); //8192 bits        

        static void Main(string[] args)
        {
            Memory memory = new Memory(MemorySize, FrameSize);

            String location = System.IO.Directory.GetCurrentDirectory();
            String buffer = Utils.ReadInputFile("../../../Inputs/data.csv");
            List<Process> processesList = Utils.CsvToProcessList(buffer);

            Console.ReadKey();

            //File.WriteAllText(@outputPath, outputText);
        }
    }
}
