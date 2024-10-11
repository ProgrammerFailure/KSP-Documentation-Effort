using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
	public enum OrbitDisplayMode
	{
		None,
		CelestialBodyOrbits,
		AllOrbits,
		PatchedConics
	}

	public static GameVariables Instance;

	public AnimationCurve reputationAddition;

	public AnimationCurve reputationSubtraction;

	public float reputationKerbalDeath;

	public float reputationKerbalRecovery;

	public float partRecoveryValueFactor;

	public float resourceRecoveryValueFactor;

	public float contractDestinationWeight;

	public float contractPrestigeTrivial;

	public float contractPrestigeSignificant;

	public float contractPrestigeExceptional;

	public float contractFundsAdvanceFactor;

	public float contractFundsCompletionFactor;

	public float contractFundsFailureFactor;

	public float contractReputationCompletionFactor;

	public float contractReputationFailureFactor;

	public float contractScienceCompletionFactor;

	public float mentalityFundsTrivial;

	public float mentalityFundsSignificant;

	public float mentalityFundsExceptional;

	public float mentalityReputationTrivial;

	public float mentalityReputationSignificant;

	public float mentalityReputationExceptional;

	public float mentalityScienceTrivial;

	public float mentalityScienceSignificant;

	public float mentalityScienceExceptional;

	public float mentalityExpiryTrivial;

	public float mentalityExpirySignificant;

	public float mentalityExpiryExceptional;

	public float mentalityDeadlineTrivial;

	public float mentalityDeadlineSignificant;

	public float mentalityDeadlineExceptional;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameVariables()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetRecoveredPartValue(float pValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetRecoveredResourceValue(float rscValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractPrestigeFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractFundsAdvanceFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractFundsCompletionFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractFundsFailureFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractReputationCompletionFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractReputationFailureFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractScienceCompletionFactor(Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractDestinationWeight(CelestialBody body)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float ScoreSituation(Vessel.Situations sit, CelestialBody where)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float ScoreFlightEnvelope(float altitude, float altEnvelope, float speed, float speedEnvelope)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetMentalityFundsFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetMentalityReputationFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetMentalityScienceFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetMentalityExpiryFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetMentalityDeadlineFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetPartCountLimit(float editorNormLevel, bool isVAB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedFuelTransfer(float editorNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedActionGroupsStock(float editorNormLevel, bool isVAB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedActionGroupsCustom(float editorNormLevel, bool isVAB)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetCraftMassLimit(float editorNormLevel, bool isPad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Vector3 GetCraftSizeLimit(float editorNormLevel, bool isPad)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetActiveStrategyLimit(float adminNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetStrategyCommitRange(float adminNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetStrategyLevelLimit(float adminNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetActiveContractsLimit(float mCtrlNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetContractLevelLimit(float mCtrlNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedFlightPlanning(float mCtrlNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetActiveCrewLimit(float astroComplexNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetCrewLevelLimit(float astroComplexNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetRecruitHireCost(int currentActive, float baseCost, float flatRate, float rateModifier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetRecruitHireCost(int currentActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedEVA(float astroComplexNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedEVAFlags(float astroComplexNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedEVAClamber(float astroComplexNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool EVAIsPossible(bool evaUnlocked, Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetEVALockedReason(Vessel v, ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual DoubleCurve GetDSNRangeCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual DoubleCurve GetDSNPowerCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual DoubleCurve GetDSNScienceCurve()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual double GetDSNRange(float level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual OrbitDisplayMode GetOrbitDisplayMode(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetTrackedObjectLimit(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual int GetPatchesAheadLimit(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UnlockedSpaceObjectDiscovery(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual UntrackedObjectClass MinTrackedObjectSize(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool ManeuverToolAvailable(float tsNormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetScienceCostLimit(float RnDnormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetDataToScienceRatio(float RnDnormLevel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetExperimentLevel(float RnDnormLevel)
	{
		throw null;
	}
}
