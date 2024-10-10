using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public static class InputLockManager
{
	public static Dictionary<string, ulong> lockStack = new Dictionary<string, ulong>();

	public static ulong lockMask = 0uL;

	public static ulong LockMask => lockMask;

	public static void RecalcMask()
	{
		lockMask = 0uL;
		Dictionary<string, ulong>.ValueCollection.Enumerator enumerator = lockStack.Values.GetEnumerator();
		while (enumerator.MoveNext())
		{
			lockMask |= enumerator.Current;
		}
	}

	public static ControlTypes SetControlLock(ControlTypes locks, string lockID)
	{
		ControlTypes controlTypes = (ControlTypes)lockMask;
		lockStack[lockID] = (ulong)locks;
		RecalcMask();
		GameEvents.onInputLocksModified.Fire(new GameEvents.FromToAction<ControlTypes, ControlTypes>(controlTypes, (ControlTypes)lockMask));
		if ((controlTypes & ControlTypes.flag_53) != ControlTypes.None && (lockMask & 0x8000000000000L) == 0L)
		{
			GameEvents.onGUILock.Fire();
			GameEvents.onTooltipDestroyRequested.Fire();
		}
		return locks;
	}

	public static ControlTypes SetControlLock(string lockID)
	{
		return SetControlLock(ControlTypes.All, lockID);
	}

	public static void RemoveControlLock(string lockID)
	{
		if (lockStack.ContainsKey(lockID))
		{
			ControlTypes controlTypes = (ControlTypes)lockMask;
			lockStack.Remove(lockID);
			RecalcMask();
			GameEvents.onInputLocksModified.Fire(new GameEvents.FromToAction<ControlTypes, ControlTypes>(controlTypes, (ControlTypes)lockMask));
			if ((controlTypes & ControlTypes.flag_53) == ControlTypes.None && (lockMask & 0x8000000000000L) != 0L)
			{
				GameEvents.onGUIUnlock.Fire();
			}
		}
	}

	public static void ClearControlLocks()
	{
		ControlTypes from = (ControlTypes)lockMask;
		lockStack.Clear();
		lockMask = 0uL;
		GameEvents.onInputLocksModified.Fire(new GameEvents.FromToAction<ControlTypes, ControlTypes>(from, (ControlTypes)lockMask));
	}

	public static ControlTypes GetControlLock(string lockID)
	{
		if (lockStack.ContainsKey(lockID))
		{
			return (ControlTypes)lockStack[lockID];
		}
		return ControlTypes.None;
	}

	public static bool IsLocked(ControlTypes controlType)
	{
		return (lockMask & (ulong)controlType) > 0L;
	}

	public static bool IsUnlocked(ControlTypes controlType)
	{
		return (lockMask & (ulong)controlType) == 0L;
	}

	public static bool IsAllLocked(ControlTypes mask)
	{
		return (lockMask & (ulong)mask) == (ulong)mask;
	}

	public static bool IsAnyUnlocked(ControlTypes mask)
	{
		return (lockMask & (ulong)mask) != (ulong)mask;
	}

	public static bool IsLocked(ControlTypes controlType, ControlTypes refMask)
	{
		return (refMask & controlType) > ControlTypes.None;
	}

	public static bool IsUnlocked(ControlTypes controlType, ControlTypes refMask)
	{
		return (refMask & controlType) == ControlTypes.None;
	}

	public static bool IsLocking(ControlTypes controlType, GameEvents.FromToAction<ControlTypes, ControlTypes> refMasks)
	{
		if (IsUnlocked(controlType, refMasks.from))
		{
			return IsLocked(controlType, refMasks.to);
		}
		return false;
	}

	public static bool IsUnlocking(ControlTypes controlType, GameEvents.FromToAction<ControlTypes, ControlTypes> refMasks)
	{
		if (IsLocked(controlType, refMasks.from))
		{
			return IsUnlocked(controlType, refMasks.to);
		}
		return false;
	}

	public static string PrintLockStack()
	{
		if (lockStack.Count == 0)
		{
			return Localizer.Format("#autoLOC_176849");
		}
		string text = Localizer.Format("#autoLOC_176851");
		foreach (string key in lockStack.Keys)
		{
			text = text + key + "\n";
		}
		return text + "-- \nbitmask: " + Convert.ToString((long)lockMask, 2);
	}

	public static void DebugLockStack()
	{
		Debug.Log(PrintLockStack());
	}

	public static ulong ControlLocks(int lower, int upper)
	{
		if (lower > upper)
		{
			return 0uL;
		}
		int num = upper - lower + 1;
		if (num >= 64)
		{
			return 0uL;
		}
		ulong num2 = (ulong)((1L << num) - 1L);
		return (num2 << lower) | (num2 >> -lower);
	}
}
