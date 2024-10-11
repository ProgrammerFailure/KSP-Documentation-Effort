using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KeyBinding : InputBinding, IConfigNode, ICloneable
{
	public KeyCodeExtended primary;

	public KeyCodeExtended secondary;

	public InputBindingModes switchStateSecondary;

	private const float doubleTapInterval = 0.2f;

	private int lastKeyDownFrameCount;

	private float lastKeyDownTime;

	private bool doubleDown;

	private int lastKeyUpFrameCount;

	private float lastKeyUpTime;

	private bool doubleUp;

	public string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, InputBindingModes useSwitch)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, InputBindingModes useSwitch, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, KeyCode alt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, KeyCode alt, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, KeyCode alt, InputBindingModes useSwitch, InputBindingModes useSwitchSecondary)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KeyBinding(KeyCode main, KeyCode alt, InputBindingModes useSwitch, InputBindingModes useSwitchSecondary, ControlTypes lockMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetKey(bool ignoreInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetKeyDown(bool ignoreInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetDoubleTapDown(bool ignoreInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetKeyUp(bool ignoreInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetDoubleTapUp(bool ignoreInputLock = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsNeutral()
	{
		throw null;
	}
}
