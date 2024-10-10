using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class KSPCapability : Attribute
{
	public string capabilityName = "";

	public string category { get; set; }

	public KSPCapability(string category)
	{
		this.category = category;
	}
}
