using ns11;
using ns16;
using ns9;
using UnityEngine;

namespace ns19;

public class PartWidget : MissionSummaryWidget
{
	public AvailablePart partInfo;

	public float partValue;

	public float resourcesValue;

	public float totalValue;

	public int count;

	public ImgText partWidgetQtyContent;

	public ImgText partWidgetPartValueContent;

	public ImgText partWidgetTotalValueContent;

	public static PartWidget Create(AvailablePart partInfo, float dryCost, float fuelCost, MissionRecoveryDialog missionRecoveryDialog)
	{
		PartWidget component = Object.Instantiate(AssetBase.GetPrefab("WidgetRecoveredPart")).GetComponent<PartWidget>();
		component.Init(missionRecoveryDialog);
		component.partInfo = partInfo;
		component.partValue = dryCost;
		component.resourcesValue = fuelCost;
		component.totalValue = component.partValue;
		component.count = 1;
		return component;
	}

	public static PartWidget Create(AvailablePart partInfo, float dryCost, float fuelCost, int stackQuantity, MissionRecoveryDialog missionRecoveryDialog)
	{
		PartWidget partWidget = Create(partInfo, dryCost, fuelCost, missionRecoveryDialog);
		partWidget.count = stackQuantity;
		return partWidget;
	}

	public void UpdateFields()
	{
		header.text = partInfo.title;
		partWidgetQtyContent.text = Localizer.Format("#autoLOC_476250", count.ToString("0"));
		partWidgetPartValueContent.text = Localizer.Format("#autoLOC_476251", partValue.ToString("N0"));
		partWidgetTotalValueContent.text = "+" + totalValue.ToString("N0") + " " + Localizer.Format("#autoLOC_6002218");
	}

	public void AddDuplicate(float fuelValue)
	{
		count++;
		resourcesValue += fuelValue;
		totalValue = partValue * (float)count;
	}

	public override bool Equals(object obj)
	{
		if (obj is PartWidget)
		{
			PartWidget partWidget = obj as PartWidget;
			if (partWidget.partInfo.name == partInfo.name)
			{
				return partWidget.partValue == partValue;
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
