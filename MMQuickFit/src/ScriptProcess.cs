using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScriptProcess
{
    public long memoryS { get; set; }
    public int sizeRandom { get; set; }

    public long FramesSize { get; set; }

    public Dictionary<long, bool> AllRegBase { get; set; }

    public ScriptProcess(long mS , long fZ, Dictionary<long, bool> allRegB = null, int sR = 5)
	{
        memoryS = mS;
        sizeRandom = sR;
        FramesSize = fZ;
        AllRegBase = allRegB;
    }

    public List<long> GetRegBase(long framesNeeded)
    {
        List<long> validIndexes = new List<long>();
        if (AllRegBase != null)
        {
            var regBase = new Dictionary<long, bool>();
            int regBaseValid = 0;
            long firstIndexValid = -1;
            foreach (var item in AllRegBase)
            {
                if (!item.Value)
                {
                    if (firstIndexValid.Equals(-1))
                        firstIndexValid = item.Key;
                    regBaseValid++;
                }
                else
                {
                    if (regBaseValid >= framesNeeded)
                    {
                        for (long i = firstIndexValid; i <= firstIndexValid + (this.FramesSize * (regBaseValid - framesNeeded)); i += this.FramesSize)
                        {
                            var currentRegB = AllRegBase.Where(w => w.Key.Equals(i)).FirstOrDefault();
                            regBase.Add(currentRegB.Key, currentRegB.Value);
                            validIndexes.Add(currentRegB.Key);
                        }
                        firstIndexValid = -1;
                    }
                    regBaseValid = 0;
                }
            }
            if (!validIndexes.Any(w => w.Equals(firstIndexValid)))
            {
                if (regBaseValid >= framesNeeded)
                {
                    for (long i = firstIndexValid; i <= firstIndexValid + (this.FramesSize * (regBaseValid - framesNeeded)); i += this.FramesSize)
                    {
                        var currentRegB = AllRegBase.Where(w => w.Key.Equals(i)).FirstOrDefault();
                        regBase.Add(currentRegB.Key, currentRegB.Value);
                        validIndexes.Add(currentRegB.Key);
                    }
                }
            }
        }
        else
            validIndexes.Add(0);
        
        return validIndexes;
    }

    public void CreateFile(string path, string nameProcess = "P")
    {
        
        List<int> process = new List<int>(); // processo
        List<long> regBase = new List<long>(); // inicio
        List<long> regLimite = new List<long>();  // tamanho
        
        Random p = new Random();


        for (int i = 0; process.Count < this.sizeRandom; i++)
        {
            int pValue = p.Next(sizeRandom);
            if (!process.Contains(pValue))
                process.Add(pValue);
        }

        for (int i = 0; regLimite.Count < sizeRandom; i++)
        {

            long regValor = p.Next( ( Convert.ToInt32(this.memoryS) / 3) );

            if (!regLimite.Contains(regValor))
                regLimite.Add(regValor);

            this.memoryS = this.memoryS - regLimite[i];
        }

        process.Sort();
        for (int i = 0; i < regLimite.Count; i++)
        {
            long framesNeeded = 0;
            framesNeeded += (regLimite[i] / this.FramesSize);
            framesNeeded = regLimite[i] % this.FramesSize > 0 ? framesNeeded + 1 : framesNeeded;

            var avaiableRegB = GetRegBase(framesNeeded);
            if (!avaiableRegB.Count.Equals(0) && AllRegBase != null)
            {
                Random randomRegB = new Random();
                long regB = randomRegB.Next(0, avaiableRegB.Count);
                regB = avaiableRegB[Convert.ToInt32(regB)];
                regBase.Add(regB);
                for (long usedFrames = 1; usedFrames <= framesNeeded; usedFrames++, regB += FramesSize)
                {
                    AllRegBase[regB] = true;
                }
            }
            else
                regBase.Add(0);
        }

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            for (int i = 0; i < process.Count; i++)
                writer.WriteLine(nameProcess + process[i] + ";" + regBase[i] + ";" + regLimite[i]);
        }
    }
}



