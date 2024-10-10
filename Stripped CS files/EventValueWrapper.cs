using System;
using System.Collections.Generic;
using System.Reflection;

public class EventValueWrapper
{
	public Action[] updateActions;

	public EventValueWrapper()
	{
		List<MethodInfo> list = new List<MethodInfo>();
		List<object> list2 = new List<object>();
		FieldInfo[] fields = GetType().GetFields();
		int i = 0;
		for (int num = fields.Length; i < num; i++)
		{
			FieldInfo fieldInfo = fields[i];
			MethodInfo[] methods = fieldInfo.FieldType.GetMethods();
			int j = 0;
			for (int num2 = methods.Length; j < num2; j++)
			{
				MethodInfo methodInfo = methods[j];
				if (methodInfo.Name == "Update" && methodInfo.GetParameters().Length == 0)
				{
					list.Add(methodInfo);
					list2.Add(fieldInfo.GetValue(this));
				}
			}
		}
		PropertyInfo[] properties = GetType().GetProperties();
		int k = 0;
		for (int num3 = properties.Length; k < num3; k++)
		{
			PropertyInfo propertyInfo = properties[k];
			MethodInfo[] methods2 = propertyInfo.PropertyType.GetMethods();
			int l = 0;
			for (int num4 = methods2.Length; l < num4; l++)
			{
				MethodInfo methodInfo = methods2[l];
				if (methodInfo.Name == "Update" && methodInfo.GetParameters().Length == 0)
				{
					list.Add(methodInfo);
					list2.Add(propertyInfo.GetValue(this, null));
				}
			}
		}
		int count = list.Count;
		updateActions = new Action[count];
		int m = 0;
		for (int count2 = list.Count; m < count2; m++)
		{
			updateActions[m] = (Action)Delegate.CreateDelegate(typeof(Action), list2[m], list[m]);
		}
	}

	public void Update()
	{
		int num = updateActions.Length;
		for (int i = 0; i < num; i++)
		{
			updateActions[i]();
		}
	}
}
