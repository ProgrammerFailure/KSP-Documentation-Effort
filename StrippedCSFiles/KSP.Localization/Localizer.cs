using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.Localization;

public class Localizer
{
	private static Dictionary<string, string> LanguageIds;

	private Dictionary<string, string> tagValues;

	private string _currentLanguage;

	private bool _showKeysInGame;

	private bool _overrideMELock;

	private static List<string> missingKeysList;

	private bool _debugWriteMissingKeysToLog;

	private string missingKeyPrefix;

	private List<string> outputForReplaceTags;

	public static Localizer Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public static int TagsLength
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Dictionary<string, string> Tags
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static string CurrentLanguage
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal static bool ShowKeysOnScreen
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

	internal static bool OverrideMELock
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

	internal static bool debugWriteMissingKeysToLog
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
	public Localizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Localizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Localizer Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetStringByTag(string tag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Format(string template, params string[] list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Format(string template, params object[] list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string Format(string template)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool TryGetStringByTag(string tagName, out string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void TranslateBranch(ConfigNode branchRoot)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string _Format(string template, string[] parameterList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string FormatWithLingoona(string template, string[] parameterList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ReplaceSingleTagIfFound(string tag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<string> ReplaceTags(string[] list)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetLanguageIdFromFile()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SwitchToLanguage(string lang)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _SwitchToLanguage(string lang)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void _CacheLocalStrings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshRuleSet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshTagValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddTagValuesForLanguage(string lang)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AppendMissionTags(string lang)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddTagValuesFromNode(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string unescapeStringToLingoona(string originalString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string unescapeFormattedString(string formattedString)
	{
		throw null;
	}
}
