using System;
using System.Collections.Generic;
using System.Reflection;

public class Capability
{
	public class Value
	{
		public object classObject;

		public FieldInfo field;

		public PropertyInfo property;

		public KSPCapabilityValue cap;

		public string name => cap.name;

		public string value { get; set; }

		public Value(object classObject, KSPCapabilityValue capability, FieldInfo field)
		{
			this.classObject = classObject;
			cap = capability;
			this.field = field;
			property = null;
			Update();
		}

		public Value(object classObject, KSPCapabilityValue capability, PropertyInfo property)
		{
			this.classObject = classObject;
			cap = capability;
			this.property = property;
			field = null;
			Update();
		}

		public void Update()
		{
			if (classObject == null)
			{
				value = "Obj null";
				return;
			}
			object obj = null;
			if (field != null)
			{
				obj = field.GetValue(classObject);
			}
			else
			{
				if (!(property != null))
				{
					value = "No info";
					return;
				}
				obj = property.GetValue(classObject, null);
			}
			Type type = obj.GetType();
			if (type == typeof(string))
			{
				value = (string)obj;
			}
			else if (type == typeof(bool))
			{
				value = ((bool)obj).ToString();
			}
			else if (type == typeof(int))
			{
				if (cap.format != string.Empty)
				{
					value = ((int)obj).ToString(cap.format);
				}
				else
				{
					value = ((int)obj).ToString();
				}
			}
			else if (type == typeof(double))
			{
				if (cap.format != string.Empty)
				{
					value = ((double)obj).ToString(cap.format);
				}
				else
				{
					value = ((double)obj).ToString("G17");
				}
			}
			else
			{
				if (!(type == typeof(float)))
				{
					value = "Unsupported";
					return;
				}
				if (cap.format != string.Empty)
				{
					value = ((float)obj).ToString(cap.format);
				}
				else
				{
					value = ((float)obj).ToString("G9");
				}
			}
			value += cap.units;
		}

		public override string ToString()
		{
			return name + ": " + value;
		}
	}

	public KSPCapability capability;

	public string category => capability.category;

	public string capabilityName => capability.capabilityName;

	public List<Value> values { get; set; }

	public Capability(object classObject, KSPCapability capability)
	{
		this.capability = capability;
		values = new List<Value>();
		Type type = classObject.GetType();
		FieldInfo[] fields = type.GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			KSPCapabilityValue[] array = (KSPCapabilityValue[])fieldInfo.GetCustomAttributes(typeof(KSPCapabilityValue), inherit: true);
			if (array == null || array.Length == 0)
			{
				continue;
			}
			KSPCapabilityValue[] array2 = array;
			foreach (KSPCapabilityValue kSPCapabilityValue in array2)
			{
				if (kSPCapabilityValue.capabilityName == capability.capabilityName)
				{
					values.Add(new Value(classObject, kSPCapabilityValue, fieldInfo));
				}
			}
		}
		PropertyInfo[] properties = type.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			if (!propertyInfo.CanRead)
			{
				continue;
			}
			KSPCapabilityValue[] array3 = (KSPCapabilityValue[])propertyInfo.GetCustomAttributes(typeof(KSPCapabilityValue), inherit: true);
			if (array3 == null || array3.Length == 0)
			{
				continue;
			}
			KSPCapabilityValue[] array2 = array3;
			foreach (KSPCapabilityValue kSPCapabilityValue2 in array2)
			{
				if (kSPCapabilityValue2.capabilityName == capability.capabilityName)
				{
					values.Add(new Value(classObject, kSPCapabilityValue2, propertyInfo));
				}
			}
		}
	}

	public void Update()
	{
		foreach (Value value in values)
		{
			value.Update();
		}
	}

	public override string ToString()
	{
		string text = capabilityName + " (" + category + ")";
		foreach (Value value in values)
		{
			text = text + "\n\t" + value.ToString();
		}
		return text;
	}

	public static List<Capability> GetCapabilities(object classObject)
	{
		List<Capability> list = new List<Capability>();
		KSPCapability[] array = (KSPCapability[])classObject.GetType().GetCustomAttributes(typeof(KSPCapability), inherit: true);
		if (array != null && array.Length != 0)
		{
			KSPCapability[] array2 = array;
			foreach (KSPCapability kSPCapability in array2)
			{
				list.Add(new Capability(classObject, kSPCapability));
			}
			return list;
		}
		return list;
	}

	public static List<Capability> GetCapabilities(List<object> classObjects)
	{
		List<Capability> list = new List<Capability>();
		foreach (object classObject in classObjects)
		{
			List<Capability> capabilities = GetCapabilities(classObject);
			if (capabilities.Count != 0)
			{
				list.AddRange(capabilities);
			}
		}
		return list;
	}
}
