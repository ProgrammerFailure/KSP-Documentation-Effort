using CommNet;
using Contracts;
using ns9;
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

	public float reputationKerbalDeath = 10f;

	public float reputationKerbalRecovery = 25f;

	public float partRecoveryValueFactor = 0.9f;

	public float resourceRecoveryValueFactor = 0.95f;

	public float contractDestinationWeight = 1f;

	public float contractPrestigeTrivial = 1f;

	public float contractPrestigeSignificant = 1.25f;

	public float contractPrestigeExceptional = 1.5f;

	public float contractFundsAdvanceFactor = 1f;

	public float contractFundsCompletionFactor = 1f;

	public float contractFundsFailureFactor = 1f;

	public float contractReputationCompletionFactor = 1f;

	public float contractReputationFailureFactor = 1f;

	public float contractScienceCompletionFactor = 1f;

	public float mentalityFundsTrivial = 1.1f;

	public float mentalityFundsSignificant = 1.2f;

	public float mentalityFundsExceptional = 1.3f;

	public float mentalityReputationTrivial = 1.1f;

	public float mentalityReputationSignificant = 1.2f;

	public float mentalityReputationExceptional = 1.3f;

	public float mentalityScienceTrivial = 1.1f;

	public float mentalityScienceSignificant = 1.2f;

	public float mentalityScienceExceptional = 1.3f;

	public float mentalityExpiryTrivial = 1.1f;

	public float mentalityExpirySignificant = 1.2f;

	public float mentalityExpiryExceptional = 1.3f;

	public float mentalityDeadlineTrivial = 1.1f;

	public float mentalityDeadlineSignificant = 1.2f;

	public float mentalityDeadlineExceptional = 1.3f;

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("[GameVariables]: Instance already exists!", Instance.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public virtual float GetRecoveredPartValue(float pValue)
	{
		return pValue * partRecoveryValueFactor;
	}

	public virtual float GetRecoveredResourceValue(float rscValue)
	{
		return rscValue * resourceRecoveryValueFactor;
	}

	public virtual float GetContractPrestigeFactor(Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => contractPrestigeSignificant, 
			Contract.ContractPrestige.Trivial => contractPrestigeTrivial, 
			_ => contractPrestigeExceptional, 
		};
	}

	public virtual float GetContractFundsAdvanceFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractFundsAdvanceFactor * HighLogic.CurrentGame.Parameters.Career.FundsGainMultiplier;
	}

	public virtual float GetContractFundsCompletionFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractFundsCompletionFactor * HighLogic.CurrentGame.Parameters.Career.FundsGainMultiplier;
	}

	public virtual float GetContractFundsFailureFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractFundsFailureFactor * HighLogic.CurrentGame.Parameters.Career.FundsLossMultiplier;
	}

	public virtual float GetContractReputationCompletionFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractReputationCompletionFactor * HighLogic.CurrentGame.Parameters.Career.RepGainMultiplier;
	}

	public virtual float GetContractReputationFailureFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractReputationFailureFactor * HighLogic.CurrentGame.Parameters.Career.RepLossMultiplier;
	}

	public virtual float GetContractScienceCompletionFactor(Contract.ContractPrestige prestige)
	{
		return GetContractPrestigeFactor(prestige) * contractScienceCompletionFactor * HighLogic.CurrentGame.Parameters.Career.ScienceGainMultiplier;
	}

	public virtual float GetContractDestinationWeight(CelestialBody body)
	{
		return body.scienceValues.RecoveryValue * contractDestinationWeight;
	}

	public virtual float ScoreSituation(Vessel.Situations sit, CelestialBody where)
	{
		float num = 1f;
		if (where == Planetarium.fetch.Home)
		{
			switch (sit)
			{
			case Vessel.Situations.SUB_ORBITAL:
				return 0.8f;
			case Vessel.Situations.FLYING:
				return 0.5f;
			case Vessel.Situations.SPLASHED:
				return 0.3f;
			case Vessel.Situations.LANDED:
			case Vessel.Situations.PRELAUNCH:
				return 0.1f;
			default:
				return 0.1f;
			case Vessel.Situations.ESCAPING:
				return 1.1f;
			case Vessel.Situations.ORBITING:
				return 1f;
			}
		}
		return sit switch
		{
			Vessel.Situations.SUB_ORBITAL => 1f, 
			Vessel.Situations.FLYING => 1.2f, 
			Vessel.Situations.LANDED => 1.5f, 
			Vessel.Situations.SPLASHED => 1.5f, 
			Vessel.Situations.ESCAPING => 0.6f, 
			Vessel.Situations.ORBITING => 0.8f, 
			_ => 0.1f, 
		};
	}

	public virtual float ScoreFlightEnvelope(float altitude, float altEnvelope, float speed, float speedEnvelope)
	{
		float num = 0f;
		float num2 = 0f;
		if (altEnvelope != 0f)
		{
			float num3 = Mathf.Floor(altitude / 10000f * 1000f) / 1000f;
			float num4 = Mathf.InverseLerp(0f, altitude, altEnvelope);
			num = Mathf.Max(num3 - num4, 0f);
		}
		if (speedEnvelope != 0f)
		{
			float num5 = Mathf.Floor(speed / 1000f * 100f) / 100f;
			float num6 = Mathf.InverseLerp(0f, speed, speedEnvelope);
			num2 = Mathf.Max(num5 - num6, 0f);
		}
		return num + num2;
	}

	public virtual float GetMentalityFundsFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => Mathf.Lerp(1f, mentalityFundsSignificant, mentalityFactor), 
			Contract.ContractPrestige.Trivial => Mathf.Lerp(1f, mentalityFundsTrivial, mentalityFactor), 
			_ => Mathf.Lerp(1f, mentalityFundsExceptional, mentalityFactor), 
		};
	}

	public virtual float GetMentalityReputationFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => Mathf.Lerp(1f, mentalityReputationSignificant, mentalityFactor), 
			Contract.ContractPrestige.Trivial => Mathf.Lerp(1f, mentalityReputationTrivial, mentalityFactor), 
			_ => Mathf.Lerp(1f, mentalityReputationExceptional, mentalityFactor), 
		};
	}

	public virtual float GetMentalityScienceFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => Mathf.Lerp(1f, mentalityScienceSignificant, mentalityFactor), 
			Contract.ContractPrestige.Trivial => Mathf.Lerp(1f, mentalityScienceTrivial, mentalityFactor), 
			_ => Mathf.Lerp(1f, mentalityScienceExceptional, mentalityFactor), 
		};
	}

	public virtual float GetMentalityExpiryFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => Mathf.Lerp(1f, mentalityExpirySignificant, mentalityFactor), 
			Contract.ContractPrestige.Trivial => Mathf.Lerp(1f, mentalityExpiryTrivial, mentalityFactor), 
			_ => Mathf.Lerp(1f, mentalityExpiryExceptional, mentalityFactor), 
		};
	}

	public virtual float GetMentalityDeadlineFactor(float mentalityFactor, Contract.ContractPrestige prestige)
	{
		return prestige switch
		{
			Contract.ContractPrestige.Significant => Mathf.Lerp(1f, mentalityDeadlineSignificant, mentalityFactor), 
			Contract.ContractPrestige.Trivial => Mathf.Lerp(1f, mentalityDeadlineTrivial, mentalityFactor), 
			_ => Mathf.Lerp(1f, mentalityDeadlineExceptional, mentalityFactor), 
		};
	}

	public virtual int GetPartCountLimit(float editorNormLevel, bool isVAB)
	{
		if (editorNormLevel < 0.2f)
		{
			return 30;
		}
		if (editorNormLevel < 0.4f)
		{
			return 110;
		}
		if (editorNormLevel < 0.6f)
		{
			return 255;
		}
		if (editorNormLevel < 0.8f)
		{
			return 450;
		}
		return int.MaxValue;
	}

	public virtual bool UnlockedFuelTransfer(float editorNormLevel)
	{
		return editorNormLevel > 0.2f;
	}

	public virtual bool UnlockedActionGroupsStock(float editorNormLevel, bool isVAB)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().ActionGroupsAlways)
		{
			return true;
		}
		return editorNormLevel > 0.4f;
	}

	public virtual bool UnlockedActionGroupsCustom(float editorNormLevel, bool isVAB)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().ActionGroupsAlways)
		{
			return true;
		}
		return editorNormLevel > 0.6f;
	}

	public virtual float GetCraftMassLimit(float editorNormLevel, bool isPad)
	{
		if (editorNormLevel < 0.2f)
		{
			return 18f;
		}
		if (editorNormLevel < 0.4f)
		{
			return 32f;
		}
		if (editorNormLevel < 0.6f)
		{
			return 140f;
		}
		if (editorNormLevel < 0.8f)
		{
			return 300f;
		}
		return float.MaxValue;
	}

	public virtual Vector3 GetCraftSizeLimit(float editorNormLevel, bool isPad)
	{
		if (editorNormLevel < 0.2f)
		{
			return new Vector3(15f, 20f, 15f);
		}
		if (editorNormLevel < 0.4f)
		{
			return new Vector3(20f, 28f, 20f);
		}
		if (editorNormLevel < 0.6f)
		{
			return new Vector3(28f, 36f, 28f);
		}
		if (editorNormLevel < 0.8f)
		{
			return new Vector3(36f, 50f, 36f);
		}
		return new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
	}

	public virtual int GetActiveStrategyLimit(float adminNormLevel)
	{
		if (adminNormLevel < 0.25f)
		{
			return 1;
		}
		if (adminNormLevel < 0.5f)
		{
			return 2;
		}
		if (adminNormLevel < 0.75f)
		{
			return 3;
		}
		return 5;
	}

	public virtual float GetStrategyCommitRange(float adminNormLevel)
	{
		if (adminNormLevel < 0.3f)
		{
			return 0.25f;
		}
		if (adminNormLevel < 0.6f)
		{
			return 0.6f;
		}
		return 1f;
	}

	public virtual float GetStrategyLevelLimit(float adminNormLevel)
	{
		return adminNormLevel;
	}

	public virtual int GetActiveContractsLimit(float mCtrlNormLevel)
	{
		if (mCtrlNormLevel < 0.25f)
		{
			return 2;
		}
		if (mCtrlNormLevel < 0.5f)
		{
			return 4;
		}
		if (mCtrlNormLevel < 0.75f)
		{
			return 7;
		}
		return int.MaxValue;
	}

	public virtual float GetContractLevelLimit(float mCtrlNormLevel)
	{
		return mCtrlNormLevel;
	}

	public virtual bool UnlockedFlightPlanning(float mCtrlNormLevel)
	{
		return mCtrlNormLevel > 0.4f;
	}

	public virtual int GetActiveCrewLimit(float astroComplexNormLevel)
	{
		if (astroComplexNormLevel < 0.25f)
		{
			return 5;
		}
		if (astroComplexNormLevel < 0.5f)
		{
			return 8;
		}
		if (astroComplexNormLevel < 0.75f)
		{
			return 12;
		}
		return int.MaxValue;
	}

	public virtual float GetCrewLevelLimit(float astroComplexNormLevel)
	{
		if (astroComplexNormLevel < 0.33f)
		{
			return 1f;
		}
		if (astroComplexNormLevel < 0.66f)
		{
			return 3f;
		}
		return float.MaxValue;
	}

	public static float GetRecruitHireCost(int currentActive, float baseCost, float flatRate, float rateModifier)
	{
		return Mathf.Round(baseCost + 80000f * (Mathf.Log(0.25f * (float)currentActive + 1.12f) / 500f) * 500f) * HighLogic.CurrentGame.Parameters.Career.FundsLossMultiplier;
	}

	public virtual float GetRecruitHireCost(int currentActive)
	{
		return GetRecruitHireCost(currentActive, 2000f, 1.25f, 0.015f);
	}

	public virtual bool UnlockedEVA(float astroComplexNormLevel)
	{
		return astroComplexNormLevel > 0.2f;
	}

	public virtual bool UnlockedEVAFlags(float astroComplexNormLevel)
	{
		return astroComplexNormLevel >= 0.4f;
	}

	public virtual bool UnlockedEVAClamber(float astroComplexNormLevel)
	{
		return astroComplexNormLevel > 0.6f;
	}

	public virtual bool EVAIsPossible(bool evaUnlocked, Vessel v)
	{
		if (!evaUnlocked && (!(v.mainBody == Planetarium.fetch.Home) || !v.LandedOrSplashed))
		{
			return false;
		}
		return TimeWarp.CurrentRate <= 1f;
	}

	public virtual string GetEVALockedReason(Vessel v, ProtoCrewMember crew)
	{
		if (crew.type == ProtoCrewMember.KerbalType.Tourist)
		{
			return Localizer.Format("#autoLOC_294604");
		}
		if (crew.inactive)
		{
			return Localizer.Format("#autoLOC_294609");
		}
		if (crew.KerbalRef != null)
		{
			if (crew.KerbalRef.state == Kerbal.States.DEAD)
			{
				return Localizer.Format("#autoLOC_294616");
			}
			if (crew.KerbalRef.InPart.NoAutoEVA)
			{
				return Localizer.Format("#autoLOC_294622");
			}
		}
		if (!HighLogic.CurrentGame.Parameters.Flight.CanEVA)
		{
			return Localizer.Format("#autoLOC_294628");
		}
		if (!UnlockedEVA(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.AstronautComplex)))
		{
			return Localizer.Format("#autoLOC_294633");
		}
		if (crew.rosterStatus != ProtoCrewMember.RosterStatus.Assigned)
		{
			return Localizer.Format("#autoLOC_294639");
		}
		if (v.packed)
		{
			return Localizer.Format("#autoLOC_294644");
		}
		return Localizer.Format("#autoLOC_294648");
	}

	public virtual DoubleCurve GetDSNRangeCurve()
	{
		DoubleCurve doubleCurve = new DoubleCurve();
		doubleCurve.Add(0.0, 0.0, 0.0, 0.0);
		doubleCurve.Add(1.0, 1.0, 0.0, 0.0);
		return doubleCurve;
	}

	public virtual DoubleCurve GetDSNPowerCurve()
	{
		DoubleCurve doubleCurve = new DoubleCurve();
		doubleCurve.Add(0.0, 1.0, 0.0, 0.0);
		doubleCurve.Add(1.0, 0.10000000149011612, 0.0, 0.0);
		return doubleCurve;
	}

	public virtual DoubleCurve GetDSNScienceCurve()
	{
		DoubleCurve doubleCurve = new DoubleCurve();
		doubleCurve.Add(0.0, 0.0, 0.0, 0.0);
		doubleCurve.Add(0.5, 0.10000000149011612, 0.0, 0.0);
		doubleCurve.Add(1.0, 0.75, 0.0, 0.0);
		return doubleCurve;
	}

	public virtual double GetDSNRange(float level)
	{
		double num = 0.0;
		num = (((double)level > 0.500000001) ? 250000000000.0 : ((!((double)level > 1E-09)) ? 2000000000.0 : 50000000000.0));
		return num * ((HighLogic.CurrentGame != null) ? ((double)HighLogic.CurrentGame.Parameters.CustomParams<CommNetParams>().DSNModifier) : 1.0);
	}

	public virtual OrbitDisplayMode GetOrbitDisplayMode(float tsNormLevel)
	{
		if (tsNormLevel < 0.2f)
		{
			return OrbitDisplayMode.AllOrbits;
		}
		return OrbitDisplayMode.PatchedConics;
	}

	public virtual int GetTrackedObjectLimit(float tsNormLevel)
	{
		if (tsNormLevel < 0.25f)
		{
			return 0;
		}
		if (tsNormLevel < 0.5f)
		{
			return 3;
		}
		if (tsNormLevel < 0.75f)
		{
			return 8;
		}
		return int.MaxValue;
	}

	public virtual int GetPatchesAheadLimit(float tsNormLevel)
	{
		if (tsNormLevel < 0.25f)
		{
			return 0;
		}
		if (tsNormLevel < 0.5f)
		{
			return 1;
		}
		if (tsNormLevel < 0.75f)
		{
			return 2;
		}
		return GameSettings.CONIC_PATCH_LIMIT;
	}

	public virtual bool UnlockedSpaceObjectDiscovery(float tsNormLevel)
	{
		return tsNormLevel > 0.6f;
	}

	public virtual UntrackedObjectClass MinTrackedObjectSize(float tsNormLevel)
	{
		if (tsNormLevel < 0.2f)
		{
			return UntrackedObjectClass.const_4;
		}
		if (tsNormLevel < 0.4f)
		{
			return UntrackedObjectClass.const_3;
		}
		if (tsNormLevel < 0.6f)
		{
			return UntrackedObjectClass.const_2;
		}
		if (tsNormLevel < 0.8f)
		{
			return UntrackedObjectClass.const_1;
		}
		return UntrackedObjectClass.const_0;
	}

	public virtual bool ManeuverToolAvailable(float tsNormLevel)
	{
		return tsNormLevel > 0.75f;
	}

	public virtual float GetScienceCostLimit(float RnDnormLevel)
	{
		if (RnDnormLevel < 0.25f)
		{
			return 100f;
		}
		if (RnDnormLevel < 0.5f)
		{
			return 300f;
		}
		if (RnDnormLevel < 0.75f)
		{
			return 500f;
		}
		return float.MaxValue;
	}

	public virtual float GetDataToScienceRatio(float RnDnormLevel)
	{
		if (RnDnormLevel < 0.25f)
		{
			return 0.4f;
		}
		if (RnDnormLevel < 0.5f)
		{
			return 0.6f;
		}
		if (RnDnormLevel < 0.75f)
		{
			return 0.8f;
		}
		return 1f;
	}

	public virtual float GetExperimentLevel(float RnDnormLevel)
	{
		return RnDnormLevel;
	}
}
