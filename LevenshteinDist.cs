using System;

public sealed class LevenshteinDist
{
    public static int Measure(string s1, string s2)
    {
        int[,] L = new int[s1.Length + 1, s2.Length + 1];
        for (int i=0; i<s1.Length + 1; i++)
            L[i, 0] = i;
        for (int j=0; j<s2.Length + 1; j++)
            L[0, j] = j;
        for (int i=1; i<s1.Length + 1; i++)
            for (int j=1; j<s2.Length + 1; j++)
                L[i, j] = Math.Min(L[i-1,j] + 1,
                        Math.Min(L[i, j-1] + 1,
                            L[i-1, j-1] + (s1[i-1] == s2[j-1] ? 0 : 1)));
        return L[s1.Length, s2.Length];
    }
}
