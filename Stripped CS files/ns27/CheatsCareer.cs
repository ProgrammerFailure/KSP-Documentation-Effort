using System.Collections.Generic;
using ModuleWheels;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns27;

public class CheatsCareer : MonoBehaviour
{
	public double fundsBig = 100000.0;

	public double fundsSmall = 1000.0;

	public Button fundsNegBig;

	public Button fundsNegSmall;

	public TextMeshProUGUI fundsText;

	public Button fundsPosSmall;

	public Button fundsPosBig;

	public float scienceBig = 100f;

	public float scienceSmall = 10f;

	public Button scienceNegBig;

	public Button scienceNegSmall;

	public TextMeshProUGUI scienceText;

	public Button sciencePosSmall;

	public Button sciencePosBig;

	public float repBig = 100f;

	public float repSmall = 10f;

	public Button repNegBig;

	public Button repNegSmall;

	public TextMeshProUGUI repText;

	public Button repPosSmall;

	public Button repPosBig;

	public Button maxTech;

	public Button maxFacility;

	public Button maxXP;

	public Button maxProgress;

	public Button fixRepairableParts;

	public void Awake()
	{
		GameEvents.OnFundsChanged.Add(OnFundsChanged);
		GameEvents.OnScienceChanged.Add(OnScienceChanged);
		GameEvents.OnReputationChanged.Add(OnReputationChanged);
	}

