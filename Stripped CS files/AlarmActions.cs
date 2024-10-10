using System.ComponentModel;

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

	public WarpEnum warp = WarpEnum.KillWarp;

	public MessageEnum message = MessageEnum.Yes;

	public bool deleteWhenDone;

	public bool playSound;

	public AlarmActions()
	{
	}

	public AlarmActions(WarpEnum Warp, MessageEnum Message, bool PlaySound, bool DeleteWhenDone)
	{
		warp = Warp;
		message = Message;
		playSound = PlaySound;
		deleteWhenDone = DeleteWhenDone;
	}

	public override string ToString()
	{
		return $"{warp}-{message}-{playSound}-{deleteWhenDone}";
	}

	public AlarmActions Duplicate()
	{
		return new AlarmActions(warp, message, playSound, deleteWhenDone);
	}

	public void Load(ConfigNode node)
	{
		node.TryGetEnum("actionWarp", ref warp, WarpEnum.KillWarp);
		node.TryGetEnum("actionMessage", ref message, MessageEnum.Yes);
		node.TryGetValue("actionSound", ref playSound);
		node.TryGetValue("actionDelete", ref deleteWhenDone);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("actionWarp", warp);
		node.AddValue("actionMessage", message);
		node.AddValue("actionSound", playSound);
		node.AddValue("actionDelete", deleteWhenDone);
	}
}
