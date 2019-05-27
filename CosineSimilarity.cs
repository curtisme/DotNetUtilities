using System;
using System.Collections.Generic;

public sealed class CosineSimilarity
{
    public static double Measure(string s1, string s2)
    {
        Dictionary<string, int> v1 = new Dictionary<string, int>();
        Dictionary<string, int> v2 = new Dictionary<string, int>();
        double dot = 0;
        double mag1 = 0;
        double mag2 = 0;
        foreach (string s in s1.Split(null))
        {
            if (!v1.ContainsKey(s))
                v1.Add(s, 1);
            else
                v1[s] += 1;
            if (!v2.ContainsKey(s))
                v2.Add(s, 0);
        }

        foreach (string s in s2.Split(null))
        {
            if (!v2.ContainsKey(s))
                v2.Add(s, 1);
            else
                v2[s] += 1;
            if (!v1.ContainsKey(s))
                v1.Add(s, 0);
        }

        foreach (KeyValuePair<string, int> kvp in v1)
        {
            dot += kvp.Value * v2[kvp.Key];
            mag1 += kvp.Value * kvp.Value;
            mag2 += v2[kvp.Key]*v2[kvp.Key];
        }
        return  dot/(Math.Sqrt(mag1)*Math.Sqrt(mag2));
    }
}
