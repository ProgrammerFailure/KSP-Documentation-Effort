using ns9;
using UnityEngine;

public class KSPField : FieldAttribute
{
	public bool isPersistant;

	public bool guiActive;

	public bool guiActiveEditor;

	public bool guiActiveUnfocused;

	[SerializeField]
	public string _guiUnits;

	public string guiFormat;

	public string category;

	public bool advancedTweakable;

	public float unfocusedRange;

	public string groupName;

	public string groupDisplayName;

	public bool groupStartCollapsed;

	public string guiUnits
	{
		get
		{
			return _guiUnits;
		}
		set
		{
			_guiUnits = Localizer.Format(value);
		}
	}

	public KSPField()
	{
		isPersistant = false;
		base.guiName = "";
		guiUnits = "";
		guiFormat = "";
		category = "";
		advancedTweakable = false;
		unfocusedRange = 2f;
		groupName = "";
		groupDisplayName = "";
		groupStartCollapsed = false;
	}
}
