using Expansions.Serenity;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionRoboticJointDisplay : UIPartActionItem
{
	public BasePAWGroup pawGroup;

	[SerializeField]
	public LayoutElement pawElementLayout;

	[SerializeField]
	public TextMeshProUGUI jointClass;

	[SerializeField]
	public TextMeshProUGUI txtLine1;

	[SerializeField]
	public TextMeshProUGUI txtLine2;

	[SerializeField]
	public TextMeshProUGUI txtLine3;

	[SerializeField]
	public TextMeshProUGUI txtLine4;

	[SerializeField]
	public TextMeshProUGUI txtLine5;

	[SerializeField]
	public TextMeshProUGUI txtLine6;

	[SerializeField]
	public TextMeshProUGUI txtLine7;

	public BaseServo servo;

	public ModuleRoboticServoHinge hinge;

	public ModuleRoboticRotationServo rotationServo;

	public ModuleRoboticServoRotor rotor;

	public ModuleRoboticServoPiston piston;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		servo = null;
		for (int i = 0; i < part.Modules.Count; i++)
		{
			servo = part.Modules[i] as BaseServo;
			if (servo != null)
			{
				hinge = servo as ModuleRoboticServoHinge;
				rotationServo = servo as ModuleRoboticRotationServo;
				rotor = servo as ModuleRoboticServoRotor;
				piston = servo as ModuleRoboticServoPiston;
				break;
			}
		}
		SetupItem(window, part, servo, scene, null);
	}

	public void Awake()
	{
		pawGroup = new BasePAWGroup("Debug", "#autoLOC_8320010", startCollapsed: false);
	}

	public override void UpdateItem()
	{
		jointClass.text = servo.ClassName;
		if (hinge != null)
		{
			txtLine1.text = DebugText("XDrive Spring: <<1>>", hinge.DebugServoJoint.angularXDrive.positionSpring);
			txtLine2.text = DebugText("XDrive Damper: <<1>>", hinge.DebugServoJoint.angularXDrive.positionDamper);
			txtLine3.text = DebugText("XDrive Max Force: <<1>>", hinge.DebugServoJoint.angularXDrive.maximumForce);
			txtLine4.text = DebugText("Limits: <<1>> - <<2>>", hinge.DebugServoJoint.lowAngularXLimit.limit, hinge.DebugServoJoint.highAngularXLimit.limit);
			txtLine5.text = DebugText("XLimit Spring: <<1>>", hinge.DebugServoJoint.angularXLimitSpring.spring);
			txtLine6.text = DebugText("XLimit Damper: <<1>>", hinge.DebugServoJoint.angularXLimitSpring.damper);
			txtLine7.text = DebugText("Target Rotation: <<1>>", hinge.DebugServoJoint.targetRotation.eulerAngles.ToString());
			pawElementLayout.preferredHeight = 112f;
		}
		else if (rotationServo != null)
		{
			txtLine1.text = DebugText("XDrive Spring: <<1>>", rotationServo.DebugServoJoint.angularXDrive.positionSpring);
			txtLine2.text = DebugText("XDrive Damper: <<1>>", rotationServo.DebugServoJoint.angularXDrive.positionDamper);
			txtLine3.text = DebugText("XDrive Max Force: <<1>>", rotationServo.DebugServoJoint.angularXDrive.maximumForce);
			if (!rotationServo.allowFullRotation)
			{
				txtLine4.text = DebugText("Limits: <<1>> - <<2>>", rotationServo.DebugServoJoint.lowAngularXLimit.limit, rotationServo.DebugServoJoint.highAngularXLimit.limit);
				txtLine5.text = DebugText("XLimit Spring: <<1>>", rotationServo.DebugServoJoint.angularXLimitSpring.spring);
				txtLine6.text = DebugText("XLimit Damper: <<1>>", rotationServo.DebugServoJoint.angularXLimitSpring.damper);
				txtLine7.text = DebugText("Target Rotation: <<1>>", rotationServo.DebugServoJoint.targetRotation.eulerAngles.ToString());
				pawElementLayout.preferredHeight = 112f;
			}
			else
			{
				txtLine4.text = Localizer.Format("Limits: None");
				txtLine5.text = DebugText("Target Rotation: <<1>>", rotationServo.DebugServoJoint.targetRotation.eulerAngles.ToString());
				txtLine6.gameObject.SetActive(value: false);
				txtLine7.gameObject.SetActive(value: false);
				pawElementLayout.preferredHeight = 84f;
			}
		}
		else if (rotor != null)
		{
			txtLine1.text = DebugText("XDrive Spring: <<1>>", rotor.DebugServoJoint.angularXDrive.positionSpring);
			txtLine2.text = DebugText("XDrive Damper: <<1>>", rotor.DebugServoJoint.angularXDrive.positionDamper);
			txtLine3.text = DebugText("XDrive Max Force: <<1>>", rotor.DebugServoJoint.angularXDrive.maximumForce);
			txtLine4.text = DebugText("Target Rotation: <<1>>", rotor.DebugServoJoint.targetAngularVelocity.ToString());
			txtLine5.gameObject.SetActive(value: false);
			txtLine6.gameObject.SetActive(value: false);
			txtLine7.gameObject.SetActive(value: false);
			pawElementLayout.preferredHeight = 70f;
		}
		else if (piston != null)
		{
			txtLine1.text = DebugText("Position Spring: <<1>>", piston.DebugServoJoint.xDrive.positionSpring);
			txtLine2.text = DebugText("XDrive Damper: <<1>>", piston.DebugServoJoint.xDrive.positionDamper);
			txtLine3.text = DebugText("XDrive Max Force: <<1>>", piston.DebugServoJoint.xDrive.maximumForce);
			txtLine4.text = DebugText("Limit: <<1>>", piston.DebugServoJoint.linearLimit.limit);
			txtLine5.text = DebugText("Target Position: <<1>>", piston.DebugServoJoint.targetPosition.ToString());
			txtLine6.gameObject.SetActive(value: false);
			txtLine7.gameObject.SetActive(value: false);
			pawElementLayout.preferredHeight = 84f;
		}
	}

	public string DebugText(string text, string valueString)
	{
		return Localizer.Format(text, valueString);
	}

	public string DebugText(string text, float value)
	{
		return Localizer.Format(text, value.ToString(NumberFormat(value)));
	}

	public string DebugText(string text, float value1, float value2)
	{
		return Localizer.Format(text, value1.ToString(NumberFormat(value1)), value2.ToString(NumberFormat(value2)));
	}

	public string NumberFormat(float number)
	{
		if ((double)number > 100000000.0)
		{
			return "E5";
		}
		return "0.0";
	}
}
