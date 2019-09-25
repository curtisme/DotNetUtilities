using System;
using System.ComponentModel;
using System.Collections.Generic;

public class CommandLineOptions
{
    private Dictionary<string, Type> ArgumentTypes; 
    private Dictionary<string, string> NamesAndValues; 

    public CommandLineOptions()
    {
        ArgumentTypes = null;
        NamesAndValues = new Dictionary<string, string>();
    }

    public CommandLineOptions(string argumentDefinition)
    {
        NamesAndValues = new Dictionary<string, string>();
        ArgumentTypes = new Dictionary<string, Type>();
        foreach (string nameTypePair in argumentDefinition.Split(new char[] {';'}))
        {
            string[] ntpSplit = nameTypePair.Split(new char[] {':'});
            if (ntpSplit.Length < 2)
                throw new Exception($"Syntax error while reading '{nameTypePair}'");
            if (!ArgumentTypes.ContainsKey(ntpSplit[0]))
            {
                Type t = null;
                if (ntpSplit[1] != "none")
                {
                    try
                    {
                        t = Type.GetType(ntpSplit[1], true);
                        TypeConverter tc = TypeDescriptor.GetConverter(t);
                        if (tc == null || !tc.CanConvertFrom(Type.GetType("System.String")))
                        {
                            throw new Exception($"Unable to convert '{ntpSplit[1]}' from 'System.String'");
                        }
                    }
                    catch (TypeLoadException e)
                    {
                        throw new Exception($"Unable to load type '{ntpSplit[1]}'", e);
                    }
                }
                ArgumentTypes.Add(ntpSplit[0], t);
            }
            else
            {
                throw new Exception($"'{ntpSplit[0]}' is not a unique argument name!");
            }
        }
    }

    private enum ReadArgsState 
    {
        Start,
        FoundArgName,
        FoundArgValue,
        Invalid
    }

    public void ReadArgs(string[] args)
    {
        string argName = "";
        ReadArgsState s = ReadArgsState.Start;
        for (int i=0;i<args.Length;i++)
        {
            switch(s)
            {
                case ReadArgsState.Start:
                    if (IsArgName(args[i]))
                    {
                        
                    }
                    break;
                case ReadArgsState.FoundArgName:
                    break;
                case ReadArgsState.FoundArgValue:
                    break;
                default:
                    breeak;
            }
        }
    }

    public bool OptionSet(string name)
    {
        return NamesAndValues.ContainsKey(name);
    }

    public Object GetArg(string name)
    {
        if (OptionSet(name))
            return NamesAndValues[name];
        return null;
    }

    private bool IsArgName(string arg)
    {
        return arg.StartsWith("-");
    }
}

public class Test
{
    public static void Main(string[] args)
    {
        try
        {
            CommandLineOptions ops = new CommandLineOptions(args[0]);
        }
        catch(Exception e)
        {
            Console.Error.WriteLine(e);
        }
    }
}
