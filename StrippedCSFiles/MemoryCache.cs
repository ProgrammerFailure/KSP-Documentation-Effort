using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class MemoryCache
{
	private static List<byte[]> byteCache;

	private static List<Color32[]> colorCache;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static MemoryCache()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] GetBytes(long nPixels)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] GetBytes(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static byte[] GetBytes(FileInfo file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color32[] GetColor32(long nPixels)
	{
		throw null;
	}
}
