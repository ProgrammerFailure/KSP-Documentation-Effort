namespace Expansions.Missions.Editor;

public class MEGUI_NumberRange : MEGUI_Control
{
	public float minValue;

	public float maxValue = 1f;

	public float displayMultiply = 1f;

	public string displayFormat = "F0";

	public string displayUnits = "";

	public int roundToPlaces = -1;

	public bool clampTextInput;
}
