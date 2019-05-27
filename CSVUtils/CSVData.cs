using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CSVUtils
{
	public class CSVData : IEnumerable<CSVRow>
	{
		private List<CSVRow> rows;
		private Dictionary<string, int> header;
		private CSVRow headerOrder;

		public int Count
		{
			get {return rows.Count;}
		}

		public CSVRow Header
		{
			get {return headerOrder;}
		}

		public CSVData(List<List<string>> Rows)
		{
			try
			{
				this.header = generateHeader(Rows);
				headerOrder = new CSVRow(Rows[0], null);
				this.rows = new List<CSVRow>();
				bool first = true;
				foreach (List<string> row in Rows)
				{
					if (first)
					{
						first = false;
						continue;
					}
					rows.Add(new CSVRow(row, header));
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private Dictionary<string, int> generateHeader(List<List<string>> Rows)
		{
			Dictionary<string, int> D = null;
			if (Rows.Count > 0)
			{
				D = new Dictionary<string, int>();
				for (int i=0;i<Rows[0].Count;i++)
				{
					try
					{
						D.Add(Rows[0][i], i);
					}
					catch (ArgumentException)
					{
						throw new Exception("CSV Header contains duplicate column names");
					}
				}
			}
			return D;
		}

		public CSVRow GetRow(int row)
		{
			try
			{
				return rows[row];
			}
			catch (Exception)
			{
				throw;
			}
		}

		public string GetEntry(int row, string colName)
		{
			try
			{
				return rows[row].GetEntry(colName);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void Sort(IComparer<CSVRow> comparer)
		{
			rows.Sort(comparer);
		}

		public void Sort()
		{
			rows.Sort();
		}

		public IEnumerator<CSVRow> GetEnumerator()
		{
			return new CSVRowEnumerator(this.rows);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public CSVRow Find(Predicate<CSVRow> P)
		{
			return rows.Find(P);
		}

		public int BinarySearch(CSVRow rowToFind, IComparer<CSVRow> comparer)
		{
			return rows.BinarySearch(rowToFind, comparer);
		}

		public int BinarySearch(CSVRow rowToFind)
		{
			return rows.BinarySearch(rowToFind);
		}

		public LinkedList<CSVRow> FindAllDiffsSorted(CSVData sortedFile, IComparer<CSVRow> comparer)
		{
			LinkedList<CSVRow> diffs = new LinkedList<CSVRow>();
			foreach (CSVRow thisRow in rows)
			{
				if (sortedFile.BinarySearch(thisRow, comparer) < 0)
				{
					diffs.AddLast(thisRow);
				}
			}
			return diffs;

		}

		public LinkedList<CSVRow> FindAllDiffsSorted(CSVData sortedFile)
		{
			LinkedList<CSVRow> diffs = new LinkedList<CSVRow>();
			foreach (CSVRow thisRow in rows)
			{
				if (sortedFile.BinarySearch(thisRow) < 0)
				{
					diffs.AddLast(thisRow);
				}
			}
			return diffs;

		}
		public string HeaderToString()
		{
			return headerOrder.ToString();
		}

		public void WriteTo(TextWriter writer)
		{
			writer.WriteLine(Header);
			foreach (CSVRow r in rows)
			{
				writer.WriteLine(r);
			}
		}

		public static CSVData DisjointUnion(CSVData[] csvs)
		{
			List<List<string>> newRows = new List<List<string>>();
			try
			{
				newRows.Add(csvs[0].Header.CopyToList());
				foreach (CSVData csv in csvs)
				{
					foreach (CSVRow r in csv)
					{
						newRows.Add(r.CopyToList());
					}
				}
				return new CSVData(newRows);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	public class CSVRowEnumerator : IEnumerator<CSVRow>
	{
		private List<CSVRow> rows;
		private int currIndex;
		private CSVRow currRow;

		public CSVRowEnumerator(List<CSVRow> csvRows)
		{
			rows = csvRows;
			currIndex = -1;
			currRow = null;
		}

		public bool MoveNext()
		{
			if (++currIndex < rows.Count)
			{
				currRow = rows[currIndex];
				return true;
			}
			return false;
		}

		public void Reset()
		{
			currIndex = -1;
		}

		void IDisposable.Dispose() {}

		public CSVRow Current
		{
			get {return currRow;}
		}

		object IEnumerator.Current
		{
			get {return currRow;}
		}
	}
}
