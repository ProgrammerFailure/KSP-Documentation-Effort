using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class BaseFieldList<T, U> : IEnumerable where T : BaseField<U> where U : FieldAttribute
{
	public class ReflectedData
	{
		public List<U> fieldAttributes = new List<U>();

		public List<MemberInfo> fields = new List<MemberInfo>();

		public List<UI_Control> controlAttributes = new List<UI_Control>();

		public List<FieldInfo> controls = new List<FieldInfo>();

		public ReflectedData(Type type, bool ignoreUIControl)
		{
			MemberInfo[] array = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			MemberInfo[] array2 = array;
			int i = 0;
			for (int num = array2.Length; i < num; i++)
			{
				MemberInfo memberInfo = array2[i];
				U[] array3 = (U[])memberInfo.GetCustomAttributes(typeof(U), inherit: true);
				if (array3 != null && array3.Length != 0)
				{
					U item = array3[0];
					PDebug.Log("Field: " + memberInfo.Name, PDebug.DebugLevel.FieldInit);
					FieldInfo field = type.GetField(memberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					fieldAttributes.Add(item);
					fields.Add(field);
				}
				else if (!ignoreUIControl)
				{
					UI_Control[] array4 = (UI_Control[])memberInfo.GetCustomAttributes(typeof(UI_Control), inherit: true);
					if (array4 != null && array4.Length != 0)
					{
						UI_Control item2 = array4[0];
						PDebug.Log("Ctrl: " + memberInfo.Name, PDebug.DebugLevel.FieldInit);
						FieldInfo field2 = type.GetField(memberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						controlAttributes.Add(item2);
						fields.Add(field2);
					}
				}
			}
			array = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			MemberInfo[] array5 = array;
			int j = 0;
			for (int num2 = array5.Length; j < num2; j++)
			{
				MemberInfo memberInfo2 = array5[j];
				U[] array6 = (U[])memberInfo2.GetCustomAttributes(typeof(U), inherit: true);
				if (array6 != null && array6.Length != 0)
				{
					U item3 = array6[0];
					PDebug.Log("Property: " + memberInfo2.Name, PDebug.DebugLevel.FieldInit);
					PropertyInfo property = type.GetProperty(memberInfo2.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					fieldAttributes.Add(item3);
					fields.Add(property);
				}
			}
		}
	}

	public static KSPUtil.ObjectActivator<T> ObjectActivatorLambdaExpression = null;

	public static Dictionary<Type, ReflectedData> reflectedAttributeCache = new Dictionary<Type, ReflectedData>();

	[SerializeField]
	public Type type;

	[SerializeField]
	public List<T> _fields;

	[SerializeField]
	public object host;

	public bool ignoreUIControlWhenCreatingReflectedData;

	public Type ReflectedType => type;

	public int Count => _fields.Count;

	public T this[string fieldName]
	{
		get
		{
			int count = _fields.Count;
			int num = 0;
			T val;
			while (true)
			{
				if (num < count)
				{
					val = _fields[num];
					if (val.name == fieldName)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return val;
		}
	}

	public T this[int index]
	{
		get
		{
			return _fields[index];
		}
		set
		{
			_fields[index] = value;
		}
	}

	public BaseFieldList()
	{
		_fields = new List<T>();
	}

	public BaseFieldList(object host)
		: this(host, ignoreUIControl: false)
	{
	}

	public BaseFieldList(object host, bool ignoreUIControl)
	{
		if (host == null)
		{
			PDebug.Error("Field: Host is null");
			return;
		}
		this.host = host;
		_fields = new List<T>();
		ignoreUIControlWhenCreatingReflectedData = ignoreUIControl;
		CreateList(host);
	}

	public static ReflectedData GetReflectedAttributes(Type type, bool ignoreUIControl)
	{
		if (reflectedAttributeCache.TryGetValue(type, out var value))
		{
			return value;
		}
		value = new ReflectedData(type, ignoreUIControl);
		reflectedAttributeCache[type] = value;
		return value;
	}

	public virtual void CreateList(object instance)
	{
		ReflectedData reflectedAttributes = GetReflectedAttributes(instance.GetType(), ignoreUIControlWhenCreatingReflectedData);
		_fields.Capacity = reflectedAttributes.fields.Count;
		int i = 0;
		for (int count = reflectedAttributes.fields.Count; i < count; i++)
		{
			if (ObjectActivatorLambdaExpression == null)
			{
				ObjectActivatorLambdaExpression = KSPUtil.GetActivator<T>(typeof(T).GetConstructors()[0]);
			}
			T val = ObjectActivatorLambdaExpression(reflectedAttributes.fieldAttributes[i], reflectedAttributes.fields[i], instance);
			val.SetOriginalValue();
			_fields.Add(val);
		}
	}

	public List<T>.Enumerator GetEnumerator()
	{
		return _fields.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _fields.GetEnumerator();
	}

	public object GetValue(string fieldName)
	{
		return this[fieldName]?.GetValue(host);
	}

	public V GetValue<V>(string fieldName)
	{
		T val = this[fieldName];
		if (val != null)
		{
			return val.GetValue<V>(host);
		}
		return default(V);
	}

	public bool SetValue(string fieldName, object value)
	{
		return this[fieldName]?.SetValue(value, host) ?? false;
	}

	public bool SetValue<V>(string fieldName, V value)
	{
		return this[fieldName]?.SetValue(value, host) ?? false;
	}

	public void SetOriginalValue()
	{
		int count = _fields.Count;
		for (int i = 0; i < count; i++)
		{
			_fields[i].SetOriginalValue();
		}
	}

	public void Add(T element)
	{
		_fields.Add(element);
	}
}
[Serializable]
public class BaseFieldList : BaseFieldList<BaseField, KSPField>
{
	public BaseFieldList(UnityEngine.Object host)
		: base((object)host)
	{
	}

	public BaseFieldList(object host)
		: base(host)
	{
	}

	public override void CreateList(object instance)
	{
		base.CreateList(instance);
		ReflectedData reflectedAttributes = BaseFieldList<BaseField, KSPField>.GetReflectedAttributes(instance.GetType(), ignoreUIControlWhenCreatingReflectedData);
		int i = 0;
		for (int count = reflectedAttributes.controls.Count; i < count; i++)
		{
			BaseField item = new BaseField(reflectedAttributes.controlAttributes[i], (FieldInfo)reflectedAttributes.fields[i], instance);
			_fields.Add(item);
		}
	}

	public bool TryGetFieldUIControl<T>(string fieldName, out T control) where T : UI_Control
	{
		if (base[fieldName] != null)
		{
			if (HighLogic.LoadedSceneIsEditor)
			{
				control = base[fieldName].uiControlEditor as T;
			}
			else if (HighLogic.LoadedSceneIsFlight)
			{
				control = base[fieldName].uiControlFlight as T;
			}
			else
			{
				control = null;
			}
			return true;
		}
		control = null;
		return false;
	}

	public void AddBaseFieldList(BaseFieldList addList)
	{
		for (int i = 0; i < addList.Count; i++)
		{
			_fields.Add(addList[i]);
		}
	}

	public void RemoveBaseFieldList(BaseFieldList removeList)
	{
		for (int i = 0; i < removeList.Count; i++)
		{
			_fields.Remove(removeList[i]);
		}
	}

	public bool ReadValue(string valueName, string value)
	{
		int count = _fields.Count;
		int num = 0;
		BaseField baseField;
		while (true)
		{
			if (num < count)
			{
				baseField = _fields[num];
				if (baseField.name == valueName)
				{
					break;
				}
				num++;
				continue;
			}
			PDebug.Error("Field '" + valueName + "' not found.");
			return false;
		}
		baseField.Read(value, host);
		return true;
	}

	public bool ReadValue(string valueName, string value, object host)
	{
		int count = _fields.Count;
		int num = 0;
		BaseField baseField;
		while (true)
		{
			if (num < count)
			{
				baseField = _fields[num];
				if (baseField.name == valueName)
				{
					break;
				}
				num++;
				continue;
			}
			PDebug.Error("Field '" + valueName + "' not found.");
			return false;
		}
		baseField.Read(value, host);
		return true;
	}

	public virtual void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			BaseField baseField = base[value.name];
			if (baseField == null || baseField.hasInterface || baseField.uiControlOnly)
			{
				continue;
			}
			baseField.Read(value.value, host);
			if (baseField.uiControlFlight.GetType() != typeof(UI_Label))
			{
				ConfigNode node2 = node.GetNode(value.name + "_UIFlight");
				if (node2 != null)
				{
					baseField.uiControlFlight.Load(node2, host);
				}
			}
			else if (baseField.uiControlEditor.GetType() != typeof(UI_Label))
			{
				ConfigNode node3 = node.GetNode(value.name + "_UIEditor");
				if (node3 != null)
				{
					baseField.uiControlEditor.Load(node3, host);
				}
			}
		}
		for (int j = 0; j < node.nodes.Count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			BaseField baseField2 = base[configNode.name];
			if (baseField2 == null || !baseField2.hasInterface || baseField2.uiControlOnly)
			{
				continue;
			}
			object value2 = baseField2.GetValue(host);
			if (value2 == null)
			{
				continue;
			}
			if (value2 is IConfigNode configNode2)
			{
				configNode2.Load(configNode);
			}
			if (baseField2.uiControlFlight.GetType() != typeof(UI_Label))
			{
				ConfigNode node4 = node.GetNode(configNode.name + "_UIFlight");
				if (node4 != null)
				{
					baseField2.uiControlFlight.Load(node4, host);
				}
			}
			else if (baseField2.uiControlEditor.GetType() != typeof(UI_Label))
			{
				ConfigNode node5 = node.GetNode(configNode.name + "_UIEditor");
				if (node5 != null)
				{
					baseField2.uiControlEditor.Load(node5, host);
				}
			}
		}
		SetOriginalValue();
	}

	public virtual void Save(ConfigNode node)
	{
		int count = _fields.Count;
		for (int i = 0; i < count; i++)
		{
			BaseField baseField = _fields[i];
			if (!baseField.isPersistant || baseField.uiControlOnly)
			{
				continue;
			}
			if (!baseField.hasInterface)
			{
				node.AddValue(baseField.name, baseField.GetStringValue(host, gui: false));
				continue;
			}
			object value = baseField.GetValue(host);
			if (value != null && value is IConfigNode configNode)
			{
				configNode.Save(node.AddNode(baseField.name));
			}
		}
	}
}
