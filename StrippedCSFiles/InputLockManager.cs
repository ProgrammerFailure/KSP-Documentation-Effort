using System.Collections.Generic;
using System.Runtime.CompilerServices;

public static class InputLockManager
{
	public static Dictionary<string, ulong> lockStack;

	public static ulong lockMask;

	public static ulong LockMask
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static InputLockManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RecalcMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ControlTypes SetControlLock(ControlTypes locks, string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ControlTypes SetControlLock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveControlLock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearControlLocks()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ControlTypes GetControlLock(string lockID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsLocked(ControlTypes controlType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsUnlocked(ControlTypes controlType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsAllLocked(ControlTypes mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsAnyUnlocked(ControlTypes mask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsLocked(ControlTypes controlType, ControlTypes refMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsUnlocked(ControlTypes controlType, ControlTypes refMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsLocking(ControlTypes controlType, GameEvents.FromToAction<ControlTypes, ControlTypes> refMasks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsUnlocking(ControlTypes controlType, GameEvents.FromToAction<ControlTypes, ControlTypes> refMasks)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string PrintLockStack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DebugLockStack()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ulong ControlLocks(int lower, int upper)
	{
		throw null;
	}
}
