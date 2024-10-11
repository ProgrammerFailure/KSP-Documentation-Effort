using System.Runtime.CompilerServices;

namespace Expansions.Missions.Editor;

public class MEGUI_NumberRange : MEGUI_Control
{
	public float minValue;

	public float maxValue;

	public float displayMultiply;

	public string displayFormat;

	public string displayUnits;

	public int roundToPlaces;

	public bool clampTextInput;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUI_NumberRange()
	{
		throw null;
	}
}
