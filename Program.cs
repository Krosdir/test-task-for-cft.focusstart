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
        static void Main(string[] args)
        {
            foreach (string el in args)
                Console.Write(el.ToString() + " ");

            bool isTyped = false;
            bool isSortTyped = false;
            bool isOut = false;
            bool isIn = false;

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
                                    Console.WriteLine("TYPEOFDATA: По убыванию");
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
                                //FileStream fsOut = new FileStream(outFile, FileMode.OpenOrCreate);
                                isOut = true;
                                Console.WriteLine("OUTFILE: " + args[i]);
                            }
                            else
                            {
                                string inFile = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "/" + args[i].ToString();
                                //StreamReader insr = new StreamReader(inFile);
                                isIn = true;
                                Console.WriteLine("INFILE: " + args[i]);
                                
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
            Console.ReadKey();
        }
    }
}
