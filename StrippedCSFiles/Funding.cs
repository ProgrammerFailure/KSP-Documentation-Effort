using System.Runtime.CompilerServices;
using KSP.UI.Screens;

[KSPScenario((ScenarioCreationOptions)1120, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class Funding : ScenarioModule
{
	public static Funding Instance;

	private double funds;

	public double Funds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Funding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFunds(double value, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetFunds(double value, TransactionReasons reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCurrenciesModified(CurrencyModifierQuery query)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRollout(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartPurchased(AvailablePart aP)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onUpgradePurchased(PartUpgradeHandler.Upgrade upgrade)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onCrewHired(ProtoCrewMember crew, int crewCount)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool CanAfford(float cost)
	{
		throw null;
	}
}
