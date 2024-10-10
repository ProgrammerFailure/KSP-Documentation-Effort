using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ns8;

public class PluginConfigNode
{
	public Dictionary<string, object> map = new Dictionary<string, object>();

	public PluginConfigNode parent;

	public object this[string key]
	{
		get
		{
			if (!map.ContainsKey(key))
			{
				return null;
			}
			return map[key];
		}
		set
		{
			if (!map.ContainsKey(key))
			{
				map.Add(key, value);
			}
			else
			{
				map[key] = value;
			}
		}
	}

	public PluginConfigNode(PluginConfigNode parent, Dictionary<string, object> _map)
	{
		this.parent = parent;
		map = _map;
	}

	public PluginConfigNode(PluginConfigNode parent)
	{
		this.parent = parent;
	}

	public PluginConfigNode()
	{
		parent = null;
	}

	public PluginConfigNode GetParent()
	{
		return parent;
	}

	public T GetValue<T>(string key)
	{
		if (!map.ContainsKey(key))
		{
			return default(T);
		}
		object obj = map[key];
		if (obj is T)
		{
			return (T)obj;
		}
		return default(T);
	}

	public T GetValue<T>(string key, T _default)
	{
		if (!map.ContainsKey(key))
		{
			map.Add(key, _default);
			return _default;
		}
		object obj = map[key];
		if (obj is T)
		{
			return (T)obj;
		}
		return _default;
	}

	public void SetValue(string key, object value)
	{
		if (!map.ContainsKey(key))
		{
			map.Add(key, value);
		}
		else
		{
			map[key] = value;
		}
	}

	public void serializeXML(ref XmlTextWriter xml)
	{
		foreach (KeyValuePair<string, object> item in map)
		{
			string key = item.Key;
			object value = item.Value;
			switch (value.GetType().Name.ToLower())
			{
			case "vector3":
			{
				StartNamedElement(ref xml, key, "vector3");
				Vector3 vector2 = (Vector3)value;
				xml.WriteElementString("x", vector2.x.ToString("G9"));
				xml.WriteElementString("y", vector2.y.ToString("G9"));
				xml.WriteElementString("z", vector2.z.ToString("G9"));
				xml.WriteEndElement();
				break;
			}
			case "vector2":
			{
				StartNamedElement(ref xml, key, "vector2");
				Vector2 vector = (Vector2)value;
				xml.WriteElementString("x", vector.x.ToString("G9"));
				xml.WriteElementString("y", vector.y.ToString("G9"));
				xml.WriteEndElement();
				break;
			}
			case "string":
				StartNamedElement(ref xml, key, "string");
				xml.WriteString((string)value);
				xml.WriteEndElement();
				break;
			case "keycode":
				StartNamedElement(ref xml, key, "keycode");
				xml.WriteString(Enum.GetName(typeof(KeyCode), (KeyCode)value));
				xml.WriteEndElement();
				break;
			case "vector4":
			{
				StartNamedElement(ref xml, key, "vector4");
				Vector4 vector3 = (Vector4)value;
				xml.WriteElementString("w", vector3.w.ToString("G9"));
				xml.WriteElementString("x", vector3.x.ToString("G9"));
				xml.WriteElementString("y", vector3.y.ToString("G9"));
				xml.WriteElementString("z", vector3.z.ToString("G9"));
				xml.WriteEndElement();
				break;
			}
			case "byte":
				StartNamedElement(ref xml, key, "byte");
				xml.WriteString(((byte)value).ToString());
				xml.WriteEndElement();
				break;
			case "float":
				StartNamedElement(ref xml, key, "float");
				xml.WriteString(((float)value).ToString("G9"));
				xml.WriteEndElement();
				break;
			case "double":
				StartNamedElement(ref xml, key, "double");
				xml.WriteString(((double)value).ToString("G17"));
				xml.WriteEndElement();
				break;
			case "int64":
			case "long":
				StartNamedElement(ref xml, key, "long");
				xml.WriteString(((long)value).ToString());
				xml.WriteEndElement();
				break;
			case "int16":
			case "short":
				StartNamedElement(ref xml, key, "short");
				xml.WriteString(((short)value).ToString());
				xml.WriteEndElement();
				break;
			case "char":
				StartNamedElement(ref xml, key, "char");
				xml.WriteString(((char)value).ToString());
				xml.WriteEndElement();
				break;
			case "vector3d":
			{
				StartNamedElement(ref xml, key, "vector3d");
				Vector3d vector3d = (Vector3d)value;
				xml.WriteElementString("x", vector3d.x.ToString("G17"));
				xml.WriteElementString("y", vector3d.y.ToString("G17"));
				xml.WriteElementString("z", vector3d.z.ToString("G17"));
				xml.WriteEndElement();
				break;
			}
			case "pluginconfignode":
				StartNamedElement(ref xml, key, "node");
				((PluginConfigNode)value).serializeXML(ref xml);
				xml.WriteEndElement();
				break;
			case "boolean":
			case "bool":
				StartNamedElement(ref xml, key, "bool");
				xml.WriteString(((bool)value) ? "1" : "0");
				xml.WriteEndElement();
				break;
			case "int":
			case "int32":
				StartNamedElement(ref xml, key, "int");
				xml.WriteString(((int)value).ToString());
				xml.WriteEndElement();
				break;
			case "quaternion":
			{
				StartNamedElement(ref xml, key, "quaternion");
				Quaternion quaternion = (Quaternion)value;
				xml.WriteElementString("w", quaternion.w.ToString("G9"));
				xml.WriteElementString("x", quaternion.x.ToString("G9"));
				xml.WriteElementString("y", quaternion.y.ToString("G9"));
				xml.WriteElementString("z", quaternion.z.ToString("G9"));
				xml.WriteEndElement();
				break;
			}
			case "rect":
			{
				StartNamedElement(ref xml, key, "rect");
				Rect rect = (Rect)value;
				xml.WriteElementString("xmin", rect.xMin.ToString("G9"));
				xml.WriteElementString("xmax", rect.xMax.ToString("G9"));
				xml.WriteElementString("ymin", rect.yMin.ToString("G9"));
				xml.WriteElementString("ymax", rect.yMax.ToString("G9"));
				xml.WriteEndElement();
				break;
			}
			}
		}
	}

