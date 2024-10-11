using System.Runtime.CompilerServices;

public class AlarmClockSettings : IConfigNode
{
	public double defaultRawTime;

	public double defaultMapNodeMargin;

	public string soundName;

	public int soundRepeats;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockSettings()
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
