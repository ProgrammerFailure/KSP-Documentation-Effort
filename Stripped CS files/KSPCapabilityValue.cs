using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class KSPCapabilityValue : Attribute
{
	public string capabilityName = "";

	public string format = "";

	public string units = "";

	public string name { get; set; }

	public KSPCapabilityValue(string name)
	{
		this.name = name;
	}
}
