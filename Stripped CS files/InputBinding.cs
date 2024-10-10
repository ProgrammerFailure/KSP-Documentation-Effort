using System;

public class InputBinding : IConfigNode, ICloneable
{
	public InputBindingModes switchState = InputBindingModes.Any;

	public static bool linRotState;

	public bool useSwitchState;

	public ulong inputLockMask;

	public virtual void Load(ConfigNode node)
	{
		if (node.HasValue("switchState"))
		{
			switchState = (InputBindingModes)Enum.Parse(typeof(InputBindingModes), node.GetValue("switchState"));
		}
		if (node.HasValue("modeMask"))
		{
			switchState = (InputBindingModes)int.Parse(node.GetValue("modeMask"));
		}
	}

	public virtual void Save(ConfigNode node)
	{
		int num = (int)switchState;
		node.AddValue("modeMask", num.ToString());
	}

	public bool CompareSwitchState(InputBindingModes switchSt)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (FlightUIModeController.Instance != null)
			{
				InputBindingModes inputBindingModes = InputBindingModes.None;
				FlightUIMode mode = FlightUIModeController.Instance.Mode;
				inputBindingModes = ((mode == FlightUIMode.STAGING || mode != FlightUIMode.DOCKING) ? InputBindingModes.Staging : (linRotState ? InputBindingModes.Docking_Rotation : InputBindingModes.Docking_Translation));
				return (inputBindingModes & switchSt) != 0;
			}
			return true;
		}
		return true;
	}

	public bool IsLocked()
	{
		return (InputLockManager.LockMask & inputLockMask) > 0L;
	}

	public bool IsUnlocked()
	{
		return !IsLocked();
	}

	public virtual bool IsNeutral()
	{
		return true;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
