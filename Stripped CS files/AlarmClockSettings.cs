public class AlarmClockSettings : IConfigNode
{
	public double defaultRawTime = 300.0;

	public double defaultMapNodeMargin = 60.0;

	public string soundName = "Classic";

	public int soundRepeats = 3;

	public void Load(ConfigNode node)
	{
		node.TryGetValue("defaultRawTime", ref defaultRawTime);
		node.TryGetValue("defaultManeuverMargin", ref defaultMapNodeMargin);
		node.TryGetValue("soundName", ref soundName);
		node.TryGetValue("soundRepeats", ref soundRepeats);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("defaultRawTime", defaultRawTime);
		node.AddValue("defaultManeuverMargin", defaultMapNodeMargin);
		node.AddValue("soundName", soundName);
		node.AddValue("soundRepeats", soundRepeats);
	}
}
