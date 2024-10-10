using System.Collections.Generic;

namespace ModuleWheels;

public class WheelSubsystems
{
	public List<WheelSubsystem> subs;

	public WheelSubsystem.SystemTypes types;

	public Callback<WheelSubsystems> OnModified;

	public WheelSubsystems()
	{
		subs = new List<WheelSubsystem>();
		types = WheelSubsystem.SystemTypes.None;
		OnModified = delegate
		{
		};
	}

	public void AddSubsystem(WheelSubsystem d)
	{
		subs.AddUnique(d);
		types |= d.type;
		OnModified(this);
	}

	public void RemoveSubsystem(WheelSubsystem d)
	{
		subs.Remove(d);
		ResetFlags();
		OnModified(this);
	}

	public void ResetFlags()
	{
		types = WheelSubsystem.SystemTypes.None;
		int count = subs.Count;
		while (count-- > 0)
		{
			types |= subs[count].type;
		}
	}

	public bool HasType(WheelSubsystem.SystemTypes t)
	{
		return (types & t) != 0;
	}

	public bool Contains(WheelSubsystem s)
	{
		return subs.Contains(s);
	}

	public void GetReasons(ref string o, string separator = ", ")
	{
		if (o == null)
		{
			o = "";
		}
		int i = 0;
		for (int count = subs.Count; i < count; i++)
		{
			o += subs[i].reason;
			if (i > 0)
			{
				o += separator;
			}
		}
	}
}
