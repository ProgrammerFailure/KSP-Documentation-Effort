using System.Collections;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Actions;

public class ActionModifierFunds : ActionModule
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "Add", guiName = "#autoLOC_8100123")]
	public FundsModifierType modifierType;

	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100124")]
	public float modifierFunds;

	public override void Awake()
	{
		base.Awake();
		title = "#autoLOC_8003125";
		modifierType = FundsModifierType.Add;
		modifierFunds = 0f;
	}

	public override IEnumerator Fire()
	{
		if (!(Funding.Instance == null))
		{
			switch (modifierType)
			{
			default:
				Funding.Instance.AddFunds(modifierFunds, TransactionReasons.Mission);
				break;
			case FundsModifierType.Multiply:
			{
				double num = Funding.Instance.Funds * (double)modifierFunds;
				Funding.Instance.AddFunds(num - Funding.Instance.Funds, TransactionReasons.Mission);
				break;
			}
			case FundsModifierType.Divide:
			{
				double num = Funding.Instance.Funds / (double)modifierFunds;
				Funding.Instance.AddFunds(num - Funding.Instance.Funds, TransactionReasons.Mission);
				break;
			}
			case FundsModifierType.Substract:
				Funding.Instance.AddFunds(0f - modifierFunds, TransactionReasons.Mission);
				break;
			case FundsModifierType.Set:
				Funding.Instance.AddFunds((double)modifierFunds - Funding.Instance.Funds, TransactionReasons.Mission);
				break;
			}
		}
		yield break;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8003124");
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("type", ref modifierType, FundsModifierType.Multiply);
		node.TryGetValue("modifier", ref modifierFunds);
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("type", modifierType);
		node.AddValue("modifier", modifierFunds);
	}
}
