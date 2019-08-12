using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sort_it
{
    class Program
    {
        static T[] MergeSort<T>(T[] massive, string sortType) where T: IComparable<T>
        {
            if (massive.Length == 1)
                return massive;
            int midPoint = massive.Length / 2;
            var leftHalf = massive.Take(midPoint).ToArray();
            var rightHalf = massive.Skip(midPoint).ToArray();
            return Merge(MergeSort(leftHalf, sortType), MergeSort(rightHalf, sortType), sortType);
        }

        static T[] Merge<T>(T[] mass1, T[] mass2, string sortType) where T : IComparable<T>
        {
            int a = 0, b = 0;
            
            T[] mergedMass = new T[mass1.Length + mass2.Length];
            for (int i = 0; i < mass1.Length + mass2.Length; i++)
            {
                try
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0)
                        if ((int.Parse(mass1[a].ToString()) < int.Parse(mass2[b].ToString()) && (sortType == "-d")) || (!(int.Parse(mass1[a].ToString()) < int.Parse(mass2[b].ToString())) && !(sortType == "-d")))
                        {
                            mergedMass[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}",i);
                        }
                        else
                        {
                            mergedMass[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                    else
                        if (b < mass2.Length)
                        {
                            mergedMass[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            mergedMass[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                }
                catch
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0) 
                        if (((mass1[a].CompareTo(mass2[b]) < 0) && (sortType == "-d")) || (!(mass1[a].CompareTo(mass2[b]) < 0) && !(sortType == "-d")))
                        {
                            mergedMass[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            mergedMass[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                    else
                        if (b < mass2.Length)
                        {
                            mergedMass[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            mergedMass[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                }
            }
            return mergedMass;
        }

        //Если файл слишком большой по размеру, то он разбивается на равные части, которые сортируются отдельно
        static void SplitFile(string pathFile, long fileLength, long partSize, string sortType)
        {
            var totalRead = 0;
            using (var fs = new FileStream(pathFile, FileMode.Open, FileAccess.ReadWrite))
            {
                while (totalRead < fileLength)
                {
                    var part = new byte[fileLength - totalRead < partSize ? fileLength - totalRead : partSize];

                    totalRead += fs.Read(part, 0, part.Length);
                    using (var fsNew = new FileStream((Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "temp.txt"), FileMode.Append))
                    {
                        fsNew.Write(part, 0, part.Length);
                    }
                    SortFile(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "temp.txt", sortType);
                }
                using (var srIn = new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "temp.txt"))
                {
                    string line;
                    using (var swOut = new StreamWriter(fs))
                    {
                        while ((line = srIn.ReadLine()) != null)
                            swOut.WriteLine(line);
                    }
                }
                File.Delete(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "temp.txt");
            }
        }

        //Сортировка по возрастанию/убыванию
        static void SortFile(string pathFile, string sortType)
        {
            if (sortType == "-a")
                File.WriteAllLines(pathFile, MergeSort(File.ReadAllLines(pathFile, Encoding.UTF8).ToArray(), "-a"));
            else if (sortType == "-d")
                File.WriteAllLines(pathFile, MergeSort(File.ReadAllLines(pathFile, Encoding.UTF8).ToArray(), "-d"));
        }

        static void DefineTypeOfData(bool isTyped, string[] args, int iter)
        {
             if (isTyped)
                 throw new Exception("You must specify only one parameter designating the data type.");
             isTyped = true;
             if (args[iter] == "-s")
             {
                 Console.WriteLine("TYPEOFDATA: string");
             }
             else
                 Console.WriteLine("TYPEOFDATA: integer");
        }

        static void DefineTypeOfSort(bool isSortTyped, string sortType, string[] args, int iter)
        {
            if (isSortTyped)
                throw new Exception("It is necessary to specify only one parameter with the designation of the sorting type or not to specify at all (by default, sorting is ascending)");

            isSortTyped = true;
            Console.WriteLine("TYPEOFSORT: Ascending");
            if (args[iter] == "-d")
            {
                Console.WriteLine("TYPEOFSORT: Descending");
                sortType = "-d";
            }
        }

        static void Main(string[] args)
        {
            bool isTyped = false;
            bool isSortTyped = false;
            bool isOut = false;
            bool isIn = false;

            string sortType = "-a";
            string outFile = null;

            FileStream fsOut = null;
            StreamWriter swOut = null;

            try
            {
                if (args.Length < 3)
                    throw new Exception("Insufficient arguments (at least 3: data type, output file, input file)");
                Console.WriteLine();

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-s" || args[i] == "-i")
                    {
                        DefineTypeOfData(isTyped, args, i);
                        continue;
                    }
                    if (args[i] == "-a" || args[i] == "-d")
                    {
                        DefineTypeOfSort(isSortTyped, sortType, args, i);
                        continue;
                    }
                    if (!isOut)
                    {
                        string[] argWords = args[i].Split('.');
                        outFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + args[i].ToString();
                        fsOut = new FileStream(outFile, FileMode.Append);
                        swOut = new StreamWriter(fsOut);
                        isOut = true;
                        Console.WriteLine("OUTFILE: " + args[i]);
                        Console.WriteLine("OUTFILE PATH: " + outFile);
                        continue;
                    }
                    else
                    {
                        string inFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + args[i].ToString();
                        if (!File.Exists(inFile))
                            throw new Exception("Missing readable file(s) with given name(s)");
                        
                        long fileLength = new FileInfo(inFile).Length / 1024 /1024;
                        
                        PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                        float available = ramCounter.NextValue();
                        ramCounter.Dispose();

                        Console.WriteLine("FILE_LENGTH: " + fileLength);
                        Console.WriteLine("AVAILABLE_RAM: " + available);
                        if (fileLength > available)
                            SplitFile(inFile, fileLength, (fileLength / (fileLength / (long)(available))), sortType);
                        else
                            SortFile(inFile, sortType);
                        Console.WriteLine("INFILE: " + args[i]);
                        Console.WriteLine("INFILE PATH: " + inFile);
                        isIn = true;
                        StreamReader srIn = new StreamReader(inFile);
                        string line;
                        while ((line = srIn.ReadLine()) != null)
                        {
                            swOut.WriteLine(line);
                        }
                        srIn.Close();
                        continue;
                    }
                    throw new Exception("Invalid parameter(s)");
                }
                if (!isIn)
                    throw new Exception("Missing file(s) read");
                if (!isSortTyped)
                    Console.WriteLine("TYPEOFSORT: Descending");
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
            finally
            {
                swOut.Close();
                fsOut.Close();
                SortFile(outFile, sortType);
                Console.WriteLine("Task competed");
            }
        }
    }
}