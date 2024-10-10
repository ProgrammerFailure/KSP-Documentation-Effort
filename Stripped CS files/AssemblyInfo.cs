using System;

public class AssemblyInfo
{
	public string name;

	public string path;

	public Version assemblyVersion;

	public bool isDuplicate;

	public AssemblyInfo(string n, string p, Version aV)
	{
		name = n;
		path = p;
		assemblyVersion = aV;
		isDuplicate = false;
	}
}
