using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Serialization;

namespace EdyCommonTools;

public static class ObjectUtility
{
	public delegate TResult Func<T1, TResult>(T1 arg1);

	public static Dictionary<Type, Delegate> _cachedIL = new Dictionary<Type, Delegate>();

	public static T CloneObject<T>(T source)
	{
		if (!typeof(T).IsSerializable)
		{
			throw new ArgumentException("The type must be serializable.", "source");
		}
		if (source == null)
		{
			return default(T);
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		Stream stream = new MemoryStream();
		using (stream)
		{
			xmlSerializer.Serialize(stream, source);
			stream.Seek(0L, SeekOrigin.Begin);
			return (T)xmlSerializer.Deserialize(stream);
		}
	}

	public static T CloneObjectFast<T>(T myObject)
	{
		Delegate value = null;
		if (!_cachedIL.TryGetValue(typeof(T), out value))
		{
			DynamicMethod dynamicMethod = new DynamicMethod("DoClone", typeof(T), new Type[1] { typeof(T) }, restrictedSkipVisibility: true);
			ConstructorInfo constructor = myObject.GetType().GetConstructor(new Type[0]);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			iLGenerator.Emit(OpCodes.Newobj, constructor);
			iLGenerator.Emit(OpCodes.Stloc_0);
			FieldInfo[] fields = myObject.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo field in fields)
			{
				iLGenerator.Emit(OpCodes.Ldloc_0);
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldfld, field);
				iLGenerator.Emit(OpCodes.Stfld, field);
			}
			iLGenerator.Emit(OpCodes.Ldloc_0);
			iLGenerator.Emit(OpCodes.Ret);
			value = dynamicMethod.CreateDelegate(typeof(Func<T, T>));
			_cachedIL.Add(typeof(T), value);
		}
		return ((Func<T, T>)value)(myObject);
	}

	public static void CopyObjectOverwrite<T>(T source, ref T target)
	{
		FieldInfo[] fields = source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			fieldInfo.SetValue(target, fieldInfo.GetValue(source));
		}
	}
}
