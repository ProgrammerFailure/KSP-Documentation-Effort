using System;
using System.Runtime.CompilerServices;

public class InputBinding : IConfigNode, ICloneable
{
	public InputBindingModes switchState;

	public static bool linRotState;

	public bool useSwitchState;

	public ulong inputLockMask;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InputBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CompareSwitchState(InputBindingModes switchSt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsLocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsUnlocked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsNeutral()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object Clone()
	{
		throw null;
	}
}
