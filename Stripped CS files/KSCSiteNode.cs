using ns23;
using ns9;
using UnityEngine;

public class KSCSiteNode : ISiteNode
{
	public PSystemSetup.SpaceCenterFacility runwayFacility;

	public string GetName()
	{
		return "KSC";
	}

	public string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8003001");
	}

	public Transform GetWorldPos()
	{
		if (runwayFacility == null)
		{
			runwayFacility = PSystemSetup.Instance.GetSpaceCenterFacility("Runway");
		}
		return runwayFacility.facilityTransform;
	}

	public void UpdateNodeCaption(MapNode mn, MapNode.CaptionData data)
	{
		data.Header = GetDisplayName();
	}
}
