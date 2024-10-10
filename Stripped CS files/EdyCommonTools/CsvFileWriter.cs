using System.IO;
using System.Text;

namespace EdyCommonTools;

public class CsvFileWriter : StreamWriter
{
	public CsvFileWriter(Stream stream)
		: base(stream)
	{
	}

	public CsvFileWriter(string filename)
		: base(filename)
	{
	}

	public void WriteRow(CsvRow row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (string item in row)
		{
			if (!flag)
			{
				stringBuilder.Append(',');
			}
			if (item.IndexOfAny(new char[2] { '"', ',' }) != -1)
			{
				stringBuilder.AppendFormat("\"{0}\"", item.Replace("\"", "\"\""));
			}
			else
			{
				stringBuilder.Append(item);
			}
			flag = false;
		}
		row.LineText = stringBuilder.ToString();
		WriteLine(row.LineText);
	}
}
