using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions;

public class DynamicModule : IConfigNode, IMENodeDisplay, ICloneable
{
	public MENode node;

	public List<string> parametersDisplayedInSAP;

	public static ObjectIDGenerator ObjectIDGen = new ObjectIDGenerator();

	public string name { get; set; }

	public string id { get; set; }

	public virtual bool canBeDisplayedInEditor => true;

	public DynamicModule()
	{
		name = GetType().Name;
		id = GetModuleInstanceID();
		parametersDisplayedInSAP = new List<string>();
	}

	public DynamicModule(MENode node)
		: this()
	{
		this.node = node;
	}

	public string GetModuleInstanceID()
	{
		bool firstTime = false;
		return ObjectIDGen.GetId(this, out firstTime).ToString();
	}

	public virtual string GetDisplayName()
	{
		return GetName();
	}

	public virtual string GetDisplayToolTip()
	{
		return "";
	}

	public virtual List<string> GetDefaultPinnedParameters()
	{
		return new List<string>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DynamicModule dynamicModule))
		{
			return false;
		}
		return name.Equals(dynamicModule.name);
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public string GetName()
	{
		return name + "_" + id;
	}

	public void AddParameterToSAP(string parameter)
	{
		parametersDisplayedInSAP.AddUnique(parameter);
	}

	public void RemoveParameterFromSAP(string parameter)
	{
		parametersDisplayedInSAP.Remove(parameter);
		if (node != null && node.HasNodeBodyParameter(GetName(), parameter))
		{
			node.RemoveParameterFromNodeBody(GetName(), parameter);
			UpdateNodeBodyUI();
		}
	}

	public void AddParameterToNodeBody(string parameter)
	{
		if (node != null)
		{
			node.AddParameterToNodeBody(GetName(), parameter);
		}
	}

	public void AddParameterToNodeBodyAndUpdateUI(string parameter)
	{
		if (node != null)
		{
			node.AddParameterToNodeBody(GetName(), parameter);
			UpdateNodeBodyUI();
		}
	}

	public void RemoveParameterFromNodeBody(string parameter)
	{
		if (node != null)
		{
			node.RemoveParameterFromNodeBody(GetName(), parameter);
		}
	}

	public void RemoveParameterFromNodeBodyAndUpdateUI(string parameter)
	{
		if (node != null)
		{
			node.RemoveParameterFromNodeBody(GetName(), parameter);
			UpdateNodeBodyUI();
		}
	}

	public bool HasNodeBodyParameter(string parameter)
	{
		if (node != null)
		{
			return node.HasNodeBodyParameter(GetName(), parameter);
		}
		return false;
	}

	public bool HasSAPParameter(string parameter)
	{
		return parametersDisplayedInSAP.Contains(parameter);
	}

	public virtual string GetNodeBodyParameterString(BaseAPField field)
	{
		string text = "None";
		if (field.GetValue() != null)
		{
			text = MissionsUtils.GetFieldValueForDisplay(field);
		}
		return Localizer.Format("#autoLOC_8004190", field.guiName, text);
	}

	public void UpdateNodeBodyUI()
	{
		if (node != null)
		{
			node.guiNode.DisplayNodeBodyParameters();
		}
	}

	public List<IMENodeDisplay> GetInternalParametersToDisplay()
	{
		return new List<IMENodeDisplay>();
	}

	public MENode GetNode()
	{
		return node;
	}

	public virtual string GetInfo()
	{
		return GetType().Name;
	}

	public void SetNode(MENode node)
	{
		this.node = node;
	}

	public virtual void Initialize()
	{
	}

	public virtual void Destroy()
	{
	}

	public virtual void ParameterSetupComplete()
	{
	}

	public virtual void Load(ConfigNode node)
	{
		string value = "";
		if (node.TryGetValue("id", ref value))
		{
			id = value;
		}
	}

	public virtual void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("id", id);
	}

	public virtual object Clone()
	{
		FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		DynamicModule dynamicModule = (DynamicModule)Activator.CreateInstance(GetType(), new object[1] { node });
		for (int i = 0; i < fields.Length; i++)
		{
			fields[i].SetValue(dynamicModule, fields[i].GetValue(this));
		}
		return dynamicModule;
	}
}
