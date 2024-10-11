using System;
using System.Runtime.CompilerServices;

[Serializable]
public class DiscoveryInfo : IConfigNode
{
	private IDiscoverable host;

	public KnowledgeItem<string> trackingStatus;

	public KnowledgeItem<UntrackedObjectClass> size;

	public KnowledgeItem<double> signalStrengthPercent;

	public KnowledgeItem<string> signalStrengthLevel;

	public KnowledgeItem<string> name;

	public KnowledgeItem<string> displayName;

	public KnowledgeItem<string> situation;

	public KnowledgeItem<double> distance;

	public KnowledgeItem<double> speed;

	public KnowledgeItem<string> type;

	public KnowledgeItem<float> mass;

	private static string cacheAutoLOC_475347;

	private static string cacheAutoLOC_6002221;

	private static string cacheAutoLOC_6002222;

	private static string cacheAutoLOC_900650;

	private static string cacheAutoLOC_461382;

	private static string cacheAutoLOC_900652;

	private static string cacheAutoLOC_6002223;

	private static string cacheAutoLOC_6002220;

	private static string cacheAutoLOC_6002224;

	private static string cacheAutoLOC_463448;

	private static string cacheAutoLOC_6002225;

	private static string cacheAutoLOC_900381;

	private static string cacheAutoLOC_6002226;

	private static string cacheAutoLOC_7001415;

	private static string cacheAutoLOC_286029;

	private static string cacheAutoLOC_6002227;

	private static string cacheAutoLOC_7001411;

	private static string cacheAutoLOC_6002219;

	private static string cacheAutoLOC_6002228;

	private static string cacheAutoLOC_900380;

	private static string cacheAutoLOC_6002229;

	private static string cacheAutoLOC_7001407;

	private static string cacheAutoLOC_184746;

	private static string cacheAutoLOC_184750;

	private static string cacheAutoLOC_184754;

	private static string cacheAutoLOC_184758;

	private static string cacheAutoLOC_184762;

	private static string cacheAutoLOC_184772;

	private static string cacheAutoLOC_184774;

	private static string cacheAutoLOC_184776;

	private static string cacheAutoLOC_184778;

	private static string cacheAutoLOC_184780;

	private static string cacheAutoLOC_184782;

	private static string cacheAutoLOC_184791;

	private static string cacheAutoLOC_184793;

	private static string cacheAutoLOC_184795;

	private static string cacheAutoLOC_184797;

	private static string cacheAutoLOC_184799;

	private static string cacheAutoLOC_6011124;

	private static string cacheAutoLOC_6011125;

	private static string cacheAutoLOC_6011126;

	private static string cacheAutoLOC_6011129;

	private static string cacheAutoLOC_6002410;

	private static string cacheAutoLOC_6011130;

	private static string cacheAutoLOC_6011131;

	private static string cacheAutoLOC_6011132;

	private static string cacheAutoLOC_6011133;

	public DiscoveryLevels Level
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

	public double lastObservedTime
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

	public double fadeUT
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

	public double unobservedLifetime
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

	public double referenceLifetime
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

	public UntrackedObjectClass objectSize
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DiscoveryInfo(IDiscoverable host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DiscoveryInfo(IDiscoverable host, double untrackedLifetime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DiscoveryInfo(IDiscoverable host, DiscoveryLevels level, double untrackedLifetime)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLevel(DiscoveryLevels level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLastObservedTime(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUnobservedLifetime(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetReferenceLifetime(double time)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUntrackedObjectSize(UntrackedObjectClass size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CompileInfoGetters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSignalStrength(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double GetSignalLife(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HaveKnowledgeAbout(DiscoveryLevels lvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSignalStrengthCaption(double signal)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSizeClassDescription(UntrackedObjectClass sizeClass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSizeClassSizes(UntrackedObjectClass sizeClass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UntrackedObjectClass GetClassFromSize(float size, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetClassRadius(UntrackedObjectClass sizeClass, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static UntrackedObjectClass GetObjectClass(string classString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetMinRadius(UntrackedObjectClass sizeClass, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetMaxRadius(UntrackedObjectClass sizeClass, int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
