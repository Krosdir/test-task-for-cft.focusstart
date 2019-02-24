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
            return Merge(MergeSort(massive.Take(midPoint).ToArray(), sortType), MergeSort(massive.Skip(midPoint).ToArray(), sortType), sortType);
        }

        static T[] Merge<T>(T[] mass1, T[] mass2, string sortType) where T : IComparable<T>
        {
            int a = 0, b = 0;
            
            T[] merged = new T[mass1.Length + mass2.Length];
            for (int i = 0; i < mass1.Length + mass2.Length; i++)
            {
                try
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0)
                        if ((int.Parse(mass1[a].ToString()) < int.Parse(mass2[b].ToString()) && (sortType == "-d")) || (!(int.Parse(mass1[a].ToString()) < int.Parse(mass2[b].ToString())) && !(sortType == "-d")))
                        {
                            merged[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}",i);
                        }
                        else
                        {
                            merged[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                    else
                        if (b < mass2.Length)
                        {
                            merged[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            merged[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                }
                catch
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0) 
                        if (((mass1[a].CompareTo(mass2[b]) < 0) && (sortType == "-d")) || (!(mass1[a].CompareTo(mass2[b]) < 0) && !(sortType == "-d")))
                        {
                            merged[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            merged[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                    else
                        if (b < mass2.Length)
                        {
                            merged[i] = mass2[b++];
                            Console.WriteLine("changed from b massiv, line {0}", i);
                        }
                        else
                        {
                            merged[i] = mass1[a++];
                            Console.WriteLine("changed from a massiv, line {0}", i);
                        }
                }
            }
            return merged;
        }

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
                        {
                            swOut.WriteLine(line);
                        }
                    }
                }
                File.Delete(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + "temp.txt");
            }
        }

        static void SortFile(string pathFile, string sortType)
        {
            if (sortType == "-a")
                File.WriteAllLines(pathFile, MergeSort(File.ReadAllLines(pathFile, Encoding.UTF8).ToArray(), "-a"));
            else if (sortType == "-d")
                File.WriteAllLines(pathFile, MergeSort(File.ReadAllLines(pathFile, Encoding.UTF8).ToArray(), "-d"));
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
                else
                {
                    Console.WriteLine();
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "-s" || args[i] == "-i")
                        {
                            if (isTyped == false)
                            {
                                isTyped = true;
                                if (args[i] == "-s")
                                {
                                    Console.WriteLine("TYPEOFDATA: string");
                                }
                                else
                                    Console.WriteLine("TYPEOFDATA: integer");
                            }
                            else
                                throw new Exception("You must specify only one parameter designating the data type.");
                        }
                        else if (args[i] == "-a" || args[i] == "-d")
                        {
                            if (isSortTyped == false)
                            {
                                isSortTyped = true;

                                if (args[i] == "-a")
                                    Console.WriteLine("TYPEOFSORT: Ascending");
                                else
                                {
                                    Console.WriteLine("TYPEOFSORT: Descending");
                                    sortType = "-d";
                                }
                            }
                            else
                                throw new Exception("It is necessary to specify only one parameter with the designation of the sorting type or not to specify at all (by default, sorting is ascending)");
                        }
                        else if (args[i].Length > 2)
                        {
                            if (isOut == false)
                            {
                                string[] argWords = args[i].Split('.');
                                outFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + args[i].ToString();
                                fsOut = new FileStream(outFile, FileMode.Append);
                                swOut = new StreamWriter(fsOut);
                                isOut = true;
                                Console.WriteLine("OUTFILE: " + args[i]);
                                Console.WriteLine("OUTFILE PATH: " + outFile);
                            }
                            else
                            {
                                string inFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\" + args[i].ToString();
                                if (File.Exists(inFile))
                                {
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
                                    StreamReader srIn = new StreamReader(inFile);
                                    isIn = true;
                                    Console.WriteLine("INFILE: " + args[i]);
                                    Console.WriteLine("INFILE PATH: " + inFile);
                                    string line;
                                    while ((line = srIn.ReadLine()) != null)
                                    {
                                        swOut.WriteLine(line);
                                    }
                                    srIn.Close();
                                }
                                else
                                    throw new Exception("Missing readable file(s) with given name(s)");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid parameter(s)");
                        }
                    }
                    if (isIn == false)
                        throw new Exception("Missing file(s) read");
                    if (isSortTyped == false)
                        Console.WriteLine("TYPEOFSORT: Descending");
                }
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