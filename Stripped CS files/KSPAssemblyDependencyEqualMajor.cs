using System;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class KSPAssemblyDependencyEqualMajor : Attribute
{
	public string name;

	public int versionMajor;

	public int versionMinor;

	public int versionRevision;

	public KSPAssemblyDependencyEqualMajor(string name, int versionMajor, int versionMinor)
	{
		this.name = name;
		this.versionMajor = versionMajor;
		this.versionMinor = versionMinor;
		versionRevision = 0;
	}

	public KSPAssemblyDependencyEqualMajor(string name, int versionMajor, int versionMinor, int versionRevision)
	{
		this.name = name;
		this.versionMajor = versionMajor;
		this.versionMinor = versionMinor;
		this.versionRevision = versionRevision;
	}
}
