using System;
using System.Collections.Generic;
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

                string name = splitData[0];
                int regB = Int32.Parse(splitData[1]);
                int regL = Int32.Parse(splitData[2]);

                Process _process = new Process(name, regB, regL);

                processListToReturn.Add(_process);
            }

            return processListToReturn;
        }

        public static void ProcessListToCsv(List<Frame> framesList, string outputPath = "../../../Outputs/outputData.csv")
        {
            var line = string.Empty;

            framesList = framesList.Where(w => w.Process != null).ToList();

            if (framesList == null)
                throw new Exception();

            foreach (var frame in framesList)
            {
                var process = frame.Process;
                line += process.Name + ";" + process.RegB + ";" + process.RegL + "\r\n";
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