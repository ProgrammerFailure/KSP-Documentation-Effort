using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace KSP.IO;

public class IOUtils
{
	internal static string PluginRootPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IOUtils()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetFilePathFor(Type T, string file, Vessel flight = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void Cleanup(List<Guid> validFlightIDs)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void RemoveFlightData(Guid flightID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<DirectoryInfo> GetFlightDirectories(Guid[] flightID, bool include)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static System.IO.FileMode ConvertMode(FileMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static System.IO.SeekOrigin ConvertSeek(SeekOrigin origin)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Exception WrapException(Exception e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static System.IO.FileAccess ConvertAccess(FileAccess access)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static System.IO.FileShare ConvertShare(FileShare share)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string GetPartPathFor(Part part, string file)
	{
		throw null;
	}
}
