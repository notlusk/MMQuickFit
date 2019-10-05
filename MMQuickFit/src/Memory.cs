using System;
using System.Collections.Generic;
using System.Text;

namespace MMQuickFit.src
{
    class Frame {
        Process Process { get; set; }

        public Frame() {
        }
    }

    class Memory
    {
        public long Size { get; set; }
        public long FramesSize { get; set; }
        public List<Frame> Frames { get; set; }

        public Memory(long ms, long fs) {
            Size = ms;
            FramesSize = fs;
        }

        public static void InitializeMemory(List<Process> Processes){

        }
    }
}