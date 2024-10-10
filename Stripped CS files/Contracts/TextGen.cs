using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace Contracts;

public static class TextGen
{
	public static Dictionary<string, ListDictionary<string, string>> texts = new Dictionary<string, ListDictionary<string, string>>();

	public static bool setup;

	public static string currentContractType;

	public static string currentAgency;

	public static string currentTopic;

	public static string currentSubject;

	public static void Setup()
	{
		ConfigNode mergedConfigNodes = GameDatabase.Instance.GetMergedConfigNodes("STORY_DEF", mergeChildren: true);
		texts.Clear();
		int num = 0;
		for (int i = 0; i < mergedConfigNodes.nodes.Count; i++)
		{
			ConfigNode configNode = mergedConfigNodes.nodes[i];
			texts[configNode.name] = new ListDictionary<string, string>();
			int count = configNode.values.Count;
			while (count-- > 0)
			{
				texts[configNode.name].Add(configNode.values[count].name, configNode.values[count].value);
				num++;
			}
		}
		Debug.Log("Text Generator Loaded: " + num + " entries.");
		setup = true;
	}

	public static string GenerateBackStories(string contractType, string agency, string topic, string subject, int seed, bool allowGenericIntroduction, bool allowGenericProblem, bool allowGenericConclusion)
	{
		if (!setup)
		{
			Setup();
		}
		currentContractType = (contractType.EndsWith("Contract") ? contractType.Substring(0, contractType.LastIndexOf("Contract")) : contractType);
		currentAgency = agency;
		currentSubject = subject;
		currentTopic = topic;
		Random.InitState(seed);
		string text = StringBuilderCache.Format("{0} {1} {2}", GetSentence("Introduction", allowGenericIntroduction), GetSentence("Problem", allowGenericProblem), GetSentence("Conclusion", allowGenericConclusion));
		int num = 8;
		int num2 = 0;
		while (text.Contains("[") && num2 < num)
		{
			num2++;
			text = ProcessTextReplacements(text, num2 >= num);
		}
		return Localizer.Format(text, currentAgency, currentTopic);
	}

	public static string GetSentence(string sentenceType, bool allowGeneric)
	{
		ListDictionary<string, string> value = null;
		if (!texts.TryGetValue(sentenceType, out value))
		{
			Debug.LogError(StringBuilderCache.Format("[TextGen]: No loaded texts for '{0}'", sentenceType));
			return string.Empty;
		}
		int num = 0;
		List<string> val = null;
		if (!string.IsNullOrEmpty(currentSubject) && value.TryGetValue(currentSubject, out val))
		{
			num += val.Count;
			if (val.Count == 0)
			{
				val = null;
			}
		}
		List<string> val2 = null;
		if ((num == 0 || allowGeneric) && value.TryGetValue(currentContractType, out val2))
		{
			num += val2.Count;
			if (val2.Count == 0)
			{
				val2 = null;
			}
		}
		List<string> val3 = null;
		if ((num == 0 || allowGeneric) && value.TryGetValue("Generic", out val3))
		{
			num += val3.Count;
			if (val3.Count == 0)
			{
				val3 = null;
			}
		}
		int num2 = Random.Range(0, num);
		if (val2 != null)
		{
			if (num2 < val2.Count)
			{
				return val2[num2];
			}
			num2 -= val2.Count;
		}
		if (val != null)
		{
			if (num2 < val.Count)
			{
				return val[num2];
			}
			num2 -= val.Count;
		}
		if (val3 != null && num2 < val3.Count)
		{
			return val3[num2];
		}
		Debug.LogWarning(StringBuilderCache.Format("[TextGen]: Unable to generate text for {0}!", sentenceType));
		return string.Empty;
	}

	public static string ProcessTextReplacements(string src, bool wipeReplacements)
	{
		if (src.Contains("["))
		{
			int num = src.LastIndexOf('[');
			while (num != -1)
			{
				int num2 = src.IndexOf(']', num);
				if (num2 != -1)
				{
					int startIndex = num + 1;
					int length = num2 - num - 1;
					string text = src.Substring(startIndex, length);
					if (!text.Contains("[") && !text.Contains("]"))
					{
						bool flag = false;
						if (num >= 4)
						{
							string text2 = src.Substring(num - 4, 4);
							if (text2[0] == '<' && text2[1] == '<' && text2[3] == ':')
							{
								flag = true;
							}
						}
						src = src.Remove(num, num2 - num + 1);
						string value = string.Empty;
						if (!(text == "Agency"))
						{
							if (text == "Topic")
							{
								value = ((!flag) ? "<<2>>" : "2");
							}
						}
						else
						{
							value = ((!flag) ? "<<1>>" : "1");
						}
						src = src.Insert(num, value);
						num = src.LastIndexOf('[');
						continue;
					}
					Debug.LogError("TextGenerator Syntax Error: Mismatched brackets on replacement block: " + src + ". nesting is not supported, ensure all opening brackets are closed.");
					return src;
				}
				Debug.LogError("TextGenerator Syntax Error: Missing closing bracket on replacement block: " + src);
				return src;
			}
		}
		return src;
	}

	public static string ReplaceCharAt(string src, char repl, int index)
	{
		return src.Remove(index, 1).Insert(index, repl.ToString());
	}
}
