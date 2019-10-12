using MMQuickFit.src;
using System;
using System.Collections.Generic;

namespace MMQuickFit
{
    class Program
    {
        public static long MemorySize = 10 * Utils.IntPow(2, 10);
        public static long FrameSize = 1 * Utils.IntPow(2, 10);        

        static void Main(string[] args)
        {
            Memory memory = new Memory(MemorySize, FrameSize);

            String Buffer = Utils.ReadInputFile("../../../Inputs/data.csv");
            List<Process> processesList = Utils.CsvToProcessList(Buffer);

            memory.InitializeMemory(processesList);
            memory.PrintMemory();

            Console.WriteLine();

            /*Process process1 = new Process("D", FrameSize * 2, 1024);

            memory.insertProcess(2 ,process1);
            memory.PrintMemory();*/

            Console.ReadKey();

            //File.WriteAllText(@outputPath, outputText);
        }
    }
}
