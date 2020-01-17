using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CSVUtils
{
    public class CSVReader
    {
        private int[,] tf  = {
            { 0, 3, 6, 4, 5},
            { 1, 1, 2, 1, 1},
            { 0, 3, 1, 4,-1},
            { 0,-1,-1,-1,-1},
            { 0, 3, 6, 4, 5},
            { 0, 3, 5, 4, 5},
            { 1, 1, 2, 1, 1}
        };
        /*private int[,] tf  = {
          { 0, 3, 1, 4, 5},
          { 1, 1, 2, 1, 1},
          { 0, 3, 1, 4,-1},
          { 0,-1,-1,-1,-1},
          { 0, 3, 1, 4, 5},
          { 0, 3, 5, 4, 5}
          };*/

        private Dictionary<string, StreamReader> openFiles;

        public CSVReader()
        {
            openFiles = new Dictionary<string, StreamReader>();
        }

        private int symbolToInt(int c)
        {
            int outString = -1;
            switch (c)
            {
                case 10:
                    outString = 0;
                    break;
                case 13:
                    outString = 1;
                    break;
                case 34:
                    outString = 2;
                    break;
                case 44:
                    outString = 3;
                    break;
                default:
                    outString = 4;
                    break;
            }
            return outString;
        }

        public bool ValidateSyntax(String filePath)
        {
            int currentState = 0, c;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while ((c = sr.Read()) != -1 && currentState != -1)
                    {
                        currentState = tf[currentState, symbolToInt(c)];
                    }
                }
                return currentState == 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*public CSVData ReadAll(string filePath)
          {
          int currentState = 0, c;
          List<List<string>> csv = new List<List<string>>();
          List<string> tmpRow = new List<string>();
          StringBuilder sb = new StringBuilder();
          StringBuilder currentRow = new StringBuilder();
          try
          {
          using (StreamReader sr = new StreamReader(filePath))
          {
          while ((c = sr.Read()) != -1 && currentState != -1)
          {
          currentRow.Append((char)c);
          currentState = tf[currentState, symbolToInt(c)];
          switch (currentState)
          {
          case 0:
          tmpRow.Add(sb.ToString());
          csv.Add(tmpRow);
          sb = new StringBuilder();
          currentRow = new StringBuilder();
          tmpRow = new List<string>();
          break;
          case 1:
          case 2:
          case 5:
          sb.Append((char)c);
          break;
          case 4:
          tmpRow.Add(sb.ToString());
          sb = new StringBuilder();
          break;
          default:
          break;
          }
          }
          }
          if (currentState != 0)
          {
          StringBuilder message = new StringBuilder();
          message.Append(String.Format("Error parsing CSV file, invalid syntax in row {0}:\n", csv.Count));
          message.Append(currentRow.ToString());
          throw new Exception(message.ToString());
          }
          return new CSVData(csv);
          }
          catch (Exception)
          {
          throw;
          }
          }*/

        public CSVData ReadAll(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    return ReadAll(sr);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CSVData ReadAll(TextReader inFile)
        {
            int currentState = 0, c;
            List<List<string>> csv = new List<List<string>>();
            List<string> tmpRow = new List<string>();
            StringBuilder sb = new StringBuilder();
            StringBuilder currentRow = new StringBuilder();
            try
            {
                while ((c = inFile.Read()) != -1 && currentState != -1)
                {
                    currentRow.Append((char)c);
                    currentState = tf[currentState, symbolToInt(c)];
                    switch (currentState)
                    {
                        case 0:
                            tmpRow.Add(sb.ToString());
                            csv.Add(tmpRow);
                            sb = new StringBuilder();
                            currentRow = new StringBuilder();
                            tmpRow = new List<string>();
                            break;
                        case 1:
                        //case 2:
                        case 5:
                            sb.Append((char)c);
                            break;
                        case 4:
                            tmpRow.Add(sb.ToString());
                            sb = new StringBuilder();
                            break;
                        default:
                            break;
                    }
                }
                if (currentState != 0)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append(String.Format("Error parsing CSV file, invalid syntax in row {0}:\n", csv.Count));
                    message.Append(currentRow.ToString());
                    throw new Exception(message.ToString());
                }
                return new CSVData(csv);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<string> ReadRow(string filePath)
        {
            int currentState = 0, c;
            List<string> row = new List<string>();
            StringBuilder sb = new StringBuilder();
            StringBuilder currentRow = new StringBuilder();
            StreamReader sr;
            try
            {
                if (openFiles.ContainsKey(filePath))
                {
                    sr = openFiles[filePath];
                }
                else
                {
                    sr = new StreamReader(filePath);
                    openFiles.Add(filePath, sr);
                }
                if ((c = sr.Read()) == -1)
                {
                    return null;
                }
                while (c != -1 && currentState != -1)
                {
                    currentRow.Append((char)c);
                    currentState = tf[currentState, symbolToInt(c)];
                    switch (currentState)
                    {
                        case 0:
                            row.Add(sb.ToString());
                            sb = new StringBuilder();
                            goto end_of_row_reached;
                        case 1:
                            sb.Append((char)c);
                            break;
                        case 4:
                            row.Add(sb.ToString());
                            sb = new StringBuilder();
                            break;
                        case 5:
                            sb.Append((char)c);
                            break;
                        default:
                            break;
                    }
                    c = sr.Read();
                }
                if (currentState != 0)
                {
                    StringBuilder message = new StringBuilder();
                    message.Append(String.Format("Error reading next row from CSV file {0}:\n", filePath));
                    message.Append(currentRow.ToString());
                    throw new Exception(message.ToString());
                }
end_of_row_reached:
                return row;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
