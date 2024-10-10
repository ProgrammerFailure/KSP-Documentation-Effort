using ns2;
using ns9;
using TMPro;
using UnityEngine;

[UI_Label]
public class UIPartActionFuelFlowOverlay : UIPartActionItem
{
	[SerializeField]
	public UIButtonToggle flowToggle;

	[SerializeField]
	public TextMeshProUGUI flowText;

	public bool isConsumer;

	public bool isProvider;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		SetupItem(window, part, null, scene, null);
		if (FuelFlowOverlay.instance != null)
		{
			isConsumer = FuelFlowOverlay.instance.isConsumer(part);
			isProvider = FuelFlowOverlay.instance.isProvider(part);
			if (isConsumer || isProvider)
			{
				flowToggle.onToggle.AddListener(FlowToggle);
				flowToggle.SetState(part.fuelFlowOverlayEnabled);
				flowText.text = Localizer.Format("#autoLOC_5700004", isConsumer);
			}
		}
	}

	public void FlowToggle()
	{
		part.fuelFlowOverlayEnabled = !part.fuelFlowOverlayEnabled;
		if (part.fuelFlowOverlayEnabled)
		{
			FuelFlowOverlay.instance.SpawnOverlay(part);
		}
		else
		{
			FuelFlowOverlay.instance.ClearOverlay();
		}
	}
}
