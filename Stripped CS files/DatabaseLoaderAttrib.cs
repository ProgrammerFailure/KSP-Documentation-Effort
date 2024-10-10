using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DatabaseLoaderAttrib : Attribute
{
	public string[] extensions;

	public DatabaseLoaderAttrib(string[] extensions)
	{
		this.extensions = extensions;
	}
}
