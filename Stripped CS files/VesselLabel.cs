using UnityEngine.EventSystems;

public class VesselLabel : BaseLabel
{
	public Vessel vessel { get; set; }

	public void Setup(VesselLabels labels, Vessel vessel)
	{
		Setup(labels, labels.GetLabelType(vessel));
		this.vessel = vessel;
	}

	public override void OnClick(PointerEventData ptrData)
	{
		base.OnClick(ptrData);
		if (ptrData.button == PointerEventData.InputButton.Left && ptrData.clickCount == 2)
		{
			labels.OnDoubleClickLabel(this);
		}
	}
}
