using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LineBreakingTable()
		{
			throw null;
		}
	}

	private static TMP_Settings s_Instance;

	[SerializeField]
	private bool m_enableWordWrapping;

	[SerializeField]
	private bool m_enableKerning;

	[SerializeField]
	private bool m_enableExtraPadding;

	[SerializeField]
	private bool m_enableTintAllSprites;

	[SerializeField]
	private bool m_enableParseEscapeCharacters;

	[SerializeField]
	private int m_missingGlyphCharacter;

	[SerializeField]
	private bool m_warningsDisabled;

	[SerializeField]
	private TMP_FontAsset m_defaultFontAsset;

	[SerializeField]
	private string m_defaultFontAssetPath;

	[SerializeField]
	private float m_defaultFontSize;

	[SerializeField]
	private float m_defaultAutoSizeMinRatio;

	[SerializeField]
	private float m_defaultAutoSizeMaxRatio;

	[SerializeField]
	private Vector2 m_defaultTextMeshProTextContainerSize;

	[SerializeField]
	private Vector2 m_defaultTextMeshProUITextContainerSize;

	[SerializeField]
	private bool m_autoSizeTextContainer;

	[SerializeField]
	private List<TMP_FontAsset> m_fallbackFontAssets;

	[SerializeField]
	private bool m_matchMaterialPreset;

	[SerializeField]
	private TMP_SpriteAsset m_defaultSpriteAsset;

	[SerializeField]
	private string m_defaultSpriteAssetPath;

	[SerializeField]
	private string m_defaultColorGradientPresetsPath;

	[SerializeField]
	private bool m_enableEmojiSupport;

	[SerializeField]
	private TMP_StyleSheet m_defaultStyleSheet;

	[SerializeField]
	private TextAsset m_leadingCharacters;

	[SerializeField]
	private TextAsset m_followingCharacters;

	[SerializeField]
	private LineBreakingTable m_linebreakingRules;

	public static string version
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableWordWrapping
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableKerning
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableExtraPadding
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableTintAllSprites
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableParseEscapeCharacters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static int missingGlyphCharacter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool warningsDisabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static TMP_FontAsset defaultFontAsset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string defaultFontAssetPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float defaultFontSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float defaultTextAutoSizingMinRatio
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float defaultTextAutoSizingMaxRatio
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector2 defaultTextMeshProTextContainerSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Vector2 defaultTextMeshProUITextContainerSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool autoSizeTextContainer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<TMP_FontAsset> fallbackFontAssets
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool matchMaterialPreset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static TMP_SpriteAsset defaultSpriteAsset
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string defaultSpriteAssetPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string defaultColorGradientPresetsPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool enableEmojiSupport
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

	public static TMP_StyleSheet defaultStyleSheet
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static TextAsset leadingCharacters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static TextAsset followingCharacters
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static LineBreakingTable linebreakingRules
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static TMP_Settings instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_Settings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_Settings LoadDefaultSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_Settings GetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_FontAsset GetFontAsset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_SpriteAsset GetSpriteAsset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TMP_StyleSheet GetStyleSheet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void LoadLinebreakingRules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Dictionary<int, char> GetCharacters(TextAsset file)
	{
		throw null;
	}
}
