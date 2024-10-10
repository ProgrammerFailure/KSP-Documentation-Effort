using System;
using System.Reflection;
using System.Runtime.Serialization;

public class KSPFontTypeConverter : SerializationBinder
{
	public override Type BindToType(string assemblyName, string typeName)
	{
		Type result = null;
		if (!assemblyName.Contains("TextMeshPro") && !assemblyName.Contains("KSPAssetCompiler"))
		{
			return result;
		}
		assemblyName = Assembly.GetExecutingAssembly().FullName;
		return Type.GetType($"{typeName}, {assemblyName}");
	}
}
