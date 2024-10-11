using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BaseFieldList<R, K> : IEnumerable where R : BaseField<K> where K : FieldAttribute
{
	public class ReflectedData
	{
		public List<K> fieldAttributes;

		public List<MemberInfo> fields;

		public List<UI_Control> controlAttributes;

		public List<FieldInfo> controls;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReflectedData(Type type, bool ignoreUIControl)
		{
			throw null;
		}
	}

	protected static KSPUtil.ObjectActivator<R> ObjectActivatorLambdaExpression;

	protected static Dictionary<Type, ReflectedData> reflectedAttributeCache;

	[SerializeField]
	private Type type;

	[SerializeField]
	protected List<R> _fields;

	[SerializeField]
	protected object host;

	protected bool ignoreUIControlWhenCreatingReflectedData;

	public Type ReflectedType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public R this[string fieldName]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public R this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseFieldList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseFieldList(object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseFieldList(object host, bool ignoreUIControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BaseFieldList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static ReflectedData GetReflectedAttributes(Type type, bool ignoreUIControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CreateList(object instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<R>.Enumerator GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public object GetValue(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetValue<T>(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string fieldName, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue<T>(string fieldName, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOriginalValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Add(R element)
	{
		throw null;
	}
}
[Serializable]
public class BaseFieldList : BaseFieldList<BaseField, KSPField>
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseFieldList(UnityEngine.Object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BaseFieldList(object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void CreateList(object instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetFieldUIControl<T>(string fieldName, out T control) where T : UI_Control
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddBaseFieldList(BaseFieldList addList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveBaseFieldList(BaseFieldList removeList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadValue(string valueName, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadValue(string valueName, string value, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}
}
