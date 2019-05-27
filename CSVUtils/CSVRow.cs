using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CSVUtils
{
	public class CSVRow : IEnumerable<string>, IComparable<CSVRow>
	{
		private List<string> row;
		Dictionary<string, int> header;

		public CSVRow(List<string> Row, Dictionary<string, int> Header)
		{
			this.row = Row;
			this.header = Header;
		}

		public string GetEntry(string colName)
		{
			try
			{
				return row[header[colName]];
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void SetEntry(string colName, string val)
		{
			try
			{
				row[header[colName]] = val;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public int CompareTo(CSVRow row2)
		{
			IEnumerator<string> row2Enum = row2.GetEnumerator();
			int cmp = 0;
			foreach (string thisEntry in row)
			{
				if (!row2Enum.MoveNext())
				{
					cmp = 1;
					goto end;
				}
				cmp = thisEntry.CompareTo(row2Enum.Current);
				if (cmp != 0)
				{
					goto end;
				}
			}
			if (row2Enum.MoveNext())
			{
				cmp = -1;
			}
end:
			return cmp;
		}

		public IEnumerator<string> GetEnumerator()
		{
			return new CSVEntryEnumerator(row);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string entry in row)
			{
				sb.Append(entry+",");
			}
			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		public List<string> CopyToList()
		{
			List<string> listy = new List<string>(row.Count);
			foreach (string s in row)
			{
				listy.Add(String.Copy(s));
			}
			return listy;
		}
	}

	public class CSVEntryEnumerator : IEnumerator<string>
	{
		private List<string> row;
		private int currIndex;
		private string currEntry;

		public CSVEntryEnumerator(List<string> Row)
		{
			row = Row;
			currIndex = -1;
			currEntry = null;
		}

		public bool MoveNext()
		{
			if (++currIndex < row.Count)
			{
				currEntry = row[currIndex];
				return true;
			}
			return false;
		}

		public void Reset()
		{
			currIndex = -1;
		}

		void IDisposable.Dispose() {}

		public string Current
		{
			get {return currEntry;}
		}

		object IEnumerator.Current
		{
			get {return currEntry;}
		}
	}
}
