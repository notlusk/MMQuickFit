﻿using System;
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

            for (int i = 0; i < FramesQTD; i++)
            {               
                long memoryframeLocation = (i * Utils.IntPow(2, 10));

                Frame frameToInsert = new Frame(memoryframeLocation);

                if (mapProcess.ContainsKey(memoryframeLocation))
                    frameToInsert.Process = mapProcess[memoryframeLocation];
                else
                    frameToInsert.Process = null;

                this.Frames.Add(frameToInsert);
            }
        }

        public void PrintMemory() {
            Console.WriteLine("--------------MEMORY-------------------\n");
            foreach (var frame in this.Frames)
            {
                if(frame.Process != null)
                    Console.WriteLine(String.Format("{1} - {0}", frame.Process.Name, frame.RegB));
                else
                    Console.WriteLine(String.Format("{1} - {0}", "---", frame.RegB));
            }
            Console.WriteLine("\n---------------------------------------");
        }
    }
}