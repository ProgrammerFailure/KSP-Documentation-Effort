using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PartInfo : Attribute
{
	public string name;

	public PartInfo()
	{
		name = "";
	}

	public PartInfo(string name)
	{
		this.name = name;
	}
}
