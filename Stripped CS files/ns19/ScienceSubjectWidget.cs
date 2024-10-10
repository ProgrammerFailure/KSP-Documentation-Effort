using ns11;
using ns16;
using ns9;
using UnityEngine;

namespace ns19;

public class ScienceSubjectWidget : MissionSummaryWidget
{
	public ScienceSubject subject;

	public float dataGathered;

	public float scienceAmount;

	public ImgText scienceWidgetDataContent;

	public ImgText scienceWidgetValueContent;

	public ImgText scienceWidgetScienceContent;

	public static ScienceSubjectWidget Create(ScienceSubject subject, float dataGathered, float scienceAmount, MissionRecoveryDialog missionRecoveryDialog)
	{
		ScienceSubjectWidget component = Object.Instantiate(AssetBase.GetPrefab("WidgetRecoveredScience")).GetComponent<ScienceSubjectWidget>();
		component.subject = subject;
		component.Init(missionRecoveryDialog);
		component.dataGathered = dataGathered;
		component.scienceAmount = scienceAmount;
		return component;
	}

	public void UpdateFields()
	{
		header.text = "<i>" + subject.title + "</i>";
		scienceWidgetDataContent.text = Localizer.Format("#autoLOC_476374", dataGathered.ToString("0.0"));
		scienceWidgetValueContent.text = Localizer.Format("#autoLOC_476375", (scienceAmount / dataGathered).ToString("0.00"));
		scienceWidgetScienceContent.text = Localizer.Format("#autoLOC_6001494", scienceAmount.ToString("0.0"));
	}
}
