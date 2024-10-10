using System;
using ns9;
using UnityEngine;

[Serializable]
public class MiniBiome : ScriptableObject
{
	public string TagKey = "";

	public string LocalizedTag = "";

	public string TagKeyID = "";

	public string GetTagKeyString => ConvertTagtoBiome(TagKey);

	public string GetDisplayName
	{
		get
		{
			if (!string.IsNullOrEmpty(LocalizedTag))
			{
				return Localizer.Format(LocalizedTag);
			}
			return "";
		}
	}

	public string GetLocalizedTag
	{
		get
		{
			if (!string.IsNullOrEmpty(LocalizedTag))
			{
				return LocalizedTag;
			}
			return "";
		}
	}

	public static string ConvertTagtoLandedAt(string tagname)
	{
		if (string.IsNullOrEmpty(tagname))
		{
			return string.Empty;
		}
		if (!tagname.Contains("Pad") && !tagname.Contains("LaunchPad"))
		{
			if (tagname.Contains("Runway"))
			{
				return "Runway";
			}
			string text = tagname;
			if (tagname.Contains("KSC") && tagname != "KSC")
			{
				text = text.Replace("KSC", "");
			}
			if (tagname.Contains("Grounds") && tagname != "Grounds")
			{
				text = text.Replace("Grounds", "");
			}
			return text.Replace('_', ' ').Trim();
		}
		return "LaunchPad";
	}

	public static string ConvertTagtoBiome(string tagname)
	{
		tagname = ConvertTagtoLandedAt(tagname);
		return tagname.Replace(" ", string.Empty).Trim();
	}
}
