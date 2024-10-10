using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_Quaternion]
public class MEGUIParameterQuaternion : MEGUICompoundParameter
{
	public delegate void OnValueChanged(Quaternion newValue);

	public enum EulerAxis
	{
		x,
		y,
		z
	}

	public OnValueChanged onValueChanged;

	public MEGUIParameterNumberRange rangeX;

	public MEGUIParameterNumberRange rangeY;

	public MEGUIParameterNumberRange rangeZ;

	public MissionQuaternion FieldValue
	{
		get
		{
			return (MissionQuaternion)field.GetValue();
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
		rangeX = subParameters["eulerX"] as MEGUIParameterNumberRange;
		rangeY = subParameters["eulerY"] as MEGUIParameterNumberRange;
		rangeZ = subParameters["eulerZ"] as MEGUIParameterNumberRange;
		rangeX.slider.onValueChanged.AddListener(OnValueChange);
		rangeY.slider.onValueChanged.AddListener(OnValueChange);
		rangeZ.slider.onValueChanged.AddListener(OnValueChange);
	}

	public void OnValueChange(float newValue)
	{
		if (onValueChanged != null)
		{
			onValueChanged(FieldValue.quaternion);
		}
	}

	public void SetQuaternion(Quaternion quaternion)
	{
		Vector3 eulerAngles = quaternion.eulerAngles;
		rangeX.slider.value = eulerAngles.x;
		rangeY.slider.value = eulerAngles.y;
		rangeZ.slider.value = eulerAngles.z;
		FieldValue.quaternion = quaternion;
	}

	public void ToggleEulerAxis(EulerAxis axis, bool toggleValue)
	{
		switch (axis)
		{
		case EulerAxis.x:
			rangeX.Toggle(toggleValue);
			break;
		case EulerAxis.y:
			rangeY.Toggle(toggleValue);
			break;
		case EulerAxis.z:
			rangeZ.Toggle(toggleValue);
			break;
		}
	}
}
