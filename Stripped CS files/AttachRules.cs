using System;

[Serializable]
public class AttachRules
{
	public bool stack;

	public bool srfAttach;

	public bool allowStack;

	public bool allowSrfAttach;

	public bool allowCollision;

	public bool allowDock;

	public bool allowRotate;

	public bool allowRoot;

	public bool StackOrSurfaceAttachable
	{
		get
		{
			if (!srfAttach)
			{
				return stack;
			}
			return true;
		}
	}

	public static AttachRules Parse(string value)
	{
		string[] array = value.Split(',');
		if (array.Length < 5)
		{
			throw new Exception("Attach rules are 8 ints, either ones or zeros, separated by commas!");
		}
		AttachRules attachRules = new AttachRules();
		attachRules.stack = array[0] == "1";
		attachRules.srfAttach = array[1] == "1";
		attachRules.allowStack = array[2] == "1";
		attachRules.allowSrfAttach = array[3] == "1";
		attachRules.allowCollision = array[4] == "1";
		if (array.Length >= 6)
		{
			attachRules.allowDock = array[5] == "1";
		}
		else
		{
			attachRules.allowDock = false;
		}
		if (array.Length >= 7)
		{
			attachRules.allowRotate = array[6] == "1";
		}
		else
		{
			attachRules.allowRotate = false;
		}
		if (array.Length >= 8)
		{
			attachRules.allowRoot = array[7] == "1";
		}
		else
		{
			attachRules.allowRoot = true;
		}
		return attachRules;
	}

	public string String()
	{
		return (stack ? "1" : "0") + "," + (srfAttach ? "1" : "0") + "," + (allowStack ? "1" : "0") + "," + (allowSrfAttach ? "1" : "0") + "," + (allowCollision ? "1" : "0") + "," + (allowDock ? "1" : "0") + "," + (allowRotate ? "1" : "0");
	}
}
