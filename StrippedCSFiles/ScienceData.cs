using System.Runtime.CompilerServices;

public class ScienceData : IConfigNode
{
	public float labValue;

	public float dataAmount;

	public float scienceValueRatio;

	public float baseTransmitValue;

	public float transmitBonus;

	public string subjectID;

	public string title;

	public bool triggered;

	public uint container;

	public string extraResultString;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceData(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceData(float amount, float xmitValue, float xmitBonus, string id, string dataName, bool triggered = false, uint container = 0u)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceData(float amount, float scienceValueRatio, float xmitValue, float xmitBonus, string id, string dataName, bool triggered = false, uint container = 0u, string extraResultString = "")
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceData CopyOf(ScienceData src)
	{
		throw null;
	}
}
