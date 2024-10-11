using System;
using System.Runtime.CompilerServices;

public static class PDebug
{
	[Flags]
	public enum DebugLevel
	{
		None = 0,
		Error = 1,
		Warning = 2,
		General = 4,
		PartInit = 8,
		FieldInit = 0x10,
		ModuleLoader = 0x20,
		PartLoader = 0x40,
		GameSettings = 0x80,
		ResourceNetwork = 0x100,
		ConfigNode = 0x200,
		GameDatabase = 0x400,
		Everything = 0x3FFFFFFF
	}

	public static DebugLevel debugLevel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PDebug()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Log(object msg, DebugLevel level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Log(object msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void General(object msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Warning(object msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Error(object msg)
	{
		throw null;
	}
}
