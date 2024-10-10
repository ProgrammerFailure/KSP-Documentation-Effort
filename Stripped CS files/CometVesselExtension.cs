using System;
using System.Reflection;
using UnityEngine;

public static class CometVesselExtension
{
	public static T GetCopyOf<T>(this Component comp, T other) where T : Component
	{
		Type type = comp.GetType();
		if (type != other.GetType())
		{
			return null;
		}
		BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (propertyInfo.CanWrite)
			{
				try
				{
					propertyInfo.SetValue(comp, propertyInfo.GetValue(other, null), null);
				}
				catch
				{
				}
			}
		}
		FieldInfo[] fields = type.GetFields(bindingAttr);
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(comp, fieldInfo.GetValue(other));
		}
		return comp as T;
	}
}
