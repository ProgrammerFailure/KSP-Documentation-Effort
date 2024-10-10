using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

[MEGUI_Time]
public class MEGUIParameterTime : MEGUIParameter
{
	public MEGUITimeControl timeControl;

	public double FieldValue
	{
		get
		{
			return (double)field.GetValue();
		}
		set
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryValueChange);
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		title.text = name;
		gapDisplayPartner = (RectTransform)timeControl.timeButton.gameObject.transform;
		timeControl.time = FieldValue;
		timeControl.onValueChange.AddListener(OnValueChange);
		if (MissionEditorLogic.Instance.CurrentSelectedNode != null && MissionEditorLogic.Instance.CurrentSelectedNode.Node.isStartNode)
		{
			timeControl.isDate = true;
		}
	}

	public void OnValueChange(double time)
	{
		FieldValue = timeControl.time;
		UpdateNodeBodyUI();
	}

	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		string value = "";
		if (data.TryGetValue("value", ref value))
		{
			double num = Convert.ToSingle(value);
			field.SetValue(num);
			timeControl.time = num;
			timeControl.UpdateValues();
		}
	}
}
