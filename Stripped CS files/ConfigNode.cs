using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using ns9;
using UnityEngine;

public class ConfigNode
{
	public class WriteLinkList : List<WriteLinkList.WriteLink>
	{
		public class WriteLink
		{
			public int uid;

			public object target;

			public List<ConfigNode> values;

			public WriteLink(int uid, object target)
			{
				this.uid = uid;
				this.target = target;
				values = new List<ConfigNode>();
			}
		}

		public int uid;

		public WriteLink this[object target]
		{
			get
			{
				int count = base.Count;
				do
				{
					if (count-- <= 0)
					{
						return null;
					}
				}
				while (this[count].target != target);
				return this[count];
			}
		}

		public WriteLinkList()
		{
			uid = 0;
		}

		public int AssignTarget(object target)
		{
			WriteLink writeLink = this[target];
			if (writeLink == null)
			{
				writeLink = new WriteLink(uid++, target);
				Add(writeLink);
			}
			return writeLink.uid;
		}

		public int AssignLink(object target)
		{
			WriteLink writeLink = this[target];
			if (writeLink == null)
			{
				writeLink = new WriteLink(uid++, target);
				Add(writeLink);
			}
			return writeLink.uid;
		}
	}

	public class ReadLinkList : List<ReadLinkList.LinkTarget>
	{
		public class LinkTarget
		{
			public int linkID;

			public object target;

			public List<LinkEndpoint> links;

			public LinkTarget()
			{
				links = new List<LinkEndpoint>();
			}

			public LinkTarget(int linkID, object target)
			{
				this.linkID = linkID;
				this.target = target;
				links = new List<LinkEndpoint>();
			}
		}

		public class LinkEndpoint
		{
			public ReadFieldList.FieldItem field;

			public int fieldIndex;

			public LinkEndpoint(ReadFieldList.FieldItem field, int fieldIndex)
			{
				this.field = field;
				this.fieldIndex = fieldIndex;
			}
		}

		public LinkTarget GetByLinkID(int linkID)
		{
			int count = base.Count;
			do
			{
				if (count-- <= 0)
				{
					return null;
				}
			}
			while (base[count].linkID != linkID);
			return base[count];
		}

		public void AssignLinkable(int linkID, object target)
		{
			LinkTarget byLinkID = GetByLinkID(linkID);
			if (byLinkID == null)
			{
				Add(new LinkTarget(linkID, target));
			}
			else
			{
				byLinkID.target = target;
			}
		}

		public void AssignLink(int linkID, ReadFieldList.FieldItem field, int fieldIndex)
		{
			LinkTarget linkTarget = GetByLinkID(linkID);
			if (linkTarget == null)
			{
				linkTarget = new LinkTarget(linkID, null);
				Add(linkTarget);
			}
			linkTarget.links.Add(new LinkEndpoint(field, fieldIndex));
		}

		public void Link()
		{
			int count = base.Count;
			for (int i = 0; i < count; i++)
			{
				LinkTarget linkTarget = base[i];
				if (linkTarget.target == null)
				{
					continue;
				}
				int count2 = linkTarget.links.Count;
				for (int j = 0; j < count2; j++)
				{
					LinkEndpoint linkEndpoint = linkTarget.links[j];
					if (linkEndpoint.fieldIndex == -1)
					{
						linkEndpoint.field.fieldInfo.SetValue(linkEndpoint.field.host, linkTarget.target);
					}
					else if (linkEndpoint.field.fieldInfo.GetValue(linkEndpoint.field.host) is IList list)
					{
						list[linkEndpoint.fieldIndex] = linkTarget.target;
					}
				}
			}
		}
	}

	public class ReadFieldList : List<ReadFieldList.FieldItem>
	{
		public class FieldItem
		{
			public string name;

			public FieldInfo fieldInfo;

			public Persistent kspField;

			public object host;

			public Type fieldType;

			public FieldItem(FieldInfo fieldInfo, Persistent kspField, object host)
			{
				name = RetrieveFieldName(fieldInfo, kspField);
				this.fieldInfo = fieldInfo;
				this.kspField = kspField;
				this.host = host;
				fieldType = fieldInfo.FieldType;
			}
		}

		public object host;

		public FieldItem this[string name]
		{
			get
			{
				int count = base.Count;
				do
				{
					if (count-- <= 0)
					{
						return null;
					}
				}
				while (!(base[count].name == name));
				return base[count];
			}
		}

		public ReadFieldList(object o)
		{
			host = o;
			Type type = o.GetType();
			MemberInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			MemberInfo[] array = fields;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				MemberInfo memberInfo = array[i];
				Persistent[] array2 = (Persistent[])memberInfo.GetCustomAttributes(typeof(Persistent), inherit: true);
				int num2 = array2.Length;
				for (int j = 0; j < num2; j++)
				{
					Persistent kspField = array2[j];
					FieldInfo field = type.GetField(memberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					Add(new FieldItem(field, kspField, host));
				}
			}
		}
	}

	[Serializable]
	public class Value
	{
		public string name;

		public string value;

		public string comment;

		public Value(string name, string value)
		{
			this.name = name;
			this.value = value;
		}

		public Value(string name, string value, string vcomment)
		{
			this.name = name;
			this.value = value;
			comment = vcomment;
		}

		public void Sanitize(bool sanitizeName)
		{
			if (sanitizeName)
			{
				SanitizeString(ref name, isName: true);
			}
			SanitizeString(ref value, isName: false);
		}

		public void SanitizeString(ref string stringValue, bool isName)
		{
			bool flag = false;
			for (int num = stringValue.Length - 1; num >= 0; num--)
			{
				switch (stringValue[num])
				{
				case '{':
				case '}':
					flag = true;
					break;
				case '=':
					if (isName)
					{
						flag = true;
					}
					break;
				}
				if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				stringValue = stringValue.Replace('{', '[');
				stringValue = stringValue.Replace('}', ']');
				if (isName)
				{
					stringValue = stringValue.Replace('=', '-');
				}
			}
		}
	}

	[Serializable]
	public class ValueList : IEnumerable
	{
		public int listCapacitySteps = 10;

		[SerializeField]
		public List<Value> values;

		public int Count => values.Count;

		public Value this[int index] => values[index];

		public ValueList()
		{
			values = new List<Value>(listCapacitySteps);
		}

		public void Add(Value v)
		{
			v.Sanitize(sanitizeName: true);
			if (values.Count > values.Capacity - 1)
			{
				values.Capacity += listCapacitySteps;
			}
			values.Add(v);
		}

		public IEnumerator GetEnumerator()
		{
			return values.GetEnumerator();
		}

		public bool SetValue(string name, string newValue, string newcomment, bool createIfNotFound = false)
		{
			return SetValue(name, newValue, newcomment, 0, createIfNotFound);
		}

		public bool SetValue(string name, string newValue, bool createIfNotFound = false)
		{
			return SetValue(name, newValue, null, 0, createIfNotFound);
		}

		public bool SetValue(string name, string newValue, int index, bool createIfNotFound = false)
		{
			return SetValue(name, newValue, null, index, createIfNotFound);
		}

		public bool SetValue(string name, string newValue, string newComment, int index, bool createIfNotFound = false)
		{
			int num = 0;
			int num2 = 0;
			int count = values.Count;
			Value value;
			while (true)
			{
				if (num2 < count)
				{
					value = values[num2];
					if (value.name == name)
					{
						if (num == index)
						{
							break;
						}
						num++;
					}
					num2++;
					continue;
				}
				if (createIfNotFound)
				{
					values.Add(new Value(name, newValue, newComment));
				}
				return false;
			}
			value.value = newValue;
			if (newComment != null)
			{
				value.comment = newComment;
			}
			value.Sanitize(sanitizeName: false);
			return true;
		}

		public string GetValue(string name)
		{
			int num = 0;
			int count = values.Count;
			Value value;
			while (true)
			{
				if (num < count)
				{
					value = values[num];
					if (value.name == name)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return value.value;
		}

		public string GetValue(string name, int index)
		{
			int num = 0;
			int num2 = 0;
			int count = values.Count;
			Value value;
			while (true)
			{
				if (num2 < count)
				{
					value = values[num2];
					if (value.name == name)
					{
						if (num == index)
						{
							break;
						}
						num++;
					}
					num2++;
					continue;
				}
				return null;
			}
			return value.value;
		}

		public string[] GetValues()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				Value value = values[i];
				list.Add(value.value);
			}
			return list.ToArray();
		}

