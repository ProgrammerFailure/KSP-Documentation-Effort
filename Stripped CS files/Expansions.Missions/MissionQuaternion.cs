using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

public class MissionQuaternion
{
	public Vector3 angles;

	[MEGUI_NumberRange(canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = 0f, resetValue = "0.0", displayFormat = "F3", guiName = "#autoLOC_8200030")]
	public float eulerX
	{
		get
		{
			return angles.x;
		}
		set
		{
			angles.x = value;
		}
	}

	[MEGUI_NumberRange(canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = 0f, resetValue = "0.0", displayFormat = "F3", guiName = "#autoLOC_8200031")]
	public float eulerY
	{
		get
		{
			return angles.y;
		}
		set
		{
			angles.y = value;
		}
	}

	[MEGUI_NumberRange(canBeReset = true, maxValue = 360f, clampTextInput = true, roundToPlaces = 6, displayUnits = "", minValue = 0f, resetValue = "0.0", displayFormat = "F3", guiName = "#autoLOC_8200032")]
	public float eulerZ
	{
		get
		{
			return angles.z;
		}
		set
		{
			angles.z = value;
		}
	}

	public Vector3 eulerAngles
	{
		get
		{
			return angles;
		}
		set
		{
			angles = value;
		}
	}

	public Quaternion quaternion
	{
		get
		{
			return Quaternion.Euler(angles);
		}
		set
		{
			angles = value.eulerAngles;
		}
	}

	public MissionQuaternion()
	{
		angles = Vector3.zero;
	}

	public MissionQuaternion(Vector3 newAngles)
	{
		angles = newAngles;
	}
}
