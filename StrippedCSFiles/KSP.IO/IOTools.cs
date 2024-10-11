using System;
using System.Runtime.CompilerServices;

namespace KSP.IO;

[Obsolete("Methods moved to IOUtils")]
public class IOTools
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public IOTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] SerializeToBinary(object something)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static object DeserializeFromBinary(byte[] input)
	{
		throw null;
	}
}
