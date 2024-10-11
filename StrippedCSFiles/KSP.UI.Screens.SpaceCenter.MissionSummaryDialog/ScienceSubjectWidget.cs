using System.Runtime.CompilerServices;
using KSP.UI.Util;

namespace KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

public class ScienceSubjectWidget : MissionSummaryWidget
{
	public ScienceSubject subject;

	public float dataGathered;

	public float scienceAmount;

	public ImgText scienceWidgetDataContent;

	public ImgText scienceWidgetValueContent;

	public ImgText scienceWidgetScienceContent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceSubjectWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScienceSubjectWidget Create(ScienceSubject subject, float dataGathered, float scienceAmount, MissionRecoveryDialog missionRecoveryDialog)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFields()
	{
		throw null;
	}
}
