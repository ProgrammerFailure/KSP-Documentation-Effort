using UnityEngine;

public class ReputationWidget : CurrencyWidget
{
	public Gauge gauge;

	public void Awake()
	{
		GameEvents.OnReputationChanged.Add(onReputationChanged);
	}

	public void OnDestroy()
	{
		GameEvents.OnReputationChanged.Remove(onReputationChanged);
	}

	public void onReputationChanged(float rep, TransactionReasons reason)
	{
		gauge.setValue(Mathf.InverseLerp(0f - Reputation.RepRange, Reputation.RepRange, rep) * 2f - 1f);
	}

	public override void DelayedStart()
	{
		if (Reputation.Instance != null)
		{
			onReputationChanged(Reputation.Instance.reputation, TransactionReasons.None);
		}
	}

	public override bool OnAboutToStart()
	{
		if (HighLogic.CurrentGame != null)
		{
			return Reputation.Instance != null;
		}
		return false;
	}
}
