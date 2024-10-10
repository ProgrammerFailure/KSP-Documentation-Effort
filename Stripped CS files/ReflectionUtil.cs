using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ReflectionUtil
{
	public class AttributedType<Tatt>
	{
		public Type Type { get; set; }

		public Tatt Attribute { get; set; }

		public AttributedType(Type t, Tatt attrib)
		{
			Type = t;
			Attribute = attrib;
		}
	}

	public static List<AttributedType<Tatt>> GetAttributedTypesInAssemblies<Tbase, Tatt>(List<Assembly> assemblies) where Tbase : class where Tatt : Attribute
	{
		List<AttributedType<Tatt>> list = new List<AttributedType<Tatt>>();
		int i = 0;
		for (int count = assemblies.Count; i < count; i++)
		{
			try
			{
				Type[] types = assemblies[i].GetTypes();
				int j = 0;
				for (int num = types.Length; j < num; j++)
				{
					if (types[j].IsSubclassOf(typeof(Tbase)) || types[j] == typeof(Tbase))
					{
						Tatt[] array = (Tatt[])types[j].GetCustomAttributes(typeof(Tatt), inherit: true);
						if (array.Length != 0)
						{
							list.Add(new AttributedType<Tatt>(types[j], array[0]));
						}
					}
				}
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				if (ex is ReflectionTypeLoadException)
				{
					text += "\n\nAdditional information about this exception:";
					Exception[] loaderExceptions = ((ReflectionTypeLoadException)ex).LoaderExceptions;
					foreach (Exception ex2 in loaderExceptions)
					{
						text = text + "\n\n " + ex2.ToString();
					}
				}
				Debug.LogError("Exception when getting assembly attributes: " + text);
			}
		}
		Debug.Log("[ReflectionUtil]: Found " + list.Count + " types with " + typeof(Tatt).Name + " attribute in " + assemblies.Count + " assemblies.");
		return list;
	}
}
