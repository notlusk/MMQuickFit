using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MMQuickFit.src
{
    class Utils
    {
        public static string ReadInputFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Process> CsvToProcessList(string inputFile)
        {
            var processListToReturn = new List<Process>();
            var processFound = inputFile.Split('\n');

            if (processFound == null)
                throw new Exception();

            foreach (string line in processFound)
            {
                var splitData = line.Split(';');
                if(splitData.Length == 3)
                {
                    string name = splitData[0];
                    int regB = Int32.Parse(splitData[1]);
                    int regL = Int32.Parse(splitData[2]);

                    Process _process = new Process(name, regB, regL);
                    if(!processListToReturn.Any(a=>a.Name.Equals(_process.Name)))
                        processListToReturn.Add(_process);
                }
            }

            return processListToReturn;
        }

        public static void AddTimeProcessTaked(Stopwatch stopwatch, string name,string outputPath = "../../../Outputs/timeTaked")
        {
            var line = name + " = " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds.";
            File.WriteAllText(@outputPath + name + ".txt" , line);
        }

        public static void ProcessListToCsv(List<Frame> framesList, string outputPath = "../../../Outputs/outputData.csv")
        {
            var line = string.Empty;

            if (framesList == null)
                throw new Exception();

            var auxFrameName = string.Empty;
            long auxFrameRegL = 0;
            foreach (var frame in framesList)
            {
                if (frame.Process != null)
                {
                    auxFrameRegL = auxFrameName.Equals(frame.Process.Name) ? auxFrameRegL : 0;
                    var regL = frame.Process.RegL;
                    if (auxFrameName.Equals(frame.Process.Name))
                    {
                        regL = frame.Process.RegL - auxFrameRegL;

                        if (frame.Process.RegL > Memory.FrameSize && regL > Memory.FrameSize)
                            regL = Memory.FrameSize;
                    }
                    else
                    {
                        if (frame.Process.RegL > Memory.FrameSize)
                            regL = Memory.FrameSize;
                    }
                    line += frame.Process.Name + ";" + frame.RegB + ";" + regL + ";" + frame.Process.TimeToFindIndex + "\r\n";

                    auxFrameName = frame.Process.Name;
                    auxFrameRegL += regL;
                }
                else
                    line += "L" + ";" + frame.RegB + ";" + 0 + ";" + 0 + "\r\n";
            }
            File.WriteAllText(@outputPath, line);
        }

        public static int IntPow(int x, uint pow)
        {
            int ret = 1;

            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }

            return ret;
        }
    }
}