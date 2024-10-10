namespace Expansions.Missions.Editor;

public class MEGUIDropDownItem
{
	public string key;

	public object value;

	public string displayString;

	public MEGUIDropDownItem(string key, object value, string displayString)
	{
		this.key = key;
		this.value = value;
		this.displayString = displayString;
	}
}