	public void Start()
	{
		fundsNegBig.onClick.AddListener(OnFundsNegBigClick);
		fundsNegSmall.onClick.AddListener(OnFundsNegClick);
		fundsPosSmall.onClick.AddListener(OnFundsPosClick);
		fundsPosBig.onClick.AddListener(OnFundsPosBigClick);
		fundsNegBig.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0.0 - fundsBig, "F2");
		fundsNegSmall.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0.0 - fundsSmall, "F2");
		fundsPosBig.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(fundsBig, "F2");
		fundsPosSmall.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(fundsSmall, "F2");
		scienceNegBig.onClick.AddListener(OnScienceNegBigClick);
		scienceNegSmall.onClick.AddListener(OnScienceNegClick);
		sciencePosSmall.onClick.AddListener(OnSciencePosClick);
		sciencePosBig.onClick.AddListener(OnSciencePosBigClick);
		scienceNegBig.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0f - scienceBig, "F2");
		scienceNegSmall.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0f - scienceSmall, "F2");
		sciencePosBig.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(scienceBig, "F2");
		sciencePosSmall.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(scienceSmall, "F2");
		repNegBig.onClick.AddListener(OnRepNegBigClick);
		repNegSmall.onClick.AddListener(OnRepNegClick);
		repPosSmall.onClick.AddListener(OnRepPosClick);
		repPosBig.onClick.AddListener(OnRepPosBigClick);
		repNegBig.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0f - repBig, "F2");
		repNegSmall.GetComponentInChildren<TextMeshProUGUI>().text = KSPUtil.LocalizeNumber(0f - repSmall, "F2");
		repPosBig.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(repBig, "F2");
		repPosSmall.GetComponentInChildren<TextMeshProUGUI>().text = "+" + KSPUtil.LocalizeNumber(repSmall, "F2");
		maxTech.onClick.AddListener(OnMaxTechClick);
		maxFacility.onClick.AddListener(OnMaxFacilityClick);
		maxXP.onClick.AddListener(OnMaxXPClick);
		fixRepairableParts.onClick.AddListener(FixRepairableParts);
	}

	public void OnDestroy()
	{
		GameEvents.OnFundsChanged.Remove(OnFundsChanged);
		GameEvents.OnScienceChanged.Remove(OnScienceChanged);
		GameEvents.OnReputationChanged.Remove(OnReputationChanged);
		fixRepairableParts.onClick.RemoveListener(FixRepairableParts);
	}

	public void Update()
	{
		if (HighLogic.CurrentGame != null)
		{
			SetFundingActive(Funding.Instance != null);
			SetScienceActive(ResearchAndDevelopment.Instance != null);
			SetReputationActive(Reputation.Instance != null);
			SetMaxTechActive(ResearchAndDevelopment.Instance != null);
			SetMaxFacilityActive(HighLogic.CurrentGame.Mode == Game.Modes.CAREER && ScenarioUpgradeableFacilities.Instance != null);
			SetMaxXPActive(HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(HighLogic.CurrentGame.Mode) && HighLogic.CurrentGame.CrewRoster != null && FlightGlobals.Bodies != null);
			SetMaxProgressActive(ProgressTracking.Instance != null);
		}
	}

	public void SetFundingActive(bool active)
	{
		if (fundsNegBig.interactable != active)
		{
			fundsNegBig.interactable = active;
		}
		if (fundsNegSmall.interactable != active)
		{
			fundsNegSmall.interactable = active;
		}
		if (fundsPosSmall.interactable != active)
		{
			fundsPosSmall.interactable = active;
		}
		if (fundsPosBig.interactable != active)
		{
			fundsPosBig.interactable = active;
		}
		if (!active && fundsText.text != "N/A")
		{
			fundsText.text = "N/A";
		}
		else if (active && fundsText.text != Funding.Instance.Funds.ToString("F2"))
		{
			fundsText.text = KSPUtil.LocalizeNumber(Funding.Instance.Funds, "F2");
		}
	}

	public void OnFundsChanged(double funds, TransactionReasons reason)
	{
		fundsText.text = KSPUtil.LocalizeNumber(funds, "F2");
	}

	public void OnFundsNegBigClick()
	{
		if (!(Funding.Instance == null))
		{
			Funding.Instance.AddFunds(0.0 - fundsBig, TransactionReasons.Cheating);
		}
	}

	public void OnFundsNegClick()
	{
		if (!(Funding.Instance == null))
		{
			Funding.Instance.AddFunds(0.0 - fundsSmall, TransactionReasons.Cheating);
		}
	}

	public void OnFundsPosClick()
	{
		if (!(Funding.Instance == null))
		{
			Funding.Instance.AddFunds(fundsSmall, TransactionReasons.Cheating);
		}
	}

	public void OnFundsPosBigClick()
	{
		if (!(Funding.Instance == null))
		{
			Funding.Instance.AddFunds(fundsBig, TransactionReasons.Cheating);
		}
	}

	public void SetScienceActive(bool active)
	{
		if (scienceNegBig.interactable != active)
		{
			scienceNegBig.interactable = active;
		}
		if (scienceNegSmall.interactable != active)
		{
			scienceNegSmall.interactable = active;
		}
		if (sciencePosSmall.interactable != active)
		{
			sciencePosSmall.interactable = active;
		}
		if (sciencePosBig.interactable != active)
		{
			sciencePosBig.interactable = active;
		}
		if (!active && scienceText.text != "N/A")
		{
			scienceText.text = "N/A";
		}
		else if (active && scienceText.text != ResearchAndDevelopment.Instance.Science.ToString("F2"))
		{
			scienceText.text = KSPUtil.LocalizeNumber(ResearchAndDevelopment.Instance.Science, "F2");
		}
	}

	public void OnScienceChanged(float value, TransactionReasons reason)
	{
		scienceText.text = KSPUtil.LocalizeNumber(value, "F2");
	}

	public void OnScienceNegBigClick()
	{
		if (!(ResearchAndDevelopment.Instance == null))
		{
			ResearchAndDevelopment.Instance.CheatAddScience(0f - scienceBig);
		}
	}

	public void OnScienceNegClick()
	{
		if (!(ResearchAndDevelopment.Instance == null))
		{
			ResearchAndDevelopment.Instance.CheatAddScience(0f - scienceSmall);
		}
	}

	public void OnSciencePosClick()
	{
		if (!(ResearchAndDevelopment.Instance == null))
		{
			ResearchAndDevelopment.Instance.CheatAddScience(scienceSmall);
		}
	}

	public void OnSciencePosBigClick()
	{
		if (!(ResearchAndDevelopment.Instance == null))
		{
			ResearchAndDevelopment.Instance.CheatAddScience(scienceBig);
		}
	}

	public void SetReputationActive(bool active)
	{
		if (repNegBig.interactable != active)
		{
			repNegBig.interactable = active;
		}
		if (repNegSmall.interactable != active)
		{
			repNegSmall.interactable = active;
		}
		if (repPosSmall.interactable != active)
		{
			repPosSmall.interactable = active;
		}
		if (repPosBig.interactable != active)
		{
			repPosBig.interactable = active;
		}
		if (!active && repText.text != "N/A")
		{
			repText.text = "N/A";
		}
		else if (active && repText.text != Reputation.Instance.reputation.ToString("F2"))
		{
			repText.text = KSPUtil.LocalizeNumber(Reputation.Instance.reputation, "F2");
		}
	}

	public void OnReputationChanged(float value, TransactionReasons reason)
	{
		repText.text = KSPUtil.LocalizeNumber(value, "F2");
	}

	public void OnRepNegBigClick()
	{
		if (!(Reputation.Instance == null))
		{
			Reputation.Instance.AddReputation(0f - repBig, TransactionReasons.Cheating);
		}
	}

	public void OnRepNegClick()
	{
		if (!(Reputation.Instance == null))
		{
			Reputation.Instance.AddReputation(0f - repSmall, TransactionReasons.Cheating);
		}
	}

	public void OnRepPosClick()
	{
		if (!(Reputation.Instance == null))
		{
			Reputation.Instance.AddReputation(repSmall, TransactionReasons.Cheating);
		}
	}

	public void OnRepPosBigClick()
	{
		if (!(Reputation.Instance == null))
		{
			Reputation.Instance.AddReputation(repBig, TransactionReasons.Cheating);
		}
	}

	public void SetMaxTechActive(bool active)
	{
		if (maxTech.interactable != active)
		{
			maxTech.interactable = active;
		}
	}

	public void OnMaxTechClick()
	{
		if (!(ResearchAndDevelopment.Instance == null))
		{
			ResearchAndDevelopment.Instance.CheatTechnology();
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001911"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void SetMaxFacilityActive(bool active)
	{
		if (maxFacility.interactable != active)
		{
			maxFacility.interactable = active;
		}
	}

	public void OnMaxFacilityClick()
	{
		if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER && !(ScenarioUpgradeableFacilities.Instance == null))
		{
			ScenarioUpgradeableFacilities.Instance.CheatFacilities();
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001912"), 5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void SetMaxXPActive(bool active)
	{
		if (maxXP.interactable != active)
		{
			maxXP.interactable = active;
		}
	}

	public void OnMaxXPClick()
	{
		KerbalRoster.CheatExperience();
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001913"), 5f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void SetMaxProgressActive(bool active)
	{
		if (maxProgress.interactable != active)
		{
			maxProgress.interactable = active;
		}
	}

	public void FixRepairableParts()
	{
		for (int i = 0; i < FlightGlobals.VesselsLoaded.Count; i++)
		{
			bool num = IsVesselWithinRange(FlightGlobals.VesselsLoaded[i]);
			List<ModuleDeployablePart> list = new List<ModuleDeployablePart>();
			List<ModuleWheelDamage> list2 = new List<ModuleWheelDamage>();
			if (num)
			{
				list = FlightGlobals.VesselsLoaded[i].FindPartModulesImplementing<ModuleDeployablePart>();
				if (list.Count > 0)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].deployState == ModuleDeployablePart.DeployState.BROKEN)
						{
							list[j].CheatRepair();
						}
					}
				}
				list2 = FlightGlobals.VesselsLoaded[i].FindPartModulesImplementing<ModuleWheelDamage>();
				if (list2.Count > 0)
				{
					for (int k = 0; k < list2.Count; k++)
					{
						list2[k].SetDamaged(damaged: false);
					}
				}
			}
			list.Clear();
			list2.Clear();
			ScreenMessages.PostScreenMessage("FixRepairableParts...");
		}
	}

	public static bool IsVesselWithinRange(Vessel vessel)
	{
		return GameSettings.EVA_CONSTRUCTION_RANGE > Vector3.Distance(FlightGlobals.ActiveVessel.GetWorldPos3D(), vessel.GetWorldPos3D());
	}
}
