using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ReplaceStrings
{
    public static void Main(string[] args)
    {
        List<string> searchStrings;
        string replaceString;
        int c;
        StringReplacer stringReplacer;

        if (!ValidateArgs(args, out searchStrings, out replaceString))
            Console.WriteLine("Invalid!");
        else
        {
            stringReplacer = new StringReplacer(searchStrings, replaceString);
            while ((c = Console.In.Read()) > -1)
                stringReplacer.Write((char)c, Console.Out);
            stringReplacer.Flush(Console.Out);
        }
    }

    private class StringReplacer
    {
        private List<string> searchStrings;
        private string replaceString;
        private int[] indices;
        private StringBuilder sb;

        public StringReplacer(List<string> searchStrings, string replaceString)
        {
            this.searchStrings = searchStrings;
            this.replaceString = replaceString;
            indices = new int[searchStrings.Count];
            sb = new StringBuilder();
            resetIndices();
        }

        public void Write(char c, TextWriter writeOut)
        {
            bool matched = false;
            bool alreadyAdded = false;
            for (int i=0;i<searchStrings.Count;i++)
            {
                if (searchStrings[i][indices[i]] == c)
                {
                    matched = true;
                    if (!alreadyAdded)
                    {
                        sb.Append(c);
                        alreadyAdded = true;
                    }
                    if (++indices[i] >= searchStrings[i].Length)
                    {
                        writeOut.Write(replaceString);
                        sb.Clear();
                        resetIndices();
                        break;
                    }
                }
            }

            if (!matched)
            {
                Flush(writeOut);
                writeOut.Write(c);
                resetIndices();
            }
        }

        public void Flush(TextWriter writeOut)
        {
            if (sb.Length > 0)
            {
                writeOut.Write(sb.ToString());
                sb.Clear();
            }
        }

        private void resetIndices()
        {
            for (int i=0;i<indices.Length;i++)
                indices[i] = 0;
        }
    }

    private static bool ValidateArgs(string[] args, out List<string> searchStrings, out string replaceString)
    {
        bool isValid = false;
        int state = 0;
        int[] acceptStates = {2, 4, 8};

        searchStrings = new List<string>();
        replaceString = "";

        foreach (string arg in args)
        {
            //next state
            switch(state)
            {
                case 0:
                    switch(arg)
                    {
                        case "-f":
                            state = 1;
                            break;
                        case "-r":
                            state = 5;
                            break;
                        default:
                            state = -1;
                            break;
                    }
                    break;
                case 1:
                    switch(arg)
                    {
                        case "-r":
                            state = -1;
                            break;
                        default:
                            state = 2;
                            break;
                    }
                    break;
                case 2:
                    switch(arg)
                    {
                        case "-r":
                            state = 3;
                            break;
                        default:
                            state = 2;
                            break;
                    }
                    break;
                case 3:
                    state = 4;
                    break;
                case 4:
                    state = -1;
                    break;
                case 5:
                    switch(arg)
                    {
                        case "-f":
                            state = 7;
                            break;
                        default:
                            state = 6;
                            break;
                    }
                    break;
                case 6:
                    switch(arg)
                    {
                        case "-f":
                            state = 7;
                            break;
                        default:
                            state = -1;
                            break;
                    }
                    break;
                case 7:
                    state = 8;
                    break;
                case 8:
                    state = 8;
                    break;
            }

            //do something based on state
            switch(state)
            {
                case 2:
                case 8:
                    if (arg.Length > 0)
                        searchStrings.Add(arg);
                    break;
                case 6:
                case 4:
                    replaceString = arg;
                    break;
                default:
                    break;
            }
        }

        foreach (int acceptState in acceptStates)
            if (acceptState == state)
            {
                isValid = true;
                break;
            }

        if (isValid)
        {
            for (int i=0;i<searchStrings.Count;i++)
            {
                searchStrings[i] = escapify(searchStrings[i]);
            }
        }
        replaceString = escapify(replaceString);
        return isValid;
    }

    private static string escapify(string s)
    {
        int state = 0;
        StringBuilder sb = new StringBuilder();
        foreach (char c in s)
        {
            switch(state)
            {
                case 0:
                    switch(c)
                    {
                        case '\\':
                            state = 1;
                            break;
                        default:
                            state = 3;
                            break;
                    }
                    break;
                case 1:
                    state = 2;
                    break;
                case 2:
                case 3:
                    switch(c)
                    {
                        case '\\':
                            state = 1;
                            break;
                        default:
                            state = 3;
                            break;
                    }
                    break;
            }

            switch(state)
            {
                case 2:
                    //this case should be extended to implement recognition of more escape sequences
                    switch(c)
                    {
                        case 'r':
                            sb.Append('\r');
                            break;
                        case 'n':
                            sb.Append('\n');
                            break;
                        case 't':
                            sb.Append('\t');
                            break;
                        case '\\':
                            sb.Append('\\');
                            break;
                        default:
                            sb.Append("\\" + c);
                            break;
                    }
                    break;
                case 3:
                    sb.Append(c);
                    break;
                default:
                    break;
            }
        }
        return sb.ToString();
    }
}
