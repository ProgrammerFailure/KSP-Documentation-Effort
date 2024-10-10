using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class EffectDefinition : Attribute
{
	public string nodeName;

	public EffectDefinition(string nodeName)
	{
		this.nodeName = nodeName;
	}
}
