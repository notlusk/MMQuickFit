using MMQuickFit.src;
using System;
using System.Collections.Generic;

namespace MMQuickFit
{
    class Program
    {
        public static long MemorySize = 10 * Utils.IntPow(2, 10); //4,096 bytes
        public static long FrameSize = 1 * Utils.IntPow(2, 10); //1024 bytes        

        static void Main(string[] args)
        {
            Memory memory = new Memory(MemorySize, FrameSize);

            String Buffer = Utils.ReadInputFile("../../../Inputs/data.csv");
            List<Process> processesList = Utils.CsvToProcessList(Buffer);

            memory.InitializeMemory(processesList);
            memory.PrintMemory();

            Console.ReadKey();

            //File.WriteAllText(@outputPath, outputText);
        }
    }
}
