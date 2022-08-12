using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TestTaskGL
{
    
    class Program
    {
        static void Main(string[] args)
        {
            //Interna settings
            bool DEBUG = false;
            //Variables
            Stack<string> ReversedText = new Stack<string>();
            Stack<string> Text = new Stack<string>();
            Regex pattern = new Regex(@"\W|_|\s");
            string SourceFilePath = "Input.txt";
            char deliminator = ' ';
            ReversedText = ReadTheFile(SourceFilePath); //Data input

            //Reversing stack
            while(ReversedText.Count != 0)
            {
                Text.Push(ReversedText.Pop()); //I need this to maintain the sequence of strings like in the generic file
            }
            ReversedText.Clear(); //Additional way to clear the space

            //cloning the string into traditional array
            string[] ArrayText;
            ArrayText = Text.ToArray();
            Text.Clear();

            Stack<string> FoundWords = new Stack<string>();
            //Where the magic appears. Finding number of "words" in each line, pulling out them and getting them into new stack
            for (int i = 0; i < ArrayText.Length;  i++)
            {
                int wCount = 1;
                string line = ArrayText[i];
                for (int j = 0; j < line.Length; j++)
                {
                    if(line[j] == deliminator)
                    {
                        wCount++;
                    }

                }
                string[] words = new string[wCount+1];
                words = line.Split(deliminator);
                //Deleting extra characters, using REGEX
                for(int k = 0; k < words.Length; k++)
                {
                    if(words[k] != string.Empty)
                    FoundWords.Push(Regex.Replace(words[k], pattern.ToString(), String.Empty));
                }

            }
            //Check if some word exists in two variants    
            while(FoundWords.Count != 0)
            {
                if (ReversedText.Contains(FoundWords.Peek()) == false) //Using the ReversedText stack to save some extra RAM space
                {      
                        ReversedText.Push(FoundWords.Pop());
                }
                else    
                    FoundWords.Pop();             
            }

            string[] SortedWords = ReversedText.ToArray();
            ReversedText.Clear(); //Reclaiming some space
            Array.Sort(SortedWords); //Getting the SORTED array (case-insensetive)
            Stack<string> WordNOccur = new Stack<string>();
            string lines;
            //Looking for the occurencies on text file, creating the table of occurencies
            for(int i = 0; i< SortedWords.Length; i++)
            {
                lines = String.Empty;
                for(int j = 0; j<ArrayText.Length; j++)
                {
                    if (ArrayText[j].Contains(SortedWords[i]))
                    {
                        lines += (j+1) + ",";
                    }
                }
                WordNOccur.Push(SortedWords[i] + ";" + lines);
            }
            //Reversing last stack into first stack (Reversing is needed for maintaining alphabetical order, usage of first stack is needed for space saving.)

            //Protection
            if (Text.Count != 0) Text.Clear();

            while (WordNOccur.Count != 0)
            {
                Text.Push(WordNOccur.Pop());
            }

            FileSave("OutputResult.txt",Text);

            //DEBUG
            if (DEBUG)
            {
                Console.WriteLine("Found words (in alphabetical: \r\n");
                for(int i = 0; i< SortedWords.Length; i++)
                {
                    Console.WriteLine(SortedWords[i]);
                }
                Console.WriteLine("\r\n");

                int cnt = 1;
                Console.WriteLine("Text file contains: \r\n");
                for(int i = 0; i<ArrayText.Length; i++)
                {  
                    Console.WriteLine(ArrayText[i] + "  - Data from line " + cnt++);
                }
                Console.ReadLine();
            }
        }

        static void FileSave (string FileName, Stack <string> finalReult)
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            using (StreamWriter resultSave = new StreamWriter(FileName, true))
            {
                resultSave.WriteLine("Table format:");
                resultSave.WriteLine("Word:" + "           " + "Occurs in lines:");
                resultSave.WriteLine(String.Empty);
                while (finalReult.Count != 0)
                {
                    string[] FR = new string[2];
                    FR = finalReult.Pop().Split(';');
                    resultSave.WriteLine($"{FR[0],-15} {FR[1]}");
                }

            }
            if(File.Exists(FileName) && finalReult.Count == 0)
            {
                Console.WriteLine("Task completed successfully. \r\nCheck the " + FileName + " file with the results. \r\nThe file could be found at " + Path.GetFullPath(FileName));
                Copyright();
            }
        }

        static void Copyright()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\r\n\r\n\r\nMade by SonikPr. Press any key to continue.");
            Console.ReadKey(true);
        }
        
        //Initial file read
        static Stack<string> ReadTheFile(string path)
        {
            string line;
            Stack<string> text = new Stack<string>();
            if (File.Exists(path))
            {

                using (StreamReader DR = new StreamReader(path))
                {
                    while ((line = DR.ReadLine()) != null)
                    {
                        text.Push(line);
                    }
                }
                return text;
            }
            else
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(path.ToString() + " cannot be found..." + " Press ENTER key to continue");
            Console.ReadKey(true);
            System.Environment.Exit(0);
            return text;


        }


    }
}
