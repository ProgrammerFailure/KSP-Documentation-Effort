using System.IO;
using System.Text;
using System.Xml;

namespace ns8;

public class PluginConfiguration
{
	public string file;

	public PluginConfigNode root;

	public object this[string key]
	{
		get
		{
			return root[key];
		}
		set
		{
			root[key] = value;
		}
	}

	public PluginConfiguration(string pathToFile)
	{
		file = pathToFile;
		root = new PluginConfigNode();
	}

	public T GetValue<T>(string key)
	{
		return root.GetValue<T>(key);
	}

	public T GetValue<T>(string key, T _default)
	{
		return root.GetValue(key, _default);
	}

	public void SetValue(string key, object value)
	{
		root.SetValue(key, value);
	}

	public static PluginConfiguration CreateForType<T>(Vessel flight = null)
	{
		return new PluginConfiguration(IOUtils.GetFilePathFor(typeof(T), "config.xml", flight));
	}

	public void save()
	{
		try
		{
			if (!Directory.Exists(Path.GetDirectoryName(file)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(file));
			}
			XmlTextWriter xml = new XmlTextWriter(file, Encoding.UTF8);
			xml.Formatting = Formatting.Indented;
			xml.Indentation = 4;
			xml.IndentChar = ' ';
			xml.WriteStartDocument();
			xml.WriteStartElement("config");
			root.serializeXML(ref xml);
			xml.WriteEndElement();
			xml.WriteEndDocument();
			xml.Close();
		}
		catch (System.IO.IOException e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void load()
	{
		try
		{
			if (!Directory.Exists(Path.GetDirectoryName(file)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(file));
			}
			if (System.IO.File.Exists(file))
			{
				root = new PluginConfigNode();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(file);
				root.deserializeXML(xmlDocument.GetElementsByTagName("config")[0]);
			}
		}
		catch (System.IO.IOException e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
