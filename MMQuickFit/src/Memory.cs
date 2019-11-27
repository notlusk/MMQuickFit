using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMQuickFit.src
{
    public class Frame {
        public Process Process { get; set; }

        public long RegB { get; set; }

        public Frame(long rb) {
            RegB = rb;
        }
    }

    public class Memory
    {
        public static long MemorySize = 2500 * Utils.IntPow(2, 10);
        public static long FrameSize = 1 * Utils.IntPow(2, 10);

        private Memory GetOriginalMemory 
        {
            get
            {
                Memory originalMemory = new Memory(MemorySize, FrameSize);

                String Buffer = Utils.ReadInputFile("../../../Inputs/data.csv");
                List<Process> processesList = Utils.CsvToProcessList(Buffer);
                originalMemory.InitializeMemory(processesList);

                return originalMemory;
            }
        }

        public long Size { get; set; }
        public long FramesSize { get; set; }

        public long FramesQTD { get; set; }

        public List<Frame> Frames { get; set; }

        public Memory()
        {
            Frames = GetOriginalMemory.Frames;
            Size = GetOriginalMemory.Size;
            FramesSize = GetOriginalMemory.FramesSize;
            FramesQTD = GetOriginalMemory.FramesQTD;
        }

        public Memory(long ms, long fs) {
            Frames = new List<Frame>();
            Size = ms;
            FramesSize = fs;
            FramesQTD = ms / fs;
        }

        private void InitializeMemory(List<Process> Processes){
            Dictionary<long, Process> mapProcess = new Dictionary<long, Process>();

            foreach (var forProcess in Processes)
            {
                mapProcess.Add(forProcess.RegB, forProcess);
            }

            int i = 0;

            while(i < FramesQTD)
            {
                long memoryframeLocation = (i * Utils.IntPow(2, 10));

                if (mapProcess.ContainsKey(memoryframeLocation))
                {
                    Process processToInsertMemory = mapProcess[memoryframeLocation];

                    if (processToInsertMemory.RegL > this.FramesSize)
                    {
                        decimal framesNeeded = Math.Ceiling((decimal) processToInsertMemory.RegL / this.FramesSize);

                        for (int i2 = 0; i2 < framesNeeded; i2++)
                        {
                            memoryframeLocation = (i * Utils.IntPow(2, 10));

                            Frame frameToInsert = new Frame(memoryframeLocation);
                            frameToInsert.Process = processToInsertMemory;

                            this.Frames.Add(frameToInsert);

                            i++;
                        }
                    }
                    else
                    {
                        Frame frameToInsert = new Frame(memoryframeLocation);
                        frameToInsert.Process = processToInsertMemory;

                        this.Frames.Add(frameToInsert);
                        i++;
                    }
                }
                else
                {
                    Frame frameToInsert = new Frame(memoryframeLocation);
                    frameToInsert.Process = null;

                    this.Frames.Add(frameToInsert);
                    i++;
                }                
            }
        }
        
        public void InsertProcess(int index, Process process)
        {
            var framesNeeded = process.RegL / this.FramesSize;
            framesNeeded = process.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;
            Frame frame;

            try
            {
                if(this.Frames[index].Process != null)
                    throw new Exception("Não é possível inserir o processo, pois esse local da mémoria já está sendo utilizado!");
                else
                {
                    if(framesNeeded == 1)
                    {
                        frame = this.Frames[index];
                        process.RegB = frame.RegB;
                        if(frame.Process == null)
                            frame.Process = process;
                        else
                            throw new Exception("Não é possível inserir o processo, pois esse local da mémoria já está sendo utilizado!");
                    }                        
                    else
                    {
                        for(int i = 0; i < framesNeeded; i++)
                        {
                            if (this.Frames[index + i].Process != null)
                                throw new Exception("Não é possível inserir o processo, pois esse local da mémoria já está sendo utilizado!");
                            else
                            {
                                frame = this.Frames[index + i];
                                process.RegB = frame.RegB;
                                frame.Process = process;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int FirstFitInsertion(Process pProcess)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            int indexToReturn = 0, auxIndex = 0;

            for (int i = 0; i < this.Frames.Count; ++i)
            {
                if (this.Frames[i].Process != null)
                    indexToReturn = 0;
                else
                {
                    if (indexToReturn == 0)
                        auxIndex = i;
                    indexToReturn++;
                }
                if (indexToReturn == framesNeeded)
                {
                    //pProcess.TimeToFindIndex = i;
                    return auxIndex;
                }
            }
            return -1;
        }

        public int BestFitInsertion(Process pProcess)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            Dictionary<int, int> mapEmptyFrames = new Dictionary<int, int>();
            int emptyFrames = 0, auxIndex = 0;
            int i = 0;
            for (i = 0; i < this.Frames.Count; ++i)
            { 
                if (this.Frames[i].Process != null)
                    emptyFrames = 0;
                else
                {
                    if (emptyFrames == 0)
                        auxIndex = i;
                    emptyFrames++;
                }
                if ((i == this.Frames.Count - 1 || this.Frames[i + 1].Process != null) && emptyFrames > 0)
                {
                    mapEmptyFrames.Add(auxIndex, emptyFrames);
                    if (emptyFrames == framesNeeded)
                    {
                        pProcess.TimeToFindIndex = i;
                        return auxIndex;
                    }
                }
            }
            if (mapEmptyFrames.Count(w => w.Value >= framesNeeded) == 0)
                return -1;

            var bestFit = mapEmptyFrames.Where(w=>w.Value >= framesNeeded).OrderBy(o => o.Value).FirstOrDefault();
            pProcess.TimeToFindIndex = i; 
            return bestFit.Key;
        }

        public int WorstFitInsertion(Process pProcess)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            Dictionary<int, int> mapEmptyFrames = new Dictionary<int, int>();
            int emptyFrames = 0, auxIndex = 0;
            int i = 0;
            for (i = 0; i < this.Frames.Count; ++i)
            {
                if (this.Frames[i].Process != null)
                    emptyFrames = 0;
                else
                {
                    if (emptyFrames == 0)
                        auxIndex = i;
                    emptyFrames++;
                }
                if ((i == this.Frames.Count - 1 || this.Frames[i + 1].Process != null) && emptyFrames > 0)
                    mapEmptyFrames.Add(auxIndex, emptyFrames);
            }

            if (mapEmptyFrames.Count(w => w.Value >= framesNeeded) == 0)
                return -1;

            var worstFit = mapEmptyFrames.Where(w => w.Value >= framesNeeded).OrderByDescending(o => o.Value).FirstOrDefault();
            pProcess.TimeToFindIndex = i;
            return worstFit.Key;
        }

        public int QuickFitInsertion(Process pProcess, List<KeyValuePair<long, long>> mappedMemory, Dictionary<long, bool> allRegB)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            if (!mappedMemory.Any(a => a.Value >= framesNeeded))
                return -1;

            var placeToInsert = mappedMemory.Where(w => w.Value >= framesNeeded).FirstOrDefault().Key;
            var auxRegb = placeToInsert;

            for (long usedFrames = 1; usedFrames <= framesNeeded; usedFrames++, auxRegb += FramesSize)
                allRegB[auxRegb] = true;

            pProcess.TimeToFindIndex = 0;
            return Convert.ToInt32(placeToInsert);
        }

        public void PrintMemory() {
            List<Frame> listToPrint = new List<Frame>();
            listToPrint.AddRange(this.Frames);
            //listToPrint.Reverse();

            Console.WriteLine("--------------MEMORY-------------------\n");
            foreach (var frame in listToPrint)
            {
                if(frame.Process != null)
                    Console.WriteLine(String.Format("{1} - {2} => {0}", frame.Process.Name, frame.RegB, frame.RegB + this.FramesSize));
                else
                    Console.WriteLine(String.Format("{1} - {2} => {0}", "---", frame.RegB, frame.RegB + this.FramesSize));
            }
            Console.WriteLine("\n---------------------------------------\n\n\n");
        }
    }
}