		public string[] GetValues(string name)
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				Value value = values[i];
				if (value.name == name)
				{
					list.Add(value.value);
				}
			}
			return list.ToArray();
		}

		public string[] GetValuesStartsWith(string name)
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				Value value = values[i];
				if (value.name.StartsWith(name))
				{
					list.Add(value.value);
				}
			}
			return list.ToArray();
		}

		public List<string> GetValuesList(string name)
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				Value value = values[i];
				if (value.name == name)
				{
					list.Add(value.value);
				}
			}
			return list;
		}

		public bool Contains(string name)
		{
			int num = 0;
			int count = values.Count;
			while (true)
			{
				if (num < count)
				{
					if (values[num].name == name)
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}

		public void Clear()
		{
			values.Clear();
		}

		public void Remove(Value value)
		{
			int count = values.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (values[count] != value);
			values.RemoveAt(count);
		}

		public void RemoveValue(string name)
		{
			int count = values.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (!(values[count].name == name));
			values.RemoveAt(count);
		}

		public void RemoveValues(string name)
		{
			int count = values.Count;
			while (count-- > 0)
			{
				if (values[count].name == name)
				{
					values.RemoveAt(count);
				}
			}
		}

		public void RemoveValuesStartWith(string startsWith)
		{
			int count = values.Count;
			while (count-- > 0)
			{
				if (values[count].name.StartsWith(startsWith))
				{
					values.RemoveAt(count);
				}
			}
		}

		public void SortByName()
		{
			values.Sort((Value a, Value b) => a.name.CompareTo(b.name));
		}

		public string[] DistinctNames()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = values.Count; i < count; i++)
			{
				Value value = values[i];
				if (!list.Contains(value.name))
				{
					list.Add(value.name);
				}
			}
			return list.ToArray();
		}

		public int CountByName(string name)
		{
			int num = 0;
			int count = values.Count;
			while (count-- > 0)
			{
				if (values[count].name == name)
				{
					num++;
				}
			}
			return num;
		}
	}

	[Serializable]
	public class ConfigNodeList : IEnumerable
	{
		[SerializeField]
		public List<ConfigNode> nodes;

		public int Count => nodes.Count;

		public ConfigNode this[int index] => nodes[index];

		public ConfigNodeList()
		{
			nodes = new List<ConfigNode>();
		}

		public void Add(ConfigNode n)
		{
			nodes.Add(n);
		}

		public IEnumerator GetEnumerator()
		{
			return nodes.GetEnumerator();
		}

		public ConfigNode GetNodeID(string id)
		{
			int num = 0;
			int count = Count;
			ConfigNode configNode;
			while (true)
			{
				if (num < count)
				{
					configNode = this[num];
					if (configNode.id == id)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return configNode;
		}

		public ConfigNode GetNode(string name)
		{
			int num = 0;
			int count = Count;
			ConfigNode configNode;
			while (true)
			{
				if (num < count)
				{
					configNode = this[num];
					if (configNode.name == name)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return configNode;
		}

		public ConfigNode GetNode(string name, string valueName, string value)
		{
			int num = 0;
			int count = Count;
			ConfigNode configNode;
			while (true)
			{
				if (num < count)
				{
					configNode = this[num];
					if (configNode.name == name && configNode.GetValue(valueName) == value)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return configNode;
		}

		public ConfigNode GetNode(string name, int index)
		{
			int num = 0;
			int num2 = 0;
			int count = Count;
			ConfigNode configNode;
			while (true)
			{
				if (num2 < count)
				{
					configNode = this[num2];
					if (configNode.name == name)
					{
						if (num == index)
						{
							break;
						}
						num++;
					}
					num2++;
					continue;
				}
				return null;
			}
			return configNode;
		}

		public ConfigNode[] GetNodes(string name)
		{
			List<ConfigNode> list = new List<ConfigNode>();
			int i = 0;
			for (int count = Count; i < count; i++)
			{
				ConfigNode configNode = this[i];
				if (configNode.name == name)
				{
					list.Add(configNode);
				}
			}
			return list.ToArray();
		}

		public ConfigNode[] GetNodes(string name, string valueName, string value)
		{
			List<ConfigNode> list = new List<ConfigNode>();
			int i = 0;
			for (int count = Count; i < count; i++)
			{
				ConfigNode configNode = this[i];
				if (configNode.name == name && configNode.GetValue(valueName) == value)
				{
					list.Add(configNode);
				}
			}
			return list.ToArray();
		}

		public ConfigNode[] GetNodes()
		{
			List<ConfigNode> list = new List<ConfigNode>();
			int i = 0;
			for (int count = Count; i < count; i++)
			{
				ConfigNode item = this[i];
				list.Add(item);
			}
			return list.ToArray();
		}

		public bool SetNode(string name, ConfigNode newNode, bool createIfNotFound = false)
		{
			return SetNode(name, newNode, null, 0, createIfNotFound);
		}

		public bool SetNode(string name, ConfigNode newNode, string newComment, bool createIfNotFound = false)
		{
			return SetNode(name, newNode, newComment, 0, createIfNotFound);
		}

		public bool SetNode(string name, ConfigNode newNode, int index, bool createIfNotFound = false)
		{
			return SetNode(name, newNode, null, index, createIfNotFound);
		}

		public bool SetNode(string name, ConfigNode newNode, string newComment, int index, bool createIfNotFound = false)
		{
			int num = 0;
			int num2 = 0;
			int count = Count;
			ConfigNode configNode;
			while (true)
			{
				if (num2 < count)
				{
					configNode = this[num2];
					if (configNode.name == name)
					{
						if (num == index)
						{
							break;
						}
						num++;
					}
					num2++;
					continue;
				}
				if (createIfNotFound)
				{
					newNode.name = name;
					nodes.Add(newNode);
					if (!string.IsNullOrEmpty(newComment))
					{
						newNode.comment = newComment;
					}
					return true;
				}
				return false;
			}
			configNode.ClearData();
			newNode.CopyTo(configNode);
			if (!string.IsNullOrEmpty(newComment))
			{
				configNode.comment = newComment;
			}
			return true;
		}

		public void Remove(ConfigNode node)
		{
			int count = nodes.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (nodes[count] != node);
			nodes.RemoveAt(count);
		}

		public void RemoveNode(string name)
		{
			int count = nodes.Count;
			do
			{
				if (count-- <= 0)
				{
					return;
				}
			}
			while (!(nodes[count].name == name));
			nodes.RemoveAt(count);
		}

		public void RemoveNodesStartWith(string startsWith)
		{
			int count = nodes.Count;
			while (count-- > 0)
			{
				if (nodes[count].name.StartsWith(startsWith))
				{
					nodes.RemoveAt(count);
				}
			}
		}

		public void RemoveNodes(string name)
		{
			int count = nodes.Count;
			while (count-- > 0)
			{
				if (nodes[count].name == name)
				{
					nodes.RemoveAt(count);
				}
			}
		}

		public bool Contains(string name)
		{
			int count = Count;
			do
			{
				if (count-- <= 0)
				{
					return false;
				}
			}
			while (!(this[count].name == name));
			return true;
		}

		public string[] DistinctNames()
		{
			List<string> list = new List<string>();
			int i = 0;
			for (int count = Count; i < count; i++)
			{
				ConfigNode configNode = this[i];
				if (!list.Contains(configNode.name))
				{
					list.Add(configNode.name);
				}
			}
			return list.ToArray();
		}

		public int CountByName(string name)
		{
			int num = 0;
			int count = Count;
			while (count-- > 0)
			{
				if (this[count].name == name)
				{
					num++;
				}
			}
			return num;
		}

		public void Clear()
		{
			nodes.Clear();
		}
	}

	public string name = "";

	public string id = "";

	public string comment;

	public ValueList _values;

	public ConfigNodeList _nodes;

	public const string configTabIndent = "\t";

	public static bool removeAfterUse;

	public static int uid;

	public static WriteLinkList writeLinks;

	public static ReadLinkList _readLinks;

	public static List<IPersistenceLoad> iPersistentLoaders;

	public ValueList values => _values;

	public ConfigNodeList nodes => _nodes;

	public bool HasData
	{
		get
		{
			if (values.Count <= 0)
			{
				return nodes.Count > 0;
			}
			return true;
		}
	}

	public int CountValues => values.Count;

	public int CountNodes => nodes.Count;

	public static ReadLinkList readLinks
	{
		get
		{
			return _readLinks;
		}
		set
		{
			_readLinks = value;
		}
	}

	public ConfigNode(string name)
	{
		this.name = CleanupInput(name);
		string[] array = CustomNameSplit(this.name);
		if (array.Length > 1)
		{
			this.name = array[0];
			id = array[1];
		}
		_values = new ValueList();
		_nodes = new ConfigNodeList();
	}

	public ConfigNode(string name, string vcomment)
	{
		this.name = CleanupInput(name);
		comment = vcomment;
		string[] array = CustomNameSplit(this.name);
		if (array.Length > 1)
		{
			this.name = array[0];
			id = array[1];
		}
		_values = new ValueList();
		_nodes = new ConfigNodeList();
	}

	public ConfigNode()
	{
		name = "";
		_values = new ValueList();
		_nodes = new ConfigNodeList();
	}

	public string[] CustomNameSplit(string name)
	{
		int i;
		for (i = 0; i < name.Length && (name[i] == '(' || name[i] == ' ' || name[i] == ')'); i++)
		{
		}
		if (i == name.Length)
		{
			return new string[0];
		}
		int num = i;
		for (; i < name.Length && name[i] != '(' && name[i] != ' ' && name[i] != ')'; i++)
		{
		}
		if (i == name.Length)
		{
			return new string[0];
		}
		int length = i - num;
		for (; i < name.Length && (name[i] == '(' || name[i] == ' ' || name[i] == ')'); i++)
		{
		}
		if (i == name.Length)
		{
			return new string[0];
		}
		int num2 = i;
		for (; i < name.Length && name[i] != '(' && name[i] != ' ' && name[i] != ')'; i++)
		{
		}
		int length2 = i - num2;
		return new string[2]
		{
			name.Substring(num, length),
			name.Substring(num2, length2)
		};
	}

	public override string ToString()
	{
		return ToStringRecursive("");
	}

	public void CopyTo(ConfigNode node)
	{
		CopyToRecursive(node);
	}

	public void CopyTo(ConfigNode node, bool overwrite)
	{
		CopyToRecursive(node, overwrite);
	}

	public void CopyTo(ConfigNode node, string newName)
	{
		CopyToRecursive(node);
		node.name = newName;
	}

	public ConfigNode CreateCopy()
	{
		ConfigNode configNode = new ConfigNode();
		CopyToRecursive(configNode);
		return configNode;
	}

	public static ConfigNode Load(string fileFullName)
	{
		return Load(fileFullName, bypassLocalization: true);
	}

	public static ConfigNode Load(string fileFullName, bool bypassLocalization)
	{
		if (!File.Exists(fileFullName))
		{
			Debug.LogWarning("File '" + fileFullName + "' does not exist");
			return null;
		}
		return LoadFromStringArray(File.ReadAllLines(fileFullName), bypassLocalization);
	}

	public static ConfigNode LoadFromTextAssetResource(string resourcePath)
	{
		TextAsset textAsset = Resources.Load(resourcePath) as TextAsset;
		ConfigNode result = LoadFromStringArray(textAsset.text.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
		Resources.UnloadAsset(textAsset);
		return result;
	}

	public static ConfigNode LoadFromStringArray(string[] cfgData)
	{
		return LoadFromStringArray(cfgData, bypassLocalization: false);
	}

	public static ConfigNode LoadFromStringArray(string[] cfgData, bool bypassLocalization)
	{
		if (cfgData == null)
		{
			return null;
		}
		List<string[]> list = PreFormatConfig(cfgData);
		if (list != null && list.Count != 0)
		{
			ConfigNode configNode = RecurseFormat(list);
			if (Localizer.Instance != null && !bypassLocalization)
			{
				Localizer.TranslateBranch(configNode);
			}
			return configNode;
		}
		return null;
	}

	public bool Save(string fileFullName)
	{
		return Save(fileFullName, null);
	}

	public bool Save(string fileFullName, string header)
	{
		StreamWriter streamWriter = new StreamWriter(File.Open(fileFullName, FileMode.Create));
		if (header != null && header != "")
		{
			streamWriter.WriteLine("// " + header);
			streamWriter.WriteLine();
		}
		WriteNode(streamWriter);
		streamWriter.Close();
		return true;
	}

	public static ConfigNode CreateConfigFromObject(object obj)
	{
		return CreateConfigFromObject(obj, 0, null);
	}

	public static ConfigNode CreateConfigFromObject(object obj, ConfigNode node)
	{
		return CreateConfigFromObject(obj, 0, node);
	}

	public static ConfigNode CreateConfigFromObject(object obj, int pass)
	{
		return CreateConfigFromObject(obj, 0);
	}

	public static ConfigNode CreateConfigFromObject(object obj, int pass, ConfigNode node)
	{
		Type type = obj.GetType();
		string fullName = type.FullName;
		if (node == null)
		{
			node = new ConfigNode(fullName);
		}
		writeLinks = new WriteLinkList();
		if (!IsValue(type) && !IsArrayType(type))
		{
			WriteObject(obj, node, pass);
			writeLinks = null;
			return node;
		}
		Debug.LogWarning("Cannot create a ConfigNode from a value type");
		return null;
	}

	public static bool LoadObjectFromConfig(object obj, ConfigNode node, int pass, bool removeAfterUse)
	{
		ConfigNode.removeAfterUse = removeAfterUse;
		readLinks = new ReadLinkList();
		iPersistentLoaders = new List<IPersistenceLoad>();
		if (!ReadObject(obj, node))
		{
			return false;
		}
		readLinks.Link();
		readLinks = null;
		int i = 0;
		for (int count = iPersistentLoaders.Count; i < count; i++)
		{
			iPersistentLoaders[i].PersistenceLoad();
		}
		if (removeAfterUse)
		{
			node.RemoveValues("");
			node.RemoveNodes("");
			removeAfterUse = false;
		}
		return true;
	}

	public static bool LoadObjectFromConfig(object obj, ConfigNode node, int pass)
	{
		return LoadObjectFromConfig(obj, node, pass, removeAfterUse: false);
	}

	public static bool LoadObjectFromConfig(object obj, ConfigNode node)
	{
		return LoadObjectFromConfig(obj, node, 0, removeAfterUse: false);
	}

	public static object CreateObjectFromConfig(string typeName, ConfigNode node)
	{
		object obj = CreateObject(typeName);
		if (obj == null)
		{
			Debug.LogWarning("Cannot create child instance");
			return null;
		}
		LoadObjectFromConfig(obj, node);
		return obj;
	}

	public static object CreateObjectFromConfig(ConfigNode node)
	{
		return CreateObjectFromConfig(node.name, node);
	}

	public static T CreateObjectFromConfig<T>(ConfigNode node)
	{
		object obj = CreateObjectFromConfig(typeof(T).AssemblyQualifiedName, node);
		if (obj == null)
		{
			return default(T);
		}
		try
		{
			return (T)obj;
		}
		catch
		{
			Debug.LogWarning("Cannot cast to destination type");
			return default(T);
		}
	}

	public void ClearData()
	{
		nodes.Clear();
		values.Clear();
	}

	public void AddData(ConfigNode node)
	{
		int i = 0;
		for (int count = node.values.Count; i < count; i++)
		{
			Value value = node.values[i];
			AddValue(value.name, value.value, value.comment);
		}
		int j = 0;
		for (int count2 = node.nodes.Count; j < count2; j++)
		{
			ConfigNode node2 = AddNode(node.nodes[j].name);
			node.nodes[j].CopyTo(node2);
		}
	}

	public bool HasValue(string name)
	{
		return values.Contains(name);
	}

	public bool HasValue()
	{
		return values.Count > 0;
	}

	public void AddValue(string name, object value, string vcomment)
	{
		if (value == null)
		{
			Debug.LogError(StringBuilderCache.Format("Input is null for field '{0}' in config node '{1}'\n{2}", name, this.name, Environment.StackTrace));
			value = "";
		}
		Value v = new Value(name, CleanupInput((value == null) ? null : Convert.ToString(value, CultureInfo.InvariantCulture)), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, string value, string vcomment)
	{
		if (value == null)
		{
			Debug.LogError(StringBuilderCache.Format("Input is null for field '{0}' in config node '{1}'\n{2}", name, this.name, Environment.StackTrace));
			value = "";
		}
		Value v = new Value(name, CleanupInput(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, object value)
	{
		if (value == null)
		{
			Debug.LogError(StringBuilderCache.Format("Input is null for field '{0}' in config node '{1}'\n{2}", name, this.name, Environment.StackTrace));
			value = "";
		}
		Value v = new Value(name, CleanupInput(Convert.ToString(value, CultureInfo.InvariantCulture)));
		values.Add(v);
	}

	public void AddValue(string name, string value)
	{
		if (value == null)
		{
			Debug.LogError(StringBuilderCache.Format("Input is null for field '{0}' in config node '{1}'\n{2}", name, this.name, Environment.StackTrace));
			value = "";
		}
		Value v = new Value(name, CleanupInput(value));
		values.Add(v);
	}

	public void AddValue(string name, bool value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, bool value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, byte value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, byte value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, sbyte value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, sbyte value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, char value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, char value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, decimal value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, decimal value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, double value, string vcomment)
	{
		Value v = new Value(name, value.ToString("G17", CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, double value)
	{
		Value v = new Value(name, value.ToString("G17", CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, float value, string vcomment)
	{
		Value v = new Value(name, value.ToString("G9", CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, float value)
	{
		Value v = new Value(name, value.ToString("G9", CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, int value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, int value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, uint value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, uint value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, long value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, long value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, ulong value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, ulong value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, short value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, short value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, ushort value, string vcomment)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, ushort value)
	{
		Value v = new Value(name, value.ToString(CultureInfo.InvariantCulture));
		values.Add(v);
	}

	public void AddValue(string name, Vector2 value, string vcomment)
	{
		Value v = new Value(name, WriteVector(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Vector2 value)
	{
		Value v = new Value(name, WriteVector(value));
		values.Add(v);
	}

	public void AddValue(string name, Vector3 value, string vcomment)
	{
		Value v = new Value(name, WriteVector(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Vector3 value)
	{
		Value v = new Value(name, WriteVector(value));
		values.Add(v);
	}

	public void AddValue(string name, Vector3d value, string vcomment)
	{
		Value v = new Value(name, WriteVector(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Vector3d value)
	{
		Value v = new Value(name, WriteVector(value));
		values.Add(v);
	}

	public void AddValue(string name, Vector4 value, string vcomment)
	{
		Value v = new Value(name, WriteVector(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Vector4 value)
	{
		Value v = new Value(name, WriteVector(value));
		values.Add(v);
	}

	public void AddValue(string name, Quaternion value, string vcomment)
	{
		Value v = new Value(name, WriteQuaternion(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Quaternion value)
	{
		Value v = new Value(name, WriteQuaternion(value));
		values.Add(v);
	}

	public void AddValue(string name, QuaternionD value, string vcomment)
	{
		Value v = new Value(name, WriteQuaternion(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, QuaternionD value)
	{
		Value v = new Value(name, WriteQuaternion(value));
		values.Add(v);
	}

	public void AddValue(string name, Matrix4x4 value, string vcomment)
	{
		Value v = new Value(name, WriteMatrix4x4(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Matrix4x4 value)
	{
		Value v = new Value(name, WriteMatrix4x4(value));
		values.Add(v);
	}

	public void AddValue(string name, Color value, string vcomment)
	{
		Value v = new Value(name, WriteColor(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Color value)
	{
		Value v = new Value(name, WriteColor(value));
		values.Add(v);
	}

	public void AddValue(string name, Color32 value, string vcomment)
	{
		Value v = new Value(name, WriteColor(value), vcomment);
		values.Add(v);
	}

	public void AddValue(string name, Color32 value)
	{
		Value v = new Value(name, WriteColor(value));
		values.Add(v);
	}

	public string GetValue(string name)
	{
		return values.GetValue(name);
	}

	public string GetValue(string name, int index)
	{
		return values.GetValue(name, index);
	}

	public string[] GetValues()
	{
		return values.GetValues();
	}

	public string[] GetValues(string name)
	{
		return values.GetValues(name);
	}

	public List<string> GetValuesList(string name)
	{
		return values.GetValuesList(name);
	}

	public string[] GetValuesStartsWith(string name)
	{
		return values.GetValuesStartsWith(name);
	}

	public bool SetValue(string name, string newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue, createIfNotFound);
	}

	public bool SetValue(string name, string newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue, index, createIfNotFound);
	}

	public bool SetValue(string name, string newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue, vcomment, createIfNotFound);
	}

	public bool SetValue(string name, string newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue, vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, bool newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, bool newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, bool newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, bool newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, byte newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, byte newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, byte newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, byte newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, sbyte newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, sbyte newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, sbyte newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, sbyte newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, char newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, char newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, char newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, char newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, decimal newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, decimal newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, decimal newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, decimal newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, double newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G17", CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, double newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G17", CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, double newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G17", CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, double newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G17", CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, float newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G9", CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, float newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G9", CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, float newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G9", CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, float newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString("G9", CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, int newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, int newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, int newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, int newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, uint newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, uint newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, uint newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, uint newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, long newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, long newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, long newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, long newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, ulong newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, ulong newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, ulong newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, ulong newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, short newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, short newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, short newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, short newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, ushort newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), createIfNotFound);
	}

	public bool SetValue(string name, ushort newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), index, createIfNotFound);
	}

	public bool SetValue(string name, ushort newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, ushort newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, newValue.ToString(CultureInfo.InvariantCulture), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Vector2 newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Vector2 newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Vector2 newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Vector2 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Vector3 newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Vector3 newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Vector3 newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Vector3 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Vector3d newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Vector3d newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Vector3d newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Vector3d newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Vector4 newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Vector4 newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Vector4 newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Vector4 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteVector(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Quaternion newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Quaternion newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Quaternion newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Quaternion newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, QuaternionD newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), createIfNotFound);
	}

	public bool SetValue(string name, QuaternionD newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, QuaternionD newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, QuaternionD newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteQuaternion(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Matrix4x4 newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteMatrix4x4(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Matrix4x4 newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteMatrix4x4(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Matrix4x4 newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteMatrix4x4(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Matrix4x4 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteMatrix4x4(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Color newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Color newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Color newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Color newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), vcomment, index, createIfNotFound);
	}

	public bool SetValue(string name, Color32 newValue, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), createIfNotFound);
	}

	public bool SetValue(string name, Color32 newValue, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), index, createIfNotFound);
	}

	public bool SetValue(string name, Color32 newValue, string vcomment, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), vcomment, createIfNotFound);
	}

	public bool SetValue(string name, Color32 newValue, string vcomment, int index, bool createIfNotFound = false)
	{
		return values.SetValue(name, WriteColor(newValue), vcomment, index, createIfNotFound);
	}

	public void RemoveValue(string name)
	{
		values.RemoveValue(name);
	}

	public void RemoveValues(params string[] names)
	{
		int i = 0;
		for (int num = names.Length; i < num; i++)
		{
			values.RemoveValues(names[i]);
		}
	}

	public void RemoveValues(string startsWith)
	{
		values.RemoveValues(startsWith);
	}

	public void RemoveValuesStartWith(string startsWith)
	{
		values.RemoveValuesStartWith(startsWith);
	}

	public void ClearValues()
	{
		values.Clear();
	}

	public bool HasNodeID(string id)
	{
		int count = nodes.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(nodes[count].id == id));
		return true;
	}

	public bool HasNode(string name)
	{
		int count = nodes.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!(nodes[count].name == name));
		return true;
	}

	public bool HasNode()
	{
		return nodes.Count > 0;
	}

	public ConfigNode AddNode(string name)
	{
		if (name.Trim().Length == 0)
		{
			return null;
		}
		ConfigNode configNode = new ConfigNode(name);
		nodes.Add(configNode);
		return configNode;
	}

	public ConfigNode AddNode(string name, string vcomment)
	{
		if (name.Trim().Length == 0)
		{
			return null;
		}
		ConfigNode configNode = new ConfigNode(name, vcomment);
		nodes.Add(configNode);
		return configNode;
	}

	public ConfigNode AddNode(ConfigNode node)
	{
		nodes.Add(node);
		return node;
	}

	public ConfigNode AddNode(string name, ConfigNode node)
	{
		if (name.Trim().Length == 0)
		{
			return null;
		}
		node.name = name;
		nodes.Add(node);
		return node;
	}

	public ConfigNode GetNodeID(string id)
	{
		return nodes.GetNodeID(id);
	}

	public ConfigNode GetNode(string name)
	{
		return nodes.GetNode(name);
	}

	public ConfigNode GetNode(string name, string valueName, string value)
	{
		return nodes.GetNode(name, valueName, value);
	}

	public ConfigNode GetNode(string name, int index)
	{
		return nodes.GetNode(name, index);
	}

	public ConfigNode[] GetNodes(string name)
	{
		return nodes.GetNodes(name);
	}

	public ConfigNode[] GetNodes(string name, string valueName, string value)
	{
		return nodes.GetNodes(name, valueName, value);
	}

	public ConfigNode[] GetNodes()
	{
		return nodes.GetNodes();
	}

	public bool SetNode(string name, ConfigNode newNode, bool createIfNotFound = false)
	{
		return nodes.SetNode(name, newNode, 0, createIfNotFound);
	}

	public bool SetNode(string name, ConfigNode newNode, int index, bool createIfNotFound = false)
	{
		return nodes.SetNode(name, newNode, index, createIfNotFound);
	}

	public void RemoveNode(string name)
	{
		nodes.RemoveNode(name);
	}

	public void RemoveNode(ConfigNode node)
	{
		nodes.Remove(node);
	}

	public void RemoveNodesStartWith(string startsWith)
	{
		nodes.RemoveNodesStartWith(startsWith);
	}

	public void RemoveNodes(string name)
	{
		nodes.RemoveNodes(name);
	}

	public void ClearNodes()
	{
		nodes.Clear();
	}

	[Obsolete("ConfigNode.CompileConfig has been moved to GameDatabase.CompileConfig.", true)]
	public static void CompileConfig(ConfigNode node)
	{
	}

	public static void Merge(ConfigNode mergeTo, ConfigNode mergeFrom)
	{
		mergeTo.name = mergeFrom.name;
		mergeTo.id = mergeFrom.id;
		int i = 0;
		for (int count = mergeTo.values.Count; i < count; i++)
		{
			if (mergeTo.values[i].name.StartsWith("@"))
			{
				string[] array = mergeTo.values[i].name.Split(new char[2] { '@', '/' }, StringSplitOptions.RemoveEmptyEntries);
				MergeValue(mergeFrom, array, 0, mergeTo.values[i].value);
			}
		}
		mergeTo.RemoveValuesStartWith("@");
		int j = 0;
		for (int count2 = mergeTo.nodes.Count; j < count2; j++)
		{
			if (mergeTo.nodes[j].name.StartsWith("@"))
			{
				string[] array2 = mergeTo.nodes[j].name.Split(new char[2] { '@', '/' }, StringSplitOptions.RemoveEmptyEntries);
				MergeNode(mergeFrom, array2, 0, mergeTo.nodes[j]);
			}
		}
		mergeTo.RemoveNodesStartWith("@");
		int k = 0;
		for (int count3 = mergeFrom.values.Count; k < count3; k++)
		{
			mergeTo.AddValue(mergeFrom.values[k].name, mergeFrom.values[k].value);
		}
		int l = 0;
		for (int count4 = mergeFrom.nodes.Count; l < count4; l++)
		{
			mergeTo.AddNode(mergeFrom.nodes[l].CreateCopy());
		}
	}

	public static void MergeValue(ConfigNode node, string[] id, int index, string value)
	{
		if (index == id.Length - 1)
		{
			if (node.HasValue(id[index]))
			{
				node.SetValue(id[index], value);
			}
			else
			{
				node.AddValue(id[index], value);
			}
			return;
		}
		ConfigNode nodeID = node.GetNodeID(id[index]);
		if (nodeID != null)
		{
			MergeValue(nodeID, id, index + 1, value);
			return;
		}
		Debug.Log("ConfigNode: Cannot merge value from id " + DebugStringArray(id) + " index " + index);
	}

	public static void MergeNode(ConfigNode node, string[] id, int index, ConfigNode value)
	{
		ConfigNode nodeID = node.GetNodeID(id[index]);
		if (nodeID == null)
		{
			Debug.Log("ConfigNode: Cannot merge node from id " + DebugStringArray(id) + " index " + index);
		}
		else if (index == id.Length - 1)
		{
			nodeID.ClearData();
			value.CopyTo(node);
		}
		else
		{
			MergeNode(node, id, index + 1, value);
		}
	}

	public static string DebugStringArray(string[] array)
	{
		string text = "";
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			if (text != "")
			{
				text += "\n";
			}
			text += array[i];
		}
		return text;
	}

	public static void WriteObject(object obj, ConfigNode node, int pass)
	{
		if (obj is IPersistenceSave persistenceSave)
		{
			persistenceSave.PersistenceSave();
		}
		if (HasAttribute(obj.GetType(), typeof(PersistentLinkable)))
		{
			node.AddValue("link_uid", writeLinks.AssignTarget(obj));
		}
		Type type = obj.GetType();
		MemberInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		MemberInfo[] array = fields;
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			MemberInfo memberInfo = array[i];
			Persistent[] array2 = (Persistent[])memberInfo.GetCustomAttributes(typeof(Persistent), inherit: true);
			int num2 = array2.Length;
			for (int j = 0; j < num2; j++)
			{
				Persistent persistent = array2[j];
				if (!persistent.isPersistant || (pass != 0 && persistent.pass != 0 && (persistent.pass & pass) == 0))
				{
					continue;
				}
				string fieldName = RetrieveFieldName(memberInfo, persistent);
				FieldInfo field = type.GetField(memberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				object value = field.GetValue(obj);
				Type fieldType = field.FieldType;
				if (value == null)
				{
					continue;
				}
				if (persistent.link)
				{
					if (!HasAttribute(obj.GetType(), typeof(PersistentLinkable)))
					{
						Debug.LogWarning("Field: '" + memberInfo.Name + "' does not reference a PersistentLinkable type");
					}
					else
					{
						WriteValue(fieldName, typeof(int), writeLinks.AssignLink(value), node);
					}
				}
				else if (IsValue(fieldType))
				{
					WriteValue(fieldName, fieldType, value, node);
				}
				else if (IsArrayType(fieldType))
				{
					WriteArrayTypes(fieldName, fieldType, value, node, persistent);
				}
				else
				{
					WriteStruct(fieldName, field, value, node, pass);
				}
			}
		}
	}

	public static bool WriteValue(string fieldName, Type fieldType, object value, ConfigNode node)
	{
		if (fieldType.IsValueType)
		{
			if (fieldType == typeof(bool))
			{
				node.AddValue(fieldName, ((bool)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(double))
			{
				node.AddValue(fieldName, ((double)value).ToString("G17", CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(float))
			{
				node.AddValue(fieldName, ((float)value).ToString("G9", CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(int))
			{
				node.AddValue(fieldName, ((int)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(Enum))
			{
				node.AddValue(fieldName, ((int)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType.IsEnum)
			{
				node.AddValue(fieldName, WriteEnum((Enum)value));
				return true;
			}
			if (fieldType == typeof(uint))
			{
				node.AddValue(fieldName, ((uint)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(Vector2))
			{
				node.AddValue(fieldName, WriteVector((Vector2)value));
				return true;
			}
			if (fieldType == typeof(Vector3))
			{
				node.AddValue(fieldName, WriteVector((Vector3)value));
				return true;
			}
			if (fieldType == typeof(Vector3d))
			{
				node.AddValue(fieldName, WriteVector((Vector3)value));
				return true;
			}
			if (fieldType == typeof(Vector4))
			{
				node.AddValue(fieldName, WriteVector((Vector4)value));
				return true;
			}
			if (fieldType == typeof(Quaternion))
			{
				node.AddValue(fieldName, WriteQuaternion((Quaternion)value));
				return true;
			}
			if (fieldType == typeof(QuaternionD))
			{
				node.AddValue(fieldName, WriteQuaternion((QuaternionD)value));
				return true;
			}
			if (fieldType == typeof(Matrix4x4))
			{
				node.AddValue(fieldName, WriteMatrix4x4((Matrix4x4)value));
				return true;
			}
			if (fieldType == typeof(Color))
			{
				node.AddValue(fieldName, WriteColor((Color)value));
				return true;
			}
			if (fieldType == typeof(Color32))
			{
				node.AddValue(fieldName, WriteColor((Color32)value));
				return true;
			}
			if (fieldType == typeof(long))
			{
				node.AddValue(fieldName, ((long)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(ulong))
			{
				node.AddValue(fieldName, ((ulong)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(short))
			{
				node.AddValue(fieldName, ((short)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(ushort))
			{
				node.AddValue(fieldName, ((ushort)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(byte))
			{
				node.AddValue(fieldName, ((byte)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(sbyte))
			{
				node.AddValue(fieldName, ((sbyte)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(char))
			{
				node.AddValue(fieldName, ((char)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
			if (fieldType == typeof(decimal))
			{
				node.AddValue(fieldName, ((decimal)value).ToString(CultureInfo.InvariantCulture));
				return true;
			}
		}
		else if (fieldType == typeof(string))
		{
			node.AddValue(fieldName, (string)value);
			return true;
		}
		return false;
	}

	public static bool WriteStruct(string fieldName, FieldInfo fieldInfo, object value, ConfigNode node, int pass)
	{
		ConfigNode configNode = new ConfigNode();
		WriteObject(value, configNode, pass);
		if (configNode.HasData)
		{
			configNode.CopyTo(node.AddNode(fieldName));
			return true;
		}
		return false;
	}

	public static bool WriteArrayTypes(string fieldName, Type fieldType, object value, ConfigNode node, Persistent kspField)
	{
		return WriteArray(fieldName, fieldType, value, node, kspField);
	}

	public static bool HasAttribute(Type type, Type attributeType)
	{
		object[] customAttributes = type.GetCustomAttributes(inherit: true);
		int num = customAttributes.Length;
		do
		{
			if (num-- <= 0)
			{
				return false;
			}
		}
		while (!(customAttributes[num].GetType() == attributeType));
		return true;
	}

	public static bool WriteArray(string fieldName, Type fieldType, object value, ConfigNode node, Persistent kspField)
	{
		if (!(value is IList list))
		{
			return false;
		}
		Type type = FindIEnumerable(fieldType);
		ConfigNode configNode = new ConfigNode();
		string fieldName2 = ((kspField.collectionIndex != "") ? kspField.collectionIndex : "item");
		if (kspField.link)
		{
			if (!HasAttribute(type, typeof(PersistentLinkable)))
			{
				Debug.LogWarning("Field: '" + fieldName + "' does not reference a linkable type");
				return false;
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				WriteValue(fieldName, typeof(int), writeLinks.AssignLink(list[i]), node);
			}
			return true;
		}
		if (IsValue(type))
		{
			int count2 = list.Count;
			for (int j = 0; j < count2; j++)
			{
				WriteValue(fieldName2, type, list[j], configNode);
			}
		}
		else
		{
			if (IsArrayType(type))
			{
				Debug.LogWarning("Arrays of arrays are not supported yet");
				return false;
			}
			int count3 = list.Count;
			for (int k = 0; k < count3; k++)
			{
				ConfigNode configNode2 = new ConfigNode();
				WriteObject(list[k], configNode2, kspField.pass);
				if (configNode2.HasData)
				{
					configNode2.CopyTo(configNode.AddNode(fieldName2));
				}
			}
		}
		if (configNode.HasData)
		{
			configNode.CopyTo(node.AddNode(fieldName));
		}
		return true;
	}

	public static object CreateObject(string name)
	{
		if (name != null && !(name == ""))
		{
			Type type = Type.GetType(name);
			if (type == null)
			{
				Debug.LogWarning("Cannot create object: Class name '" + name + "' not found");
				return null;
			}
			if (IsValue(type))
			{
				Debug.LogWarning("Cannot create a value type straight from a ConfigNode");
				return null;
			}
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
			if (constructor == null)
			{
				Debug.LogWarning("Class of type '" + type.Name + "' requires a parameterless constructor");
				return null;
			}
			return constructor.Invoke(new object[0]);
		}
		Debug.LogWarning("Cannot create object: Node name is empty");
		return null;
	}

	public static object CreateNodeObject(ConfigNode node)
	{
		return CreateObject(node.name);
	}

	public static bool ReadObject(object obj, ConfigNode node)
	{
		ReadFieldList readFieldList = new ReadFieldList(obj);
		if (HasAttribute(obj.GetType(), typeof(PersistentLinkable)))
		{
			if (!node.HasValue("link_uid"))
			{
				readLinks.AssignLinkable(-1, obj);
			}
			else
			{
				int linkID = int.Parse(node.GetValue("link_uid"));
				readLinks.AssignLinkable(linkID, obj);
			}
		}
		int count = node.values.Count;
		for (int i = 0; i < count; i++)
		{
			Value value = node.values[i];
			ReadFieldList.FieldItem fieldItem = readFieldList[value.name];
			if (fieldItem == null)
			{
				continue;
			}
			if (fieldItem.kspField.link)
			{
				int linkID2 = int.Parse(value.value);
				readLinks.AssignLink(linkID2, fieldItem, -1);
				if (removeAfterUse)
				{
					value.name = "";
				}
			}
			else if (IsValue(fieldItem.fieldType))
			{
				object obj2 = ReadValue(fieldItem.fieldType, value.value);
				if (obj2 != null)
				{
					fieldItem.fieldInfo.SetValue(fieldItem.host, obj2);
				}
				if (removeAfterUse)
				{
					value.name = "";
				}
			}
		}
		count = node.nodes.Count;
		for (int j = 0; j < count; j++)
		{
			ConfigNode configNode = node.nodes[j];
			ReadFieldList.FieldItem fieldItem2 = readFieldList[configNode.name];
			if (fieldItem2 != null)
			{
				if (IsArrayType(fieldItem2.fieldType))
				{
					ReadArray(fieldItem2, configNode);
				}
				else
				{
					ReadObject(fieldItem2, configNode);
				}
				if (removeAfterUse && removeAfterUse)
				{
					configNode.name = "";
				}
			}
		}
		if (obj is IPersistenceLoad item)
		{
			iPersistentLoaders.Add(item);
		}
		return true;
	}

	public static bool ReadObject(ReadFieldList.FieldItem field, ConfigNode node)
	{
		object value = field.fieldInfo.GetValue(field.host);
		if (value == null)
		{
			if (field.fieldType.IsSubclassOf(typeof(MonoBehaviour)))
			{
				return ReadObjectCreateMonoBehaviour(field, node);
			}
			return ReadObjectCreateNew(field, node);
		}
		return ReadObject(value, node);
	}

	public static bool ReadObjectCreateNew(ReadFieldList.FieldItem field, ConfigNode node)
	{
		ConstructorInfo constructor = field.fieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
		if (constructor == null)
		{
			Debug.LogWarning("Subclass of type '" + field.fieldType.Name + "' requires a parameterless constructor");
			return false;
		}
		object obj = constructor.Invoke(new object[0]);
		if (obj == null)
		{
			Debug.LogWarning("Cannot create child instance");
			return false;
		}
		ReadObject(obj, node);
		field.fieldInfo.SetValue(field.host, obj);
		return true;
	}

	public static bool ReadObjectCreateMonoBehaviour(ReadFieldList.FieldItem field, ConfigNode node)
	{
		MonoBehaviour monoBehaviour = field.host as MonoBehaviour;
		if (monoBehaviour == null)
		{
			Debug.LogWarning("Cannot instantiate a MonoBehaviour type inside a non-MonoBehaviour class");
			return false;
		}
		GameObject gameObject = null;
		switch (field.kspField.relationship)
		{
		case PersistentRelation.NoRelation:
			gameObject = new GameObject();
			break;
		case PersistentRelation.SameObject:
			gameObject = monoBehaviour.gameObject;
			break;
		case PersistentRelation.ChildObject:
			gameObject = new GameObject();
			gameObject.transform.parent = monoBehaviour.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			break;
		}
		MonoBehaviour monoBehaviour2 = (MonoBehaviour)gameObject.AddComponent(field.fieldType);
		ReadObject(monoBehaviour2, node);
		field.fieldInfo.SetValue(field.host, monoBehaviour2);
		return true;
	}

	public static object ReadValue(Type fieldType, string value)
	{
		if (fieldType.IsValueType)
		{
			if (fieldType == typeof(bool))
			{
				bool result = false;
				if (bool.TryParse(value, out result))
				{
					return result;
				}
				return null;
			}
			if (fieldType == typeof(byte))
			{
				byte result2 = 0;
				if (byte.TryParse(value, out result2))
				{
					return result2;
				}
				return null;
			}
			if (fieldType == typeof(sbyte))
			{
				sbyte result3 = 0;
				if (sbyte.TryParse(value, out result3))
				{
					return result3;
				}
				return null;
			}
			if (fieldType == typeof(char))
			{
				char result4 = '\0';
				if (char.TryParse(value, out result4))
				{
					return result4;
				}
				return null;
			}
			if (fieldType == typeof(decimal))
			{
				decimal result5 = default(decimal);
				if (decimal.TryParse(value, out result5))
				{
					return result5;
				}
				return null;
			}
			if (fieldType == typeof(double))
			{
				double result6 = 0.0;
				if (double.TryParse(value, out result6))
				{
					return result6;
				}
				return null;
			}
			if (fieldType == typeof(float))
			{
				float result7 = 0f;
				if (float.TryParse(value, out result7))
				{
					return result7;
				}
				return null;
			}
			if (fieldType == typeof(int))
			{
				int result8 = 0;
				if (int.TryParse(value, out result8))
				{
					return result8;
				}
				return null;
			}
			if (fieldType == typeof(uint))
			{
				uint result9 = 0u;
				if (uint.TryParse(value, out result9))
				{
					return result9;
				}
				return null;
			}
			if (fieldType == typeof(long))
			{
				long result10 = 0L;
				if (long.TryParse(value, out result10))
				{
					return result10;
				}
				return null;
			}
			if (fieldType == typeof(ulong))
			{
				ulong result11 = 0uL;
				if (ulong.TryParse(value, out result11))
				{
					return result11;
				}
				return null;
			}
			if (fieldType == typeof(short))
			{
				short result12 = 0;
				if (short.TryParse(value, out result12))
				{
					return result12;
				}
				return null;
			}
			if (fieldType == typeof(ushort))
			{
				ushort result13 = 0;
				if (ushort.TryParse(value, out result13))
				{
					return result13;
				}
				return null;
			}
			if (fieldType == typeof(Vector2))
			{
				return ParseVector2(value);
			}
			if (fieldType == typeof(Vector3))
			{
				return ParseVector3(value);
			}
			if (fieldType == typeof(Vector3d))
			{
				return ParseVector3D(value);
			}
			if (fieldType == typeof(Vector4))
			{
				return ParseVector4(value);
			}
			if (fieldType == typeof(Quaternion))
			{
				return ParseQuaternion(value);
			}
			if (fieldType == typeof(QuaternionD))
			{
				return ParseQuaternionD(value);
			}
			if (fieldType == typeof(Color))
			{
				return ParseColor(value);
			}
			if (fieldType == typeof(Color32))
			{
				return ParseColor32(value);
			}
			if (fieldType.IsEnum)
			{
				return ParseEnum(fieldType, value);
			}
		}
		else if (fieldType == typeof(string))
		{
			return value;
		}
		return null;
	}

	public static bool ReadArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		Type type = FindIEnumerable(field.fieldType);
		if (type == null)
		{
			Debug.LogWarning("Not IEnumerable type?");
			return false;
		}
		if (field.kspField.link)
		{
			return ReadLinkArray(field, node);
		}
		if (!type.IsAssignableFrom(typeof(MonoBehaviour)) && !type.IsSubclassOf(typeof(MonoBehaviour)))
		{
			if (IsValue(type))
			{
				return ReadValueArray(field, node);
			}
			if (IsArray(type))
			{
				Debug.LogWarning("Arrays of arrays are not supported.");
				return false;
			}
			return ReadObjectArray(field, node);
		}
		return ReadComponentArray(field, node);
	}

	public static bool ReadValueArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		Type type = FindIEnumerable(field.fieldType);
		if (type == null)
		{
			Debug.LogWarning("Not IEnumerable type?");
			return false;
		}
		List<object> list = new List<object>();
		int i = 0;
		for (int count = node.values.Count; i < count; i++)
		{
			Value value = node.values[i];
			object obj = ReadValue(type, value.value);
			if (obj != null)
			{
				list.Add(obj);
			}
		}
		object obj2 = null;
		if (IsArray(field.fieldType))
		{
			Array array = Array.CreateInstance(type, list.Count);
			int j = 0;
			for (int length = array.Length; j < length; j++)
			{
				array.SetValue(list[j], j);
			}
			obj2 = array;
		}
		if (IsGenericArray(field.fieldType))
		{
			IList list2 = (IList)Activator.CreateInstance(field.fieldType);
			int k = 0;
			for (int count2 = list.Count; k < count2; k++)
			{
				list2.Add(list[k]);
			}
			obj2 = list2;
		}
		if (obj2 != null)
		{
			field.fieldInfo.SetValue(field.host, obj2);
			return true;
		}
		return false;
	}

	public static bool ReadComponentArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		Type type = FindIEnumerable(field.fieldType);
		if (type == null)
		{
			Debug.LogWarning("Not IEnumerable type?");
			return false;
		}
		if (!type.IsAssignableFrom(typeof(MonoBehaviour)) && !type.IsSubclassOf(typeof(MonoBehaviour)))
		{
			Debug.LogWarning("Not MonoBehaviour type?");
			return false;
		}
		MonoBehaviour monoBehaviour = field.host as MonoBehaviour;
		if (monoBehaviour == null)
		{
			Debug.LogWarning("Cannot instantiate a MonoBehaviour type inside a non-MonoBehaviour class");
			return false;
		}
		GameObject gameObject = null;
		List<object> list = new List<object>();
		int i = 0;
		for (int count = node.nodes.Count; i < count; i++)
		{
			ConfigNode node2 = node.nodes[i];
			switch (field.kspField.relationship)
			{
			case PersistentRelation.NoRelation:
				gameObject = new GameObject();
				break;
			case PersistentRelation.SameObject:
				gameObject = monoBehaviour.gameObject;
				break;
			case PersistentRelation.ChildObject:
				gameObject = new GameObject();
				gameObject.transform.parent = monoBehaviour.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				break;
			}
			MonoBehaviour monoBehaviour2 = (MonoBehaviour)gameObject.AddComponent(type);
			if (monoBehaviour2 == null)
			{
				Debug.LogError("Cannot create component of type '" + type.Name + "'");
				if (gameObject != monoBehaviour.gameObject)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
					continue;
				}
			}
			ReadObject(monoBehaviour2, node2);
			list.Add(monoBehaviour2);
		}
		object obj = null;
		if (IsArray(field.fieldType))
		{
			Array array = Array.CreateInstance(type, list.Count);
			int j = 0;
			for (int length = array.Length; j < length; j++)
			{
				array.SetValue(list[j], j);
			}
			obj = array;
		}
		if (IsGenericArray(field.fieldType))
		{
			IList list2 = (IList)Activator.CreateInstance(field.fieldType);
			int k = 0;
			for (int count2 = list.Count; k < count2; k++)
			{
				list2.Add(list[k]);
			}
			obj = list2;
		}
		if (obj != null)
		{
			field.fieldInfo.SetValue(field.host, obj);
			return true;
		}
		return false;
	}

	public static bool ReadObjectArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		Type type = FindIEnumerable(field.fieldType);
		if (type == null)
		{
			Debug.LogWarning("Not IEnumerable type?");
			return false;
		}
		List<object> list = new List<object>();
		int num = 0;
		int count = node.nodes.Count;
		while (true)
		{
			if (num < count)
			{
				ConfigNode node2 = node.nodes[num];
				ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				if (!(constructor == null))
				{
					object obj = constructor.Invoke(new object[0]);
					if (obj == null)
					{
						break;
					}
					ReadObject(obj, node2);
					list.Add(obj);
					num++;
					continue;
				}
				Debug.LogWarning("Subclass of type '" + type.Name + "' requires a parameterless constructor");
				return false;
			}
			object obj2 = null;
			if (IsArray(field.fieldType))
			{
				Array array = Array.CreateInstance(type, list.Count);
				int i = 0;
				for (int length = array.Length; i < length; i++)
				{
					array.SetValue(list[i], i);
				}
				obj2 = array;
			}
			if (IsGenericArray(field.fieldType))
			{
				IList list2 = (IList)Activator.CreateInstance(field.fieldType);
				int j = 0;
				for (int count2 = list.Count; j < count2; j++)
				{
					list2.Add(list[j]);
				}
				obj2 = list2;
			}
			if (obj2 != null)
			{
				field.fieldInfo.SetValue(field.host, obj2);
				return true;
			}
			return false;
		}
		Debug.LogWarning("Cannot create child instance");
		return false;
	}

	public static bool ReadLinkArray(ReadFieldList.FieldItem field, ConfigNode node)
	{
		Type type = FindIEnumerable(field.fieldType);
		if (type == null)
		{
			Debug.LogWarning("Not IEnumerable type?");
			return false;
		}
		List<object> list = new List<object>();
		int num = 0;
		int i = 0;
		for (int count = node.values.Count; i < count; i++)
		{
			Value value = node.values[i];
			readLinks.AssignLink(int.Parse(value.value), field, num++);
			list.Add(null);
		}
		object obj = null;
		if (IsArray(field.fieldType))
		{
			Array array = Array.CreateInstance(type, list.Count);
			int j = 0;
			for (int length = array.Length; j < length; j++)
			{
				array.SetValue(list[j], j);
			}
			obj = array;
		}
		if (IsGenericArray(field.fieldType))
		{
			IList list2 = (IList)Activator.CreateInstance(field.fieldType);
			int k = 0;
			for (int count2 = list.Count; k < count2; k++)
			{
				list2.Add(list[k]);
			}
			obj = list2;
		}
		if (obj != null)
		{
			field.fieldInfo.SetValue(field.host, obj);
			return true;
		}
		return false;
	}

	public static string RetrieveFieldName(MemberInfo field, Persistent attr)
	{
		if (attr.name == "")
		{
			return field.Name;
		}
		return attr.name;
	}

	public string ToStringRecursive(string indent)
	{
		string text = "";
		text = ((!(id != "")) ? (text + indent + name + (string.IsNullOrEmpty(comment) ? "" : (" // " + comment)) + "\n") : (text + indent + name + " (" + id + ")" + (string.IsNullOrEmpty(comment) ? "" : (" // " + comment)) + "\n"));
		text = text + indent + "{\n";
		string text2 = indent + "\t";
		for (int i = 0; i < values.Count; i++)
		{
			Value value = values[i];
			text = text + text2 + value.name + " = " + value.value + (string.IsNullOrEmpty(value.comment) ? "" : (" // " + value.comment)) + "\n";
		}
		int count = nodes.Count;
		for (int j = 0; j < count; j++)
		{
			text += nodes[j].ToStringRecursive(text2);
		}
		return text + indent + "}\n";
	}

	public void CopyToRecursive(ConfigNode node, bool overwrite = false)
	{
		if (node.name == "")
		{
			node.name = name;
		}
		if (node.id == "")
		{
			node.id = id;
		}
		if (!string.IsNullOrEmpty(comment))
		{
			node.comment = comment;
		}
		int i = 0;
		for (int count = values.Count; i < count; i++)
		{
			Value value = values[i];
			if (overwrite)
			{
				node.SetValue(value.name, value.value, value.comment, createIfNotFound: true);
			}
			else
			{
				node.AddValue(value.name, value.value, value.comment);
			}
		}
		int j = 0;
		for (int count2 = nodes.Count; j < count2; j++)
		{
			ConfigNode configNode = nodes[j];
			if (overwrite)
			{
				node.RemoveNode(configNode.name);
			}
			configNode.CopyToRecursive(node.AddNode(configNode.name));
		}
	}

	public static bool IsValue(Type fieldType)
	{
		if (!fieldType.IsValueType)
		{
			if (fieldType == typeof(string))
			{
				return true;
			}
			return false;
		}
		if (fieldType == typeof(bool))
		{
			return true;
		}
		if (fieldType == typeof(byte))
		{
			return true;
		}
		if (fieldType == typeof(sbyte))
		{
			return true;
		}
		if (fieldType == typeof(char))
		{
			return true;
		}
		if (fieldType == typeof(decimal))
		{
			return true;
		}
		if (fieldType == typeof(double))
		{
			return true;
		}
		if (fieldType == typeof(float))
		{
			return true;
		}
		if (fieldType == typeof(int))
		{
			return true;
		}
		if (fieldType == typeof(uint))
		{
			return true;
		}
		if (fieldType == typeof(long))
		{
			return true;
		}
		if (fieldType == typeof(ulong))
		{
			return true;
		}
		if (fieldType == typeof(short))
		{
			return true;
		}
		if (fieldType == typeof(ushort))
		{
			return true;
		}
		if (fieldType == typeof(Vector2))
		{
			return true;
		}
		if (fieldType == typeof(Vector3))
		{
			return true;
		}
		if (fieldType == typeof(Vector3d))
		{
			return true;
		}
		if (fieldType == typeof(Vector4))
		{
			return true;
		}
		if (fieldType == typeof(Quaternion))
		{
			return true;
		}
		if (fieldType == typeof(QuaternionD))
		{
			return true;
		}
		if (fieldType == typeof(Matrix4x4))
		{
			return true;
		}
		if (fieldType == typeof(Color))
		{
			return true;
		}
		if (fieldType == typeof(Color32))
		{
			return true;
		}
		if (fieldType.IsEnum)
		{
			return true;
		}
		return false;
	}

	public static bool IsArrayType(Type fieldType)
	{
		if (!IsArray(fieldType) && !IsGenericArray(fieldType))
		{
			return IsEnumerable(fieldType);
		}
		return true;
	}

	public static bool IsArray(Type fieldType)
	{
		return fieldType.IsArray;
	}

	public static bool IsGenericArray(Type fieldType)
	{
		if (fieldType.IsGenericType)
		{
			Type[] genericArguments = fieldType.GetGenericArguments();
			if (genericArguments != null && genericArguments.Length != 0)
			{
				int num = genericArguments.Length;
				for (int i = 0; i < num; i++)
				{
					if (typeof(IEnumerable<>).MakeGenericType(genericArguments[i]).IsAssignableFrom(fieldType))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsEnumerable(Type fieldType)
	{
		Type[] interfaces = fieldType.GetInterfaces();
		if (interfaces != null && interfaces.Length != 0)
		{
			int num = interfaces.Length;
			while (num-- > 0)
			{
				if (interfaces[num] is IEnumerable)
				{
					return true;
				}
			}
		}
		if (fieldType.BaseType != null && fieldType.BaseType != typeof(object))
		{
			return IsEnumerable(fieldType.BaseType);
		}
		return false;
	}

	public static Type GetElementType(Type seqType)
	{
		Type type = FindIEnumerable(seqType);
		if (type == null)
		{
			return null;
		}
		return type.GetGenericArguments()[0];
	}

	public static Type FindIEnumerable(Type seqType)
	{
		if (!(seqType == null) && !(seqType == typeof(string)))
		{
			if (seqType.IsArray)
			{
				return seqType.GetElementType();
			}
			if (seqType.IsGenericType)
			{
				Type[] genericArguments = seqType.GetGenericArguments();
				if (genericArguments != null && genericArguments.Length != 0)
				{
					int num = genericArguments.Length;
					for (int i = 0; i < num; i++)
					{
						if (typeof(IEnumerable<>).MakeGenericType(genericArguments[i]).IsAssignableFrom(seqType))
						{
							return genericArguments[i];
						}
					}
				}
			}
			Type[] interfaces = seqType.GetInterfaces();
			if (interfaces != null && interfaces.Length != 0)
			{
				int num2 = interfaces.Length;
				for (int j = 0; j < num2; j++)
				{
					if (FindIEnumerable(interfaces[j]) != null)
					{
						return interfaces[j];
					}
				}
			}
			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
			{
				return FindIEnumerable(seqType.BaseType);
			}
			return null;
		}
		return null;
	}

	public static string CleanupInput(string value)
	{
		value = value.Replace("\n", "");
		value = value.Replace("\r", "");
		value = value.Replace("\t", " ");
		return value;
	}

	public static string WriteBoolArray(bool[] flags)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		for (int i = 0; i < flags.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(flags[i].ToString(CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteEnumIntArray<T>(T[] flags) where T : struct
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		for (int i = 0; i < flags.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(((int)(object)flags[i]).ToString(CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteStringArray(string[] strings)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		for (int i = 0; i < strings.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(strings[i]);
		}
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector2 vector)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(29);
		stringBuilder.Append(vector.x.ToString("G9", CultureInfo.InvariantCulture)).Append(",").Append(vector.y.ToString("G9", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector3 vector)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(44);
		stringBuilder.Append(vector.x.ToString("G9", CultureInfo.InvariantCulture)).Append(",").Append(vector.y.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(vector.z.ToString("G9", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector3d vector)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(79);
		stringBuilder.Append(vector.x.ToString("G17", CultureInfo.InvariantCulture)).Append(",").Append(vector.y.ToString("G17", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(vector.z.ToString("G17", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteVector(Vector4 vector)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(59);
		stringBuilder.Append(vector.x.ToString("G9", CultureInfo.InvariantCulture)).Append(",").Append(vector.y.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(vector.z.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(vector.w.ToString("G9", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteQuaternion(Quaternion quaternion)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(59);
		stringBuilder.Append(quaternion.x.ToString("G9", CultureInfo.InvariantCulture)).Append(",").Append(quaternion.y.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(quaternion.z.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(quaternion.w.ToString("G9", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteQuaternion(QuaternionD quaternion)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(91);
		stringBuilder.Append(quaternion.x.ToString("G17", CultureInfo.InvariantCulture)).Append(",").Append(quaternion.y.ToString("G17", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(quaternion.z.ToString("G17", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(quaternion.w.ToString("G17", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteMatrix4x4(Matrix4x4 matrix)
	{
		return matrix.m00.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m01.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m02.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m03.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m10.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m11.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m12.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m13.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m20.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m21.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m22.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m23.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m30.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m31.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m32.ToString("G9", CultureInfo.InvariantCulture) + "," + matrix.m33.ToString("G9", CultureInfo.InvariantCulture);
	}

	public static string WriteColor(Color color)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(59);
		stringBuilder.Append(color.r.ToString("G9", CultureInfo.InvariantCulture)).Append(",").Append(color.g.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(color.b.ToString("G9", CultureInfo.InvariantCulture))
			.Append(",")
			.Append(color.a.ToString("G9", CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteColor(Color32 color)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire(15);
		stringBuilder.Append(color.r.ToString(CultureInfo.InvariantCulture)).Append(",").Append(color.g.ToString(CultureInfo.InvariantCulture))
			.Append(",")
			.Append(color.b.ToString(CultureInfo.InvariantCulture))
			.Append(",")
			.Append(color.a.ToString(CultureInfo.InvariantCulture));
		return stringBuilder.ToStringAndRelease();
	}

	public static string WriteEnum(Enum en)
	{
		return en.ToString();
	}

	public static Vector2 ParseVector2(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 2)
		{
			Debug.LogWarning("WARNING: Vector2 entry is not formatted properly! Proper format for Vector2 is x,y");
			return Vector2.zero;
		}
		return new Vector2(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture));
	}

	public static Vector3 ParseVector3(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 3)
		{
			Debug.LogWarning("WARNING: Vector3 entry is not formatted properly! Proper format for Vector3 is x,y,z");
			return Vector3.zero;
		}
		return new Vector3(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture), float.Parse(array[2], CultureInfo.InvariantCulture));
	}

	public static Vector3d ParseVector3D(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 3)
		{
			Debug.LogWarning("WARNING: Vector3D entry is not formatted properly! Proper format for Vector3D is x,y,z");
			return Vector3d.zero;
		}
		return new Vector3d(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]));
	}

	public static Vector4 ParseVector4(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 4)
		{
			Debug.LogWarning("WARNING: Vector4 entry is not formatted properly! Proper format for Vector4s is x,y,z,w");
			return Vector4.zero;
		}
		return new Vector4(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static Quaternion ParseQuaternion(string quaternionString)
	{
		string[] array = quaternionString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 4)
		{
			Debug.LogWarning("WARNING: Quaternion entry is not formatted properly! Proper format for Quaternion is x,y,z,w");
			return Quaternion.identity;
		}
		return new Quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
	}

	public static QuaternionD ParseQuaternionD(string quaternionString)
	{
		string[] array = quaternionString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 4)
		{
			Debug.LogWarning("WARNING: QuaternionD entry is not formatted properly! Proper format for QuaternionD is x,y,z,w");
			return QuaternionD.identity;
		}
		return new QuaternionD(double.Parse(array[0]), double.Parse(array[1]), double.Parse(array[2]), double.Parse(array[3]));
	}

	public static Matrix4x4 ParseMatrix4x4(string quaternionString)
	{
		string[] array = quaternionString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 16)
		{
			Debug.LogWarning("WARNING: Matrix4x4 entry is not formatted properly! Proper format for Matrix4x4 is 16 csv floats (m00,m01,m02,m03,m10,m11..m33)");
			return Matrix4x4.identity;
		}
		Matrix4x4 identity = Matrix4x4.identity;
		identity.m00 = float.Parse(array[0]);
		identity.m01 = float.Parse(array[1]);
		identity.m02 = float.Parse(array[2]);
		identity.m03 = float.Parse(array[3]);
		identity.m10 = float.Parse(array[4]);
		identity.m11 = float.Parse(array[5]);
		identity.m11 = float.Parse(array[6]);
		identity.m12 = float.Parse(array[7]);
		identity.m20 = float.Parse(array[8]);
		identity.m21 = float.Parse(array[9]);
		identity.m22 = float.Parse(array[10]);
		identity.m23 = float.Parse(array[11]);
		identity.m30 = float.Parse(array[12]);
		identity.m31 = float.Parse(array[13]);
		identity.m32 = float.Parse(array[14]);
		identity.m33 = float.Parse(array[15]);
		return identity;
	}

	public static Color ParseColor(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length >= 3 && array.Length <= 4)
		{
			if (array.Length == 3)
			{
				return new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
			}
			return new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		}
		Debug.LogWarning("WARNING: Color entry is not formatted properly! Proper format for Colors is r,g,b{,a}");
		return Color.white;
	}

	public static bool CheckAndParseColor(string colorString, out Color color)
	{
		try
		{
			string[] array = colorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 3 || array.Length > 4)
			{
				Debug.LogWarning("WARNING: Color entry is not formatted properly! Proper format for Colors is r,g,b{,a}");
				color = Color.white;
				return false;
			}
			if (array.Length == 3)
			{
				color = new Color(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture), float.Parse(array[2], CultureInfo.InvariantCulture));
			}
			else
			{
				color = new Color(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture), float.Parse(array[2], CultureInfo.InvariantCulture), float.Parse(array[3], CultureInfo.InvariantCulture));
			}
		}
		catch (Exception)
		{
			Debug.LogWarning("WARNING: Color entry is not formatted properly! Proper format for Colors is r,g,b{,a}");
			color = Color.white;
			return false;
		}
		return true;
	}

	public static Color32 ParseColor32(string vectorString)
	{
		string[] array = vectorString.Split(new char[3] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length >= 3 && array.Length <= 4)
		{
			if (array.Length == 3)
			{
				return new Color32(byte.Parse(array[0]), byte.Parse(array[1]), byte.Parse(array[2]), byte.MaxValue);
			}
			return new Color32(byte.Parse(array[0]), byte.Parse(array[1]), byte.Parse(array[2]), byte.Parse(array[3]));
		}
		Debug.LogWarning("WARNING: Color entry is not formatted properly! Proper format for Colors is r,g,b{,a}");
		return Color.white;
	}

	public static Enum ParseEnum(Type enumType, string vectorString)
	{
		return (Enum)Enum.Parse(enumType, vectorString);
	}

	public static ConfigNode RecurseFormat(List<string[]> cfg)
	{
		int index = 0;
		ConfigNode configNode = new ConfigNode("root");
		RecurseFormat(cfg, ref index, configNode);
		return configNode;
	}

	public static void RecurseFormat(List<string[]> cfg, ref int index, ConfigNode node)
	{
		while (true)
		{
			if (index >= cfg.Count)
			{
				return;
			}
			if (cfg[index].Length == 2)
			{
				node.values.Add(new Value(cfg[index][0], cfg[index][1]));
				index++;
				continue;
			}
			if (cfg[index][0] == "{")
			{
				ConfigNode configNode = new ConfigNode("");
				node.nodes.Add(configNode);
				index++;
				RecurseFormat(cfg, ref index, configNode);
				continue;
			}
			if (cfg[index][0] == "}")
			{
				break;
			}
			if (NextLineIsOpenBrace(cfg, index))
			{
				ConfigNode configNode2 = new ConfigNode(cfg[index][0]);
				node.nodes.Add(configNode2);
				index += 2;
				RecurseFormat(cfg, ref index, configNode2);
			}
			else
			{
				index++;
			}
		}
		index++;
	}

	public static bool NextLineIsOpenBrace(List<string[]> cfg, int index)
	{
		int num = index + 1;
		if (num < cfg.Count && cfg[num].Length == 1 && cfg[num][0] == "{")
		{
			return true;
		}
		return false;
	}

	public static bool NextLineIsCloseBrace(List<string[]> cfg, int index)
	{
		int num = index + 1;
		if (num < cfg.Count && cfg[num].Length == 1 && cfg[num][0] == "}")
		{
			return true;
		}
		return false;
	}

	public static List<string[]> PreFormatConfig(string[] cfgData)
	{
		if (cfgData != null && cfgData.Length >= 1)
		{
			List<string> list = new List<string>(cfgData);
			int num = list.Count;
			while (--num >= 0)
			{
				list[num] = list[num];
				int num2;
				if ((num2 = list[num].IndexOf("//")) != -1)
				{
					if (num2 == 0)
					{
						list.RemoveAt(num);
						continue;
					}
					list[num] = list[num].Remove(num2);
				}
				list[num] = list[num].Trim();
				if (list[num].Length == 0)
				{
					list.RemoveAt(num);
				}
				else if ((num2 = list[num].IndexOf("}", 0)) != -1 && (num2 != 0 || list[num].Length != 1))
				{
					if (num2 > 0)
					{
						list.Insert(num, list[num].Substring(0, num2));
						num++;
						list[num] = list[num].Substring(num2);
						num2 = 0;
					}
					if (num2 < list[num].Length - 1)
					{
						list.Insert(num + 1, list[num].Substring(num2 + 1));
						list[num] = "}";
						num += 2;
					}
				}
				else if ((num2 = list[num].IndexOf("{", 0)) != -1 && (num2 != 0 || list[num].Length != 1))
				{
					if (num2 > 0)
					{
						list.Insert(num, list[num].Substring(0, num2));
						num++;
						list[num] = list[num].Substring(num2);
						num2 = 0;
					}
					if (num2 < list[num].Length - 1)
					{
						list.Insert(num + 1, list[num].Substring(num2 + 1));
						list[num] = "{";
						num += 2;
					}
				}
			}
			List<string[]> list2 = new List<string[]>(list.Count);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				string[] array = CustomEqualSplit(list[i]);
				if (array != null && array.Length != 0)
				{
					list2.Add(array);
				}
			}
			return list2;
		}
		Debug.LogError("Error: Empty part config file");
		return null;
	}

	public static string[] CustomEqualSplit(string str)
	{
		if (str == null)
		{
			return null;
		}
		int num = str.IndexOf('=');
		if (num < 0)
		{
			return new string[1] { str.Trim() };
		}
		string[] array = new string[2];
		if (num == 0)
		{
			array[0] = string.Empty;
			array[1] = str.Substring(1, str.Length - 1).Trim();
		}
		else if (num == str.Length - 1)
		{
			array[0] = str.Substring(0, str.Length - 1).Trim();
			array[1] = string.Empty;
		}
		else
		{
			array[0] = str.Substring(0, num).Trim();
			array[1] = str.Substring(num + 1, str.Length - 1 - num).Trim();
		}
		return array;
	}

	public void WriteNode(StreamWriter sw)
	{
		WriteRootNode(sw);
	}

	public void WriteRootNode(StreamWriter sw)
	{
		int i = 0;
		for (int count = values.Count; i < count; i++)
		{
			Value value = values[i];
			sw.WriteLine(value.name + " = " + value.value + (string.IsNullOrEmpty(value.comment) ? "" : (" // " + value.comment)));
		}
		int j = 0;
		for (int count2 = nodes.Count; j < count2; j++)
		{
			nodes[j].WriteNodeString(sw, "");
		}
		sw.Flush();
	}

	public void WriteNodeString(StreamWriter sw, string indent)
	{
		sw.WriteLine(indent + name + (string.IsNullOrEmpty(comment) ? "" : (" // " + comment)));
		sw.WriteLine(indent + "{");
		string text = indent + "\t";
		int i = 0;
		for (int count = values.Count; i < count; i++)
		{
			Value value = values[i];
			sw.WriteLine(text + value.name + " = " + value.value + (string.IsNullOrEmpty(value.comment) ? "" : (" // " + value.comment)));
		}
		int j = 0;
		for (int count2 = nodes.Count; j < count2; j++)
		{
			nodes[j].WriteNodeString(sw, text);
		}
		sw.WriteLine(indent + "}");
		sw.Flush();
	}

	public static ConfigNode Parse(string s)
	{
		return RecurseFormat(PreFormatConfig(s.Split('\n', '\r')));
	}

	public bool TryGetNode(string name, ref ConfigNode node)
	{
		ConfigNode node2 = GetNode(name);
		if (node2 != null)
		{
			node = node2;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref string value)
	{
		string value2 = GetValue(name);
		if (value2 != null)
		{
			value = value2;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref string[] value)
	{
		string value2 = GetValue(name);
		if (value2 != null)
		{
			value = ParseExtensions.ParseArray(value2);
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref float value)
	{
		string value2 = GetValue(name);
		if (value2 != null && float.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref double value)
	{
		string value2 = GetValue(name);
		if (value2 != null && double.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref int value)
	{
		string value2 = GetValue(name);
		if (value2 != null && int.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref uint value)
	{
		string value2 = GetValue(name);
		if (value2 != null && uint.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref long value)
	{
		string value2 = GetValue(name);
		if (value2 != null && long.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref ulong value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ulong.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref bool value)
	{
		string value2 = GetValue(name);
		if (value2 != null && bool.TryParse(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector3 value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector3(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector3d value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector3d(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector2 value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector2(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector2d value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector2d(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector4 value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector4(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Vector4d value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseVector4d(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Quaternion value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseQuaternion(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref QuaternionD value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseQuaternionD(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Rect value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseRect(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool TryGetValue(string name, ref Color value)
	{
		string value2 = GetValue(name);
		if (value2 != null)
		{
			if (ParseExtensions.TryParseColor(value2, out var result))
			{
				value = result;
				return true;
			}
			if (ColorUtility.TryParseHtmlString(value2, out result))
			{
				value = result;
				return true;
			}
		}
		return false;
	}

	public bool TryGetValue(string name, ref Color32 value)
	{
		string value2 = GetValue(name);
		if (value2 != null && ParseExtensions.TryParseColor32(value2, out var result))
		{
			value = result;
			return true;
		}
		return false;
	}

	public bool HasValues(params string[] values)
	{
		return Array.TrueForAll(values, (string v) => HasValue(v));
	}

	public bool TryGetValue(string name, ref Guid value)
	{
		string value2 = GetValue(name);
		if (value2 != null)
		{
			try
			{
				value = new Guid(value2);
				return true;
			}
			catch (Exception ex)
			{
				Debug.Log("Unable to TryGetValue Guid " + name);
				Debug.Log("Err: " + ex);
				value = Guid.Empty;
				return false;
			}
		}
		return false;
	}

	public bool TryGetEnum<T>(string name, ref T value, T defaultValue) where T : IComparable, IFormattable, IConvertible
	{
		string value2 = "";
		value = defaultValue;
		if (TryGetValue(name, ref value2))
		{
			try
			{
				if (Enum.IsDefined(typeof(T), value2))
				{
					value = (T)Enum.Parse(typeof(T), value2);
					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.Log("Unable to TryGetValue on Enum " + name);
				Debug.Log("Err: " + ex);
				return false;
			}
		}
		return false;
	}

	public bool TryGetEnum(string name, Type enumType, ref Enum value)
	{
		string value2 = "";
		if (TryGetValue(name, ref value2))
		{
			try
			{
				if (Enum.IsDefined(enumType, value2))
				{
					value = (Enum)Enum.Parse(enumType, value2);
					return true;
				}
			}
			catch (Exception ex)
			{
				Debug.Log("Unable to TryGetValue on Enum " + name);
				Debug.Log("Err: " + ex);
				return false;
			}
		}
		return false;
	}
}