	public void deserializeXML(XmlNode node)
	{
		if (!node.HasChildNodes)
		{
			return;
		}
		foreach (XmlNode childNode in node.ChildNodes)
		{
			if (childNode != null && childNode.NodeType == XmlNodeType.Element)
			{
				handleElement(childNode);
			}
		}
	}

	public void handleElement(XmlNode node)
	{
		string value = node.Attributes["name"].Value;
		try
		{
			switch (node.Name)
			{
			case "vector2":
			{
				Vector2 vector3 = default(Vector2);
				vector3.x = float.Parse(node.ChildNodes[0].InnerText);
				vector3.y = float.Parse(node.ChildNodes[1].InnerText);
				SetValue(value, vector3);
				break;
			}
			case "string":
				SetValue(value, node.InnerText);
				break;
			case "vector4":
			{
				Vector4 vector2 = default(Vector4);
				vector2.w = float.Parse(node.ChildNodes[0].InnerText);
				vector2.x = float.Parse(node.ChildNodes[1].InnerText);
				vector2.y = float.Parse(node.ChildNodes[2].InnerText);
				vector2.z = float.Parse(node.ChildNodes[3].InnerText);
				SetValue(value, vector2);
				break;
			}
			case "vector3":
			{
				Vector3 vector = default(Vector3);
				vector.x = float.Parse(node.ChildNodes[0].InnerText);
				vector.y = float.Parse(node.ChildNodes[1].InnerText);
				vector.z = float.Parse(node.ChildNodes[2].InnerText);
				SetValue(value, vector);
				break;
			}
			case "byte":
				SetValue(value, byte.Parse(node.InnerText));
				break;
			case "keycode":
				SetValue(value, Enum.Parse(typeof(KeyCode), node.InnerText));
				break;
			case "double":
				SetValue(value, double.Parse(node.InnerText));
				break;
			case "int":
				SetValue(value, int.Parse(node.InnerText));
				break;
			case "char":
				SetValue(value, node.InnerText[0]);
				break;
			case "float":
				SetValue(value, float.Parse(node.InnerText));
				break;
			case "long":
				SetValue(value, long.Parse(node.InnerText));
				break;
			case "short":
				SetValue(value, short.Parse(node.InnerText));
				break;
			case "pluginconfignode":
				new PluginConfigNode(this).deserializeXML(node);
				SetValue(value, node);
				break;
			case "bool":
				SetValue(value, node.InnerText == "1");
				break;
			case "quaternion":
			{
				Quaternion quaternion = default(Quaternion);
				quaternion.w = float.Parse(node.ChildNodes[0].InnerText);
				quaternion.x = float.Parse(node.ChildNodes[1].InnerText);
				quaternion.y = float.Parse(node.ChildNodes[2].InnerText);
				quaternion.z = float.Parse(node.ChildNodes[3].InnerText);
				SetValue(value, quaternion);
				break;
			}
			case "rect":
			{
				Rect rect = default(Rect);
				rect.xMin = float.Parse(node.ChildNodes[0].InnerText);
				rect.xMax = float.Parse(node.ChildNodes[1].InnerText);
				rect.yMin = float.Parse(node.ChildNodes[2].InnerText);
				rect.yMax = float.Parse(node.ChildNodes[3].InnerText);
				SetValue(value, rect);
				break;
			}
			case "vector3d":
			{
				Vector3d vector3d = default(Vector3d);
				vector3d.x = double.Parse(node.ChildNodes[0].InnerText);
				vector3d.y = double.Parse(node.ChildNodes[1].InnerText);
				vector3d.z = double.Parse(node.ChildNodes[2].InnerText);
				SetValue(value, vector3d);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			string text = $"ERROR PARSING K:V {node.Name}:{node.InnerText}: ";
			MonoBehaviour.print(text + node.OuterXml + " could not be parsed: " + ex);
		}
	}

	public void StartNamedElement(ref XmlTextWriter xml, string name, string typeID)
	{
		xml.WriteStartElement(typeID);
		xml.WriteAttributeString("name", name);
	}
}
