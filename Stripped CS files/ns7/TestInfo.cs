using System;

namespace ns7;

[AttributeUsage(AttributeTargets.Method)]
public class TestInfo : Attribute
{
	public string Name { get; set; }

	public string Author { get; set; }

	public string SinceVersion { get; set; }

	public TestInfo(string name)
	{
		Name = name;
		Author = "UNKNOWN";
		SinceVersion = "UNKNOWN";
	}

	public TestInfo()
	{
	}

	public override string ToString()
	{
		return $"{Name} by {Author} (added in v{SinceVersion})";
	}
}
