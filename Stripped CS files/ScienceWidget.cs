using TMPro;

public class ScienceWidget : CurrencyWidget
{
	public TextMeshProUGUI text;

	public void Awake()
	{
		GameEvents.OnScienceChanged.Add(onScienceChange);
	}

	public void OnDestroy()
	{
		GameEvents.OnScienceChanged.Remove(onScienceChange);
	}

	public void onScienceChange(float sci, TransactionReasons reason)
	{
		text.text = KSPUtil.LocalizeNumber(sci, "0.0");
	}

	public override void DelayedStart()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			onScienceChange(ResearchAndDevelopment.Instance.Science, TransactionReasons.None);
		}
	}

	public override bool OnAboutToStart()
	{
		if (HighLogic.CurrentGame != null)
		{
			return ResearchAndDevelopment.Instance != null;
		}
		return false;
	}
}
