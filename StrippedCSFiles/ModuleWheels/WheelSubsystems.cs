using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ModuleWheels;

public class WheelSubsystems
{
	private List<WheelSubsystem> subs;

	private WheelSubsystem.SystemTypes types;

	public Callback<WheelSubsystems> OnModified;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WheelSubsystems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSubsystem(WheelSubsystem d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveSubsystem(WheelSubsystem d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResetFlags()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasType(WheelSubsystem.SystemTypes t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Contains(WheelSubsystem s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetReasons(ref string o, string separator = ", ")
	{
		throw null;
	}
}
