using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro;

[Serializable]
[ExecuteInEditMode]
public class TMP_Settings : ScriptableObject
{
	public class LineBreakingTable
	{
		public Dictionary<int, char> leadingCharacters;

		public Dictionary<int, char> followingCharacters;
	}

	public static TMP_Settings s_Instance;

	[SerializeField]
	public bool m_enableWordWrapping;

	[SerializeField]
	public bool m_enableKerning;

	[SerializeField]
	public bool m_enableExtraPadding;

	[SerializeField]
	public bool m_enableTintAllSprites;

	[SerializeField]
	public bool m_enableParseEscapeCharacters;

	[SerializeField]
	public int m_missingGlyphCharacter;

	[SerializeField]
	public bool m_warningsDisabled;

	[SerializeField]
	public TMP_FontAsset m_defaultFontAsset;

	[SerializeField]
	public string m_defaultFontAssetPath;

	[SerializeField]
	public float m_defaultFontSize;

	[SerializeField]
	public float m_defaultAutoSizeMinRatio;

	[SerializeField]
	public float m_defaultAutoSizeMaxRatio;

	[SerializeField]
	public Vector2 m_defaultTextMeshProTextContainerSize;

	[SerializeField]
	public Vector2 m_defaultTextMeshProUITextContainerSize;

	[SerializeField]
	public bool m_autoSizeTextContainer;

	[SerializeField]
	public List<TMP_FontAsset> m_fallbackFontAssets;

	[SerializeField]
	public bool m_matchMaterialPreset;

	[SerializeField]
	public TMP_SpriteAsset m_defaultSpriteAsset;

	[SerializeField]
	public string m_defaultSpriteAssetPath;

	[SerializeField]
	public string m_defaultColorGradientPresetsPath;

	[SerializeField]
	public bool m_enableEmojiSupport;

	[SerializeField]
	public TMP_StyleSheet m_defaultStyleSheet;

	[SerializeField]
	public TextAsset m_leadingCharacters;

	[SerializeField]
	public TextAsset m_followingCharacters;

	[SerializeField]
	public LineBreakingTable m_linebreakingRules;

	public static string version => "1.2.2";

	public static bool enableWordWrapping => instance.m_enableWordWrapping;

	public static bool enableKerning => instance.m_enableKerning;

	public static bool enableExtraPadding => instance.m_enableExtraPadding;

	public static bool enableTintAllSprites => instance.m_enableTintAllSprites;

	public static bool enableParseEscapeCharacters => instance.m_enableParseEscapeCharacters;

	public static int missingGlyphCharacter => instance.m_missingGlyphCharacter;

	public static bool warningsDisabled => instance.m_warningsDisabled;

	public static TMP_FontAsset defaultFontAsset => instance.m_defaultFontAsset;

	public static string defaultFontAssetPath => instance.m_defaultFontAssetPath;

	public static float defaultFontSize => instance.m_defaultFontSize;

	public static float defaultTextAutoSizingMinRatio => instance.m_defaultAutoSizeMinRatio;

	public static float defaultTextAutoSizingMaxRatio => instance.m_defaultAutoSizeMaxRatio;

	public static Vector2 defaultTextMeshProTextContainerSize => instance.m_defaultTextMeshProTextContainerSize;

	public static Vector2 defaultTextMeshProUITextContainerSize => instance.m_defaultTextMeshProUITextContainerSize;

	public static bool autoSizeTextContainer => instance.m_autoSizeTextContainer;

	public static List<TMP_FontAsset> fallbackFontAssets => instance.m_fallbackFontAssets;

	public static bool matchMaterialPreset => instance.m_matchMaterialPreset;

	public static TMP_SpriteAsset defaultSpriteAsset => instance.m_defaultSpriteAsset;

	public static string defaultSpriteAssetPath => instance.m_defaultSpriteAssetPath;

	public static string defaultColorGradientPresetsPath => instance.m_defaultColorGradientPresetsPath;

	public static bool enableEmojiSupport
	{
		get
		{
			return instance.m_enableEmojiSupport;
		}
		set
		{
			instance.m_enableEmojiSupport = value;
		}
	}

	public static TMP_StyleSheet defaultStyleSheet => instance.m_defaultStyleSheet;

	public static TextAsset leadingCharacters => instance.m_leadingCharacters;

	public static TextAsset followingCharacters => instance.m_followingCharacters;

	public static LineBreakingTable linebreakingRules
	{
		get
		{
			if (instance.m_linebreakingRules == null)
			{
				LoadLinebreakingRules();
			}
			return instance.m_linebreakingRules;
		}
	}

	public static TMP_Settings instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = Resources.Load<TMP_Settings>("TMP Settings");
			}
			return s_Instance;
		}
	}

	public static TMP_Settings LoadDefaultSettings()
	{
		if (s_Instance == null)
		{
			TMP_Settings tMP_Settings = Resources.Load<TMP_Settings>("TMP Settings");
			if (tMP_Settings != null)
			{
				s_Instance = tMP_Settings;
			}
		}
		return s_Instance;
	}

	public static TMP_Settings GetSettings()
	{
		if (instance == null)
		{
			return null;
		}
		return instance;
	}

	public static TMP_FontAsset GetFontAsset()
	{
		if (instance == null)
		{
			return null;
		}
		return instance.m_defaultFontAsset;
	}

	public static TMP_SpriteAsset GetSpriteAsset()
	{
		if (instance == null)
		{
			return null;
		}
		return instance.m_defaultSpriteAsset;
	}

	public static TMP_StyleSheet GetStyleSheet()
	{
		if (instance == null)
		{
			return null;
		}
		return instance.m_defaultStyleSheet;
	}

	public static void LoadLinebreakingRules()
	{
		if (!(instance == null))
		{
			if (s_Instance.m_linebreakingRules == null)
			{
				s_Instance.m_linebreakingRules = new LineBreakingTable();
			}
			s_Instance.m_linebreakingRules.leadingCharacters = GetCharacters(s_Instance.m_leadingCharacters);
			s_Instance.m_linebreakingRules.followingCharacters = GetCharacters(s_Instance.m_followingCharacters);
		}
	}

	public static Dictionary<int, char> GetCharacters(TextAsset file)
	{
		Dictionary<int, char> dictionary = new Dictionary<int, char>();
		string text = file.text;
		foreach (char c in text)
		{
			if (!dictionary.ContainsKey(c))
			{
				dictionary.Add(c, c);
			}
		}
		return dictionary;
	}
}
