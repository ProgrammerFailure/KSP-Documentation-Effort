using System;
using UnityEngine;

public class KeyBinding : InputBinding, IConfigNode, ICloneable
{
	public KeyCodeExtended primary;

	public KeyCodeExtended secondary;

	public InputBindingModes switchStateSecondary = InputBindingModes.Any;

	public const float doubleTapInterval = 0.2f;

	public int lastKeyDownFrameCount;

	public float lastKeyDownTime;

	public bool doubleDown;

	public int lastKeyUpFrameCount;

	public float lastKeyUpTime;

	public bool doubleUp;

	public string name
	{
		get
		{
			if (primary.isNone)
			{
				return secondary.ToString();
			}
			return primary.ToString();
		}
	}

	public KeyBinding()
	{
		primary = new KeyCodeExtended();
		secondary = new KeyCodeExtended();
	}

	public KeyBinding(ControlTypes lockMask)
	{
		primary = new KeyCodeExtended();
		secondary = new KeyCodeExtended();
		inputLockMask = (ulong)lockMask;
	}

	public KeyBinding(KeyCode main)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended();
	}

	public KeyBinding(KeyCode main, ControlTypes lockMask)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended();
		inputLockMask = (ulong)lockMask;
	}

	public KeyBinding(KeyCode main, InputBindingModes useSwitch)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended();
		switchState = useSwitch;
	}

	public KeyBinding(KeyCode main, InputBindingModes useSwitch, ControlTypes lockMask)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended();
		switchState = useSwitch;
		inputLockMask = (ulong)lockMask;
	}

	public KeyBinding(KeyCode main, KeyCode alt)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended(alt);
	}

	public KeyBinding(KeyCode main, KeyCode alt, ControlTypes lockMask)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended(alt);
		inputLockMask = (ulong)lockMask;
	}

	public KeyBinding(KeyCode main, KeyCode alt, InputBindingModes useSwitch, InputBindingModes useSwitchSecondary)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended(alt);
		switchState = useSwitch;
		switchStateSecondary = useSwitchSecondary;
	}

	public KeyBinding(KeyCode main, KeyCode alt, InputBindingModes useSwitch, InputBindingModes useSwitchSecondary, ControlTypes lockMask)
	{
		primary = new KeyCodeExtended(main);
		secondary = new KeyCodeExtended(alt);
		switchState = useSwitch;
		switchStateSecondary = useSwitchSecondary;
		inputLockMask = (ulong)lockMask;
	}

	public override void Save(ConfigNode node)
	{
		node.AddValue("primary", primary.ToString());
		node.AddValue("secondary", secondary.ToString());
		node.AddValue("group", inputLockMask.ToString());
		base.Save(node);
		int num = (int)switchStateSecondary;
		node.AddValue("modeMaskSec", num.ToString());
	}

	public override void Load(ConfigNode node)
	{
		primary = new KeyCodeExtended(node.GetValue("primary"));
		secondary = new KeyCodeExtended(node.GetValue("secondary"));
		if (node.HasValue("group"))
		{
			inputLockMask = ulong.Parse(node.GetValue("group"));
		}
		base.Load(node);
		if (node.HasValue("modeMaskSec"))
		{
			switchStateSecondary = (InputBindingModes)int.Parse(node.GetValue("modeMaskSec"));
		}
	}

	public bool GetKey(bool ignoreInputLock = false)
	{
		if (IsLocked() && !ignoreInputLock)
		{
			return false;
		}
		if (ExtendedInput.GetKey(primary) && CompareSwitchState(switchState))
		{
			return true;
		}
		if (ExtendedInput.GetKey(secondary))
		{
			return CompareSwitchState(switchStateSecondary);
		}
		return false;
	}

	public bool GetKeyDown(bool ignoreInputLock = false)
	{
		if (IsLocked() && !ignoreInputLock)
		{
			return false;
		}
		if (ExtendedInput.GetKeyDown(primary) && CompareSwitchState(switchState))
		{
			return true;
		}
		if (ExtendedInput.GetKeyDown(secondary))
		{
			return CompareSwitchState(switchStateSecondary);
		}
		return false;
	}

	public bool GetDoubleTapDown(bool ignoreInputLock = false)
	{
		doubleDown = false;
		if (IsLocked() && !ignoreInputLock)
		{
			return false;
		}
		if ((ExtendedInput.GetKeyDown(primary) && CompareSwitchState(switchState)) || (ExtendedInput.GetKeyDown(secondary) && CompareSwitchState(switchStateSecondary)))
		{
			doubleDown = Time.realtimeSinceStartup < lastKeyDownTime + 0.2f;
			if (lastKeyDownFrameCount != Time.frameCount)
			{
				lastKeyDownTime = Time.realtimeSinceStartup;
				lastKeyDownFrameCount = Time.frameCount;
			}
		}
		return doubleDown;
	}

	public bool GetKeyUp(bool ignoreInputLock = false)
	{
		if (IsLocked() && !ignoreInputLock)
		{
			return false;
		}
		if (ExtendedInput.GetKeyUp(primary) && CompareSwitchState(switchState))
		{
			return true;
		}
		if (ExtendedInput.GetKeyUp(secondary))
		{
			return CompareSwitchState(switchStateSecondary);
		}
		return false;
	}

	public bool GetDoubleTapUp(bool ignoreInputLock = false)
	{
		doubleUp = false;
		if (IsLocked() && !ignoreInputLock)
		{
			return false;
		}
		if ((ExtendedInput.GetKeyUp(primary) && CompareSwitchState(switchState)) || (ExtendedInput.GetKeyUp(secondary) && CompareSwitchState(switchStateSecondary)))
		{
			doubleUp = Time.realtimeSinceStartup < lastKeyUpTime + 0.2f;
			if (lastKeyUpFrameCount != Time.frameCount)
			{
				lastKeyUpTime = Time.realtimeSinceStartup;
				lastKeyUpFrameCount = Time.frameCount;
			}
		}
		return doubleUp;
	}

	public override bool IsNeutral()
	{
		return !GetKey();
	}
}
