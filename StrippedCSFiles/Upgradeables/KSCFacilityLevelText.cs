using System;
using System.Runtime.CompilerServices;

namespace Upgradeables;

[Serializable]
public class KSCFacilityLevelText
{
	public string textBase;

	public SpaceCenterFacility facility;

	public string linePrefix;

	private static string[] FacilityNames;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSCFacilityLevelText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KSCFacilityLevelText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetText(float normLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string ProcessTextTags(string src, float level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetValue(string valueTag, float facilityLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetValueOrFallback(float value, float threshold, string format, string fallback, Func<float, string> postProcess = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static KSCFacilityLevelText CreateFromAsset(KSCUpgradeableLevelText asset)
	{
		throw null;
	}
}
