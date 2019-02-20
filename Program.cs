using System;
using System.Collections.Generic;
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
                if (sortType == "-a")
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0)
                        if (mass1[a].CompareTo(mass2[b]) > 0)
                            merged[i] = mass2[b++];
                        else
                            merged[i] = mass1[a++];
                    else
                        if (b < mass2.Length)
                            merged[i] = mass2[b++];
                        else
                            merged[i] = mass1[a++];
                }
                else if (sortType == "-d")
                {
                    if (b.CompareTo(mass2.Length) < 0 && a.CompareTo(mass1.Length) < 0)
                        if (mass1[a].CompareTo(mass2[b]) > 0)
                            merged[i] = mass1[a++];
                        else
                            merged[i] = mass2[b++];
                    else
                        if (b < mass2.Length)
                            merged[i] = mass2[b++];
                        else
                            merged[i] = mass1[a++];
                }
            }
            return merged;
        }

        static void Main(string[] args)
        {
            //string[] arr = new string[10];
            //Random rd = new Random();

            //arr[0] = "варпапр";
            //arr[1] = "гшд";
            //arr[2] = "тьбюьб";
            //arr[3] = "цук";
            //arr[4] = "япроп";
            //arr[5] = "фывф";
            //arr[6] = "ываы";
            //arr[7] = "авыаы";
            //arr[8] = "гншл";
            //arr[9] = "паноенг";
            //Console.WriteLine("Массив перед сортировкой:");
            //foreach (string x in arr)
            //    System.Console.Write(x + " ");
            //arr = MergeSort(arr, "-a");
            //Console.WriteLine("\n\nМассив после сортировки:");
            //foreach (string x in arr)
            //    System.Console.Write(x + " ");
            //Console.ReadKey();

            foreach (string el in args)
                Console.Write(el.ToString() + " ");

            bool isTyped = false;
            bool isSortTyped = false;
            bool isOut = false;
            bool isIn = false;

            FileStream fsOut = null;
            StreamWriter swOut = null;

            try
            {
                if (args.Length < 3)
                    throw new Exception("Недостаточно аргументов (как минимум 3: тип данных, выходной файл, входной файл)");
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
                                    Console.WriteLine("TYPEOFDATA: string");
                                else
                                    Console.WriteLine("TYPEOFDATA: integer");
                                //Метод для определения типа
                            }
                            else
                                throw new Exception("Необходимо указать только один параметр с обозначением типа данных");
                        }
                        else if (args[i] == "-a" || args[i] == "-d")
                        {
                            if (isSortTyped == false)
                            {
                                isSortTyped = true;

                                if (args[i] == "-a")
                                    Console.WriteLine("TYPEOFSORT: По возрастанию");
                                else
                                    Console.WriteLine("TYPEOFSORT: По убыванию");
                                //Метод для определения типа сортировка
                            }
                            else
                                throw new Exception("Необходимо указать только один параметр с обозначением типа сортировки или не указывать вовсе(по умолчанию сортировка по возрастанию)");
                        }
                        else if (args[i].Length > 2)
                        {
                            if (isOut == false)
                            {
                                string[] argWords = args[i].Split('.');
                                string outFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/" + args[i].ToString();
                                fsOut = new FileStream(outFile, FileMode.Append);
                                swOut = new StreamWriter(fsOut);
                                isOut = true;
                                Console.WriteLine("OUTFILE: " + args[i]);
                                Console.WriteLine("OUTFILE PATH: " + outFile);
                            }
                            else
                            {
                                string inFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/" + args[i].ToString();
                                if (File.Exists(inFile))
                                {
                                    List<int> intList = new List<int>();
                                    int intValue = 0;
                                    for (int q = 0; q < File.ReadAllLines(inFile, Encoding.Default).ToList().Count; q++)
                                    {
                                        if (int.TryParse((File.ReadAllLines(inFile, Encoding.Default).ToList()[q]), out intValue))
                                            intList.Add(intValue);
                                    }
                                    if (intList.Count == File.ReadAllLines(inFile, Encoding.Default).ToList().Count)
                                        File.WriteAllLines(inFile, MergeSort(intList.Select(x => int.Parse(x.ToString())).ToArray(), "-a").Select(x => x.ToString()).ToArray());
                                    else
                                        File.WriteAllLines(inFile, MergeSort(File.ReadAllLines(inFile, Encoding.UTF8).ToArray(), "-a"));
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
                                    throw new Exception("Отсутствует считывываемый файл(ы) с данным(и) именем(ми)");
                            }
                        }
                        else
                        {
                            throw new Exception("Некорректный параметр(ы)");
                        }
                    }
                    if (isIn == false)
                        throw new Exception("Отсутствует считываемый файл(ы)");
                    if (isSortTyped == false)
                        Console.WriteLine("TYPEOFSORT: По убыванию");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
            finally
            {
                swOut.Close();
                fsOut.Close();
            }

        }
    }
}