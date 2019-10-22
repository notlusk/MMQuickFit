using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMQuickFit.src
{
    class Frame {
        public Process Process { get; set; }

        public long RegB { get; set; }

        public Frame(long rb) {
            RegB = rb;
        }
    }

    class Memory
    {
        public long Size { get; set; }
        public long FramesSize { get; set; }

        public long FramesQTD { get; set; }

        public List<Frame> Frames { get; set; }

        public Memory(long ms, long fs) {
            Frames = new List<Frame>();
            Size = ms;
            FramesSize = fs;
            FramesQTD = ms / fs;
        }

        public void InitializeMemory(List<Process> Processes){
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
                    return auxIndex;
            }
            return -1;
        }

        public int BestFitInsertion(Process pProcess)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            Dictionary<int, int> mapEmptyFrames = new Dictionary<int, int>();
            int emptyFrames = 0, auxIndex = 0;

            for (int i = 0; i < this.Frames.Count; ++i)
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

            if (mapEmptyFrames.Count == 0)
                return -1;

            var bestFit = mapEmptyFrames.Where(w=>w.Value >= framesNeeded).OrderBy(o => o.Value).FirstOrDefault();
            return bestFit.Key;
        }

        public int WorstFitInsertion(Process pProcess)
        {
            var framesNeeded = pProcess.RegL / this.FramesSize;
            framesNeeded = pProcess.RegL % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            Dictionary<int, int> mapEmptyFrames = new Dictionary<int, int>();
            int emptyFrames = 0, auxIndex = 0;

            for (int i = 0; i < this.Frames.Count; ++i)
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

            if (mapEmptyFrames.Count == 0)
                return -1;

            var worstFit = mapEmptyFrames.Where(w => w.Value >= framesNeeded).OrderByDescending(o => o.Value).FirstOrDefault();
            return worstFit.Key;
        }

        public void PrintMemory() {
            List<Frame> listToPrint = this.Frames;
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
