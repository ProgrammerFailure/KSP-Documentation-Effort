using System;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Editor;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class MEGUI_Control : FieldAttribute
{
	public enum InputContentType
	{
		Standard,
		Autocorrected,
		IntegerNumber,
		DecimalNumber,
		Alphanumeric,
		Name,
		EmailAddress,
		Password,
		Pin,
		Custom,
		Percentage
	}

	public bool gapDisplay;

	public string onValueChange;

	public string onControlCreated;

	public string onControlSetupComplete;

	public string group;

	public string groupDisplayName;

	public string resetValue;

	public bool canBeReset;

	public bool canBePinned;

	public bool hideWhenSiblingsExist;

	public bool hideWhenStartNode;

	public bool hideWhenDocked;

	public bool hideWhenInputConnected;

	public bool hideWhenOutputConnected;

	public bool hideWhenNoTestModules;

	public bool hideWhenNoActionModules;

	public bool hideOnSetup;

	[SerializeField]
	public string _toolTip;

	public bool groupStartCollapsed;

	public int order;

	public bool tabStop;

	public CheckpointValidationType checkpointValidation;

	public string compareValuesForCheckpoint;

	public string Tooltip
	{
		get
		{
			return _toolTip;
		}
		set
		{
			_toolTip = Localizer.Format(value);
		}
	}

	public MEGUI_Control()
	{
		gapDisplay = false;
		onValueChange = "";
		onControlCreated = "";
		group = "";
		resetValue = null;
		canBeReset = true;
		canBePinned = true;
		hideWhenSiblingsExist = false;
		hideWhenStartNode = false;
		hideWhenDocked = false;
		hideWhenInputConnected = false;
		hideWhenOutputConnected = false;
		hideWhenNoTestModules = false;
		hideWhenNoActionModules = false;
		hideOnSetup = false;
		Tooltip = "";
		groupStartCollapsed = false;
		order = -1;
		tabStop = false;
		checkpointValidation = CheckpointValidationType.Equals;
		compareValuesForCheckpoint = null;
	}
}
