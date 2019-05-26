using System;

namespace DataStructures
{
	public class HeapDriver
	{
		public static void Main()
		{
			Heap<int> H = new Heap<int>(5, (x , y) => x < y);
			H.Add(0);
			H.Add(1);
			H.Add(2);
			while (!H.IsEmpty())
			{
				Console.WriteLine(H.GetNext());
			}
		}
	}
}
