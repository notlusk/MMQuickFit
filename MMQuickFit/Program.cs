using MMQuickFit.src;
using System;
using System.Collections.Generic;

namespace MMQuickFit
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            var allRegBase = new Dictionary<long, bool>();
            for (long memorySize = 0; memorySize < Memory.MemorySize; memorySize = memorySize + Memory.FrameSize)
            {
                allRegBase.Add(memorySize, false);
            }
            Console.WriteLine();
            var scriptProcessMemory = new ScriptProcess(Memory.MemorySize/4, Memory.FrameSize, allRegBase);
            scriptProcessMemory.CreateFile("../../../Inputs/data.csv", "M");
            var orignalMemory = new Memory();

            orignalMemory.PrintMemory();

            Console.WriteLine();
            var scriptProcess = new ScriptProcess(orignalMemory.Size/3, orignalMemory.FramesSize);
            scriptProcess.CreateFile("../../../Inputs/processos.csv");
            String Buffer = Utils.ReadInputFile("../../../Inputs/processos.csv");
            List<Process> listProcess = Utils.CsvToProcessList(Buffer);

            FirstFit(listProcess);
            BestFit(listProcess);
            WorstFit(listProcess);

            Console.WriteLine("Done!");
        }
        
        public static void FirstFit(List<Process> processList)
        {
            var firstFitMemory = new Memory(); 
            foreach (var process in processList)
            {
                firstFitMemory.InsertProcess(firstFitMemory.FirstFitInsertion(process), process);
            }

            Console.WriteLine("--------------FIRST FIT----------------\n");
            firstFitMemory.PrintMemory();

            Utils.ProcessListToCsv(firstFitMemory.Frames, "../../../Outputs/firstFitData.csv");
        }

        public static void BestFit(List<Process> processList)
        {
            Memory bestFitMemory = new Memory();

            foreach (var process in processList)
            {
                bestFitMemory.InsertProcess(bestFitMemory.BestFitInsertion(process), process);
            }

            Console.WriteLine("---------------BEST FIT----------------\n");
            bestFitMemory.PrintMemory();

            Utils.ProcessListToCsv(bestFitMemory.Frames, "../../../Outputs/bestFitData.csv");
        }

        public static void WorstFit(List<Process> processList)
        {
            Memory worstFitMemory = new Memory();

            foreach (var process in processList)
            {
                worstFitMemory.InsertProcess(worstFitMemory.WorstFitInsertion(process), process);
            }

            Console.WriteLine("--------------WORST FIT----------------\n");
            worstFitMemory.PrintMemory();

            Utils.ProcessListToCsv(worstFitMemory.Frames, "../../../Outputs/worstFitData.csv");
        }
    }
}
