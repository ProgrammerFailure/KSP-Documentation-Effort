using ns11;

public class FundsWidget : CurrencyWidget
{
	public ns11.Tumbler tumblers;

	public void Awake()
	{
		GameEvents.OnFundsChanged.Add(onFundsChanged);
	}

	public void OnDestroy()
	{
		GameEvents.OnFundsChanged.Remove(onFundsChanged);
	}

	public void onFundsChanged(double funds, TransactionReasons reason)
	{
		tumblers.SetValue(funds);
	}

	public override void DelayedStart()
	{
		if (Funding.Instance != null)
		{
			onFundsChanged(Funding.Instance.Funds, TransactionReasons.None);
		}
	}

	public override bool OnAboutToStart()
	{
		if (HighLogic.CurrentGame != null)
		{
			return Funding.Instance != null;
		}
		return false;
	}
}
