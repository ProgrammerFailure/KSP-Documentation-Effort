using System.Runtime.CompilerServices;
using Expansions.Serenity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionRoboticJointDisplay : UIPartActionItem
{
	internal BasePAWGroup pawGroup;

	[SerializeField]
	private LayoutElement pawElementLayout;

	[SerializeField]
	private TextMeshProUGUI jointClass;

	[SerializeField]
	private TextMeshProUGUI txtLine1;

	[SerializeField]
	private TextMeshProUGUI txtLine2;

	[SerializeField]
	private TextMeshProUGUI txtLine3;

	[SerializeField]
	private TextMeshProUGUI txtLine4;

	[SerializeField]
	private TextMeshProUGUI txtLine5;

	[SerializeField]
	private TextMeshProUGUI txtLine6;

	[SerializeField]
	private TextMeshProUGUI txtLine7;

	private BaseServo servo;

	private ModuleRoboticServoHinge hinge;

	private ModuleRoboticRotationServo rotationServo;

	private ModuleRoboticServoRotor rotor;

	private ModuleRoboticServoPiston piston;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionRoboticJointDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string DebugText(string text, string valueString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string DebugText(string text, float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string DebugText(string text, float value1, float value2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string NumberFormat(float number)
	{
		throw null;
	}
}
