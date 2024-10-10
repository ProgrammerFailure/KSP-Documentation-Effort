using ns11;
using UnityEngine;

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

	public static float RepRange = 1000f;

	public float rep;

	public static float CurrentRep
	{
		get
		{
			if (Instance != null)
			{
				return Instance.rep;
			}
			return 0f;
		}
	}

	public static float UnitRep
	{
		get
		{
			if (Instance != null)
			{
				return Instance.rep / RepRange;
			}
			return 0f;
		}
	}

	public float reputation => rep;

	public float AddReputation(float r, TransactionReasons reason)
	{
		float num = addReputation_granular(r);
		CurrencyModifierQuery data = new CurrencyModifierQuery(reason, 0f, 0f, r);
		GameEvents.Modifiers.OnCurrencyModifierQuery.Fire(data);
		GameEvents.Modifiers.OnCurrencyModified.Fire(data);
		if (reason == TransactionReasons.None)
		{
			Debug.Log("Added " + num + " (" + r + ") reputation. Total Rep: " + reputation);
		}
		else
		{
			Debug.Log("Added " + num + " (" + r + ") reputation: '" + reason.ToString() + "'.");
		}
		if (num != 0f)
		{
			GameEvents.OnReputationChanged.Fire(rep, reason);
		}
		return num;
	}

	public void SetReputation(float value, TransactionReasons reason)
	{
		rep = Mathf.Clamp(value, 0f - RepRange, RepRange);
		GameEvents.OnReputationChanged.Fire(rep, reason);
	}

	public void addReputation_discrete(float reputation, TransactionReasons reason)
	{
		float num = ModifyReputationDelta(reputation);
		if (reason == TransactionReasons.None)
		{
			Debug.Log("Adding " + num + " (" + reputation + ") reputation.");
		}
		else
		{
			Debug.Log("Adding " + num + " (" + reputation + ") reputation: '" + reason.ToString() + "'.");
		}
		if (num != 0f)
		{
			rep += num;
			GameEvents.OnReputationChanged.Fire(rep, reason);
		}
	}

	public float addReputation_granular(float value)
	{
		int num = (int)Mathf.Abs(value);
		float delta = 1f * Mathf.Sign(value);
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i <= num; i++)
		{
			num3 = ((i == num) ? ModifyReputationDelta(value - num2) : ModifyReputationDelta(delta));
			rep += num3;
			num2 += num3;
		}
		return num2;
	}

	public float ModifyReputationDelta(float delta)
	{
		float time = rep / RepRange;
		if (delta < 0f)
		{
			return delta * GameVariables.Instance.reputationSubtraction.Evaluate(time);
		}
		return delta * GameVariables.Instance.reputationAddition.Evaluate(time);
	}

	public override void OnAwake()
	{
		if (Instance != null)
		{
			Debug.LogError("[Reputation Module]: Instance already exists!", Instance.gameObject);
		}
		if (HighLogic.CurrentGame != null)
		{
			rep = HighLogic.CurrentGame.Parameters.Career.StartingReputation;
		}
		Instance = this;
	}

	public void Start()
	{
		GameEvents.onCrewKilled.Add(OnCrewKilled);
		GameEvents.onVesselRecoveryProcessing.Add(onvesselRecoveryProcessing);
		GameEvents.Modifiers.OnCurrencyModified.Add(OnCurrenciesModified);
	}

	public void OnDestroy()
	{
		GameEvents.onCrewKilled.Remove(OnCrewKilled);
		GameEvents.onVesselRecoveryProcessing.Remove(onvesselRecoveryProcessing);
		GameEvents.Modifiers.OnCurrencyModified.Remove(OnCurrenciesModified);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnCurrenciesModified(CurrencyModifierQuery query)
	{
		float effectDelta = query.GetEffectDelta(Currency.Reputation);
		addReputation_granular(effectDelta);
		if (effectDelta != 0f)
		{
			GameEvents.OnReputationChanged.Fire(rep, query.reason);
		}
	}

	public void OnCrewKilled(EventReport evt)
	{
		if (evt.eventType == FlightEvents.CREW_KILLED)
		{
			AddReputation(GameVariables.Instance.reputationKerbalDeath * HighLogic.CurrentGame.Parameters.Career.RepLossMultiplier, TransactionReasons.VesselLoss);
		}
	}

	public void onvesselRecoveryProcessing(ProtoVessel pv, MissionRecoveryDialog mrDialog, float recoveryScore)
	{
		if (pv != null)
		{
			float num = 0f;
			for (int num2 = pv.GetVesselCrew().Count; num2 > 0; num2--)
			{
				num += AddReputation(GameVariables.Instance.reputationKerbalRecovery * HighLogic.CurrentGame.Parameters.Career.RepGainMultiplier, TransactionReasons.VesselRecovery);
			}
			if (mrDialog != null)
			{
				mrDialog.reputationEarned = num;
			}
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("rep"))
		{
			rep = float.Parse(node.GetValue("rep"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("rep", rep);
	}
}
