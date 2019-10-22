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

            var listProcess = new List<Process>();

            listProcess.Add(new Process("D", FrameSize * 2, 1024));
            listProcess.Add(new Process("E", FrameSize * 2, 100));
            listProcess.Add(new Process("F", FrameSize * 2, 2024));
            foreach (var item in listProcess)
            {
                //memory.InsertProcess(memory.FirstFitInsertion(item), item);
                //memory.InsertProcess(memory.BestFitInsertion(item), item);
                memory.InsertProcess(memory.WorstFitInsertion(item), item);
            }
            memory.PrintMemory();

            Console.ReadKey();

            //File.WriteAllText(@outputPath, outputText);
        }
    }
}
