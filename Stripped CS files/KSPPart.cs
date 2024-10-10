using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class KSPPart : Attribute
{
	public DefaultIcons stageIcon;

	public KSPPart()
	{
		stageIcon = DefaultIcons.CUSTOM;
	}
}
