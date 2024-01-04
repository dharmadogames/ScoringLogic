using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringLogic
{
    internal class csvLibrary
    {
        public class csvSheetCell
        {
            public List<char> chars;

            public csvSheetCell(string inputString)
            {
                chars = new List<char>();
                for (int i = 0; i < inputString.Length; i++) { chars.Add(inputString[i]); }
            }

            public void print()
            {
                foreach (char c in chars) 
                {
                Console.Write(c);
                }
            }
            public override string ToString()
            {
                string outputString = "";
                foreach (char c in chars) { outputString += c; }
                return outputString;
            }

            public void set(string inputString) 
            {
                chars = new List<char>();
                for (int i = 0; i < inputString.Length; i++) 
                {
                    chars.Add(inputString[i]); 
                }
            }

        }
        public class csvSheetLine
        {
           public List<csvSheetCell> cells;
           
           public csvSheetLine()
            {
                cells = new List<csvSheetCell>();
            }

            public void print()
            {
                foreach (csvSheetCell cell in cells) 
                {
                    cell.print();
                    Console.Write(",");
                }
            }

            public override string ToString() 
            {
                if(cells == null || cells.Count == 0 ) { return "\n"; }
                string line = string.Empty;
                csvSheetCell lastCell = cells.Last();
                foreach (csvSheetCell cell in cells)
                {
                   line += cell.ToString();
                   if (cell != lastCell) 
                    line += ",";
                }
                line += "\n";
                return line;
            }

            public csvSheetCell this[int i]
            {
                get { return cells[i]; }
                set { cells[i] = new csvSheetCell(value.ToString()); }
            }

            public bool isEmpty()
            {
                for(int i = 0; i < cells.Count; i++)
                {
                    if (cells[i].ToString() != "") return false;
                }
                return true;
            }
        }

        public class csvSheet
        {
            public List<csvSheetLine> lines;
            int cellsPerLine = 0;

            public csvSheetLine this[int i]
            {
                get { return lines[i]; }
                set => lines[i] = (csvSheetLine)value;
            }
            public void print()
            {
                Console.WriteLine("Printing:");
                foreach (csvSheetLine line in lines) 
                {
                    line.print();
                    Console.WriteLine("");
                }
            }

            public void AddText(FileStream file, string value)
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                file.Write(info, 0, info.Length);
            }
            public void exportSheet(FileStream outputFile)
            {
                foreach (csvSheetLine line in lines)
                {
                    AddText(outputFile, line.ToString());
                }
                
            }

            public void readInFile(StreamReader inputFile)
            {
                lines = new List<csvSheetLine>();
                //Each line is a list of values seperated by commas, end of line is denoted by a newline character outside the line
                string lineString;
                string cell = "";
                bool inQuotes = false;
                csvSheetLine currentLine = new csvSheetLine();

                while ((lineString = inputFile.ReadLine()) != null) //Read in lines
                {    
                    //If we are not in a quote, then the end of line denotes a end of csv line
                    if(!inQuotes)
                    {
                        //Append last cell to current line
                        currentLine.cells.Add(new csvSheetCell(cell));
                        //Reset variables
                        cell = "";
                        lines.Add(currentLine);
                        currentLine = new csvSheetLine();
                    }


                    //Go character by character to ensure no shenanigans happens
                    for(int i = 0; i < lineString.Length; i++) 
                    {
                        char currentChar = lineString[i];
                        //Need more logic for quotation marks, this works though
                        if (currentChar == '"')
                        {
                            inQuotes = !inQuotes;
                            cell = cell + currentChar;
                        }
                        else if (currentChar == ',')
                        {
                            //Check if in quotes
                            if (inQuotes)
                            {
                                //If in quotes add to cell 
                                cell = cell + currentChar;
                            }
                            else
                            {
                                //Else move on to next cell
                                //Append cell to current line
                                currentLine.cells.Add(new csvSheetCell(cell));
                                //Reset variables
                                cell = "";
                            }
                        }
                        else
                        {
                            cell = cell + currentChar;
                        }
                    }
                }
                //Append last cell of last line
                currentLine.cells.Add(new csvSheetCell(cell));
                //Append last line of sheet
                lines.Add(currentLine);
                //Remove first line of sheet (is an empty line)
                lines.Remove(lines[0]);
                this.print();
            }

        }
    }
}
