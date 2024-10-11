using System.ComponentModel;
using System.Runtime.CompilerServices;

public class AlarmActions : IConfigNode
{
	public enum WarpEnum
	{
		[Description("#autoLOC_8003567")]
		DoNothing,
		[Description("#autoLOC_8003568")]
		KillWarp,
		[Description("#autoLOC_8003569")]
		PauseGame
	}

	public enum MessageEnum
	{
		[Description("#autoLOC_8003570")]
		No,
		[Description("#autoLOC_8003571")]
		Yes,
		[Description("#autoLOC_8003572")]
		YesIfOtherVessel
	}

	public WarpEnum warp;

	public MessageEnum message;

	public bool deleteWhenDone;

	public bool playSound;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmActions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmActions(WarpEnum Warp, MessageEnum Message, bool PlaySound, bool DeleteWhenDone)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmActions Duplicate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
