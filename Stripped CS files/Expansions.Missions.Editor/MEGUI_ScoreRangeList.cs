namespace Expansions.Missions.Editor;

public class MEGUI_ScoreRangeList : MEGUI_Control
{
	public enum RangeContentType
	{
		IntegerNumber,
		DecimalNumber,
		Percentage,
		Time
	}

	public RangeContentType ContentType;

	public string minLabel;

	public string maxLabel;

	public string valueLabel;

	public MEGUI_ScoreRangeList()
	{
		canBeReset = false;
		canBePinned = true;
		minLabel = "Min";
		maxLabel = "Max";
		valueLabel = "Score";
		ContentType = RangeContentType.DecimalNumber;
	}
}
