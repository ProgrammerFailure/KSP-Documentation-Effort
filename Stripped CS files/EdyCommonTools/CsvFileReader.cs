using System.IO;

namespace EdyCommonTools;

public class CsvFileReader : StreamReader
{
	public CsvFileReader(Stream stream)
		: base(stream)
	{
	}

	public CsvFileReader(string filename)
		: base(filename)
	{
	}

	public bool ReadRow(CsvRow row)
	{
		row.LineText = ReadLine();
		if (string.IsNullOrEmpty(row.LineText))
		{
			return false;
		}
		int i = 0;
		int num = 0;
		bool flag = false;
		while (i < row.LineText.Length)
		{
			string text;
			if (row.LineText[i] == '"')
			{
				flag = true;
				i++;
				int num2 = i;
				for (; i < row.LineText.Length; i++)
				{
					if (row.LineText[i] == '"')
					{
						i++;
						if (i >= row.LineText.Length || row.LineText[i] != '"')
						{
							i--;
							break;
						}
					}
				}
				text = row.LineText.Substring(num2, i - num2);
				text = text.Replace("\"\"", "\"");
				i++;
			}
			else
			{
				flag = false;
				int num3 = i;
				for (; i < row.LineText.Length && row.LineText[i] != ','; i++)
				{
				}
				text = row.LineText.Substring(num3, i - num3);
			}
			if (num < row.Count)
			{
				row[num] = text;
			}
			else
			{
				row.Add(text);
			}
			num++;
			for (; i < row.LineText.Length && row.LineText[i] != ','; i++)
			{
			}
			if (i < row.LineText.Length)
			{
				i++;
			}
		}
		if (!flag && row.LineText.Length > 0 && row.LineText[row.LineText.Length - 1] == ',')
		{
			if (num < row.Count)
			{
				row[num] = "";
			}
			else
			{
				row.Add("");
			}
			num++;
		}
		while (row.Count > num)
		{
			row.RemoveAt(num);
		}
		return row.Count > 0;
	}
}
