using System.Runtime.CompilerServices;
using KSP.UI.Screens;

[KSPScenario((ScenarioCreationOptions)96, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class Reputation : ScenarioModule
{
	public static Reputation Instance;

	public static float RepRange;

	private float rep;

	public static float CurrentRep
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static float UnitRep
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float reputation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Reputation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Reputation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float AddReputation(float r, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetReputation(float value, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void addReputation_discrete(float reputation, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float addReputation_granular(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ModifyReputationDelta(float delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCurrenciesModified(CurrencyModifierQuery query)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCrewKilled(EventReport evt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onvesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
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
