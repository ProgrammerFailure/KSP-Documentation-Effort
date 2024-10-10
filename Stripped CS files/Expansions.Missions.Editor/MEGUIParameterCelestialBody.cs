namespace Expansions.Missions.Editor;

[MEGUI_CelestialBody]
public class MEGUIParameterCelestialBody : MEGUICompoundParameter
{
	public GAPCelestialBody gapCB;

	public MEGUIParameterDropdownList dropDownBodies;

	public MissionCelestialBody FieldValue
	{
		get
		{
			return (MissionCelestialBody)field.GetValue();
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		dropDownBodies = subParameters["body"] as MEGUIParameterDropdownList;
		dropDownBodies.dropdownList.onValueChanged.AddListener(OnParameterValueChanged);
	}

	public void OnParameterValueChanged(int value)
	{
		if (FieldValue.Body == null)
		{
			FieldValue.AnyValid = true;
		}
		if (gapCB != null)
		{
			gapCB.Load(FieldValue.Body);
		}
	}

	public void OnLeftGapClick()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.Body);
		itemIndex = (itemIndex + dropDownBodies.dropdownList.options.Count - 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public void OnRightGapClick()
	{
		int itemIndex = dropDownBodies.GetItemIndex(FieldValue.Body);
		itemIndex = (itemIndex + 1) % dropDownBodies.dropdownList.options.Count;
		dropDownBodies.dropdownList.value = itemIndex;
	}

	public override void DisplayGAP()
	{
		base.DisplayGAP();
		gapCB = MissionEditorLogic.Instance.actionPane.GAPInitialize<GAPCelestialBody>();
		gapCB.SetState(GAPCelestialBodyState.SIMPLE);
		gapCB.SuscribeToLeftButton(OnLeftGapClick);
		gapCB.SuscribeToRightButton(OnRightGapClick);
		if (FieldValue != null)
		{
			gapCB.Load(FieldValue.Body);
		}
		else
		{
			gapCB.Clean();
		}
	}
}
