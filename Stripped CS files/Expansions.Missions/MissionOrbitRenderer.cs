using ns23;
using ns9;

namespace Expansions.Missions;

public class MissionOrbitRenderer : OrbitTargetRenderer
{
	public MENode node;

	public static MissionOrbitRenderer Setup(MENode node, Vessel targetVessel, Orbit orbit, bool activedraw = true)
	{
		MissionOrbitRenderer missionOrbitRenderer = OrbitTargetRenderer.Setup<MissionOrbitRenderer>("MissionOrbitRenderer " + node.Title, KSPUtil.GenerateSuperSeed(node.id, node.mission.Seed), orbit, activedraw);
		missionOrbitRenderer.node = node;
		missionOrbitRenderer.targetVessel = targetVessel;
		missionOrbitRenderer.startColor = "<color=" + XKCDColors.HexFormat.Chartreuse + ">";
		missionOrbitRenderer.orbitDisplayUnlocked = true;
		return missionOrbitRenderer;
	}

	public override void apNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_277907", startColor, base.orbit.ApA.ToString("N0"), "</color>");
		if (node != null)
		{
			data.captionLine1 = Localizer.Format("#autoLOC_8000308", startColor, node.Title, "</color>");
			data.captionLine2 = Localizer.Format("#autoLOC_286270", base.orbit.referenceBody.displayName.LocalizeRemoveGender());
		}
	}

	public override void peNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		data.Header = Localizer.Format("#autoLOC_277915", startColor, base.orbit.PeA.ToString("N0"), "</color>");
		if (node != null)
		{
			data.captionLine1 = Localizer.Format("#autoLOC_8000308", startColor, node.Title, "</color>");
			data.captionLine2 = Localizer.Format("#autoLOC_286270", base.orbit.referenceBody.displayName.LocalizeRemoveGender());
		}
	}

	public override void ascNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		if (ANDNVisible && activeDraw)
		{
			data.Header = Localizer.Format("#autoLOC_277932", startColor, relativeInclination.ToString("0"), "</color>");
			if (node != null)
			{
				data.captionLine1 = Localizer.Format("#autoLOC_8000308", startColor, node.Title, "</color>");
				data.captionLine2 = Localizer.Format("#autoLOC_286270", base.orbit.referenceBody.displayName.LocalizeRemoveGender());
			}
		}
	}

	public override void descNode_OnUpdateCaption(MapNode n, MapNode.CaptionData data)
	{
		if (ANDNVisible && activeDraw)
		{
			data.Header = Localizer.Format("#autoLOC_277943", startColor, (0.0 - relativeInclination).ToString("0"), "</color>");
			if (node != null)
			{
				data.captionLine1 = Localizer.Format("#autoLOC_8000308", startColor, node.Title, "</color>");
				data.captionLine2 = Localizer.Format("#autoLOC_286270", base.orbit.referenceBody.displayName.LocalizeRemoveGender());
			}
		}
	}
}
