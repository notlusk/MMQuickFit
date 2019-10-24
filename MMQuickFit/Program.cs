using MMQuickFit.src;
using System;
using System.Collections.Generic;

namespace MMQuickFit
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            var orignalMemory = new Memory();

            orignalMemory.PrintMemory();

            Console.WriteLine();

            var listProcess = new List<Process>();

            listProcess.Add(new Process("D", -1, 1024));
            listProcess.Add(new Process("E", -1, 100));
            listProcess.Add(new Process("F", -1, 2024));

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
