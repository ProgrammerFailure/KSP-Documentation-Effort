using System;
using System.Runtime.CompilerServices;
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
	private string _toolTip;

	public bool groupStartCollapsed;

	public int order;

	public bool tabStop;

	public CheckpointValidationType checkpointValidation;

	public string compareValuesForCheckpoint;

	public string Tooltip
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUI_Control()
	{
		throw null;
	}
}
