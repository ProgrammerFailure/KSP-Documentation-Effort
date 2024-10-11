using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SoftMasking;

public static class MaterialReplacer
{
	private static List<IMaterialReplacer> _globalReplacers;

	public static IEnumerable<IMaterialReplacer> globalReplacers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static IEnumerable<IMaterialReplacer> CollectGlobalReplacers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsMaterialReplacerType(Type t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static IMaterialReplacer TryCreateInstance(Type t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static IEnumerable<Type> GetTypesSafe(this Assembly asm)
	{
		throw null;
	}
}
