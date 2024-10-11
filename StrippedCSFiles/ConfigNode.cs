using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ConfigNode
{
	private class WriteLinkList : List<WriteLinkList.WriteLink>
	{
		public class WriteLink
		{
			public int uid;

			public object target;

			public List<ConfigNode> values;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public WriteLink(int uid, object target)
			{
				throw null;
			}
		}

		private int uid;

		public WriteLink this[object target]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public WriteLinkList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int AssignTarget(object target)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int AssignLink(object target)
		{
			throw null;
		}
	}

	private class ReadLinkList : List<ReadLinkList.LinkTarget>
	{
		public class LinkTarget
		{
			public int linkID;

			public object target;

			public List<LinkEndpoint> links;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public LinkTarget()
			{
				throw null;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			public LinkTarget(int linkID, object target)
			{
				throw null;
			}
		}

		public class LinkEndpoint
		{
			public ReadFieldList.FieldItem field;

			public int fieldIndex;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public LinkEndpoint(ReadFieldList.FieldItem field, int fieldIndex)
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReadLinkList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkTarget GetByLinkID(int linkID)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AssignLinkable(int linkID, object target)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AssignLink(int linkID, ReadFieldList.FieldItem field, int fieldIndex)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Link()
		{
			throw null;
		}
	}

	private class ReadFieldList : List<ReadFieldList.FieldItem>
	{
		public class FieldItem
		{
			public string name;

			public FieldInfo fieldInfo;

			public Persistent kspField;

			public object host;

			public Type fieldType;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public FieldItem(FieldInfo fieldInfo, Persistent kspField, object host)
			{
				throw null;
			}
		}

		private object host;

		public FieldItem this[string name]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReadFieldList(object o)
		{
			throw null;
		}
	}

	[Serializable]
	public class Value
	{
		public string name;

		public string value;

		public string comment;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Value(string name, string value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Value(string name, string value, string vcomment)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Sanitize(bool sanitizeName)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SanitizeString(ref string stringValue, bool isName)
		{
			throw null;
		}
	}

	[Serializable]
	public class ValueList : IEnumerable
	{
		public int listCapacitySteps;

		[SerializeField]
		private List<Value> values;

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Value this[int index]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ValueList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(Value v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IEnumerator GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetValue(string name, string newValue, string newcomment, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetValue(string name, string newValue, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetValue(string name, string newValue, int index, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetValue(string name, string newValue, string newComment, int index, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetValue(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetValue(string name, int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string[] GetValues()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string[] GetValues(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string[] GetValuesStartsWith(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<string> GetValuesList(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Clear()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Remove(Value value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveValue(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveValues(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveValuesStartWith(string startsWith)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SortByName()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string[] DistinctNames()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int CountByName(string name)
		{
			throw null;
		}
	}

	[Serializable]
	public class ConfigNodeList : IEnumerable
	{
		[SerializeField]
		private List<ConfigNode> nodes;

		public int Count
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public ConfigNode this[int index]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNodeList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Add(ConfigNode n)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public IEnumerator GetEnumerator()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode GetNodeID(string id)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode GetNode(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode GetNode(string name, string valueName, string value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode GetNode(string name, int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode[] GetNodes(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode[] GetNodes(string name, string valueName, string value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ConfigNode[] GetNodes()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetNode(string name, ConfigNode newNode, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetNode(string name, ConfigNode newNode, string newComment, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetNode(string name, ConfigNode newNode, int index, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SetNode(string name, ConfigNode newNode, string newComment, int index, bool createIfNotFound = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Remove(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveNode(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveNodesStartWith(string startsWith)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveNodes(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string[] DistinctNames()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int CountByName(string name)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Clear()
		{
			throw null;
		}
	}

	public string name;

	public string id;

	public string comment;

	private ValueList _values;

	private ConfigNodeList _nodes;

	public const string configTabIndent = "\t";

	private static bool removeAfterUse;

	private static int uid;

	private static WriteLinkList writeLinks;

	private static ReadLinkList _readLinks;

	private static List<IPersistenceLoad> iPersistentLoaders;

	public ValueList values
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigNodeList nodes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasData
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CountValues
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int CountNodes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private static ReadLinkList readLinks
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
	public ConfigNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode(string name, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ConfigNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string[] CustomNameSplit(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(ConfigNode node, bool overwrite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CopyTo(ConfigNode node, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode CreateCopy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode Load(string fileFullName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode Load(string fileFullName, bool bypassLocalization)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode LoadFromTextAssetResource(string resourcePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static ConfigNode LoadFromStringArray(string[] cfgData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static ConfigNode LoadFromStringArray(string[] cfgData, bool bypassLocalization)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Save(string fileFullName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Save(string fileFullName, string header)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateConfigFromObject(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateConfigFromObject(object obj, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateConfigFromObject(object obj, int pass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode CreateConfigFromObject(object obj, int pass, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool LoadObjectFromConfig(object obj, ConfigNode node, int pass, bool removeAfterUse)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool LoadObjectFromConfig(object obj, ConfigNode node, int pass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool LoadObjectFromConfig(object obj, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static object CreateObjectFromConfig(string typeName, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static object CreateObjectFromConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T CreateObjectFromConfig<T>(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddData(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValue(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, object value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, string value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, bool value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, byte value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, byte value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, sbyte value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, sbyte value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, char value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, char value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, decimal value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, decimal value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, double value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, float value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, int value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, uint value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, uint value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, long value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, long value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, ulong value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, ulong value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, short value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, short value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, ushort value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, ushort value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector2 value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector2 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector3 value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector3 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector3d value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector3d value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector4 value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Vector4 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Quaternion value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Quaternion value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, QuaternionD value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, QuaternionD value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Matrix4x4 value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Matrix4x4 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Color value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Color32 value, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddValue(string name, Color32 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetValue(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetValue(string name, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetValues(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetValuesList(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetValuesStartsWith(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, string newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, string newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, string newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, string newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, bool newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, bool newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, bool newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, bool newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, byte newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, byte newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, byte newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, byte newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, sbyte newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, sbyte newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, sbyte newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, sbyte newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, char newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, char newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, char newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, char newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, decimal newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, decimal newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, decimal newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, decimal newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, double newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, double newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, double newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, double newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, float newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, float newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, float newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, float newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, int newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, int newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, int newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, int newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, uint newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, uint newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, uint newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, uint newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, long newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, long newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, long newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, long newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ulong newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ulong newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ulong newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ulong newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, short newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, short newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, short newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, short newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ushort newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ushort newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ushort newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, ushort newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector2 newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector2 newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector2 newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector2 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3 newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3 newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3 newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3d newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3d newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3d newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector3d newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector4 newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector4 newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector4 newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Vector4 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Quaternion newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Quaternion newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Quaternion newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Quaternion newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, QuaternionD newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, QuaternionD newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, QuaternionD newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, QuaternionD newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Matrix4x4 newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Matrix4x4 newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Matrix4x4 newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Matrix4x4 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color32 newValue, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color32 newValue, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color32 newValue, string vcomment, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetValue(string name, Color32 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveValue(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveValues(params string[] names)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveValues(string startsWith)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveValuesStartWith(string startsWith)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNodeID(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode AddNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode AddNode(string name, string vcomment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode AddNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode AddNode(string name, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetNodeID(string id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetNode(string name, string valueName, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode GetNode(string name, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode[] GetNodes(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode[] GetNodes(string name, string valueName, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode[] GetNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetNode(string name, ConfigNode newNode, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetNode(string name, ConfigNode newNode, int index, bool createIfNotFound = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNode(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNodesStartWith(string startsWith)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveNodes(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearNodes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("ConfigNode.CompileConfig has been moved to GameDatabase.CompileConfig.", true)]
	public static void CompileConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Merge(ConfigNode mergeTo, ConfigNode mergeFrom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MergeValue(ConfigNode node, string[] id, int index, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MergeNode(ConfigNode node, string[] id, int index, ConfigNode value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string DebugStringArray(string[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WriteObject(object obj, ConfigNode node, int pass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool WriteValue(string fieldName, Type fieldType, object value, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool WriteStruct(string fieldName, FieldInfo fieldInfo, object value, ConfigNode node, int pass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool WriteArrayTypes(string fieldName, Type fieldType, object value, ConfigNode node, Persistent kspField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool HasAttribute(Type type, Type attributeType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool WriteArray(string fieldName, Type fieldType, object value, ConfigNode node, Persistent kspField)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static object CreateObject(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static object CreateNodeObject(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadObject(object obj, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadObject(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadObjectCreateNew(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadObjectCreateMonoBehaviour(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static object ReadValue(Type fieldType, string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadValueArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadComponentArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadObjectArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ReadLinkArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string RetrieveFieldName(MemberInfo field, Persistent attr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ToStringRecursive(string indent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CopyToRecursive(ConfigNode node, bool overwrite = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsValue(Type fieldType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsArrayType(Type fieldType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsArray(Type fieldType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsGenericArray(Type fieldType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool IsEnumerable(Type fieldType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Type GetElementType(Type seqType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Type FindIEnumerable(Type seqType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string CleanupInput(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteBoolArray(bool[] flags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteEnumIntArray<T>(T[] flags) where T : struct
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteStringArray(string[] strings)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector2 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector3d vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteVector(Vector4 vector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(Quaternion quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteQuaternion(QuaternionD quaternion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteMatrix4x4(Matrix4x4 matrix)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteColor(Color32 color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string WriteEnum(Enum en)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector2 ParseVector2(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 ParseVector3(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3d ParseVector3D(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector4 ParseVector4(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Quaternion ParseQuaternion(string quaternionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static QuaternionD ParseQuaternionD(string quaternionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Matrix4x4 ParseMatrix4x4(string quaternionString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color ParseColor(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CheckAndParseColor(string colorString, out Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Color32 ParseColor32(string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Enum ParseEnum(Type enumType, string vectorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static ConfigNode RecurseFormat(List<string[]> cfg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RecurseFormat(List<string[]> cfg, ref int index, ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool NextLineIsOpenBrace(List<string[]> cfg, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool NextLineIsCloseBrace(List<string[]> cfg, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<string[]> PreFormatConfig(string[] cfgData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static string[] CustomEqualSplit(string str)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WriteNode(StreamWriter sw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WriteRootNode(StreamWriter sw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WriteNodeString(StreamWriter sw, string indent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode Parse(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetNode(string name, ref ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref string[] value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref double value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref int value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref uint value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref long value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref ulong value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector3 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector3d value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector2 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector2d value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector4 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Vector4d value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Quaternion value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref QuaternionD value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Rect value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Color32 value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasValues(params string[] values)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetValue(string name, ref Guid value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetEnum<T>(string name, ref T value, T defaultValue) where T : IComparable, IFormattable, IConvertible
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool TryGetEnum(string name, Type enumType, ref Enum value)
	{
		throw null;
	}
}
