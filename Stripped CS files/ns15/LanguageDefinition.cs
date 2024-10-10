using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ns15;

[Serializable]
public class LanguageDefinition
{
	public string name;

	public string font;

	public Dictionary<string, string> translationDict;

	public Dictionary<string, string> untranslated;

	public bool storeUntranslated;

	public List<DictionarySerializerHelper> translations;

	public bool overwrite;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string Font
	{
		get
		{
			return font;
		}
		set
		{
			font = value;
		}
	}

	public bool StoreUntranslated
	{
		get
		{
			return storeUntranslated;
		}
		set
		{
			storeUntranslated = value;
		}
	}

	public LanguageDefinition()
	{
		translations = new List<DictionarySerializerHelper>();
		translationDict = new Dictionary<string, string>();
		untranslated = new Dictionary<string, string>();
	}

	public LanguageDefinition(string langName, string fontName)
	{
		name = langName;
		font = fontName;
		translations = new List<DictionarySerializerHelper>();
		translationDict = new Dictionary<string, string>();
		untranslated = new Dictionary<string, string>();
	}

	public LanguageDefinition(string path, bool localPath = true)
	{
		translationDict = new Dictionary<string, string>();
		translations = new List<DictionarySerializerHelper>();
		Deserialize(path, localPath);
	}

	public string Translation(string input)
	{
		if (translationDict.TryGetValue(input, out var value))
		{
			return value;
		}
		if (storeUntranslated)
		{
			untranslated[input] = "UNTRANSLATED";
		}
		return input;
	}

	public string TranslateWithFormat(string baseString, params object[] args)
	{
		string format = Translation(baseString);
		int num = args.Length;
		object[] array = new object[num];
		int num2 = num;
		while (num2-- > 0)
		{
			if (args[num2] is string)
			{
				array[num2] = Translation(args[num2] as string);
			}
			else
			{
				array[num2] = args[num2];
			}
		}
		return string.Format(format, array);
	}

	public void AddTranslation(string input, string output)
	{
		translationDict[input] = output;
	}

	public void RemoveTranslation(string input)
	{
		translationDict.Remove(input);
	}

	public void MergeInto(LanguageDefinition parent)
	{
		foreach (KeyValuePair<string, string> item in translationDict)
		{
			if (overwrite || !parent.translationDict.ContainsKey(item.Key))
			{
				parent.translationDict[item.Key] = item.Value;
			}
		}
		if (overwrite && !string.IsNullOrEmpty(font))
		{
			parent.font = font;
		}
	}

	public string GetFullPath(string path)
	{
		return Path.Combine(Path.Combine(KSPUtil.ApplicationRootPath, "GameData"), path);
	}

	public void Serialize(string path, bool localPath = true)
	{
		if (localPath)
		{
			path = GetFullPath(path);
		}
		TextWriter textWriter = new StreamWriter(path);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LanguageDefinition));
		if (textWriter != null && xmlSerializer != null)
		{
			translations.Clear();
			foreach (KeyValuePair<string, string> item in translationDict)
			{
				translations.Add(new DictionarySerializerHelper(item.Key, item.Value));
			}
			xmlSerializer.Serialize(textWriter, this);
			textWriter.Close();
		}
		translations.Clear();
	}

	public void SerializeUntranslated(string path, bool localPath = true)
	{
		if (localPath)
		{
			path = GetFullPath(path);
		}
		TextWriter textWriter = new StreamWriter(path);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LanguageDefinition));
		if (textWriter != null && xmlSerializer != null)
		{
			translations.Clear();
			foreach (KeyValuePair<string, string> item in untranslated)
			{
				translations.Add(new DictionarySerializerHelper(item.Key, item.Value));
			}
			xmlSerializer.Serialize(textWriter, this);
			textWriter.Close();
		}
		translations.Clear();
	}

	public void Deserialize(string path, bool localPath = true)
	{
		if (localPath)
		{
			path = GetFullPath(path);
		}
		TextReader textReader = new StreamReader(path);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(LanguageDefinition));
		if (textReader == null || xmlSerializer == null)
		{
			return;
		}
		LanguageDefinition languageDefinition = (LanguageDefinition)xmlSerializer.Deserialize(textReader);
		name = languageDefinition.name;
		font = languageDefinition.font;
		foreach (DictionarySerializerHelper translation in languageDefinition.translations)
		{
			translationDict[(string)translation.Key] = (string)translation.Value;
		}
		textReader.Close();
	}
}
