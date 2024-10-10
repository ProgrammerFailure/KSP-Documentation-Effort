using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace SoftMasking;

public static class MaterialReplacer
{
	public static List<IMaterialReplacer> _globalReplacers;

	public static IEnumerable<IMaterialReplacer> globalReplacers
	{
		get
		{
			if (_globalReplacers == null)
			{
				_globalReplacers = CollectGlobalReplacers().ToList();
			}
			return _globalReplacers;
		}
	}

	public static IEnumerable<IMaterialReplacer> CollectGlobalReplacers()
	{
		return from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly x) => x.GetTypesSafe())
			where IsMaterialReplacerType(t)
			select TryCreateInstance(t) into t
			where t != null
			select t;
	}

	public static bool IsMaterialReplacerType(Type t)
	{
		if (!(t is TypeBuilder) && !t.IsAbstract && t.IsDefined(typeof(GlobalMaterialReplacerAttribute), inherit: false))
		{
			return typeof(IMaterialReplacer).IsAssignableFrom(t);
		}
		return false;
	}

	public static IMaterialReplacer TryCreateInstance(Type t)
	{
		try
		{
			return (IMaterialReplacer)Activator.CreateInstance(t);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Could not create instance of {0}: {1}", t.Name, ex);
			return null;
		}
	}

	public static IEnumerable<Type> GetTypesSafe(this Assembly asm)
	{
		try
		{
			return asm.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			return ex.Types.Where((Type t) => t != null);
		}
	}
}
