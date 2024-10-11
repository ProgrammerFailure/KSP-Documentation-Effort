using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class VesselModuleManager
{
	public class VesselModuleWrapper
	{
		public Type type;

		public bool active;

		public int order;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public VesselModuleWrapper(Type type)
		{
			throw null;
		}
	}

	private static List<VesselModuleWrapper> modules;

	public static List<VesselModuleWrapper> Modules
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static VesselModuleManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CompileModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<VesselModuleWrapper> GetModules(bool activeOnly, bool order)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselModuleWrapper GetWrapper(Type moduleType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselModuleWrapper GetWrapper(string moduleTypeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetWrapperActive(Type moduleType, bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetWrapperActive(string moduleTypeName, bool isActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AddModulesToVessel(Vessel vessel, List<VesselModule> modules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveModulesFromVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool RemoveModuleOfType(Type vesselModuleType)
	{
		throw null;
	}
}
