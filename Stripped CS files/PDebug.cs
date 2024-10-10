using System;
using UnityEngine;

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

	public static DebugLevel debugLevel = DebugLevel.Error | DebugLevel.Warning | DebugLevel.General;

	public static void Log(object msg, DebugLevel level)
	{
		if ((debugLevel & level) != 0)
		{
			Debug.Log(msg);
		}
	}

	public static void Log(object msg)
	{
		General(msg);
	}

	public static void General(object msg)
	{
		if ((debugLevel & DebugLevel.General) != 0)
		{
			Debug.Log(msg);
		}
	}

	public static void Warning(object msg)
	{
		if ((debugLevel & DebugLevel.Warning) != 0)
		{
			Debug.LogWarning(msg);
		}
	}

	public static void Error(object msg)
	{
		if ((debugLevel & DebugLevel.Error) != 0)
		{
			Debug.LogError(msg);
		}
	}
}
