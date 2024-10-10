using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public abstract class UI_Control : Attribute
{
	public UI_Scene scene = UI_Scene.All;

	public bool controlEnabled = true;

	public bool requireFullControl;

	public UI_Scene affectSymCounterparts = UI_Scene.Editor;

	public BaseField field;

	public UIPartActionItem partActionItem;

	public Callback<BaseField, object> onFieldChanged;

	public Callback<BaseField, object> onSymmetryFieldChanged;

	public bool suppressEditorShipModified;

	public UI_Control()
	{
	}

	public void Setup(BaseField field)
	{
		this.field = field;
	}

	public void SetSceneVisibility(UI_Scene scene, bool state)
	{
		switch (scene)
		{
		case UI_Scene.All:
			field.guiActive = true;
			field.guiActiveEditor = true;
			break;
		case UI_Scene.None:
			field.guiActive = false;
			field.guiActiveEditor = false;
			break;
		case UI_Scene.Editor:
			field.guiActiveEditor = state;
			break;
		case UI_Scene.Flight:
			field.guiActive = state;
			break;
		}
	}

	public virtual void Load(ConfigNode node, object host)
	{
		ParseEnabled(out controlEnabled, node, "controlEnabled", field.name);
	}

	public virtual void Save(ConfigNode node, object host)
	{
		node.AddValue("controlEnabled", controlEnabled.ToString());
	}

	public static bool ParseEnabled(out bool value, ConfigNode node, string valueName, string FieldUIControlName)
	{
		value = true;
		string value2 = node.GetValue(valueName);
		if (value2 == null)
		{
			return false;
		}
		if (!bool.TryParse(value2, out value))
		{
			Debug.LogError("FieldUIControl(" + FieldUIControlName + "): 'enabled' cannot be parsed");
			return false;
		}
		return true;
	}

	public static bool ParseFloat(out float value, ConfigNode node, string valueName, string FieldUIControlName, string errorNoValue)
	{
		value = 0f;
		string value2 = node.GetValue(valueName);
		if (value2 == null)
		{
			if (errorNoValue != null)
			{
				Debug.LogError("FieldUIControl(" + FieldUIControlName + "): " + errorNoValue);
			}
			return false;
		}
		if (!float.TryParse(value2, out value))
		{
			Debug.LogError("FieldUIControl(" + FieldUIControlName + "): '" + valueName + "' cannot be parsed");
			return false;
		}
		return true;
	}

	public static bool ParseString(out string value, ConfigNode node, string valueName, string FieldUIControlName, string errorNoValue)
	{
		value = "";
		string value2 = node.GetValue(valueName);
		if (value2 == null)
		{
			if (errorNoValue != null)
			{
				Debug.LogError("FieldUIControl(" + FieldUIControlName + "): " + errorNoValue);
			}
			return false;
		}
		value = value2;
		return true;
	}
}
