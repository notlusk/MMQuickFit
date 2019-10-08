using System;
using System.Collections.Generic;
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

                    if (processToInsertMemory.RegL > this.FrameSize)
                    {
                        decimal framesNeeded = Math.Ceiling((decimal) processToInsertMemory.RegL / this.FrameSize);

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
        
        /*public void insertProcess(int index, Process process)
        {           
            Frame frame = this.Frames[index];
            int framesNeeded = process.RegL / this.FramesSize;
            
            try
            {
                if(frame.Process != null)
                    throw new Exception("Não é possível inserir o processo, pois esse local da mémoria já está sendo utilizado!");
                else
                {
                    if(framesNeeded == 1)
                        frame.Process = process;
                    else
                    {
                        for(int i = 0; i < framesNeeded; i++)
                        {
                            if(this.Frames[index + i].Process != null)
                                throw new Exception("Não é possível inserir o processo, pois esse local da mémoria já está sendo utilizado!");
                            else
                            {
                                frame = this.Frames[index + i];
                                frame.Process = process;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.getMessage());
            }
        }*/
        
        /*public void FirstFitInsertion(Process pProcess){
            int framesNeeded = pProcess.RegL / this.FramesSize;
            
            for(int i = this.Frames.Count; i > 0; --i){
                int k = i;
                int frames
                
                while(){
                    if(this.Frames[k].Process != null)
                        
                       
                    k++;
                }       
                
                if()
            }
        }*/

        public void PrintMemory() {
            List<Frame> listToPrint = this.Frames;
            listToPrint.Reverse();

            Console.WriteLine("--------------MEMORY-------------------\n");
            foreach (var frame in listToPrint)
            {
                if(frame.Process != null)
                    Console.WriteLine(String.Format("{1} - {2} => {0}", frame.Process.Name, frame.RegB, frame.RegB + this.FrameSize));
                else
                    Console.WriteLine(String.Format("{1} - {2} => {0}", "---", frame.RegB, frame.RegB + this.FrameSize));
            }
            Console.WriteLine("\n---------------------------------------\n\n\n");
        }
    }
}
