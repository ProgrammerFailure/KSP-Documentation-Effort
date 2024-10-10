using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class KSPModule : Attribute
{
	public string guiName;

	public KSPModule(string guiName)
	{
		this.guiName = guiName;
	}
}
