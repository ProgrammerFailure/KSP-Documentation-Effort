using ns11;
using ns16;
using ns9;
using UnityEngine;

namespace ns19;

public class ResourceWidget : MissionSummaryWidget
{
	public PartResourceDefinition rscDef;

	public float unitValue;

	public float amount;

	public float totalValue;

	public ImgText rscWidgetQtyContent;

	public ImgText rscWidgetUnitValueContent;

	public ImgText rscWidgetTotalValueContent;

	public static ResourceWidget Create(PartResourceDefinition rscDef, float amount, float unitCost, MissionRecoveryDialog missionRecoveryDialog)
	{
		ResourceWidget component = Object.Instantiate(AssetBase.GetPrefab("WidgetRecoveredResource")).GetComponent<ResourceWidget>();
		component.Init(missionRecoveryDialog);
		component.rscDef = rscDef;
		component.unitValue = unitCost;
		component.amount = amount;
		component.totalValue = component.unitValue * amount;
		return component;
	}

	public void UpdateFields()
	{
		header.text = rscDef.displayName;
		rscWidgetQtyContent.text = Localizer.Format("#autoLOC_476314", amount.ToString("0.00"));
		rscWidgetUnitValueContent.text = Localizer.Format("#autoLOC_476315", unitValue.ToString("N2"));
		rscWidgetTotalValueContent.text = "+" + totalValue.ToString("N0") + " " + Localizer.Format("#autoLOC_6002218");
	}

	public void AddAmount(float amount)
	{
		this.amount += amount;
		totalValue = unitValue * this.amount;
	}

	public override bool Equals(object obj)
	{
		if (obj is ResourceWidget)
		{
			ResourceWidget resourceWidget = obj as ResourceWidget;
			if (resourceWidget.rscDef.name == rscDef.name)
			{
				return resourceWidget.unitValue == unitValue;
			}
			return false;
		}
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
