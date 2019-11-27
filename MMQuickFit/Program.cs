using MMQuickFit.src;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MMQuickFit
{
    public class Program
    {
       public static Dictionary<long, bool> allRegBase = null;
        public static void Main(string[] args)
        {
            allRegBase = new Dictionary<long, bool>();
            for (long memorySize = 0; memorySize < Memory.MemorySize; memorySize = memorySize + Memory.FrameSize)
            {
                allRegBase.Add(memorySize, false);
            }
            Console.WriteLine();
            var scriptProcessMemory = new ScriptProcess(Memory.MemorySize, Memory.FrameSize, allRegBase);
            scriptProcessMemory.CreateFile("../../../Inputs/data.csv", "M");
            var orignalMemory = new Memory();

            orignalMemory.PrintMemory();

            Console.WriteLine();
            var scriptProcess = new ScriptProcess(orignalMemory.Size, orignalMemory.FramesSize, null, 500);
            scriptProcess.CreateFile("../../../Inputs/processos.csv");
            String Buffer = Utils.ReadInputFile("../../../Inputs/processos.csv");
            List<Process> listProcess = Utils.CsvToProcessList(Buffer);

            FirstFit(listProcess);
            BestFit(listProcess);
            WorstFit(listProcess);
            QuickFit(listProcess);

            Console.WriteLine("Done!");
        }
        
        public static void FirstFit(List<Process> processList)
        {
            
            var firstFitMemory = new Memory(); 
            foreach (var process in processList)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var index = firstFitMemory.FirstFitInsertion(process);
                process.TimeToFindIndex = stopwatch.Elapsed.TotalMilliseconds;
                firstFitMemory.InsertProcess(index, process);
                stopwatch.Stop();
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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var index = bestFitMemory.BestFitInsertion(process);
                process.TimeToFindIndex = stopwatch.Elapsed.TotalMilliseconds;
                bestFitMemory.InsertProcess(index, process);
                stopwatch.Stop();
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
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var index = worstFitMemory.WorstFitInsertion(process);
                process.TimeToFindIndex = stopwatch.Elapsed.TotalMilliseconds;
                worstFitMemory.InsertProcess(index, process);
                stopwatch.Stop();
            }

            Console.WriteLine("--------------WORST FIT----------------\n");
            worstFitMemory.PrintMemory();

            Utils.ProcessListToCsv(worstFitMemory.Frames, "../../../Outputs/worstFitData.csv");
        }

        public static void QuickFit(List<Process> processList)
        {
            Memory quickFitMemory = new Memory();
            var thisMemoryMappedRegB = new Dictionary<long, bool>();
            thisMemoryMappedRegB = allRegBase;
            foreach (var process in processList)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var mappedRoles = new Dictionary<long, long>(); //RegBase, EmptyValues

                long firstIndex = -1;
                int emptyRoles = 0;
                foreach (var regb in thisMemoryMappedRegB)
                {
                    if (!regb.Value)
                    {
                        if(firstIndex.Equals(-1))
                            firstIndex = regb.Key;
                        emptyRoles++;
                    }
                    else
                    {
                        if (!firstIndex.Equals(-1))
                            mappedRoles.Add(firstIndex, emptyRoles);
                        emptyRoles = 0;
                        firstIndex = -1;
                    }
                }
                var listOfMappedRoles = mappedRoles.OrderBy(o => o.Value).ToList();
                var regBaseToInsert = quickFitMemory.QuickFitInsertion(process, listOfMappedRoles, thisMemoryMappedRegB);
                var frameToInsert = quickFitMemory.Frames.Where(w => w.RegB == regBaseToInsert).FirstOrDefault();
                var index = quickFitMemory.Frames.IndexOf(frameToInsert);
                process.TimeToFindIndex = stopwatch.Elapsed.TotalMilliseconds;
                quickFitMemory.InsertProcess(index, process);
                stopwatch.Stop();
            }

            Console.WriteLine("--------------QUICK FIT----------------\n");
            quickFitMemory.PrintMemory();

            Utils.ProcessListToCsv(quickFitMemory.Frames, "../../../Outputs/quickFitData.csv");
        }
    }
}
