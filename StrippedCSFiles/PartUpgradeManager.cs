using System.Runtime.CompilerServices;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.EDITOR,
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT
})]
public class PartUpgradeManager : ScenarioModule
{
	protected static PartUpgradeManager instance;

	private static PartUpgradeHandler handler;

	public static PartUpgradeManager Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static PartUpgradeHandler Handler
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
	public PartUpgradeManager()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}
}
