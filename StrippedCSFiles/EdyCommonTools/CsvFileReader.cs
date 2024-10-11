using System.IO;
using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public class CsvFileReader : StreamReader
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public CsvFileReader(Stream stream)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CsvFileReader(string filename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadRow(CsvRow row)
	{
		throw null;
	}
}
