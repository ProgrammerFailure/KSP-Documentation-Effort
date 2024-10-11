using System.Runtime.CompilerServices;

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUI_ScoreRangeList()
	{
		throw null;
	}
}
