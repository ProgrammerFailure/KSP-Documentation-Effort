using System.IO;
using System.Runtime.CompilerServices;

namespace EdyCommonTools;

public class CsvFileWriter : StreamWriter
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public CsvFileWriter(Stream stream)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CsvFileWriter(string filename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void WriteRow(CsvRow row)
	{
		throw null;
	}
}
