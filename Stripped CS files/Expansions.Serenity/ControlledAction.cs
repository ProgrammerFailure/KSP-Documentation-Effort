using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public class ControlledAction : ControlledBase
{
	[SerializeField]
	public string actionName = "";

	[SerializeField]
	public List<float> times;

	[SerializeField]
	public List<BaseAction> SymmetryActions;

	public float tempTime;

	public BaseAction Action { get; set; }

	public override string BaseName => actionName;

	public ControlledAction(ControlledAction sourceAction)
		: this(sourceAction.Part, sourceAction.Module, sourceAction.Action, sourceAction.Controller)
	{
		times = new List<float>(sourceAction.times);
	}

	public ControlledAction(Part part, PartModule module, BaseAction action, ModuleRoboticController controller)
		: base(part, module, controller)
	{
		actionName = action.name;
		Action = action;
		times = new List<float>();
		RebuildSymmetryList();
	}

	public ControlledAction()
	{
		Action = null;
		times = new List<float>();
		SymmetryActions = new List<BaseAction>();
	}

	public void ReverseTimes()
	{
		List<float> list = new List<float>();
		int count = times.Count;
		while (count-- > 0)
		{
			list.Add(1f - times[count]);
		}
		times = list;
	}

	public void RescaleTimes(float adjustmentRatio, float minSpace = 0.01f)
	{
		List<float> list = new List<float>();
		for (int i = 0; i < times.Count; i++)
		{
			tempTime = times[i];
			tempTime *= adjustmentRatio;
			if (adjustmentRatio > 1f)
			{
				tempTime = Mathf.Min(tempTime, 1f - minSpace * (float)(times.Count - i - 1));
			}
			list.Add(tempTime);
		}
		times = list;
	}

	public override bool OnChangeSymmetryMaster(Part newPart, out uint oldPartId)
	{
		oldPartId = 0u;
		BaseAction baseAction = null;
		for (int i = 0; i < newPart.Actions.Count; i++)
		{
			baseAction = newPart.Actions[actionName];
			if (baseAction != null)
			{
				break;
			}
		}
		if (baseAction == null)
		{
			Debug.LogErrorFormat("[ControlledAction]: Cannot change Symmetry Master to {0}. No {1} field on this part", newPart.persistentId, actionName);
			return false;
		}
		oldPartId = partId;
		Action = baseAction;
		return true;
	}

	public override bool OnAssignReferenceVars()
	{
		if (base.Module != null)
		{
			BaseAction baseAction = base.Module.Actions[actionName];
			if (baseAction != null)
			{
				Action = baseAction;
			}
		}
		else if (base.Part.Actions[actionName] != null)
		{
			Action = base.Part.Actions[actionName];
		}
		else
		{
			for (int i = 0; i < base.Part.Modules.Count; i++)
			{
				BaseAction baseAction2 = base.Part.Modules[i].Actions[actionName];
				if (baseAction2 != null)
				{
					Action = baseAction2;
					base.Module = base.Part.Modules[i];
					moduleId = base.Module.GetPersistentId();
					persistentActionId = base.Module.GetPersistenActiontId();
					break;
				}
			}
		}
		if (Action == null)
		{
			Debug.LogWarningFormat("[ModuleRoboticController]: Unable to find Action in vessel part: {0} - {1}", base.Part.name, actionName);
			return false;
		}
		return true;
	}

	public override void ClearSymmetryLists()
	{
		if (SymmetryActions == null)
		{
			SymmetryActions = new List<BaseAction>();
		}
		else
		{
			SymmetryActions.Clear();
		}
	}

	public override void AddSymmetryPart(Part part)
	{
		if (part.Actions[actionName] != null)
		{
			SymmetryActions.Add(part.Actions[actionName]);
			return;
		}
		int num = 0;
		BaseAction baseAction;
		while (true)
		{
			if (num < part.Modules.Count)
			{
				baseAction = part.Modules[num].Actions[actionName];
				if (baseAction != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		SymmetryActions.Add(baseAction);
	}

	public override void OnLoad(ConfigNode node)
	{
		node.TryGetValue("actionName", ref actionName);
		ConfigNode node2 = new ConfigNode();
		if (!node.TryGetNode("TIMES", ref node2))
		{
			return;
		}
		List<string> valuesList = node2.GetValuesList("time");
		for (int i = 0; i < valuesList.Count; i++)
		{
			times.Add(float.Parse(valuesList[i]));
		}
		int count = times.Count;
		while (count-- > 0)
		{
			if (times[count] > 1f || times[count] < 0f)
			{
				times.RemoveAt(count);
			}
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("actionName", actionName);
		ConfigNode configNode = node.AddNode("TIMES");
		for (int i = 0; i < times.Count; i++)
		{
			configNode.AddValue("time", times[i]);
		}
	}
}
