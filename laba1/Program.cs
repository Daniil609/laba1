using System;
using System.IO;
using System.Collections.Generic;
using System.IO.Compression;

namespace laba1
{
    class Program
    {
        
         static void Main(string[] args)
        {
            List<string> used_files = new List<string>();
            
            int collichestvo = 0;
            int size = 0;
            string parth = @"/Users/tomashikdaniil";
            string options = "1) Choose the directory." +
                    " \n2) Write in file. \n3) Read information from file. \n4) Get information about the file. \n5) Copy from one file to another. " +
                     " \n6) Delete file. \n7) Compress file. \n8) Decompress file. \n9) Rename file.";


            int namberOfSysyle = 0;
            while (true)
            {
                if (namberOfSysyle == 0)
                {
                    Console.WriteLine("Welcom to this very simple app by working with files. Chose what you like to do:\n"+options );
                }
                else
                {
                    Console.WriteLine("What you like to do:\n" + options);
                }
                var namberByUser = Console.ReadLine();
                switch (namberByUser)
                {
                    case "1": CreateOpenDirectiory(); break;
                    case "2": Write(); break;
                    case "3": Read(); break;
                    case "4": Copy(); break;
                    case "5": Copy(); break;
                    case "6": DeleteFile(); break;
                    case "7": Compress(); break;
                    case "8": Decompress(); break;
                    case "9": RenameFile(); break;
                }
                namberOfSysyle++;
            }

            void CreateOpenDirectiory()
            {
                Console.WriteLine("Enter Path");
                parth = Console.ReadLine();
                DirectoryInfo dirInfo = new DirectoryInfo(parth);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
            }
            void Write()
            {
                string text = "";
                Console.WriteLine("Chose file(.txt):");
                string name = Console.ReadLine();

                used_files.Add(name);
                Console.WriteLine("How many car you would like to add");
                collichestvo = Int32.Parse(Console.ReadLine());

                List<Car> cars = new List<Car>();
                getCarInfo(ref cars, collichestvo);
                int i = 0;
                foreach (Car p in cars)
                {
                    text += $"{i + 1}) marka: {p.Marka}  Country: {p.Country} year: {p.Year()} \n";
                    i++;
                }
                using (FileStream sourceStream = new FileStream($"{parth}" + @"\" + $"{name}.txt", FileMode.OpenOrCreate))
                {
                    string sourceFile = $"{parth}" + @"\" + $"{name}.txt";

                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    // запись массива байтов в файл
                    sourceStream.Write(array, 0, array.Length);
                    size = array.Length / collichestvo;
                    Console.WriteLine("\nText written to file!\n");



                }
            }
            void Read()
            {
                    Console.WriteLine("\nChoose filename:(.txt)");
                    string name = Console.ReadLine();
                try
                {
                    using (FileStream fstream = File.OpenRead($@"{parth}\{name}.txt"))
                    {
                        // преобразуем строку в байты
                        byte[] array = new byte[size];

                        if (used_files.Contains(name))
                        {
                            Console.WriteLine($"\nThere should be informaton about {fstream.Length / size} cars: \n Information about which " +
                                $"acr is interesting for you? \n");
                        }
                        else
                        {
                            Console.WriteLine("In this arn't any information about cars! Please, try to open another file...");
                            Read();
                        }
                        int choice = int.Parse(Console.ReadLine());

                        // считываем данные
                        fstream.Seek((size - 1) * (choice - 1), SeekOrigin.Begin);
                        fstream.Read(array, 0, size);

                        // декодируем байты в строку
                        string textFromFile = System.Text.Encoding.Default.GetString(array);
                        Console.WriteLine($"Inforamtion about {choice} car: {textFromFile}");
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("This file is not found! Please, try again...");
                    return;
                }


            }
            void getCarInfo(ref List<Car> ppl, int n)
                {
                    int i = 0;
                   
                        while (i < n)
                        {
                            Console.WriteLine($"Enter the info about {i + 1} car: \n");
                            Console.WriteLine("Marka: ");
                            string marka = Console.ReadLine();
                            Console.WriteLine("Country: ");
                            string country = (Console.ReadLine());
                            Console.WriteLine("Year: ");
                            string year = Console.ReadLine();
                            ppl.Add(new Car { Marka = marka, Country = country, Year = year });
                            i++;
                        }
                    

            }
            void DeleteFile()
            {
                Console.WriteLine("\nPlease, enter path to the source file you want to delete: ");
                string source = Console.ReadLine();
                if (File.Exists(source))
                {
                    // Use a try block to catch IOExceptions, to
                    // handle the case of the file already being
                    // opened by another process.
                    try
                    {
                        File.Delete(source);
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("\nThis file isn't exist or it has been deleted earlier\n");
                }
            }
            void Copy()
            {
                Console.WriteLine("\nPlease, enter path to the source file: ");
                string source = Console.ReadLine();
                Console.WriteLine("Please, enter path to the destination file: ");
                string dest = Console.ReadLine();
                File.Copy(source, dest, true);
            }
            void Compress()
            {
                Console.WriteLine("Please, choose the name of file you want to compose.");
                string name = Console.ReadLine();
                string sourceFile = $"{parth}" + @"\" + $"{name}.txt";

                FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);

                using (FileStream targetStream = File.Create($"{parth}" + @"\" + $"{name}.gz"))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
                    }
                }
            }

            void Decompress()
            {
                Console.WriteLine("Please, choose the name of file you want to compose.  (.gz)");
                string name = Console.ReadLine();
                string compressedFile = $"{parth}" + @"\" + $"{name}.gz";

                FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate);

                Console.WriteLine("Please, choose the name of result file.");
                string name_res = Console.ReadLine();
                string targetFile = $"{parth}" + @"\" + $"{name_res}.txt";

                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("File restored: {0}", targetFile);
                    }
                }
                used_files.Add(name_res);
            }
            void RenameFile()
            {
                Console.WriteLine("Please, enter the name of file you want to rename.");
                string oldName = Console.ReadLine();
                string oldSource = $"{parth}" + @"\" + $"{oldName}.txt";


                Console.WriteLine("How you will call a new file?");
                string newName = Console.ReadLine();
                string newSource = $"{parth}" + @"\" + $"{newName}.txt";

                if (File.Exists(oldSource))
                {
                    File.Copy(oldSource, newSource, true);
                    File.Delete(oldSource);
                    Console.WriteLine("\nFile have been renamed!");
                    used_files.Remove(oldName);
                    used_files.Add(newName);
                }
                else
                {
                    Console.WriteLine("Nothing happened because current file doesn't exist");
                }
            }

        }
    }
}

     
