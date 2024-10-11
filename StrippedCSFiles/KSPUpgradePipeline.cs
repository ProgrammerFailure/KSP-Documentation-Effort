using System.Runtime.CompilerServices;
using SaveUpgradePipeline;

public static class KSPUpgradePipeline
{
	public enum UpgradeFailOption
	{
		Cancel,
		LoadAnyway
	}

	private static bool Enabled;

	private static global::SaveUpgradePipeline.SaveUpgradePipeline _pipeline;

	public static int FailDialogIndex;

	private static global::SaveUpgradePipeline.SaveUpgradePipeline Pipeline
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static KSPUpgradePipeline()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static bool Init()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Process(ConfigNode n, string saveName, LoadContext loadContext, Callback<ConfigNode> onSucceed, Callback<UpgradeFailOption, ConfigNode> onFail)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnFinish(UpgradeFailOption opt, ConfigNode n, Callback<UpgradeFailOption, ConfigNode> onFail)
	{
		throw null;
	}
}
